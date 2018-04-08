using System;
using System.Collections;
using UnityEngine;

public class SpecialDelivery : Quest
{

    private void Start()
    {
        base.Start();
        CanBeCompleted = true;
        QuestObjective = MissionTexts.SPECIAL_DELIVERY;
       
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
        //call ui + string
        print("Mission failed");
        //StartCoroutine(MissionData());
    }


}



