using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FileSystem
{
    public static void Save(int level)
    {
        PlayerPrefs.SetInt("Level", level);
    }

    public static int Load()
    {
        int level = PlayerPrefs.GetInt("Level");
        return level;
    }
}