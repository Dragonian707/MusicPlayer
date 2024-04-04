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
    AudioSource source;

    public Text debugText;

    private void Start()
    {
        Permission.RequestUserPermission(Permission.ExternalStorageRead);
        source = FindObjectOfType<AudioSource>();
    }
    
    public async void SetSong(int song)
    {
        if (source.clip != null) { DestroyImmediate(source.clip, true); }
        
        //waits for the correct song to be retrieved and downloaded before putting it in the audiosource
        AudioClip s = await LoadClip(GlobalValues.SongPaths[song]);
        if (s == null) { return; }
        
        source.clip = s;
        source.Play();
    }

    async Task<AudioClip> LoadClip(string path)
    {
        try
        {
            Uri uri = new Uri(path); //needs to be Uri in order to work on phone

            AudioClip x = null;
            using (UnityWebRequest rq = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.MPEG))
            {
                //create a handler to make the song not download, but streaming it instead making it take way less memory
                DownloadHandlerAudioClip handler = new DownloadHandlerAudioClip(string.Empty, AudioType.MPEG);
                handler.streamAudio = true;
                rq.downloadHandler = handler;

                try
                {
                    rq.SendWebRequest();
                    while (!rq.isDone) await Task.Delay(5);

                    x = DownloadHandlerAudioClip.GetContent(rq);
                    rq.Dispose();
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                    debugText.text = e.ToString();
                }
            }
            return x;
            //if (x.length > 330) { return x; }

            /*using (UnityWebRequest rq2 = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.MPEG))
            { 
                Debug.Log("downloading actual song");
                try
                {
                    rq2.SendWebRequest();
                    while (!rq2.isDone) await Task.Delay(5);
                
                    x = DownloadHandlerAudioClip.GetContent(rq2);
                    rq2.Dispose();
                    Debug.Log("Completed");
                }
                catch (Exception e2)
                {
                    Debug.Log(e2.Message);
                    debugText.text = e2.ToString();
                }
            }
            return x;*/
        }
        catch (Exception f)
        {
            debugText.text = f.ToString();
        }
        return null;
    }

    //--------------------------------------usefull things------------------------------------------------\\
    //Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); //gets the path to where 'my documents' are *on PC
    //t.text = Directory.GetFiles("/sdcard/Download").Length.ToString(); //gets the download directory in INTERNAL storage...
    //t.text = Directory.GetFiles("/storage/emulated/0/Music", "*.mp3", SearchOption.AllDirectories).Length.ToString(); //also gets the internal storage
    //source.time = 0; //sets the song playing to the start
    //
    //DownloadHandlerAudioClip handler = new DownloadHandlerAudioClip(string.Empty, AudioType.MPEG);
    //handler.streamAudio = true; //streams the audio instead of first downloading it. Saving a lot of memory, but costing quality

}
