using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using TagLib;

public class Manager : MonoBehaviour
{
    public List<AudioClip> clips = new List<AudioClip>();
    public List<TagLib.File> IDtags = new List<TagLib.File>();
    public AudioSource source;
    public Text t;
    public GameObject parent;
    public GameObject prefab;

    private void Start()
    {
        Permission.RequestUserPermission(Permission.ExternalStorageRead);
        t.text = "have asked..";
    }

    public void SetSong(int song)
    {

    }

    public void PlayTheSong()
    {
        Debug.Log(clips.Count);
        t.text = "";
        //GetComponent<AudioSource>().Play();
        //Debug.Log(j);
        //TagLib.File f = TagLib.File.Create(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "/Bone Dry.mp3");
        //t.text = f.Tag.Title;
        //Debug.Log(Directory.GetCurrentDirectory());
        try
        {
            //string[] songs = Directory.GetFiles("sdcard/Music"); //returns the paths to the files

            //foreach (string s in songs)
            //{
            //    t.text += s + "\n";
            //    //TagLib.File.Create(s); //will work great later
            //}
            //TagLib.File f = TagLib.File.Create("/sdcard/Music/bad-apple.mp3");
            //t.text = f.Tag.Title;
            //t.text = Resources.Load("/sdcard/Music/bad-apple.mp3").GetType().ToString();
        }
        catch (Exception e)
        {
            t.text = e.ToString();
        }
        //Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); //gets the path to where 'my documents' are *on PC
        //t.text = Directory.GetFiles("/sdcard/Download").Length.ToString(); //gets the download directory in INTERNAL storage...
        //t.text = Directory.GetFiles("/storage/emulated/0/Music", "*.mp3", SearchOption.AllDirectories).Length.ToString(); //also gets the internal storage
    }

}
