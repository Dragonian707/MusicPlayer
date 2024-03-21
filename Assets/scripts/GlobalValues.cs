using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalValues
{
    private static List<string> songNames = new List<string>();
    public static List<string> SongNames { get { return songNames; } set { songNames = value; } }


    private static string[] songPaths;
    public static string[] SongPaths { get { return songPaths; } set { songPaths = value; } }


    private static List<int> songNumber = new List<int>();
    public static List<int> SongNumber { get { return songNumber; } set { songNumber = value; } }
}
