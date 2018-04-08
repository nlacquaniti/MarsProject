using System.Collections;
using UnityEngine;

public class CollectInfos : Quest
{
    private Transform[] _Targets;


    private void Start()
    {
        base.Start();
        QuestObjective = MissionTexts.COLLECT_INFOS_PHASE_0 + " " + TargetCollection.instance.TargetCount + " / " + TargetCollection.instance.MaxTargetCount;
    }

   
    protected override void MissionCompleted()
    {
        print("Mission completed");

        if (currentTime <= GoldTime)
        {
            //unlock gold medal
            UnlockedMedal = "Gold";
            //ui + string + gold medal

        }
        else if (currentTime > GoldTime && currentTime < SilverTime)
        {
            //unlock silver medal
            UnlockedMedal = "Silver";
            //ui + string + gold medal

        }
        else
        {
            //unlock bronze medal
            UnlockedMedal = "Bronze";
            //ui + string + gold medal
        }

        //save time
        print("Mission Completed " + UnlockedMedal);
        IsMissionCompleted = true;
        //StartCoroutine(MissionData()); 
    }

    protected override void MissionFailed()
    {
        print("Mission failed");
        //StartCoroutine(MissionData());
    }

   
}
