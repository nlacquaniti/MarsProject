using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MissionSelection : MonoBehaviour
{
    public static bool invalidZone = false;

    public int zoneIndex = 0;
    public int missioniIndex = 0;

    public static string[] goldNumber = { "0", "0", "0", "0" };
    public static string[] silverNumber = { "0", "0", "0", "0" };
    public static string[] bronzeNumber = { "0", "0", "0", "0" };

    public static string[] collectableNumber = { "0", "0", "0", "0" };

    public bool lockMissions = true;

    private int _UnlockedZone = 0;

    private Transform m_Zone;
    private Transform m_Missions;

    private Text[] _MedalCountTxts =  new Text[3];
    private Text[] _MedalTimersTxts = new Text[3];

    private Text _CollectableCountTxt = null;
    private Text _RecordTxt = null;
    private Text _MissionDescription = null;

    private Image _MapImage = null;
    private Image _RecordMedalBox = null;

    private Sprite[] _MedalsSprites = new Sprite[3];

    private Sprite[] _VallesMarinerisMaps = new Sprite[7];
    private Sprite[] _MonsOlympusMaps = new Sprite[7];
    private Sprite[] _PolonordMaps = new Sprite[7];
    private Sprite[] _CrateriMaps = new Sprite[7];



    private static Tweener _LockShake;
    private static GameObject _LockImg;
    private static MissionUI[] _MissionContainer;

    private GameObject[] _SecondImageObj = new GameObject[4];
    private GameObject[] _FistImageObj = new GameObject[3];

    private bool m_StopInput = false;
    private bool isFirstImage = true;
    

    private delegate void ChangeImage();
    private ChangeImage changeImage;
    
    private void Start()
    { 
        if(_MissionContainer == null)
        {
            _MissionContainer = Resources.LoadAll<MissionUI>("Missions");
            
        }

        m_Zone = GameObject.Find("ZoneImg").GetComponent<Transform>();
        m_Missions = GameObject.Find("MissioniImg").GetComponent<Transform>();
        _LockImg = transform.Find("LockImage").gameObject;

        _MedalCountTxts[0] = GameObject.Find("GoldValue").GetComponent<Text>();
        _MedalCountTxts[1] = GameObject.Find("SilverValue").GetComponent<Text>();
        _MedalCountTxts[2] = GameObject.Find("BronzeValue").GetComponent<Text>();

        _FistImageObj[0] = GameObject.Find("ZoneMedalsBox").gameObject;
        _FistImageObj[1] = GameObject.Find("ZoneCollectableBox").gameObject;

        _SecondImageObj[0] = GameObject.Find("Zone").transform.Find("MissionMedalTimeBox").gameObject;
        _SecondImageObj[1] = GameObject.Find("Zone").transform.Find("MissionCollectableBox").gameObject;
        _SecondImageObj[2] = GameObject.Find("Zone").transform.Find("PlayerRecordBox").gameObject;
        _SecondImageObj[3] = GameObject.Find("Zone").transform.Find("MissionEnvironmentBox").gameObject;

        _MedalTimersTxts[0] = GameObject.Find("Zone").transform.Find("MissionMedalTimeBox").transform.Find("GoldTime").GetComponent<Text>();
        _MedalTimersTxts[1] = GameObject.Find("Zone").transform.Find("MissionMedalTimeBox").transform.Find("SilverTime").GetComponent<Text>();
        _MedalTimersTxts[2] = GameObject.Find("Zone").transform.Find("MissionMedalTimeBox").transform.Find("BronzeTime").GetComponent<Text>();

        _MissionDescription = GameObject.Find("MissionInfos").GetComponent<Text>();


        _RecordMedalBox = GameObject.Find("Zone").transform.Find("PlayerRecordBox").Find("MedalSpace").GetComponent<Image>();
        _MapImage = transform.Find("MapImage").GetComponent<Image>();

        _MedalsSprites[0] = Resources.Load<Sprite>("Sprites/MedalGold");
        _MedalsSprites[1] = Resources.Load<Sprite>("Sprites/MedalSilver");
        _MedalsSprites[2] = Resources.Load<Sprite>("Sprites/MedalBronze");

        _VallesMarinerisMaps = Resources.LoadAll<Sprite>("MapsSprites/Valles");
        _MonsOlympusMaps = Resources.LoadAll<Sprite>("MapsSprites/Olympus");
        _PolonordMaps = Resources.LoadAll<Sprite>("MapsSprites/Polo");
        _CrateriMaps = Resources.LoadAll<Sprite>("MapsSprites/Crateri");


        _RecordTxt = GameObject.Find("Zone").transform.Find("PlayerRecordBox").transform.Find("RecordTime").GetComponent<Text>();
        _CollectableCountTxt = GameObject.Find("CollectableValue").GetComponent<Text>();

        m_Zone.GetChild(0).GetChild(zoneIndex).gameObject.SetActive(true);
        m_Zone.GetChild(1).GetChild(zoneIndex).gameObject.SetActive(false);

        MenuInizialeContainer.instance.zoneFrameHighlight.SetActive(true);
        MenuInizialeContainer.instance.zoneFrameNormal.SetActive(false);

        _LockShake = _LockImg.transform.GetChild(0).transform.DOShakePosition(0.5f, 50f, 10, 50, true, false);

        var _UnlockedAreas = DataContainer.MissionsCompleted();


        if (lockMissions)
        {
            print("Missions locked");

            if (_UnlockedAreas < 3)
            {
                _UnlockedZone = 1;
                print("Everything locked");
            }
            else if (_UnlockedAreas >= 3 && _UnlockedAreas < 7)
            {
                _UnlockedZone = 2;
                print("Second area unlocked");
            }
            else if (_UnlockedAreas >= 7 && _UnlockedAreas < 14)
            {
                _UnlockedZone = 3;
                print("Third area unlocked");
            }
            else if (_UnlockedAreas >= 14)
            {
                _UnlockedZone = 4;
                print("Fourth area unlocked");
            }
        }
        else
        {
            _UnlockedZone = 10;
        }

        changeImage += ChangeZoneImage;
        changeImage += ChangeTextsCount;
        changeImage += ChangeMapImage;

        ChangeTextsCount();

        _MissionDescription.text = TextContainer.MAPS_DESCRIPTION[zoneIndex];
    }

    private void Update()
    {
       #region Navigation
        if (!m_StopInput)
        {
            if (!MenuManager.pauseMenuActive)
            {
                if (Input.GetAxis(InputContainer.HORIZONTAL) < 0)
                {
                    if (zoneIndex > 0 && isFirstImage)
                    {
                        zoneIndex--;
                        changeImage();

                        _MissionDescription.text = TextContainer.MAPS_DESCRIPTION[zoneIndex];

                        if (zoneIndex >= _UnlockedZone)
                        {
                            _LockImg.SetActive(true);
                            invalidZone = true;
                        }
                        else
                        {
                            _LockImg.SetActive(false);
                            invalidZone = false;
                        }

                    }

                    else if (missioniIndex > 0 && !isFirstImage)
                    {
                        missioniIndex--;
                        changeImage();
                    }

                    if (AudioManager.Audio != null)
                    {
                        AudioManager.Audio.PlayMenuSound(MenuSounds.ChangeSelection);
                    }

                }

                if (Input.GetAxis(InputContainer.HORIZONTAL) > 0)
                {
                    if (isFirstImage && zoneIndex < m_Zone.GetChild(0).childCount - 1)
                    {
                        zoneIndex++;
                        changeImage();
                        _MissionDescription.text = TextContainer.MAPS_DESCRIPTION[zoneIndex];

                        if (zoneIndex >= _UnlockedZone)
                        {
                            _LockImg.SetActive(true);
                            invalidZone = true;
                        }
                        else
                        {
                            _LockImg.SetActive(false);
                            invalidZone = false;
                        }
                    }

                    else if (!isFirstImage && missioniIndex < m_Missions.GetChild(0).childCount - 1)
                    {
                        missioniIndex++;
                        changeImage();


                    }

                    if (AudioManager.Audio != null)
                    {
                        AudioManager.Audio.PlayMenuSound(MenuSounds.ChangeSelection);
                    }


                }


                if (Input.GetAxis(InputContainer.VERTICAL) > 0)
                {
                    _MissionDescription.text = TextContainer.MAPS_DESCRIPTION[zoneIndex];

                    changeImage += ChangeVerticalImage;
                    changeImage += ChangeImagesObjects;
                    changeImage += ChangeZoneImage;
                    changeImage += ChangeTextsCount;
                    changeImage -= DebugText;
                    changeImage -= ChangeMissionImage;
                    changeImage();
                    changeImage -= ChangeVerticalImage;
                    changeImage -= ChangeImagesObjects;

                    if (AudioManager.Audio != null)
                    {
                        AudioManager.Audio.PlayMenuSound(MenuSounds.ChangeSelection);
                    }
                }

                if (Input.GetAxis(InputContainer.VERTICAL) < 0)
                {
                    changeImage += ChangeVerticalImage;
                    changeImage += ChangeImagesObjects;
                    changeImage += ChangeMissionImage;
                    changeImage += DebugText;
                    changeImage -= ChangeZoneImage;
                    changeImage -= ChangeTextsCount;
                    changeImage();
                    changeImage -= ChangeVerticalImage;
                    changeImage -= ChangeImagesObjects;

                    if (AudioManager.Audio != null)
                    {
                        AudioManager.Audio.PlayMenuSound(MenuSounds.ChangeSelection);
                    }
                }

            }
        }
        #endregion

       #region ResetAxis
        if (Input.GetAxis(InputContainer.VERTICAL) == 0 && Input.GetAxis(InputContainer.HORIZONTAL) == 0)
        {
            m_StopInput = false;
        }
        else
        {
            m_StopInput = true;
        }
#endregion

    }


    #region ImagesManagment

    private void ChangeVerticalImage()
    {
        if(isFirstImage)
        {
            MenuInizialeContainer.instance.zoneFrameHighlight.SetActive(false);
            MenuInizialeContainer.instance.zoneFrameNormal.SetActive(true);

            MenuInizialeContainer.instance.missioniFrameHighlight.SetActive(true);
            MenuInizialeContainer.instance.missioniFrameNormal.SetActive(false);

            m_Zone.GetChild(0).GetChild(zoneIndex).gameObject.SetActive(false);
            m_Zone.GetChild(1).GetChild(zoneIndex).gameObject.SetActive(true);

            m_Missions.GetChild(0).GetChild(missioniIndex).gameObject.SetActive(true);
            m_Missions.GetChild(1).GetChild(missioniIndex).gameObject.SetActive(false);
        }
        else
        {
            MenuInizialeContainer.instance.missioniFrameHighlight.SetActive(false);
            MenuInizialeContainer.instance.missioniFrameNormal.SetActive(true);

            MenuInizialeContainer.instance.zoneFrameHighlight.SetActive(true);
            MenuInizialeContainer.instance.zoneFrameNormal.SetActive(false);

            m_Missions.GetChild(0).GetChild(missioniIndex).gameObject.SetActive(false);
            m_Missions.GetChild(1).GetChild(missioniIndex).gameObject.SetActive(true);
            

            m_Zone.GetChild(0).GetChild(zoneIndex).gameObject.SetActive(true);
            m_Zone.GetChild(1).GetChild(zoneIndex).gameObject.SetActive(false);
        }

        isFirstImage = !isFirstImage;
    }



    private void ChangeZoneImage()
    {

        m_Zone.GetChild(0).GetChild(zoneIndex).gameObject.SetActive(true);

        if (zoneIndex > 0)
        {
            m_Zone.GetChild(0).GetChild(zoneIndex - 1).gameObject.SetActive(false);
        }

        if (zoneIndex < m_Zone.GetChild(0).childCount - 1)
        {
            m_Zone.GetChild(0).GetChild(zoneIndex + 1).gameObject.SetActive(false);
        }
    }

    private void ChangeMissionImage()
    {
        m_Missions.GetChild(0).GetChild(missioniIndex).gameObject.SetActive(true);

        if (missioniIndex > 0)
        {
            m_Missions.GetChild(0).GetChild(missioniIndex - 1).gameObject.SetActive(false);
            
        }
        if (missioniIndex < m_Missions.GetChild(0).childCount - 1)
        {
            m_Missions.GetChild(0).GetChild(missioniIndex + 1).gameObject.SetActive(false);
            
        }
    }
#endregion

    public static void ShakeLockIcon()
    {
        if (!_LockShake.IsPlaying())
        {

            _LockShake = _LockImg.transform.GetChild(0).transform.DOShakePosition(0.5f, 50f, 10, 50, true, false); 
        }

    }

    private void DebugText()
    {

        var _Bronze = TimeConversion.RacingTime(_MissionContainer[missioniIndex].Bronze[zoneIndex]);
        var _Silver = TimeConversion.RacingTime(_MissionContainer[missioniIndex].Silver[zoneIndex]);
        var _Gold = TimeConversion.RacingTime(_MissionContainer[missioniIndex].Gold[zoneIndex]);

        var _QuestType = _MissionContainer[missioniIndex].CurrentQuest[zoneIndex].ToString();

        var _Record = DataContainer.GetUserSaveInfoString(m_Zone.GetChild(0).GetChild(zoneIndex).gameObject.name
            + m_Missions.GetChild(0).GetChild(missioniIndex).gameObject.name).Split('/');

        //MenuInizialeContainer.instance.missionInfo.text = " Gold: " + _Gold + "\n Silver: " + _Silver + "\n Bronze: "
        //                                                   + _Bronze + "\n Quest: " + _QuestType;


        print(_QuestType);

        switch (_QuestType)
        {
            case "SpecialDelivery":
                _MissionDescription.text = TextContainer.MISSIONS_DESCRIPTIONS[0];
                break;
            case "CollectInfos":
                _MissionDescription.text = TextContainer.MISSIONS_DESCRIPTIONS[1];
                break;
            case "FillOfEnergy":
                _MissionDescription.text = TextContainer.MISSIONS_DESCRIPTIONS[2];
                break;
            case "FreeRoaming":
                _MissionDescription.text = TextContainer.MISSIONS_DESCRIPTIONS[3];
                break;

        }


        _MedalTimersTxts[0].text = _Gold;
        _MedalTimersTxts[1].text = _Silver;
        _MedalTimersTxts[2].text = _Bronze;

        if (_Record[0] != "00 : 00 : 00 ")
        {
            _SecondImageObj[2].SetActive(true);

            _RecordTxt.text = _Record[0];

            switch (_Record[1])
            {
                case "Gold":
                    _RecordMedalBox.sprite = _MedalsSprites[0];
                    break;
                case "Silver":
                    _RecordMedalBox.sprite = _MedalsSprites[1];
                    break;
                case "Bronze":
                    _RecordMedalBox.sprite = _MedalsSprites[2];
                    break;
            }
            
        }
        else
        {
            _SecondImageObj[2].SetActive(false);
        }

        if(_QuestType == "FreeRoaming")
        {
             print("Second obj disable");
            _SecondImageObj[0].SetActive(false);
            _SecondImageObj[1].SetActive(false);
            _SecondImageObj[2].SetActive(false);
        }
        else if( _QuestType != "FreeRoaming" && !_SecondImageObj[0].activeInHierarchy)
        {
            print("Reactive second obj");
            _SecondImageObj[0].SetActive(true);
            _SecondImageObj[1].SetActive(true);
            _SecondImageObj[2].SetActive(true);
        }

        //print(m_Zone.GetChild(0).GetChild(zoneIndex).gameObject.name
        //    + m_Missions.GetChild(0).GetChild(missioniIndex).gameObject.name);
       

        //print(_Record[0]);
        //MenuInizialeContainer.instance.userInfo.text = " Record: " + _Record[0] + "\n Medal: " +
        //                                               _Record[1] + " \n Collectable found: " + _Record[2] + 
        //                                               "\n MissionCompleted: " + _Record[3];
    }

    private void ChangeTextsCount()
    {
        _MedalCountTxts[0].text = goldNumber[zoneIndex];
        _MedalCountTxts[1].text = silverNumber[zoneIndex];
        _MedalCountTxts[2].text = bronzeNumber[zoneIndex];

        _CollectableCountTxt.text = collectableNumber[zoneIndex] + "/6";
    }

    private void ChangeImagesObjects()
    {
        if(isFirstImage)
        {
            _FistImageObj[0].SetActive(true);
            _FistImageObj[1].SetActive(true);


            for (int i = 0; i < _SecondImageObj.Length; i++)
            {
                _SecondImageObj[i].SetActive(false);
            }
            
            
        }
        else
        {
            _FistImageObj[0].SetActive(false);
            _FistImageObj[1].SetActive(false);

            for (int i = 0; i < _SecondImageObj.Length; i++)
            {
                if (i != 2)
                {
                    _SecondImageObj[i].SetActive(true);
                }
            }
            
            

        }
    }

    private void ChangeMapImage()
    {
        switch (zoneIndex)
        {
            case 0:
                _MapImage.sprite = _VallesMarinerisMaps[missioniIndex];
                break;
            case 1:
                _MapImage.sprite = _MonsOlympusMaps[missioniIndex];
                break;
            case 2:
                _MapImage.sprite = _PolonordMaps[missioniIndex];
                break;
            case 3:
                _MapImage.sprite = _CrateriMaps[missioniIndex];
                break;
        }
    }

    private void OnDisable()
    {
        MenuInizialeContainer.instance.zoneName = m_Zone.GetChild(0).GetChild(zoneIndex).name;
        MenuInizialeContainer.instance.missionName = m_Missions.GetChild(0).GetChild(missioniIndex).name;
    }
}


