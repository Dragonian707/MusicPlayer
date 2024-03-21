using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TagLib;

public class PlaySong : MonoBehaviour
{
    int clipNumber;
    [SerializeField] Text songTitle;
    [SerializeField] Text artistAndAlbum;
    public void StartSong()
    {
        FindObjectOfType<SongManager>().SetSong(clipNumber, false);
    }

    public void SetupButton(int clip, File tag)
    {
        clipNumber = clip;

        songTitle.text = tag.Tag.Title;

        if (tag.Tag.Performers.Length > 0)
        {
            artistAndAlbum.text = tag.Tag.Performers[0];
        }
        if (tag.Tag.Album != null)
        {
            artistAndAlbum.text += " - ";
            artistAndAlbum.text += tag.Tag.Album;
        }
    }
}
