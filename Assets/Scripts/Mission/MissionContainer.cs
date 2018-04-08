using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "Project Mars Tools / Create Mission container")]
public class MissionContainer : ScriptableObject
{
    private  List<MissionUI> _MissionList = new List<MissionUI>();

    public  List<MissionUI> MissionList { get { return _MissionList; } set { value = _MissionList; } }

    public  void GetMissionList(MissionUI[] missionList)
    {
        foreach(MissionUI mission in missionList)
        {
            _MissionList.Add(mission);
        }
    }
    
}