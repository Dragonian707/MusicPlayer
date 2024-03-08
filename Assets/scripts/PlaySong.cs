using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySong : MonoBehaviour
{
    public int clipNumber;
    public void StartSong()
    {
        FindObjectOfType<Manager>().SetSong(clipNumber);
    }
}
