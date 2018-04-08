using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class HUD : MonoBehaviour
{
    public static HUD instance;

    public string ChangeMedalTime
    {
        get
        {
            return _Texts[(int)TextsName.MedalTime].text;
        }
        set
        {
            _Texts[(int)TextsName.MedalTime].text = value;

            if (_Images[(int)ImagesName.Medal].sprite == null)
            {
                _Images[(int)ImagesName.Medal].sprite = _GoldMedal;

                if (!_GameObjects[(int)GameObjectsName.MedalBox].activeInHierarchy)
                    _GameObjects[(int)GameObjectsName.MedalBox].SetActive(true);
            }
            else if (_Images[(int)ImagesName.Medal].sprite == _GoldMedal)
            {
                _Images[(int)ImagesName.Medal].sprite = _SilverMedal;

                if (!_GameObjects[(int)GameObjectsName.MedalBox].activeInHierarchy)
                    _GameObjects[(int)GameObjectsName.MedalBox].SetActive(true);
            }
            else if (_Images[(int)ImagesName.Medal].sprite == _SilverMedal)
            {
                _Images[(int)ImagesName.Medal].sprite = _BronzeMedal;

                if(!_GameObjects[(int)GameObjectsName.MedalBox].activeInHierarchy)
                    _GameObjects[(int)GameObjectsName.MedalBox].SetActive(true);
            }
        }
    }

    public bool WindEffect
    {
        get
        {
            return _IsWindCoroutineRunning;
        }
        set
        {
            _IsWindCoroutineRunning = value;

            if (value)
            {
                StartCoroutine(_WindCoroutine = WindValue());
            }

            else
            {
                StopCoroutine(_WindCoroutine);
                _Texts[(int)TextsName.WindSpeed].text = "0";
                _Images[(int)ImagesName.WindDirection].transform.eulerAngles = Vector3.zero;
            }
        }
    }

    private bool _IsWindCoroutineRunning = false;
    private bool _MissionCompleted = false;

    private Canvas _Canvas;
    private Text[] _Texts = new Text[13];
    private GameObject[] _GameObjects = new GameObject[7];
    private Image[] _Images = new Image[17];

    private enum TextsName {Objective, Timer, MedalTime, RecordTime, Speed, Fuel, Temperature, Accessory1, Accessory2, WindSpeed, QuestResult, TimeResult, NewRecordResult};
    private enum GameObjectsName {ObjectiveBox, PauseBox, MedalBox, RecordBox, CollectableBox, BoxSlot1, BoxSlot2 };
    private enum ImagesName {SpeedBar, FuelBar, TemperatureBar, Slot1, Slot2, Resume, Restart, Garage, Confirm, Yes, No, WindDirection, Medal, ResultMedal, RestartResult, GarageResult, EngineValue };

    private Sprite _LightBlue;
    private Sprite _DarkBlue;

    private Sprite _GoldMedal;
    private Sprite _SilverMedal;
    private Sprite _BronzeMedal;

    private float _NormalizedTemperature;
    private bool _Slot1Cooldown = false;
    private bool _Slot2Cooldown = false;
    private bool _IsPauseMenuOpen = false;

    private IEnumerator _WindCoroutine;
    private IEnumerator _LerpCoroutineIncrese;
    private IEnumerator _LerpCoroutineStart;

    
    private float _StartingXScale;
    private float _IncreasedXScale;
    [SerializeField] private float _XScaleMultiplier = .3f;
    [SerializeField] private Color _NormalEngineBonus, _OptimalEngineBonus, _BadEngineBonus;

    private void Start()
    {
        instance = this;
        
        _Canvas = GetComponent<Canvas>();
        _Canvas.worldCamera = Camera.main;

        _GameObjects[(int)GameObjectsName.ObjectiveBox] = GetUIComponent<Image>("BoxObjective").gameObject;
        _GameObjects[(int)GameObjectsName.PauseBox] = transform.Find("PauseBox").gameObject;
        _GameObjects[(int)GameObjectsName.MedalBox] = GetUIComponent<Image>("BoxMedalTime").gameObject;
        _GameObjects[(int)GameObjectsName.RecordBox] = GetUIComponent<Image>("BoxRecord").gameObject;
        _GameObjects[(int)GameObjectsName.CollectableBox] = GetUIComponent<Image>("BoxCollectable").gameObject;
        _GameObjects[(int)GameObjectsName.BoxSlot1] = transform.Find("BoxDx").GetChild(2).gameObject;
        _GameObjects[(int)GameObjectsName.BoxSlot2] = transform.Find("BoxDx").GetChild(3).gameObject;


        _Images[(int)ImagesName.SpeedBar] = GetUIComponent<Image>("BoxDx", 0);
        _Images[(int)ImagesName.FuelBar] = GetUIComponent<Image>("BoxDx", 1);
        _Images[(int)ImagesName.TemperatureBar] = GetUIComponent<Image>("BoxSx", 1);
        _Images[(int)ImagesName.Slot1] = transform.Find("BoxDx").GetChild(2).GetChild(0).GetComponent<Image>();
        _Images[(int)ImagesName.Slot2] = transform.Find("BoxDx").GetChild(3).GetChild(0).GetComponent<Image>();
        _Images[(int)ImagesName.Resume] = GetUIComponent<Image>("PauseBox", 0);
        _Images[(int)ImagesName.Restart] = GetUIComponent<Image>("PauseBox", 1);
        _Images[(int)ImagesName.Garage] = GetUIComponent<Image>("PauseBox", 2);
        _Images[(int)ImagesName.Confirm] = GetUIComponent<Image>("PauseBox", 3);
        _Images[(int)ImagesName.Yes] = transform.Find("PauseBox").GetChild(3).GetChild(0).GetComponent<Image>();
        _Images[(int)ImagesName.No] = transform.Find("PauseBox").GetChild(3).GetChild(1).GetComponent<Image>();
        _Images[(int)ImagesName.WindDirection] = GetUIComponent<Image>("BoxSx", 6);
        _Images[(int)ImagesName.Medal] = transform.Find("BoxMedalTime").GetChild(1).GetChild(0).GetComponent<Image>();
        _Images[(int)ImagesName.ResultMedal] = GetUIComponent<Image>("Result", 5);
        _Images[(int)ImagesName.RestartResult] = GetUIComponent<Image>("Result", 1);
        _Images[(int)ImagesName.GarageResult] = GetUIComponent<Image>("Result", 2);
        _Images[(int)ImagesName.EngineValue] = GetUIComponent<Image>("BoxDx", 9);

        _Texts[(int)TextsName.Objective] = GetUIComponent<Text>("BoxObjective", 0);
        _Texts[(int)TextsName.Timer] = GetUIComponent<Text>("BoxTime", 0);
        _Texts[(int)TextsName.MedalTime] = GetUIComponent<Text>("BoxMedalTime", 0);
        _Texts[(int)TextsName.RecordTime] = GetUIComponent<Text>("BoxRecord", 1);
        _Texts[(int)TextsName.Speed] = GetUIComponent<Text>("BoxDx", 4);
        _Texts[(int)TextsName.Fuel] = GetUIComponent<Text>("BoxDx", 6);
        _Texts[(int)TextsName.Temperature] = GetUIComponent<Text>("BoxSx", 2);
        _Texts[(int)TextsName.Accessory1] = transform.Find("BoxDx").GetChild(2).GetChild(1).GetComponent<Text>();
        _Texts[(int)TextsName.Accessory2] = transform.Find("BoxDx").GetChild(3).GetChild(1).GetComponent<Text>();
        _Texts[(int)TextsName.WindSpeed] = GetUIComponent<Text>("BoxSx", 7);
        _Texts[(int)TextsName.QuestResult] = GetUIComponent<Text>("Result", 0);
        _Texts[(int)TextsName.TimeResult] = transform.Find("Result").GetChild(3).GetChild(0).GetComponent<Text>();
        _Texts[(int)TextsName.NewRecordResult] = transform.Find("Result").GetChild(4).GetChild(0).GetComponent<Text>();

        _DarkBlue = _Images[(int)ImagesName.Resume].sprite;
        _LightBlue = _Images[(int)ImagesName.Restart].sprite;

        _Images[(int)ImagesName.RestartResult].sprite = _DarkBlue;

        _StartingXScale = _Images[(int)ImagesName.Restart].transform.localScale.x;
        _IncreasedXScale = _Images[(int)ImagesName.Restart].transform.localScale.x + _XScaleMultiplier;

        _GoldMedal = Resources.Load<Sprite>("Sprites/MedalGold");
        _SilverMedal = Resources.Load<Sprite>("Sprites/MedalSilver");
        _BronzeMedal = Resources.Load<Sprite>("Sprites/MedalBronze");

        StartCoroutine(SetSpeedValue());
        StartCoroutine(FuelValue());
        StartCoroutine(TimerValue());
        StartCoroutine(TemperatureValue());
        StartCoroutine(SetEngineValue());

    }


    private IEnumerator ScaleMenu(Transform t, bool increase)
    {

        while(true)
        {
            if (increase)
            {
                t.localScale = new Vector3(t.localScale.x + 0.1f, t.localScale.y, t.localScale.z);
                if (t.localScale.x >= _IncreasedXScale)
                {
                    
                    t.localScale = new Vector3(_IncreasedXScale, t.localScale.y, t.localScale.z);
                    break;
                }
            }
            else
            {
                t.localScale = new Vector3(t.localScale.x - 0.1f, t.localScale.y, t.localScale.z);
                if (t.localScale.x <= _StartingXScale)
                {
                   
                    t.localScale = new Vector3(_StartingXScale, t.localScale.y, t.localScale.z);
                    break;
                }
            }
            yield return null;
        }
       
    }

    public IEnumerator ShowObjectiveBox()
    {
        if (_GameObjects[(int)GameObjectsName.ObjectiveBox].activeInHierarchy)
        {
            yield return new WaitForSeconds(5);
            _GameObjects[(int)GameObjectsName.ObjectiveBox].SetActive(false);
        }
        else
        {
            _GameObjects[(int)GameObjectsName.ObjectiveBox].SetActive(true);
            yield return new WaitForSeconds(5);
            _GameObjects[(int)GameObjectsName.ObjectiveBox].SetActive(false);
        }
    }

    public IEnumerator ShowObjectiveBox(string str)
    {
        while(_Texts[0] == null)
        {
            print("loading");
            yield return null;
        }

        ObjectiveBoxText(str);

        if (_GameObjects[(int)GameObjectsName.ObjectiveBox].activeInHierarchy)
        {
            yield return new WaitForSeconds(5);
            _GameObjects[(int)GameObjectsName.ObjectiveBox].SetActive(false);
        }
        else
        {
            _GameObjects[(int)GameObjectsName.ObjectiveBox].SetActive(true);
            yield return new WaitForSeconds(5);
            _GameObjects[(int)GameObjectsName.ObjectiveBox].SetActive(false);
        }

    }

    public IEnumerator ShowCollectableBox()
    {
        _GameObjects[(int)GameObjectsName.CollectableBox].SetActive(true);

        yield return new WaitForSeconds(2.0f);

        _GameObjects[(int)GameObjectsName.CollectableBox].SetActive(false);
    }

    public void ObjectiveBoxText(string str)
    {
        _Texts[(int)TextsName.Objective].text = str;
    }

    public IEnumerator SlotBarDecrease(int slot, float duration)
    {
        var _DecreaseRate = 1 / duration;

        if(slot == 1)
        {
            var _Slot1 = _Images[(int)ImagesName.Slot1];

            while (!_Slot1Cooldown)
            {
                if (_Slot1.fillAmount <= 0)
                    _Slot1Cooldown = true;

                _Slot1.fillAmount -= _DecreaseRate * Time.deltaTime;
                yield return null;
            }
        }

        else
        {
            var _Slot2 = _Images[(int)ImagesName.Slot2];

            while (!_Slot2Cooldown)
            {
                if (_Slot2.fillAmount <= 0)
                    _Slot2Cooldown = true;

                _Slot2.fillAmount -= _DecreaseRate * Time.deltaTime;
                yield return null;
            }
        }
    }

    public IEnumerator SlotBarCooldown(int slot, float cooldown)
    {
        print("Cooldown " + cooldown);
        var _IncreaseRate = 1 / cooldown;

        if (slot == 1)
        {
            _Slot1Cooldown = true;
            var _Slot1 = _Images[(int)ImagesName.Slot1];
            _Slot1.fillAmount = 0;

            while (true)
            {
                if (_Slot1.fillAmount >= 1)
                    break;

                _Slot1.fillAmount += _IncreaseRate * Time.deltaTime;
                yield return null;
            }
            _Slot1.fillAmount = 1;
            _Slot1Cooldown = false;
        }
        else
        {
            _Slot2Cooldown = true;
            var _Slot2 = _Images[(int)ImagesName.Slot2];
            _Slot2.fillAmount = 0;

            while(true)
            {
                if (_Slot2.fillAmount >= 1)
                    break;

                _Slot2.fillAmount += _IncreaseRate * Time.deltaTime;
                yield return null;
            }
            _Slot2.fillAmount = 1;
            _Slot2Cooldown = false;
        }


    }

    public IEnumerator MedalWarning()
    {
        while(true)
        {
            if (_GameObjects[(int)GameObjectsName.MedalBox].activeInHierarchy)
                _GameObjects[(int)GameObjectsName.MedalBox].SetActive(false);
            else
                _GameObjects[(int)GameObjectsName.MedalBox].SetActive(true);

            yield return new WaitForSeconds(0.2f);
        }
    }

    public void SetAccessoryName(int slot, string name)
    {
        if (slot == 1)
        {
            _GameObjects[(int)GameObjectsName.BoxSlot1].transform.Find(name).gameObject.SetActive(true);
            //if (name.Contains("_"))
            //{
            //    var _NewName = name.Split('_');
            //    _Texts[(int)TextsName.Accessory1].text = _NewName[0] + " " + _NewName[1];
                
            //}
            //else
            //{
            //    _Texts[(int)TextsName.Accessory1].text = name;
            //}
           

        }
        else
        {
            _GameObjects[(int)GameObjectsName.BoxSlot2].transform.Find(name).gameObject.SetActive(true);
            //if (name.Contains("_"))
            //{
            //    var _NewName = name.Split('_');
            //    _Texts[(int)TextsName.Accessory2].text = _NewName[0] + " " + _NewName[1];
            //}
            //else
            //{
            //    _Texts[(int)TextsName.Accessory2].text = name;
            //}
            
        }
            
    }

    public void PauseMenuInteraction()
    {
        if (!_MissionCompleted)
        {
            if (!_GameObjects[(int)GameObjectsName.PauseBox].activeInHierarchy)
            {
                Time.timeScale = 0;
                _GameObjects[(int)GameObjectsName.PauseBox].SetActive(true);
                _IsPauseMenuOpen = true;
                StartCoroutine(PauseMenu());
            }
            else
            {
                Time.timeScale = 1;
                _GameObjects[(int)GameObjectsName.PauseBox].SetActive(false);
                _Images[(int)ImagesName.Confirm].gameObject.SetActive(false);
                _IsPauseMenuOpen = false;
                StopCoroutine(PauseMenu());
            }
        }
    }

    public void ShowQuestResult(bool missionCompleted)
    {

        _MissionCompleted = true;

        var _MissionCurrentTime = Quest.currentQuest.currentTime;

        _Images[(int)ImagesName.RestartResult].gameObject.SetActive(true);
        _Images[(int)ImagesName.GarageResult].gameObject.SetActive(true);

        _Texts[(int)TextsName.QuestResult].gameObject.SetActive(true);

        if (missionCompleted)
        {
            var _MissionData = MenuInizialeContainer.instance.zoneName + MenuInizialeContainer.instance.zoneName;

            _Texts[(int)TextsName.TimeResult].text = TimeConversion.RacingTime(_MissionCurrentTime);
            _Texts[(int)TextsName.TimeResult].gameObject.SetActive(true);

            _Texts[(int)TextsName.QuestResult].text = "Mission Completed";
            _Texts[(int)TextsName.QuestResult].color = Color.green;

            _Images[(int)ImagesName.ResultMedal].sprite = _Images[(int)ImagesName.Medal].sprite;
            _Images[(int)ImagesName.ResultMedal].gameObject.SetActive(true);

            if (DataContainer.MissionAvabile(_MissionData) && DataContainer.NewRecordCheck(_MissionData, _MissionCurrentTime))
            {
                if (DataContainer.NewRecordCheck(_MissionData, _MissionCurrentTime))
                {
                    StartCoroutine(ResultRecordFlash());
                }
            }
            else
            {
                StartCoroutine(ResultRecordFlash());
            }

        }
        else
        {
            _Texts[(int)TextsName.QuestResult].text = "Mission Failed";
            _Texts[(int)TextsName.QuestResult].color = Color.red;
        }

        Time.timeScale = 0;
        StartCoroutine(ResultMenu());


    }

    public void CheckMissionName(string name)
    {

        if (name == "FreeRoaming")
        {
            _GameObjects[(int)GameObjectsName.ObjectiveBox].SetActive(false);
            _GameObjects[(int)GameObjectsName.MedalBox].SetActive(false);
        }
        else
        {
            SetMissionRecord();
        }
    }

    private IEnumerator ResultRecordFlash()
    {
        _Texts[(int)TextsName.NewRecordResult].text = TimeConversion.RacingTime(Quest.currentQuest.currentTime);

        while (true)
        {
            if (_Texts[(int)TextsName.NewRecordResult].gameObject.activeInHierarchy)
            {
                _Texts[(int)TextsName.NewRecordResult].gameObject.SetActive(false);
            }
            else
            {
                _Texts[(int)TextsName.NewRecordResult].gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator ResultMenu()
    {
        int _Index = 14;
        bool _InputAvabile = true;
        Time.timeScale = 0;
        StartCoroutine(_LerpCoroutineIncrese = ScaleMenu(_Images[_Index].transform, true));

        while (true)
        {
            if (Input.GetAxisRaw(InputContainer.HORIZONTAL) > 0 && _InputAvabile && _Index < 15)
            {
                _Images[_Index].sprite = _LightBlue;

                StartCoroutine(_LerpCoroutineStart = ScaleMenu(_Images[_Index].transform, false));
                _Index++;
                StartCoroutine(_LerpCoroutineIncrese = ScaleMenu(_Images[_Index].transform, true));

                _Images[_Index].sprite = _DarkBlue;

            }
            else if (Input.GetAxisRaw(InputContainer.HORIZONTAL) < 0 && _InputAvabile && _Index > 14)
            {
                _Images[_Index].sprite = _LightBlue;

                StartCoroutine(_LerpCoroutineStart = ScaleMenu(_Images[_Index].transform, false));
                _Index--;
                StartCoroutine(_LerpCoroutineIncrese = ScaleMenu(_Images[_Index].transform, true));

                _Images[_Index].sprite = _DarkBlue;
            }

            if (Input.GetButtonDown(InputContainer.SUBMIT))
            {
                if (_Index == 14)
                {
                    Time.timeScale = 1;
                    StartCoroutine(SceneLoader.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name));
                }
                else
                {
                    Time.timeScale = 1;
                    StartCoroutine(SceneLoader.LoadSceneAsync(ScenesContainer.MENU_SELEZIONE));
                }
            }

            if (!_InputAvabile)
            {
                _InputAvabile = (Input.GetAxisRaw(InputContainer.VERTICAL) == 0 && Input.GetAxisRaw(InputContainer.HORIZONTAL) == 0);
            }

            yield return null;
        }
    }

    private IEnumerator PauseMenu()
    {
        if (!_MissionCompleted)
        {
            int _IndexVertical = 5;
            int _IndexHorizontal = 9;
            bool _InputAvabile = true;
            bool _FistMenuTrigger = true;
            bool _SecondMenuTrigger = true;

            //////////////////RESET ICONS/////////////
            _Images[(int)ImagesName.Resume].sprite = _DarkBlue;
            _Images[(int)ImagesName.Restart].sprite = _LightBlue;
            _Images[(int)ImagesName.Garage].sprite = _LightBlue;
            _Images[(int)ImagesName.Yes].sprite = _DarkBlue;
            _Images[(int)ImagesName.No].sprite = _LightBlue;

            _Images[(int)ImagesName.Resume].transform.localScale = new Vector3(_StartingXScale, 1, 1);
            _Images[(int)ImagesName.Restart].transform.localScale = new Vector3(_StartingXScale, 1, 1);
            _Images[(int)ImagesName.Garage].transform.localScale = new Vector3(_StartingXScale, 1, 1);
            _Images[(int)ImagesName.Yes].transform.localScale = new Vector3(_StartingXScale, 1, 1);
            _Images[(int)ImagesName.No].transform.localScale = new Vector3(_StartingXScale, 1, 1);

            while (_IsPauseMenuOpen)
            {
                /////////////////FIRST STATE//////////////////////
                if (!_Images[(int)ImagesName.Confirm].gameObject.activeInHierarchy)
                {
                    if (_FistMenuTrigger)
                    {
                        _FistMenuTrigger = false;
                        StartCoroutine(_LerpCoroutineIncrese = ScaleMenu(_Images[_IndexVertical].transform, true));

                    }

                    if (Input.GetAxisRaw(InputContainer.VERTICAL) > 0 && _InputAvabile && _IndexVertical > 5)
                    {

                        _InputAvabile = false;
                        _Images[_IndexVertical].sprite = _LightBlue;

                        StartCoroutine(_LerpCoroutineStart = ScaleMenu(_Images[_IndexVertical].transform, false));
                        _IndexVertical--;
                        StartCoroutine(_LerpCoroutineIncrese = ScaleMenu(_Images[_IndexVertical].transform, true));

                        _Images[_IndexVertical].sprite = _DarkBlue;

                        AudioManager.Audio.PlayMenuSound(MenuSounds.ChangeSelection);
                    }
                    else if (Input.GetAxisRaw(InputContainer.VERTICAL) < 0 && _InputAvabile && _IndexVertical < 7)
                    {

                        _InputAvabile = false;
                        _Images[_IndexVertical].sprite = _LightBlue;

                        StartCoroutine(_LerpCoroutineStart = ScaleMenu(_Images[_IndexVertical].transform, false));
                        _IndexVertical++;
                        StartCoroutine(_LerpCoroutineIncrese = ScaleMenu(_Images[_IndexVertical].transform, true));

                        _Images[_IndexVertical].sprite = _DarkBlue;

                        AudioManager.Audio.PlayMenuSound(MenuSounds.ChangeSelection);
                    }

                    if (Input.GetButtonDown(InputContainer.SUBMIT) && !_Images[(int)ImagesName.Confirm].gameObject.activeInHierarchy)
                    {
                        if (_IndexVertical == 6 || _IndexVertical == 7)
                        {
                            _Images[(int)ImagesName.Confirm].gameObject.SetActive(true);
                            _Images[(int)ImagesName.Yes].transform.localScale = new Vector3(_StartingXScale, 1, 1);
                            _Images[(int)ImagesName.No].transform.localScale = new Vector3(_StartingXScale, 1, 1);
                            
                            yield return null;
                        }
                        else
                        {
                            PauseMenuInteraction();
                        }
                        AudioManager.Audio.PlayMenuSound(MenuSounds.ConfirmSelection);
                    }


                }

                ///////////////////////////SECOND STATE///////////////////////
                if (_Images[(int)ImagesName.Confirm].gameObject.activeInHierarchy)
                {
                    if (_SecondMenuTrigger)
                    {
                        _SecondMenuTrigger = false;
                        StartCoroutine(_LerpCoroutineIncrese = ScaleMenu(_Images[(int)ImagesName.Yes].transform, true));
                    }

                    if (Input.GetAxisRaw(InputContainer.HORIZONTAL) > 0 && _InputAvabile && _IndexHorizontal < 10)
                    {
                        _Images[_IndexHorizontal].sprite = _LightBlue;

                        StartCoroutine(_LerpCoroutineStart = ScaleMenu(_Images[_IndexHorizontal].transform, false));
                        _IndexHorizontal++;
                        StartCoroutine(_LerpCoroutineIncrese = ScaleMenu(_Images[_IndexHorizontal].transform, true));

                        _Images[_IndexHorizontal].sprite = _DarkBlue;

                    }
                    else if (Input.GetAxisRaw(InputContainer.HORIZONTAL) < 0 && _InputAvabile && _IndexHorizontal > 9)
                    {
                        _Images[_IndexHorizontal].sprite = _LightBlue;

                        StartCoroutine(_LerpCoroutineStart = ScaleMenu(_Images[_IndexHorizontal].transform, false));
                        _IndexHorizontal--;
                        StartCoroutine(_LerpCoroutineIncrese = ScaleMenu(_Images[_IndexHorizontal].transform, true));

                        _Images[_IndexHorizontal].sprite = _DarkBlue;
                    }

                    if (Input.GetButtonDown(InputContainer.SUBMIT))
                    {
                        if (_IndexVertical == 6 && _IndexHorizontal == 9)
                        {
                            Time.timeScale = 1;
                            StartCoroutine(SceneLoader.LoadSceneAsync(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name));
                        }
                        else if (_IndexVertical == 7 && _IndexHorizontal == 9)
                        {
                            Time.timeScale = 1;
                            StartCoroutine(SceneLoader.LoadSceneAsync(ScenesContainer.MENU_SELEZIONE));
                        }
                        else if (_IndexHorizontal == 10)
                        {
                            _Images[(int)ImagesName.Confirm].gameObject.SetActive(false);
                            _Images[(int)ImagesName.Yes].sprite = _DarkBlue;
                            _Images[(int)ImagesName.No].sprite = _LightBlue;
                            _SecondMenuTrigger = true;
                            _IndexHorizontal = 9;
                        }


                    }

                    if (Input.GetButtonDown(InputContainer.ACCESSORYRIGHT))
                    {
                        _Images[(int)ImagesName.Confirm].gameObject.SetActive(false);
                        _Images[(int)ImagesName.Yes].sprite = _DarkBlue;
                        _Images[(int)ImagesName.No].sprite = _LightBlue;
                        _SecondMenuTrigger = true;
                        _IndexHorizontal = 9;
                        AudioManager.Audio.PlayMenuSound(MenuSounds.MenuBack);
                    }


                }

                if (!_InputAvabile)
                {
                    _InputAvabile = (Input.GetAxisRaw(InputContainer.VERTICAL) == 0 && Input.GetAxisRaw(InputContainer.HORIZONTAL) == 0);
                }
                yield return null;
            }
        }
    }

    private IEnumerator SetSpeedValue()
    {
        while (true)
        {
            _Texts[(int)TextsName.Speed].text = Mathf.Abs(NewVehicleController.vehicleController.currentSpeed).ToString();
            //normalized = (x - min(x)) / (max(x) - min(x))
            _Images[(int)ImagesName.SpeedBar].fillAmount = Mathf.Abs(NewVehicleController.vehicleController.currentSpeed / (NewVehicleController.vehicleController.maxSpeed));
            yield return new WaitForSeconds(0.2f);
        }
    }

    private IEnumerator TimerValue()
    {
        while (Quest.currentQuest == null)
        {
            yield return null;
        }

        while (true)
        {
            _Texts[(int)TextsName.Timer].text = TimeConversion.RacingTime(Quest.currentQuest.currentTime);
            yield return null;
        }
    }

    private IEnumerator FuelValue()
    {
        while (true)
        {
            _Texts[(int)TextsName.Fuel].text = Mathf.Clamp((int)NewVehicleController.vehicleController.currentFuel, 0, 100) + "%";
            _Images[(int)ImagesName.FuelBar].fillAmount =  NewVehicleController.vehicleController.currentFuel / 100;
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator TemperatureValue()
    {
        while (true)
        {
            _Texts[(int)TextsName.Temperature].text = Mathf.Clamp(NewVehicleController.vehicleController.currentTemperature, -140, 20) + " C°";
            _NormalizedTemperature = ((NewVehicleController.vehicleController.currentTemperature - Temperature.MIN_TEMPERATURE) / (Temperature.MAX_TEMPERATURE - Temperature.MIN_TEMPERATURE));
            _Images[(int)ImagesName.TemperatureBar].fillAmount = Mathf.Abs(_NormalizedTemperature);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator WindValue()
    {
        while (true)
        {
            _Texts[(int)TextsName.WindSpeed].text = NewVehicleController.vehicleController.currentWindSpeed.ToString();
            Quaternion rot = Quaternion.LookRotation(NewVehicleController.vehicleController.currentWindDirection);
            _Images[(int)ImagesName.WindDirection].transform.rotation = Quaternion.Euler(new Vector3(0,0, Camera.main.transform.rotation.eulerAngles.y - rot.eulerAngles.y));
            yield return null;
        }
    }

    private IEnumerator SetEngineValue()
    {
        int _LocalBonusValue = 0;

        while (true)
        {
            int _VehicleCurrentValue = NewVehicleController.vehicleController.currentEngineBonus;

            if (_LocalBonusValue != _VehicleCurrentValue)
            {
                _LocalBonusValue = _VehicleCurrentValue;

                switch (_VehicleCurrentValue)
                {
                    case -1:
                        _Images[(int)ImagesName.EngineValue].color = _BadEngineBonus;
                        break;
                    case 0:
                        _Images[(int)ImagesName.EngineValue].color = _NormalEngineBonus;
                        break;
                    case 1:
                        _Images[(int)ImagesName.EngineValue].color = _OptimalEngineBonus;
                        break;
                }
            }

            yield return new WaitForSeconds(0.5f);

        }
    }

    private void SetMissionRecord()
    {

        var str = DataContainer.GetUserSaveInfoString(MenuInizialeContainer.instance.zoneName + MenuInizialeContainer.instance.missionName).Split('/');

        if (str[0] != "00 : 00 : 00 ")
        {
            _Texts[(int)TextsName.RecordTime].text = str[0];
            _GameObjects[(int)GameObjectsName.RecordBox].SetActive(true);
        }

       
    }

    private T GetUIComponent<T>(string name, int child)
    {
       return transform.Find(name).GetChild(child).GetComponent<T>();
    }

    private T GetUIComponent<T>(string name)
    {
        return transform.Find(name).GetComponent<T>();
    }

}
