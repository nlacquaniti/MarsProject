using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSelezioneData : MonoBehaviour
{
    private const string VALLES_MARINERIS = "VallesMarineris",
                         OLYMPUS_MONS = "OlympusMons",
                         POLONORD = "Polonord",
                         CRATERI = "Crateri";

    private const string GOLD_MEDAL = "Gold",
                         SILVER_MEDAL = "Silver",
                         BRONZE_MEDAL = "Bronze";


    private void Start()
    {
        var _UnlockedAreas = DataContainer.MissionsCompleted();



        if (_UnlockedAreas < 3)
        {
            MissionSelection.goldNumber[0] = MedalsCount(VALLES_MARINERIS, GOLD_MEDAL);
            MissionSelection.silverNumber[0] = MedalsCount(VALLES_MARINERIS, SILVER_MEDAL);
            MissionSelection.bronzeNumber[0] = MedalsCount(VALLES_MARINERIS, BRONZE_MEDAL);

            MissionSelection.collectableNumber[0] = CollectableCount(VALLES_MARINERIS);
        }
        else if (_UnlockedAreas >= 3 && _UnlockedAreas < 7)
        {
            MissionSelection.goldNumber[0] = MedalsCount(VALLES_MARINERIS, GOLD_MEDAL);
            MissionSelection.silverNumber[0] = MedalsCount(VALLES_MARINERIS, SILVER_MEDAL);
            MissionSelection.bronzeNumber[0] = MedalsCount(VALLES_MARINERIS, BRONZE_MEDAL);

            MissionSelection.goldNumber[1] = MedalsCount(OLYMPUS_MONS, GOLD_MEDAL);
            MissionSelection.silverNumber[1] = MedalsCount(OLYMPUS_MONS, SILVER_MEDAL);
            MissionSelection.bronzeNumber[1] = MedalsCount(OLYMPUS_MONS, BRONZE_MEDAL);

            MissionSelection.collectableNumber[0] = CollectableCount(VALLES_MARINERIS);
            MissionSelection.collectableNumber[1] = CollectableCount(OLYMPUS_MONS);

        }
        else if (_UnlockedAreas >= 7 && _UnlockedAreas < 14)
        {
            MissionSelection.goldNumber[0] = MedalsCount(VALLES_MARINERIS, GOLD_MEDAL);
            MissionSelection.silverNumber[0] = MedalsCount(VALLES_MARINERIS, SILVER_MEDAL);
            MissionSelection.bronzeNumber[0] = MedalsCount(VALLES_MARINERIS, BRONZE_MEDAL);

            MissionSelection.goldNumber[1] = MedalsCount(OLYMPUS_MONS, GOLD_MEDAL);
            MissionSelection.silverNumber[1] = MedalsCount(OLYMPUS_MONS, SILVER_MEDAL);
            MissionSelection.bronzeNumber[1] = MedalsCount(OLYMPUS_MONS, BRONZE_MEDAL);

            MissionSelection.goldNumber[2] = MedalsCount(POLONORD, GOLD_MEDAL);
            MissionSelection.silverNumber[2] = MedalsCount(POLONORD, SILVER_MEDAL);
            MissionSelection.bronzeNumber[2] = MedalsCount(POLONORD, BRONZE_MEDAL);

            MissionSelection.collectableNumber[0] = CollectableCount(VALLES_MARINERIS);
            MissionSelection.collectableNumber[1] = CollectableCount(OLYMPUS_MONS);
            MissionSelection.collectableNumber[2] = CollectableCount(POLONORD);

        }
        else if (_UnlockedAreas >= 14)
        {
            MissionSelection.goldNumber[0] = MedalsCount(VALLES_MARINERIS, GOLD_MEDAL);
            MissionSelection.silverNumber[0] = MedalsCount(VALLES_MARINERIS, SILVER_MEDAL);
            MissionSelection.bronzeNumber[0] = MedalsCount(VALLES_MARINERIS, BRONZE_MEDAL);

            MissionSelection.goldNumber[1] = MedalsCount(OLYMPUS_MONS, GOLD_MEDAL);
            MissionSelection.silverNumber[1] = MedalsCount(OLYMPUS_MONS, SILVER_MEDAL);
            MissionSelection.bronzeNumber[1] = MedalsCount(OLYMPUS_MONS, BRONZE_MEDAL);

            MissionSelection.goldNumber[2] = MedalsCount(POLONORD, GOLD_MEDAL);
            MissionSelection.silverNumber[2] = MedalsCount(POLONORD, SILVER_MEDAL);
            MissionSelection.bronzeNumber[2] = MedalsCount(POLONORD, BRONZE_MEDAL);

            MissionSelection.goldNumber[3] = MedalsCount(CRATERI, GOLD_MEDAL);
            MissionSelection.silverNumber[3] = MedalsCount(CRATERI, SILVER_MEDAL);
            MissionSelection.bronzeNumber[3] = MedalsCount(CRATERI, BRONZE_MEDAL);

            MissionSelection.collectableNumber[0] = CollectableCount(VALLES_MARINERIS);
            MissionSelection.collectableNumber[1] = CollectableCount(OLYMPUS_MONS);
            MissionSelection.collectableNumber[2] = CollectableCount(POLONORD);
            MissionSelection.collectableNumber[3] = CollectableCount(CRATERI);
        }
    }

    private string MedalsCount(string areaName, string medalType)
    {
       return DataContainer.CurrentMissionData.FindAll(x => x.area == areaName && x.medal == medalType).Count.ToString();
    }

    private string CollectableCount(string areaName)
    {
        return DataContainer.CurrentMissionData.FindAll(x => x.area == areaName && x.collectable).Count.ToString();
    }

}
