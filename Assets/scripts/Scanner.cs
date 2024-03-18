using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Scanner : MonoBehaviour
{
    List<GameObject> buttons = new List<GameObject>();
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject parent;
    string path;
    public Text debugText;

    public void Scan()
    {
        debugText.text = "";
#if UNITY_ANDROID && !UNITY_EDITOR
        ScanMusicAndroid();
#else
        ScanMusicPC();
#endif
        debugText.text = "path: " + path;
        ScanMusic();
    }

    private void ScanMusicPC()
    {
        path = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
    }
    private void ScanMusicAndroid()
    {
        path = "/storage/emulated/0/Music"; //hard coded for now, will change later
    }

    private void ScanMusic()
    {
        for (int x = buttons.Count - 1; x >= 0; x--)
        {
            Destroy(buttons[x]);
        }
        buttons.Clear();

        string[] songs = Directory.GetFiles(path, "*.mp3", SearchOption.TopDirectoryOnly); //returns the paths to the files
        TagLib.File file;

        try
        {
            for (int i = 0; i < songs.Length; i++)
            {
                file = TagLib.File.Create(songs[i]);
                
                buttons.Add(Instantiate(prefab, parent.transform));
                
                if (file.Tag.IsEmpty)
                {
                    TagLib.Tag tag = file.Tag;
                    tag.Title = songs[i].Split('/')[^1];
                }
                GlobalValues.SongNames.Add(file.Tag.Title);
                
                buttons[i].GetComponentInChildren<PlaySong>().SetupButton(i, file);
                file.Dispose();
            }
        }
        catch (Exception e)
        {
            debugText.text = e.ToString();
        }

        Debug.Log(songs.Length);

        parent.GetComponent<RectTransform>().sizeDelta = new Vector2(parent.GetComponent<RectTransform>().sizeDelta.x, 250 * buttons.Count);

        GlobalValues.SongPaths = songs;

    }
}
