using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class Scanner : MonoBehaviour
{
    List<GameObject> buttons = new List<GameObject>();
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject parent;

    public async void ScanMusic()
    {
        for (int x = buttons.Count - 1; x >= 0; x--)
        {
            Destroy(buttons[x]);
        }

        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "/test";
        string[] songs = Directory.GetFiles(path); //returns the paths to the files
        List<TagLib.File> IDtags = new List<TagLib.File>();
        List<AudioClip> clips = new List<AudioClip>();

        for (int i = 0; i < songs.Length; i++)
        {
            IDtags.Add(TagLib.File.Create(songs[i]));

            clips.Add(await LoadClip(songs[i]));

            buttons.Add(Instantiate(prefab, parent.transform));
        }
        Debug.Log(clips.Count);
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
}
