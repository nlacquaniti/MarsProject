using System.Collections;
using UnityEngine;

public class FillOfEnergy : Quest
{

    private float _CurrentFuel;

    private void Start()
    {
        base.Start();
        CanBeCompleted = true;
        QuestObjective = QuestObjective = MissionTexts.FILL_OF_ENERGY;

    }

    
    protected override void MissionCompleted()
    {
        IsMissionCompleted = true;
        print("Mission completed");
        
        if(_CurrentFuel <= GoldTime)
        {
            //gold medal
            UnlockedMedal = "Gold";
        }
        else if(_CurrentFuel > GoldTime && _CurrentFuel <= SilverTime)
        {
            //silver medal
            UnlockedMedal = "Silver";
        }
        else
        {
            //bronze medal
            //Change Status to Failed -m "Fuel belove bronze"
            UnlockedMedal = "Bronze";
            //ChangeMissionStatus(MissionStatus.Failed, "Not enough fuel");
            
        }
    }

    protected override void MissionFailed()
    {
        print("Mission failed");
        //StartCoroutine(MissionData());
    }

}






