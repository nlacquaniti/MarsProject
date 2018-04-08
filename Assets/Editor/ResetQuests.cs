using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class ResetQuests : Editor
{
    [MenuItem("Tools/ResetMission")]
    static void ResetAllMissions()
    {
        if (EditorUtility.DisplayDialog("Warning!", "Do you really want to delete all the missions?", "Yes", "No"))
        {

            var missions = Resources.LoadAll<MissionUI>("Missions");

            for (int i = 0; i < missions.Length; i++)
            {
                for (int k = 0; k < missions[0].Gold.Length; k++)
                {
                    missions[i].Gold[k] = 0;
                    missions[i].Silver[k] = 0;
                    missions[i].Bronze[k] = 0;
                    missions[i].CurrentQuest[k] = MissionUI.QuestType.None;
                    missions[i].IsMissionAvabile[k] = false;
                    EditorUtility.SetDirty(missions[i]);
                }
            }
        }

        
    }
}
