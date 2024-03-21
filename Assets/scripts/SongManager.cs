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

    public void SetSong(int songNumber, bool shuffling)
    {
        if (songNumber < 0)
        {
            songNumber = GlobalValues.SongNames.Count - 1;
        }
        else if (songNumber >= GlobalValues.SongNames.Count)
        {
            songNumber = 0;
        }

        if (shuffling)
        {
            songProgress.value = 0;
            FindObjectOfType<Manager>().SetSong(GlobalValues.SongNumber[songNumber]);

            Text t = current.GetComponentInChildren<Text>();
            t.text = GlobalValues.SongNames[GlobalValues.SongNumber[songNumber]];
        }
        else
        {
            songProgress.value = 0;
            FindObjectOfType<Manager>().SetSong(songNumber);

            Text t = current.GetComponentInChildren<Text>();
            t.text = GlobalValues.SongNames[songNumber];
        }
        currentSong = songNumber;
    }
    //button methods
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
    public void shuffle()
    {
        GlobalValues.SongNumber.Clear();

        int t = 999;
        while (GlobalValues.SongNumber.Count != GlobalValues.SongNames.Count)
        {
            t = Random.Range(0, GlobalValues.SongNames.Count);
            if (!GlobalValues.SongNumber.Contains(t))
            {
                GlobalValues.SongNumber.Add(t);
            }
        }
        shuffled = true;
        SetSong(0, shuffled);
    }
}
