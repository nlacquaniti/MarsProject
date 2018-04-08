using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.SceneManagement;

public class MissionCreation : EditorWindow
{
    private static MissionContainer MissionContainer;
    private static bool _ResourceAvabile = false;

    enum QuestType : int {None, SpecialDelivery, CollectInfos, FillOfEnergy, FreeRoaming}
    private QuestType _SelectedQuest;

    enum Zona : int { VallesMarineris, OlympusMons, Polonord, Crateri}
    private Zona _SelectedZone;

    enum Mission : int { Mission1, Mission2, Mission3, Mission4, Mission5, Mission6, Mission7 }
    private Mission _SelectedMission;

    private Quest _InSceneQuest = null;

    private bool _MissionCreationOpen = false;
    private bool _MissionModifyOpen = false;
    private bool _MissionTypeSelection = true;

    private string _CurrentSelectedZone;
    private string _OldSelectedZone;
    private string _CurrentSelectedMission;
    private string _OldSelectedMission;
    private string _SelectedMissionType;
    

    [MenuItem("Tools/Mission/Create new mission")]
    public static void OpenWindow()
    {
        MissionCreation mc = (MissionCreation)EditorWindow.GetWindow(typeof(MissionCreation));
        
        LoadList();
        mc.Show();
    }


    private void OnGUI()
    {
        if (_ResourceAvabile)
        {
            ///Select the area
            ///Save the choise in currentZoneSelection
            _SelectedZone = (Zona)EditorGUILayout.EnumPopup("Select Area", _SelectedZone);
            _CurrentSelectedZone = _SelectedZone.ToString();
            EditorGUILayout.Space();

            ///Select the mission 
            ///Save the choise in currentMissionSelection
            _SelectedMission = (Mission)EditorGUILayout.EnumPopup("Select Mission", _SelectedMission);
            _CurrentSelectedMission = _SelectedMission.ToString();
            EditorGUILayout.Space();

            ///Check if current mission has already a script mission and display the timers
            ///If the script is null (meaning there's not a mission) ask to create a new one
            ///And go to that scene (if the scene is avabile), else show an error box and open Build settings
            if (MissionContainer.MissionList[(int)_SelectedMission].IsMissionAvabile[(int)_SelectedZone])
            {
                EditorGUILayout.LabelField("Mission Type", MissionContainer.MissionList[(int)_SelectedMission].CurrentQuest[(int)_SelectedZone].ToString());
                EditorGUILayout.LabelField("Gold Time", MissionContainer.MissionList[(int)_SelectedMission].Gold[(int)_SelectedZone].ToString());
                EditorGUILayout.LabelField("Silver Time", MissionContainer.MissionList[(int)_SelectedMission].Silver[(int)_SelectedZone].ToString());
                EditorGUILayout.LabelField("Bronze Time", MissionContainer.MissionList[(int)_SelectedMission].Bronze[(int)_SelectedZone].ToString());
                EditorGUILayout.Space();

                ///Load the mission map
                ///Ask if the user wants to modify the mission
                if (!_MissionModifyOpen)
                {
                    if (GUILayout.Button("Modify Mission"))
                    {
                        EditorSceneManager.OpenScene("Assets/Scenes/Mappe/" + _SelectedZone.ToString() + _SelectedMission.ToString() + ".unity");
                        _InSceneQuest = GameObject.Find("GameManager").GetComponent<Quest>();
                        _SelectedQuest = (MissionCreation.QuestType)(MissionContainer.MissionList[(int)_SelectedMission].CurrentQuest[(int)_SelectedZone]);
                        _OldSelectedZone = _SelectedZone.ToString();
                        _OldSelectedMission = _SelectedMission.ToString();
                        _MissionModifyOpen = true;
                    }
                    /*
                    if (GUILayout.Button("Modify mission"))
                    {
                        if (EditorUtility.DisplayDialog("Modify mission", "Are you sure?", "Yes", "No"))
                        {
                            MissionContainer.MissionList[(int)_SelectedMission].IsMissionAvabile[(int)_SelectedZone] = false;
                        }
                    }
                    */
                }

                if(_MissionModifyOpen)
                {
                    if (_CurrentSelectedZone != _OldSelectedZone.ToString() || _CurrentSelectedMission != _OldSelectedMission.ToString())
                        _MissionModifyOpen = false;

                    _SelectedQuest = (QuestType)EditorGUILayout.EnumPopup("Select Quest type", _SelectedQuest);
                    if (MissionContainer.MissionList[(int)_SelectedMission].CurrentQuest[(int)_SelectedZone] == (MissionUI.QuestType)_SelectedQuest)
                    {
                        _InSceneQuest.GoldTime = EditorGUILayout.FloatField("Gold Time", _InSceneQuest.GoldTime);
                        _InSceneQuest.SilverTime = EditorGUILayout.FloatField("Silver Time", _InSceneQuest.SilverTime);
                        _InSceneQuest.BronzeTime = EditorGUILayout.FloatField("Bronze Time", _InSceneQuest.BronzeTime);
                    }
                    else
                    {
                        if (EditorUtility.DisplayDialog("Allert!", "Quest type can't be changed directly." +
                                                       "Do you want to delete and reset the script? ", "Yes", "No"))
                        {
                            DestroyImmediate(GameObject.Find("GameManager"));
                            DestroyImmediate(GameObject.Find("Spawn"));
                            DestroyImmediate(GameObject.Find("FinalTarget"));
                            DestroyImmediate(GameObject.Find("Collectable"));
                            DestroyImmediate(GameObject.Find("HUD"));
                            DestroyImmediate(GameObject.Find("MiniMapCamera"));

                            if(GameObject.Find("TargetCollection") != null)
                            {
                                DestroyImmediate(GameObject.Find("TargetCollection"));
                            }
                            MissionContainer.MissionList[(int)_SelectedMission].CurrentQuest[(int)_SelectedZone] = MissionUI.QuestType.None;
                            MissionContainer.MissionList[(int)_SelectedMission].Gold[(int)_SelectedZone] = 0;
                            MissionContainer.MissionList[(int)_SelectedMission].Silver[(int)_SelectedZone] = 0;
                            MissionContainer.MissionList[(int)_SelectedMission].Bronze[(int)_SelectedZone] = 0;
                            MissionContainer.MissionList[(int)_SelectedMission].IsMissionAvabile[(int)_SelectedZone] = false;
                            EditorUtility.SetDirty(MissionContainer.MissionList[(int)_SelectedMission]);
                            EditorApplication.SaveScene();
                            _SelectedQuest = QuestType.None;
                            _MissionModifyOpen = false;
                        }
                        else
                        {
                            _SelectedQuest = (QuestType)(MissionContainer.MissionList[(int)_SelectedMission].CurrentQuest[(int)_SelectedZone]);
                        }
                    }

                    if (GUILayout.Button("Submit changes"))
                    {
                        MissionContainer.MissionList[(int)_SelectedMission].CurrentQuest[(int)_SelectedZone] = (MissionUI.QuestType)_SelectedQuest;
                        MissionContainer.MissionList[(int)_SelectedMission].Gold[(int)_SelectedZone] = _InSceneQuest.GoldTime;
                        MissionContainer.MissionList[(int)_SelectedMission].Silver[(int)_SelectedZone] = _InSceneQuest.SilverTime;
                        MissionContainer.MissionList[(int)_SelectedMission].Bronze[(int)_SelectedZone] = _InSceneQuest.BronzeTime;
                        MissionContainer.MissionList[(int)_SelectedMission].IsMissionAvabile[(int)_SelectedZone] = true;
                        EditorUtility.SetDirty(MissionContainer.MissionList[(int)_SelectedMission]);
                        EditorApplication.SaveScene();
                        _SelectedQuest = QuestType.None;
                        _MissionModifyOpen = false;

                    }
                    if(GUILayout.Button("Cancel"))
                    {
                        _InSceneQuest.GoldTime = MissionContainer.MissionList[(int)_SelectedMission].Gold[(int)_SelectedZone];
                        _InSceneQuest.SilverTime = MissionContainer.MissionList[(int)_SelectedMission].Silver[(int)_SelectedZone];
                        _InSceneQuest.BronzeTime = MissionContainer.MissionList[(int)_SelectedMission].Bronze[(int)_SelectedZone];
                        _SelectedQuest = QuestType.None;
                       _MissionModifyOpen = false;
                    }

                }

            }
            else
            {
                _MissionModifyOpen = false;
                if (!_MissionCreationOpen)
                {
                    if (GUILayout.Button("Mission missing, Create a new one") && !_MissionCreationOpen)
                    {

                        try
                        {
                            EditorSceneManager.OpenScene("Assets/Scenes/Mappe/" + _SelectedZone.ToString() + _SelectedMission.ToString() + ".unity");
                            _MissionCreationOpen = true;
                            _OldSelectedZone = _SelectedZone.ToString();
                            _OldSelectedMission = _SelectedMission.ToString();
                        }
                        catch
                        {
                            EditorUtility.DisplayDialog("Scene error", "Scene not found, check the build setting", "Copy that");
                            EditorWindow.GetWindow(Type.GetType("UnityEditor.BuildPlayerWindow, UnityEditor"));
                        }


                    }
                }
            }

            if (_MissionCreationOpen)
            {
                if (_CurrentSelectedZone != _OldSelectedZone.ToString() || _CurrentSelectedMission != _OldSelectedMission.ToString())
                {
                    _MissionCreationOpen = false;
                    _SelectedQuest = QuestType.None;
                }

                _SelectedQuest = (QuestType)EditorGUILayout.EnumPopup("Select Quest type", _SelectedQuest);
                if(_MissionTypeSelection)
                    _SelectedMissionType = _SelectedQuest.ToString();
                EditorGUILayout.Space();

                if (_SelectedMissionType != _SelectedQuest.ToString())
                {
                    if(GameObject.Find("GameManager") != null)
                        DestroyImmediate(GameObject.Find("GameManager").GetComponent<Quest>());
                    _InSceneQuest = null;
                    _MissionTypeSelection = true;
                }

                if (_SelectedQuest != QuestType.None)
                {

                    if (_InSceneQuest == null)
                    {
                        ///Call method to search the script from resources
                        if (GetScriptFromResources())
                        {

                        }
                        else
                        {
                            Debug.Log("Load Resources failed");
                        }
                    }

                   

                    if (_InSceneQuest != null)
                    {
                        _MissionTypeSelection = false;
                        //EditorGUILayout.ObjectField("Quest script", _InSceneQuest, typeof(Quest), true);
                        _InSceneQuest.GoldTime = EditorGUILayout.FloatField("Gold Time", _InSceneQuest.GoldTime);
                        _InSceneQuest.SilverTime = EditorGUILayout.FloatField("Silver Time", _InSceneQuest.SilverTime);
                        _InSceneQuest.BronzeTime = EditorGUILayout.FloatField("Bronze Time", _InSceneQuest.BronzeTime);

                        

                        if (GUILayout.Button("Submit"))
                        {
                            Debug.Log(_SelectedQuest.ToString());
                            MissionContainer.MissionList[(int)_SelectedMission].CurrentQuest[(int)_SelectedZone] = (MissionUI.QuestType)_SelectedQuest;
                            MissionContainer.MissionList[(int)_SelectedMission].Gold[(int)_SelectedZone] = _InSceneQuest.GoldTime;
                            MissionContainer.MissionList[(int)_SelectedMission].Silver[(int)_SelectedZone] = _InSceneQuest.SilverTime;
                            MissionContainer.MissionList[(int)_SelectedMission].Bronze[(int)_SelectedZone] = _InSceneQuest.BronzeTime;
                            MissionContainer.MissionList[(int)_SelectedMission].IsMissionAvabile[(int)_SelectedZone] = true;



                            if (_SelectedQuest != QuestType.None)
                            {
                                if (GameObject.Find("Collectable") == null)
                                {
                                    PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath("Assets/Prefabs/ScenePrefabs/Collectable.prefab", typeof(GameObject)));
                                }

                                if (GameObject.Find("HUD") == null)
                                {
                                    PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath("Assets/Prefabs/ScenePrefabs/HUD.prefab", typeof(GameObject)));
                                }
                            }

                            if (_SelectedQuest != QuestType.FreeRoaming)
                            {
                                if (GameObject.Find("FinalTarget") == null)
                                {
                                    PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath("Assets/Prefabs/ScenePrefabs/FinalTarget.prefab", typeof(GameObject)));
                                }

                                if (_SelectedQuest == QuestType.CollectInfos)
                                {
                                    PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath("Assets/Prefabs/ScenePrefabs/TargetCollection.prefab", typeof(GameObject)));
                                }

                                if(_SelectedQuest == QuestType.FillOfEnergy)
                                {
                                    PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath("Assets/Prefabs/ScenePrefabs/FuelRecharging.prefab", typeof(GameObject)));
                                }
                            }

                            
                          

                            EditorUtility.SetDirty(MissionContainer.MissionList[(int)_SelectedMission]);
                            EditorApplication.SaveScene();
                            _SelectedQuest = QuestType.None;
                            _MissionCreationOpen = false;
                           

                        }
                    }
                }
            }

           
        }
    }


    private bool GetScriptFromResources()
    {
        var scriptFound = false;
        var scripts = (Resources.Load<Quest>("MissionScripts/" + _SelectedQuest.ToString()));
        GameObject manager;

        if (GameObject.Find("GameManager") == null)
        {
            PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath("Assets/Prefabs/ScenePrefabs/GameManager.prefab", typeof(GameObject)));
        }

        if (GameObject.Find("Spawn") == null)
        {
            PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath("Assets/Prefabs/ScenePrefabs/Spawn.prefab", typeof(GameObject)));
        }

        if (GameObject.Find("MiniMapCamera") == null)
        {
            PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath("Assets/Prefabs/ScenePrefabs/MiniMapCamera.prefab", typeof(GameObject)));
        }


        if (scripts != null)
        {
            manager = GameObject.Find("GameManager");
            if (GameObject.Find("GameManager").GetComponent<Quest>() == null)
                manager.AddComponent(scripts.GetType());

            _InSceneQuest = manager.GetComponent<Quest>();
            scriptFound = true;
            Debug.Log("Script found");
        }

        return scriptFound;

    }

    public static void LoadList()
    {
        _ResourceAvabile = false;
        if (MissionContainer == null)
        {
            MissionContainer = Resources.Load<MissionContainer>("MissionCreation");

            if(MissionContainer != null && MissionContainer.MissionList.Count <= 0)
            {
                MissionContainer.GetMissionList(Resources.LoadAll<MissionUI>("Missions"));
                EditorUtility.SetDirty(MissionContainer);
            }
        }
        _ResourceAvabile = true;

    }

}
