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
    public Text t;

    public void ScanMusic()
    {
        t.text = "";
        for (int x = buttons.Count - 1; x >= 0; x--)
        {
            Destroy(buttons[x]);
        }

        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
        string[] songs = Directory.GetFiles(path, "*.mp3", SearchOption.TopDirectoryOnly); //returns the paths to the files
        List<TagLib.File> IDtags = new List<TagLib.File>();

        for (int i = 0; i < songs.Length; i++)
        {
            t.text += songs[i];
            t.text += "\n";

            IDtags.Add(TagLib.File.Create(songs[i]));
            
            //clips.Add(await LoadClip(songs[i])); //don't do this, it will crash unity with too many audioclips lol
            
            buttons.Add(Instantiate(prefab, parent.transform));
            
            if (IDtags[i].Tag.IsEmpty)
            {
                TagLib.Tag tag = IDtags[i].Tag;
                tag.Title = songs[i].Split('\\')[songs[i].Split('\\').Length - 1];
            }
            
            buttons[i].GetComponentInChildren<PlaySong>().SetupButton(i, IDtags[i]);
        }
        Debug.Log(songs.Length);

        parent.GetComponent<RectTransform>().sizeDelta = new Vector2(parent.GetComponent<RectTransform>().sizeDelta.x, 250 * songs.Length);

        FindObjectOfType<Manager>().SetValues(songs, IDtags);
    }

}
