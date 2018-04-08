using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCollection : MonoBehaviour
{
    public static TargetCollection instance;
    
    public int TargetCount { get { return _CurrentTargetCount; } }
    public int MaxTargetCount { get { return _TotalTargetCount; } }

    private int _CurrentTargetCount ;
    private int _TotalTargetCount;

    private void Awake()
    {
        instance = this;
        _TotalTargetCount  = transform.childCount;
    }

    public void TargetReached()
    {
        _CurrentTargetCount++;

        if (_CurrentTargetCount >= _TotalTargetCount)
        {
            Quest.currentQuest.QuestObjective = MissionTexts.COLLECT_INFOS_PHASE_1;
            Quest.currentQuest.CanBeCompleted = true;
            print("Quest can be completed");
        }
        else
        {
            StartCoroutine(HUD.instance.ShowObjectiveBox(MissionTexts.COLLECT_INFOS_PHASE_0 + " " + TargetCount + " / " + MaxTargetCount));
        }
        
    }

}
