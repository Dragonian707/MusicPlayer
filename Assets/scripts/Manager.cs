using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using TagLib;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class Manager : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] GameObject currentSong; // a section at the bottom showing info on the current song
    [SerializeField] Slider songProgress;
    string[] clips;
    List<TagLib.File> IDtags = new List<TagLib.File>();
    float timer;

    private void Start()
    {
        Permission.RequestUserPermission(Permission.ExternalStorageRead);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer <= 0.75f) { return; }
        if (source.clip == null) { return; }

        if (source.time >= source.clip.length) 
        {
            Destroy(source.clip);
            source.clip = null;
            return;
        }

        float setVal = source.time / source.clip.length;
        if (!SeeTooDifferent(setVal))
        {
            songProgress.value = setVal;
        }
        else
        {
            SetSongTime(songProgress.value);
        }
    }
    bool SeeTooDifferent(float value)
    {
        if (value - songProgress.value >= 0.01f || songProgress.value - value >= 0.01f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetSongTime(float when)
    {
        source.time = when * source.clip.length;
    }

    public async void SetSong(int song)
    {
        source.clip = await LoadClip(clips[song]);
        source.Play();

        Text t = currentSong.GetComponentInChildren<Text>();
        if (IDtags[song].Tag.Title != null)
        {
            t.text = IDtags[song].Tag.Title;
        }
        else
        {
            t.text = clips[song];
        }
        songProgress.value = 0;
    }

    public void SetValues(string[] s, List<TagLib.File> t)
    {
        clips = s;
        IDtags = t;
    }

    async Task<AudioClip> LoadClip(string path)
    {
        AudioClip x = null;
        using (UnityWebRequest rq = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG))
        {
            rq.SendWebRequest();

            try
            {
                while (!rq.isDone) await Task.Delay(5);

                x = DownloadHandlerAudioClip.GetContent(rq);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                //t.text = e.ToString();
            }
        }

        return x;
    }
    //--------------------------------------usefull things------------------------------------------------\\
    //Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); //gets the path to where 'my documents' are *on PC
    //t.text = Directory.GetFiles("/sdcard/Download").Length.ToString(); //gets the download directory in INTERNAL storage...
    //t.text = Directory.GetFiles("/storage/emulated/0/Music", "*.mp3", SearchOption.AllDirectories).Length.ToString(); //also gets the internal storage
    //source.time = 0; //sets the song playing to the start

}
