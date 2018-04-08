using System.Collections;
using UnityEngine;

public class FreeRoaming : Quest
{
    private new void Start()
    {
        currentQuest = this;
        HUD.instance.CheckMissionName(this.GetType().Name);
        StartCoroutine(CheckCollectableStatus());
    }

    protected override void Update()
    {
        if (_CurrentStatus == MissionStatus.RUNNING)
        {
            currentTime += Time.deltaTime;
            TimeConversion.RacingTime(currentTime);
        }
    }

    private IEnumerator CheckCollectableStatus()
    {
        while(!CollectableFound)
        {
            //Wait check
            yield return new WaitForSeconds(1.0f);
        }
        StartCoroutine(MissionData());
    }

    protected override void MissionCompleted()
    {
        //Mission can't be completed
    }

    protected override void MissionFailed()
    {
        //Mission can't be failed
    }
}





