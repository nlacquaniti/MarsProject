using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

public static class DataContainer 
{
    public static List<MissionData> CurrentMissionData = new List<MissionData>();
    private static bool _HasLoaded = false;


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void GetDataFromDisk()
    {
        if (!_HasLoaded)
        {
            if (!Directory.Exists(SavingDataManager.path))
            {
                Directory.CreateDirectory(SavingDataManager.path);
            }

            CurrentMissionData.AddRange(SavingDataManager.LoadData());
        }
      
    }

    public static int MissionsCompleted()
    {
        return CurrentMissionData.FindAll(x => x.completed).Count;
    }
    
    public static MissionData GetCurrentData(string mission)
    {
        return CurrentMissionData.Find(x => mission == x.area + x.mission);
    }


    public static string GetUserSaveInfoString(string mission)
    {
        var _Match = CurrentMissionData.Find(x => mission == x.area + x.mission);

        if (_Match != null)
            return TimeConversion.RacingTime(_Match.record) + "/" + _Match.medal + "/" + _Match.collectable + "/" + _Match.completed;

        else
            return "00 : 00 : 00 / No / No / No";

    }

    public static bool NewRecordCheck(string mission, float time)
    {
        var _Match = CurrentMissionData.Find(x => mission == x.area + x.mission);

        if (_Match != null)
        {
            return time > _Match.record;
        }
        else
        {
            return false;
        }

    }

    public static bool MissionAvabile(string mission)
    {
        var _Match = CurrentMissionData.Find(x => mission == x.area + x.mission);

        return _Match != null ? true : false;
    }

}
