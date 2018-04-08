using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Quest : MonoBehaviour
{
    public static Quest currentQuest;

    public float GoldTime;
    public float SilverTime;
    public float BronzeTime;
    public float currentTime;

    private const float WARNING_TIMER = 10f;

    public bool IsMissionCompleted { get; set; }
    public bool CollectableFound { get; set; }
    public bool CanBeCompleted { get { return _CanBeCompleted; } set { _CanBeCompleted = value; if (_CanBeCompleted) StartCoroutine(QuestFinalTarget.ShowFinalTarget()); } }
    public string QuestObjective { get { return _QuestObjective; } set { _QuestObjective = value; StartCoroutine(HUD.instance.ShowObjectiveBox(_QuestObjective)); } }
    public MissionStatus CurrentQuestStatus { get { return _CurrentStatus; } }

    public enum MissionStatus { WAITING, RUNNING, COMPLETED, FAILED };
    private enum MissionMedal { NONE, Gold, Silver, Bronze };

    private MissionMedal _CurrentMedal = MissionMedal.NONE;
    private IEnumerator _QuestCheck;
    private IEnumerator _TimeWarning;
    private string _QuestObjective = "";

    private MissionData _CurrentMission = null;

    protected string UnlockedMedal { get; set; }
    protected MissionStatus _CurrentStatus = MissionStatus.WAITING;
    protected bool missionTrigger = true;
    private bool _CanBeCompleted = false;

    private bool _WarningTrigger = true;

    private ObjectiveSounds _MedalSound;

    protected void Start()
    {
        currentQuest = this;
        HUD.instance.CheckMissionName(this.GetType().Name);
        StartCoroutine(_QuestCheck = CurrentMedal());
        CheckMissionData();
        
    }

    protected virtual void Update()
    {
        if (_CurrentStatus == MissionStatus.WAITING)
        {
            TimeConversion.RacingTime(currentTime);
        }


        else if (_CurrentStatus == MissionStatus.RUNNING)
        {

            currentTime += Time.deltaTime;

            TimeConversion.RacingTime(currentTime);

            if (currentTime > BronzeTime)
            {
                ChangeMissionStatus(MissionStatus.FAILED);
            }

            if (NewVehicleController.vehicleController.currentFuel <= 0)
            {
                ChangeMissionStatus(MissionStatus.FAILED);
            }

        }
        else if (_CurrentStatus == MissionStatus.COMPLETED && missionTrigger)
        {

            StopCoroutine(_QuestCheck);
            missionTrigger = false;
            HUD.instance.ShowObjectiveBox();
            MissionCompleted();
            //SetResult(true);
            HUD.instance.ShowQuestResult(true);
            AudioManager.Audio.PlayObjectivesSound(_MedalSound);
            StartCoroutine(MissionData());
        }
        else if (_CurrentStatus == MissionStatus.FAILED && missionTrigger)
        {
            StopCoroutine(_QuestCheck);
            missionTrigger = false;
            HUD.instance.ShowObjectiveBox();
            MissionFailed();
            HUD.instance.ShowQuestResult(false);
            AudioManager.Audio.PlayObjectivesSound(ObjectiveSounds.MissionFailed);
            //SetResult(false);
            StartCoroutine(MissionData());
        }

    }


    public void ChangeMissionStatus(MissionStatus status)
    {
        if(status != _CurrentStatus)
            _CurrentStatus = status;
    }

    protected IEnumerator MissionData()
    {
        _CurrentMission.area = MenuInizialeContainer.instance.zoneName;
        _CurrentMission.mission = MenuInizialeContainer.instance.missionName;

        if (!_CurrentMission.collectable && CollectableFound)
        {
            _CurrentMission.collectable = true;
        }
        if (_CurrentMission.medal != UnlockedMedal)
        {
            _CurrentMission.medal = UnlockedMedal;
        }
        if (currentTime < _CurrentMission.record)
        {
            _CurrentMission.record = currentTime;
        }
        else if(_CurrentMission.record == 0.0f)
        {
            _CurrentMission.record = currentTime;
        }
        if (!_CurrentMission.completed && IsMissionCompleted)
        {
            _CurrentMission.completed = true;
        }

        StartCoroutine(SaveMission(_CurrentMission));
        yield return null;
    }

    private IEnumerator SaveMission(MissionData currentMission)
    {
        print("Saving Mission");
        //GameObject.Find("HUD").transform.GetChild(1).gameObject.SetActive(true);
        yield return StartCoroutine(SavingDataManager.SaveFile(currentMission));
        print("Mission saved");
        //StartCoroutine(RetrunToMenu());

    }

    private IEnumerator CurrentMedal()
    {
        while (_CurrentStatus == MissionStatus.RUNNING || _CurrentStatus == MissionStatus.WAITING)
        {
            if (currentTime < GoldTime && _CurrentMedal != MissionMedal.Gold)
            {
                ChangeCurrentMedal(MissionMedal.Gold, GoldTime);
                _MedalSound = ObjectiveSounds.GoldGoal;

            }

            else if (currentTime > GoldTime && currentTime < SilverTime && _CurrentMedal != MissionMedal.Silver)
            {
                ChangeCurrentMedal(MissionMedal.Silver, SilverTime);
                _MedalSound = ObjectiveSounds.SilverGoal;
                AudioManager.Audio.PlayObjectivesSound(ObjectiveSounds.LostMedal);
            }

            else if (currentTime > SilverTime && currentTime < BronzeTime && _CurrentMedal != MissionMedal.Bronze)
            {
                ChangeCurrentMedal(MissionMedal.Bronze, BronzeTime);
                _MedalSound = ObjectiveSounds.BronzeGoal;
                AudioManager.Audio.PlayObjectivesSound(ObjectiveSounds.LostMedal);
            }


            if (_CurrentMedal == MissionMedal.Gold && currentTime + WARNING_TIMER >= GoldTime && _WarningTrigger)
            {
                _WarningTrigger = false;
                StartCoroutine(_TimeWarning = HUD.instance.MedalWarning());
            }
            else if (_CurrentMedal == MissionMedal.Silver && currentTime + WARNING_TIMER >= SilverTime && _WarningTrigger)
            {
                _WarningTrigger = false;
                StartCoroutine(_TimeWarning = HUD.instance.MedalWarning());
            }
            else if (_CurrentMedal == MissionMedal.Bronze && currentTime + WARNING_TIMER >= BronzeTime && _WarningTrigger)
            {
                _WarningTrigger = false;
                StartCoroutine(_TimeWarning = HUD.instance.MedalWarning());
            }

            yield return new WaitForSeconds(0.5f);
        }

    }

    private void ChangeCurrentMedal(MissionMedal medal, float time)
    {
        _WarningTrigger = true;
        _CurrentMedal = medal;

        if (_TimeWarning != null)
        {
            StopCoroutine(_TimeWarning);
        }
        HUD.instance.ChangeMedalTime = TimeConversion.RacingTime(time);
    }

    private IEnumerator RetrunToMenu()
    {
        print("Load menu");
        StartCoroutine(SceneLoader.LoadSceneAsync(ScenesContainer.MENU_SELEZIONE));
        print("Menu loaded");
        NewVehicleController.vehicleController.enabled = false;
        yield return null;

    }

    //private void SetResult(bool missionCompleted)
    //{
    //    if(missionCompleted)
    //    {
    //        _ResultText = "Mission Completed";
    //        //Set green color
    //    }
    //    else
    //    {
    //        _ResultText = "Mission Failed";
    //        //Set red color
    //    }
    //}

    private void CheckMissionData()
    {
        _CurrentMission = new MissionData();
        var _MissionName = MenuInizialeContainer.instance.zoneName + MenuInizialeContainer.instance.missionName;

        if (DataContainer.MissionAvabile(_MissionName))
        {
            var _SavedData = DataContainer.GetCurrentData(_MissionName);

            _CurrentMission.record = _SavedData.record;
            _CurrentMission.medal = _SavedData.medal;
            _CurrentMission.collectable = _SavedData.collectable;
            _CurrentMission.completed = _SavedData.completed;
        }


    }
    protected abstract void MissionCompleted();
    protected abstract void MissionFailed();
    
}





