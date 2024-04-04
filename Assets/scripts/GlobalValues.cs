using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalValues
{
    //a list containing the title to display of each song
    private static List<string> songNames = new List<string>();
    public static List<string> SongNames { get { return songNames; } set { songNames = value; } }

    //a list containing all the paths to songs for the downloader to use
    private static string[] songPaths;
    public static string[] SongPaths { get { return songPaths; } set { songPaths = value; } }

    //a list of numbers in a random order to be used when shuffling songs
    private static List<int> songNumbers = new List<int>();
    public static List<int> SongNumbers { get { return songNumbers; } set { songNumbers = value; } }
}
