using UnityEngine;
public class LoadDebug : MonoBehaviour
{
    public string zone;
    public string mission;
    public float time;
    public string medal;
    public bool completed;
    public bool collectable;
    private MissionData[] _MissionData;

    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.L))
        {
            print("Loading data");
            _MissionData = DataContainer.CurrentMissionData.ToArray();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            print("Saving data");
            var _NewMission = new MissionData();
            _NewMission.area = zone;
            _NewMission.mission = mission;
            _NewMission.record = time;
            _NewMission.medal = medal;
            _NewMission.completed = completed;
            _NewMission.collectable = collectable;
            StartCoroutine(SavingDataManager.SaveFile(_NewMission));
        }
    }
}
