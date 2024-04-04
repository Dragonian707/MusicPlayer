using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongManager : MonoBehaviour
{
    private float timer = 0f;
    [SerializeField] private AudioSource source;
    [SerializeField] private Slider songProgress;
    [SerializeField] private GameObject current;
    private int currentSong;
    private bool shuffled = false;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer <= 0.75f) { return; }
        if (source.clip == null) { return; }

        //if the song is over, destroy the song
        if (source.time >= source.clip.length)
        {
            Destroy(source.clip);
            source.clip = null;
            songProgress.value = 0;
            if (shuffled)
            {
                SetSong(currentSong + 1, shuffled);
            }
            return;
        }

        //set the slider at the bottom to the value of where in the song you are and allow the user to change where inthe song they are
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
        //check if the slider has been touched by the user to skip forward in the song
        if (value - songProgress.value >= 0.005f || songProgress.value - value >= 0.005f)
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
        //set the song to the specified value
        source.time = when * source.clip.length;
    }

    public void SetSong(int songNumber, bool shuffling)
    {
        //make sure the songnumber that was given is in range
        if (songNumber < 0)
        {
            songNumber = GlobalValues.SongNames.Count - 1;
        }
        else if (songNumber >= GlobalValues.SongNames.Count)
        {
            songNumber = 0;
        }

        //if you're shuffling the songs use the (random) songnumbers from GlobalValues
        if (shuffling)
        {
            FindObjectOfType<Manager>().SetSong(GlobalValues.SongNumbers[songNumber]);
            songProgress.value = 0;

            Text t = current.GetComponentInChildren<Text>();
            t.text = GlobalValues.SongNames[GlobalValues.SongNumbers[songNumber]];
        }
        else
        {
            FindObjectOfType<Manager>().SetSong(songNumber);
            songProgress.value = 0;

            Text t = current.GetComponentInChildren<Text>();
            t.text = GlobalValues.SongNames[songNumber];
        }
        currentSong = songNumber;
    }
    //button methods for the next and previous song, pausing and unpausing songs and to shuffle
    public void PreviousSong()
    {
        currentSong--;
        SetSong(currentSong, shuffled);
    }
    public void PauseUnpauseSong()
    {
        if (source.isPlaying)
        {
            source.Pause();
        }
        else
        {
            source.UnPause();
        }
    }
    public void NextSong()
    {
        currentSong++;
        SetSong(currentSong, shuffled);
    }
    public void Shuffle()
    {
        GlobalValues.SongNumbers.Clear();

        int t = 999;
        while (GlobalValues.SongNumbers.Count != GlobalValues.SongNames.Count)
        {
            t = Random.Range(0, GlobalValues.SongNames.Count);
            if (!GlobalValues.SongNumbers.Contains(t))
            {
                GlobalValues.SongNumbers.Add(t);
            }
        }
        shuffled = true;
        SetSong(0, shuffled);
    }
}
