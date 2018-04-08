using UnityEngine;
using System.Collections;
using System.IO;

public class SavingDataManager 
{
    public static string path = Application.persistentDataPath + "/MissionSaves/";
    
    public static IEnumerator SaveFile (MissionData file)
    {
        var _SameMission = DataContainer.CurrentMissionData.Find(x => x.area == file.area && x.mission == file.mission);

        if (_SameMission != null)
        {
            Debug.Log("Match found");


            if (file.record < _SameMission.record)
            {
                _SameMission.record = file.record;
                if (_SameMission.medal != file.medal)
                {
                    _SameMission.medal = file.medal;
                }
            }

            if (!_SameMission.completed && file.completed)
            {
                _SameMission.completed = file.completed;
            }

            if (!_SameMission.collectable && file.collectable)
            {
                _SameMission.collectable = file.collectable;
            }


        }
        else
        {
            Debug.Log("Match not found");
            DataContainer.CurrentMissionData.Add(file);
        }


        var _Contents = JsonUtility.ToJson(file, true);
        File.WriteAllText(path + file.area + file.mission + ".json", _Contents);
        yield return null;
        
    }

    public static MissionData[] LoadData()
    {

        var _AllDataStr = Directory.GetFiles(path);
        MissionData[] _Files = new MissionData[_AllDataStr.Length];
       
        for(int i = 0; i < _AllDataStr.Length; i++)
        {
            var _Content = File.ReadAllText(_AllDataStr[i]);
            _Files[i] = JsonUtility.FromJson<MissionData>(_Content);
            
        }

        return _Files;
        
    }

}
