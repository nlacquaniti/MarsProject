using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public class OpenSavesLocation : Editor
{
    private static string path = Application.persistentDataPath + "/MissionSaves/";

    [MenuItem("Saves Folder/ Open folder location")]
    public static void OpenSaveFolder()
    {
        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
       
        EditorUtility.OpenFilePanel("Game data location", path, "json");
        
    }
}
