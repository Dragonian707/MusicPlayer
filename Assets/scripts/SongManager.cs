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
            SetSong(currentSong + 1);
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

    public void SetSong(int songNumber)
    {
        if (songNumber < 0 || songNumber >= GlobalValues.SongNames.Count) { return; }

        songProgress.value = 0;
        FindObjectOfType<Manager>().SetSong(songNumber);

        Text t = current.GetComponentInChildren<Text>();
        t.text = GlobalValues.SongNames[songNumber];

        currentSong = songNumber;
    }
    //button methods
    public void PreviousSong()
    {
        currentSong--;
        SetSong(currentSong);
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
        SetSong(currentSong);
    }
}
