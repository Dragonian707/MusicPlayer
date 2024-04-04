using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TagLib;

//script put on all buttons in order for them to work
public class PlaySong : MonoBehaviour
{
    int clipNumber;
    [SerializeField] Text songTitle;
    [SerializeField] Text artistAndAlbum;
    public void StartSong()
    {
        FindObjectOfType<SongManager>().SetSong(clipNumber, false);
    }

    //sets up the button with its text and which number corresponds to the song to play
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
