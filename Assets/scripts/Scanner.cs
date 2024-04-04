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
        //another path for sd-card = /storage/3E1B-3826/Download
    }

    private void ScanMusic()
    {
        //make sure there are no active buttons anymore
        for (int x = buttons.Count - 1; x >= 0; x--)
        {
            Destroy(buttons[x]);
        }
        buttons.Clear();

        string[] songs = Directory.GetFiles(path, "*.mp3", SearchOption.TopDirectoryOnly); //returns the paths to the files
        TagLib.File file;

        try
        {
            //for all the song paths that were found, make a button with the song name, artist and album (if applicable) that will play the song if pressed
            for (int i = 0; i < songs.Length; i++)
            {
                file = TagLib.File.Create(songs[i]);
                
                buttons.Add(Instantiate(prefab, parent.transform));

                //depending on if this is played on mobile or PC cut the string path if there was no title
#if UNITY_ANDROID && !UNITY_EDITOR
                if (file.Tag.IsEmpty)
                {
                    TagLib.Tag tag = file.Tag;
                    tag.Title = songs[i].Split('/')[^1];
                }
#else
                if (file.Tag.IsEmpty)
                {
                    TagLib.Tag tag = file.Tag;
                    tag.Title = songs[i].Split('\\')[^1];
                    file.Save();
                }
#endif

                //add the song names and their number to a global list to be referenced somewhere else
                GlobalValues.SongNames.Add(file.Tag.Title);
                GlobalValues.SongNumbers.Add(i);

                buttons[i].GetComponentInChildren<PlaySong>().SetupButton(i, file);
                file.Dispose();
            }
        }
        catch (Exception e)
        {
            debugText.text = e.ToString();
        }

        Debug.Log(songs.Length);

        //make the scrollarea where all buttons are larger to fit all the buttons and still scroll
        parent.GetComponent<RectTransform>().sizeDelta = new Vector2(parent.GetComponent<RectTransform>().sizeDelta.x, 250 * buttons.Count + 5000);

        //also add the paths to the songs in another list to be used to get the sound file
        GlobalValues.SongPaths = songs;

    }
}
