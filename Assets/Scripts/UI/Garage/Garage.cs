using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class Garage : MonoBehaviour
{
    public static Garage instance;

	[Header("FuckingTricicloCinemachine")]
	public CinemachineVirtualCamera TricicloLoom;
	public CinemachineVirtualCamera TricicloEngine;
	public CinemachineVirtualCamera TricicloWheels;
	public CinemachineVirtualCamera TricicloSkin;
	public CinemachineVirtualCamera TricicloAcc1;
	public CinemachineVirtualCamera TricicloAcc2;

	[Header("FuckingClassicaCinemachine")]
	public CinemachineVirtualCamera ClassicaLoom;
	public CinemachineVirtualCamera ClassicaEngine;
	public CinemachineVirtualCamera ClassicaWheels;
	public CinemachineVirtualCamera ClassicaSkin;
	public CinemachineVirtualCamera ClassicaAcc1;
	public CinemachineVirtualCamera ClassicaAcc2;

	[Header("FuckingPickUpCinemachine")]
	public CinemachineVirtualCamera PickUpLoom;
	public CinemachineVirtualCamera PickUpEngine;
	public CinemachineVirtualCamera PickUpWheels;
	public CinemachineVirtualCamera PickUpSkin;
	public CinemachineVirtualCamera PickUpAcc1;
	public CinemachineVirtualCamera PickUpAcc2;


	
    [Header("Interface List")]
    public Transform[] verticalList;

    [Header("Indexs")]
    public int loomIndex = 0;
    public int engineIndex = 0;
    public int wheelIndex = 0;
    public int skinIndex = 0;

    public int slot1Index = 0;
    public int slot2Index = 0;

    public string slot1;
    public string slot2;
    public string slot3;
    public string slot4;

    [Header("Vehicles")]
    public Transform[] engines;
    public Transform[] wheels;
    public Transform[] skins;
    public Transform[] slot1Tr;
    public Transform[] slot2Tr;
    public Transform[] slot3Tr;
    public Transform[] slot4Tr;


    #region Private Components
    private Transform m_WarningIcon;
    #endregion

    #region Private Variables
    private int m_MaxLoom = 0;
    private int m_MaxEngine = 0;
    private int m_MaxWheel = 0;
    private int m_MaxSkin = 0;
    private int m_MaxSlot1 = 0;
    private int m_MaxSlot2 = 0;
    private int m_MaxSlot3 = 0;
    private int m_MaxSlot4 = 0;

    
    private int _Slot3Index = 0;
    private int _Slot4Index = 0;

    private int m_Index = 1;
    private int m_VerticalIndex = 0;
    private bool m_StopInput = false;
    private bool m_Warning = false;

    private bool isSlot1Avabile = true;
    private bool isSlot2Avabile = true;
    private bool isSlot3Avabile = false;
    private bool isSlot4Avabile = false;
    private bool _WarningTrigger = true;
    private bool[] m_WarningIndex =  new bool [4];

    private Material[] _TricicloSkins = new Material[3];
    private Material[] _ClassicaSkins = new Material[3];
    private Material[] _PickUpSkin1 = new Material[3];
    private Material[] _PickUpSkin2 = new Material[3];
    private Material[] _PickUpSkin3 = new Material[3];



    #endregion

    private void Awake()
    {
        instance = this;

        _TricicloSkins = Resources.LoadAll<Material>("Skins/Triciclo");
        _ClassicaSkins = Resources.LoadAll<Material>("Skins/Classica");
        _PickUpSkin1 = Resources.LoadAll<Material>("Skins/Pickup/First");
        _PickUpSkin2 = Resources.LoadAll<Material>("Skins/Pickup/Second");
        _PickUpSkin3 = Resources.LoadAll<Material>("Skins/Pickup/Third");

        #region Cinemachine Priority
        TricicloLoom.m_Priority = 10;
        TricicloLoom.m_Priority = 10;
        TricicloEngine.m_Priority = 10;
        TricicloWheels.m_Priority = 10;
        TricicloSkin.m_Priority = 10;
        TricicloAcc1.m_Priority = 10;
        TricicloAcc2.m_Priority = 10;

        ClassicaLoom.m_Priority = 10;
        ClassicaEngine.m_Priority = 10;
        ClassicaWheels.m_Priority = 10;
        ClassicaSkin.m_Priority = 10;
        ClassicaAcc1.m_Priority = 10;
        ClassicaAcc2.m_Priority = 10;

        PickUpLoom.m_Priority = 10;
        PickUpEngine.m_Priority = 10;
        PickUpWheels.m_Priority = 10;
        PickUpSkin.m_Priority = 10;
        PickUpAcc1.m_Priority = 10;
        PickUpAcc2.m_Priority = 10;
        #endregion

        m_WarningIcon = GameObject.Find("WarningIcon").GetComponent<Transform>();
        m_WarningIcon.gameObject.SetActive(false);

        StartManagment();
    }


    private void Update()
    {
		CameraMovement();

        #region Navigation
        if (!m_StopInput)
        {
            if (!MenuManager.pauseMenuActive)
            {
                if (m_VerticalIndex < verticalList.Length - 1)
                {
                    if ((Input.GetAxis(InputContainer.VERTICAL) < 0 || Input.GetKeyDown(KeyCode.DownArrow)))
                    {
                        _WarningTrigger = true;
                        ChangeVerticalIndex(false);

                        ResetWarning();
                        StatsBar.instance.RefreshBars();

                        if (AudioManager.Audio != null)
                        {
                            AudioManager.Audio.PlayMenuSound(MenuSounds.ChangeSelection);
                        }
                    }
                }

                if (m_VerticalIndex > 0)
                {
                    if ((Input.GetAxis(InputContainer.VERTICAL) > 0 || Input.GetKeyDown(KeyCode.UpArrow)))
                    {
                        _WarningTrigger = true;
                        ChangeVerticalIndex(true);

                        ResetWarning();
                        StatsBar.instance.RefreshBars();

                        if (AudioManager.Audio != null)
                        {
                            AudioManager.Audio.PlayMenuSound(MenuSounds.ChangeSelection);
                        }
                    }
                }

                if ((Input.GetAxis(InputContainer.HORIZONTAL) > 0 || Input.GetKeyDown(KeyCode.RightArrow)))
                {
                    _WarningTrigger = true;
                    RightSwitch();
                    StatsBar.instance.ShowNewStats();

                    if (AudioManager.Audio != null)
                    {
                        AudioManager.Audio.PlayMenuSound(MenuSounds.ChangeSelection);
                    }
                }

                if (Input.GetAxis(InputContainer.HORIZONTAL) < 0 || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    _WarningTrigger = true;
                    LeftSwitch();
                    StatsBar.instance.ShowNewStats();

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

        #region WarningCheck
        if (m_WarningIcon.gameObject.activeInHierarchy && _WarningTrigger)
        {
            _WarningTrigger = false;

            if (AudioManager.Audio != null)
            {
                AudioManager.Audio.PlayMenuSound(MenuSounds.AccessoryAlreadySelectedAlert);
            }

            m_Warning = true;
        }
        else if(!m_WarningIcon.gameObject.activeInHierarchy && _WarningTrigger)
        {
            _WarningTrigger = false;
            m_Warning = false;
        }
        #endregion

    }



    #region FramesImages
    private void ResetFrame()
    {
        MenuInizialeContainer.instance.garageHighlightFrames[m_VerticalIndex].SetActive(false);
        MenuInizialeContainer.instance.garageNormalFrames[m_VerticalIndex].SetActive(true);
    }
    private void ChangeFrame()
    {
        MenuInizialeContainer.instance.garageNormalFrames[m_VerticalIndex].SetActive(false);
        MenuInizialeContainer.instance.garageHighlightFrames[m_VerticalIndex].SetActive(true);

    }
    #endregion

    #region ImagesManagment
    private void WarningReset(Transform verticalList, ref int accessoryIndex)
    {
        verticalList.GetChild(0).GetChild(accessoryIndex).gameObject.SetActive(false);
        verticalList.GetChild(1).GetChild(accessoryIndex).gameObject.SetActive(false);

        accessoryIndex = 0;

        verticalList.GetChild(0).GetChild(accessoryIndex).gameObject.SetActive(true);
        verticalList.GetChild(1).GetChild(accessoryIndex).gameObject.SetActive(true);

        m_WarningIcon.gameObject.SetActive(false);
    }

    private void ChangeVerticalIndex(bool isGoinUp)
    {
        ResetFrame();

        verticalList[m_VerticalIndex].GetChild(0).gameObject.SetActive(true);
        verticalList[m_VerticalIndex].GetChild(1).gameObject.SetActive(false);

        if (isGoinUp)
            m_VerticalIndex--;
        else
            m_VerticalIndex++;

        verticalList[m_VerticalIndex].GetChild(1).gameObject.SetActive(true);
        verticalList[m_VerticalIndex].GetChild(0).gameObject.SetActive(false);

        ChangeFrame();

        if (m_Warning)
        {
            if (m_WarningIndex[0])
            {
                ResetAccessory(slot1Tr, slot1Index);
                WarningReset(verticalList[4], ref slot1Index);
            }
            else if (m_WarningIndex[1])
            {
                ResetAccessory(slot2Tr, slot2Index);
                WarningReset(verticalList[5], ref slot2Index);
            }
            else if (m_WarningIndex[2])
            {
                WarningReset(verticalList[6], ref _Slot3Index);
            }
            else if (m_WarningIndex[3])
            {
                WarningReset(verticalList[7], ref _Slot4Index);
            }


        }
    }

    private void ActivationManagment(ref int slotIndex, bool isGoingRight)
    {
        verticalList[m_VerticalIndex].GetChild(0).GetChild(slotIndex).gameObject.SetActive(false);
        verticalList[m_VerticalIndex].GetChild(1).GetChild(slotIndex).gameObject.SetActive(false);

        if (isGoingRight)
            slotIndex++;
        else
            slotIndex--;

        verticalList[m_VerticalIndex].GetChild(0).GetChild(slotIndex).gameObject.SetActive(true);
        verticalList[m_VerticalIndex].GetChild(1).GetChild(slotIndex).gameObject.SetActive(true);
    }
    #endregion

    #region HorizontalSwitchs
    private void RightSwitch()
    {
        switch (m_VerticalIndex)
        {
            case 0:
                if (loomIndex < m_MaxLoom)
                {
                    ResetAccessories(loomIndex);
                    ResetSkins();
                    ActivationManagment(ref loomIndex, true);
                    
                    LoomRebaseAccessory();
                    ChangeLoom();
                }
                break;

            case 1:
                if (engineIndex < m_MaxEngine)
                {
                    ActivationManagment(ref engineIndex, true);
                    ChangeEngine();
                }
                break;

            case 2:
                if (wheelIndex < m_MaxWheel)
                {
                    ActivationManagment(ref wheelIndex, true);
                    ChangeWheels();
                }
                break;

            case 3:
                if (skinIndex < m_MaxSkin)
                {
                    ActivationManagment(ref skinIndex, true);
                    ChangeSkin();
                }
                break;

            case 4:
                if (slot1Index < m_MaxSlot1 && isSlot1Avabile)
                {
                    ActivationManagment(ref slot1Index, true);
                    ChangeAccessory(slot1Tr, slot1Index);
                    ResetWarning();
                    
                    if (slot1Index == slot2Index && slot1Index != 0 && slot1Index < 8)
                    {
                        m_WarningIcon.position = verticalList[m_VerticalIndex + 1].position;
                        m_WarningIcon.gameObject.SetActive(true);
                        m_WarningIndex[1] = true;
                    }
                    else if (slot1Index == _Slot3Index && slot1Index != 0 && slot1Index < 8)
                    {
                        m_WarningIcon.position = verticalList[m_VerticalIndex + 2].position;
                        m_WarningIcon.gameObject.SetActive(true);
                        m_WarningIndex[2] = true;
                    }
                    else if (slot1Index == _Slot4Index && slot1Index != 0 && slot1Index < 8)
                    {
                        m_WarningIcon.position = verticalList[m_VerticalIndex + 3].position;
                        m_WarningIcon.gameObject.SetActive(true);
                        m_WarningIndex[3] = true;
                    }
                    else
                    {
                        m_WarningIcon.gameObject.SetActive(false);
                        ResetWarning();
                    }
                }
                break;

            case 5:
                if (slot2Index < m_MaxSlot2 && isSlot2Avabile)
                {
                    ActivationManagment(ref slot2Index, true);
                    ChangeAccessory(slot2Tr, slot2Index);
                    ResetWarning();
                    if (slot2Index == slot1Index && slot2Index != 0 && slot2Index < 8)
                    {
                        m_WarningIcon.position = verticalList[m_VerticalIndex - 1].position;
                        m_WarningIcon.gameObject.SetActive(true);
                        m_WarningIndex[0] = true;
                    }
                    else if (slot2Index == _Slot3Index && slot2Index != 0 && slot2Index < 8)
                    {
                        m_WarningIcon.position = verticalList[m_VerticalIndex + 1].position;
                        m_WarningIcon.gameObject.SetActive(true);
                        m_WarningIndex[2] = true;
                    }
                    else if (slot2Index == _Slot4Index && slot2Index != 0 && slot2Index < 8)
                    {
                        m_WarningIcon.position = verticalList[m_VerticalIndex + 2].position;
                        m_WarningIcon.gameObject.SetActive(true);
                        m_WarningIndex[3] = true;
                    }
                    else
                    {
                        m_WarningIcon.gameObject.SetActive(false);
                        ResetWarning();
                    }
                }
                break;

            case 6:
                if (_Slot3Index < m_MaxSlot3 && isSlot3Avabile)
                {
                    ActivationManagment(ref _Slot3Index, true);
                    ChangeAccessory(slot3Tr, _Slot3Index);
                    ResetWarning();
                    if (_Slot3Index == slot1Index && _Slot3Index != 0)
                    {
                        m_WarningIcon.position = verticalList[m_VerticalIndex - 2].position;
                        m_WarningIcon.gameObject.SetActive(true);
                        m_WarningIndex[0] = true;
                    }
                    else if (_Slot3Index == slot2Index && _Slot3Index != 0)
                    {
                        m_WarningIcon.position = verticalList[m_VerticalIndex - 1].position;
                        m_WarningIcon.gameObject.SetActive(true);
                        m_WarningIndex[1] = true;
                    }
                    else if (_Slot3Index == _Slot4Index && _Slot3Index != 0)
                    {
                        m_WarningIcon.position = verticalList[m_VerticalIndex + 1].position;
                        m_WarningIcon.gameObject.SetActive(true);
                        m_WarningIndex[3] = true;
                    }
                    else
                    {
                        m_WarningIcon.gameObject.SetActive(false);
                        ResetWarning();
                    }
                }
                break;

            case 7:

                if (_Slot4Index < m_MaxSlot4 && isSlot4Avabile)
                {
                    ChangeAccessory(slot4Tr, _Slot4Index);
                    ActivationManagment(ref _Slot4Index, true);
                    ResetWarning();
                    if (_Slot4Index == slot1Index && _Slot4Index != 0)
                    {
                        m_WarningIcon.position = verticalList[m_VerticalIndex - 3].position;
                        m_WarningIcon.gameObject.SetActive(true);
                        m_WarningIndex[0] = true;
                    }
                    else if (_Slot4Index == slot2Index && _Slot4Index != 0)
                    {
                        m_WarningIcon.position = verticalList[m_VerticalIndex - 2].position;
                        m_WarningIcon.gameObject.SetActive(true);
                        m_WarningIndex[1] = true;
                    }
                    else if (_Slot4Index == _Slot3Index && _Slot4Index != 0)
                    {
                        m_WarningIcon.position = verticalList[m_VerticalIndex - 1].position;
                        m_WarningIcon.gameObject.SetActive(true);
                        m_WarningIndex[2] = true;
                    }
                    else
                    {
                        m_WarningIcon.gameObject.SetActive(false);
                        ResetWarning();
                    }
                }
                break;

                
        }
        
    }
    private void LeftSwitch()
    {

        switch (m_VerticalIndex)
        {
            case 0:

                if (loomIndex > 0)
                {
                    ResetAccessories(loomIndex);
                    ResetSkins();
                    ActivationManagment(ref loomIndex, false);
                    LoomRebaseAccessory();
                    ChangeLoom();
                }
                break;

            case 1:

                if (engineIndex > 0)
                {
                    ActivationManagment(ref engineIndex, false);
                    ChangeEngine();
                }
                break;

            case 2:

                if (wheelIndex > 0)
                {
                    ActivationManagment(ref wheelIndex, false);
                    ChangeWheels();
                }
                break;
            case 3:

                if (skinIndex > 0)
                {
                    
                    ActivationManagment(ref skinIndex, false);
                    ChangeSkin();
                }
                break;

            case 4:
                if (slot1Index > 0 && isSlot1Avabile)
                {
                    ActivationManagment(ref slot1Index, false);
                    ChangeAccessory(slot1Tr, slot1Index);
                    ResetWarning();
                    if (slot1Index == slot2Index && slot1Index != 0 && slot1Index < 8)
                    {
                        m_WarningIcon.position = verticalList[m_VerticalIndex + 1].position;
                        m_WarningIcon.gameObject.SetActive(true);
                        m_WarningIndex[1] = true;
                    }
                    else if (slot1Index == _Slot3Index && slot1Index != 0 && slot1Index < 8)
                    {
                        m_WarningIcon.position = verticalList[m_VerticalIndex + 2].position;
                        m_WarningIcon.gameObject.SetActive(true);
                        m_WarningIndex[2] = true;
                    }
                    else if (slot1Index == _Slot4Index && slot1Index != 0 && slot1Index < 8)
                    {
                        m_WarningIcon.position = verticalList[m_VerticalIndex + 3].position;
                        m_WarningIcon.gameObject.SetActive(true);
                        m_WarningIndex[3] = true;
                    }
                    else
                    {
                        m_WarningIcon.gameObject.SetActive(false);
                        ResetWarning();
                    }
                }
                break;
            case 5:

                if (slot2Index > 0 && isSlot2Avabile)
                {
                    ActivationManagment(ref slot2Index, false);
                    ChangeAccessory(slot2Tr, slot2Index);
                    ResetWarning();
                    if (slot2Index == slot1Index && slot2Index != 0 && slot2Index < 8)
                    {
                        m_WarningIcon.position = verticalList[m_VerticalIndex - 1].position;
                        m_WarningIcon.gameObject.SetActive(true);
                        m_WarningIndex[0] = true;
                    }
                    else if (slot2Index == _Slot3Index && slot2Index != 0 && slot2Index < 8)
                    {
                        m_WarningIcon.position = verticalList[m_VerticalIndex + 1].position;
                        m_WarningIcon.gameObject.SetActive(true);
                        m_WarningIndex[2] = true;
                    }
                    else if (slot2Index == _Slot4Index && slot2Index != 0 && slot2Index < 8)
                    {
                        m_WarningIcon.position = verticalList[m_VerticalIndex + 2].position;
                        m_WarningIcon.gameObject.SetActive(true);
                        m_WarningIndex[3] = true;
                    }
                    else
                    {
                        m_WarningIcon.gameObject.SetActive(false);
                        ResetWarning();
                    }
                }
                break;
            case 6:

                if (_Slot3Index > 0 && isSlot3Avabile)
                {
                    ActivationManagment(ref _Slot3Index, false);
                    ChangeAccessory(slot3Tr, _Slot3Index);
                    ResetWarning();
                    if (_Slot3Index == slot1Index && _Slot3Index != 0)
                    {
                        m_WarningIcon.position = verticalList[m_VerticalIndex - 2].position;
                        m_WarningIcon.gameObject.SetActive(true);
                        m_WarningIndex[0] = true;
                    }
                    else if (_Slot3Index == slot2Index && _Slot3Index != 0)
                    {
                        m_WarningIcon.position = verticalList[m_VerticalIndex - 1].position;
                        m_WarningIcon.gameObject.SetActive(true);
                        m_WarningIndex[1] = true;
                    }
                    else if (_Slot3Index == _Slot4Index && _Slot3Index != 0)
                    {
                        m_WarningIcon.position = verticalList[m_VerticalIndex + 1].position;
                        m_WarningIcon.gameObject.SetActive(true);
                        m_WarningIndex[3] = true;
                    }
                    else
                    {
                        m_WarningIcon.gameObject.SetActive(false);
                        ResetWarning();
                    }
                }
                break;
            case 7:

                if (_Slot4Index > 0 && isSlot4Avabile)
                {
                    ActivationManagment(ref _Slot4Index, false);
                    ResetWarning();
                    ChangeAccessory(slot4Tr, _Slot4Index);
                    if (_Slot4Index == slot1Index && _Slot4Index != 0)
                    {
                        m_WarningIcon.position = verticalList[m_VerticalIndex - 3].position;
                        m_WarningIcon.gameObject.SetActive(true);
                        m_WarningIndex[0] = true;
                    }
                    else if (_Slot4Index == slot2Index && _Slot4Index != 0)
                    {
                        m_WarningIcon.position = verticalList[m_VerticalIndex - 2].position;
                        m_WarningIcon.gameObject.SetActive(true);
                        m_WarningIndex[1] = true;
                    }
                    else if (_Slot4Index == _Slot3Index && _Slot4Index != 0)
                    {
                        m_WarningIcon.position = verticalList[m_VerticalIndex - 1].position;
                        m_WarningIcon.gameObject.SetActive(true);
                        m_WarningIndex[2] = true;
                    }
                    else
                    {
                        m_WarningIcon.gameObject.SetActive(false);
                        ResetWarning();
                    }
                }
                break;

                
        }
        
    }
    
    #endregion

    #region AccessDenied
    private void LoomRebaseAccessory()
    {
        switch (loomIndex)
        {
            case 0:
                AccessoryManagment(true, true, false, false);
                break;
            case 1:
                AccessoryManagment(true, true, false, false);
                break;
            case 2:
                AccessoryManagment(true, true, false, false);
                break;
            case 3:
                AccessoryManagment(true, true, true, true);
                break;
            case 4:
                AccessoryManagment(false, true, false, false);
                break;
            case 5:
                AccessoryManagment(true, false, true, true);
                break;
        }
    }

    private void AccessoryManagment(bool slot1, bool slot2, bool slot3, bool slot4)
    {
        isSlot1Avabile = slot1;
        isSlot2Avabile = slot2;
        isSlot3Avabile = slot3;
        isSlot4Avabile = slot4;

        ResetAccessory();

        if (!isSlot1Avabile)
        {
            MenuInizialeContainer.instance.accessDenied[0].SetActive(true);
            verticalList[4].GetChild(0).GetChild(0).gameObject.SetActive(false);
            verticalList[4].GetChild(1).GetChild(0).gameObject.SetActive(false);
            slot1Index = 0;
        }
        else
        {
            MenuInizialeContainer.instance.accessDenied[0].SetActive(false);
            verticalList[4].GetChild(0).GetChild(0).gameObject.SetActive(true);
            verticalList[4].GetChild(1).GetChild(0).gameObject.SetActive(true);

        }
        if (!isSlot2Avabile)
        {
            MenuInizialeContainer.instance.accessDenied[1].SetActive(true);
            verticalList[5].GetChild(0).GetChild(0).gameObject.SetActive(false);
            verticalList[5].GetChild(1).GetChild(0).gameObject.SetActive(false);
            slot2Index = 0;
        }
        else
        {
            MenuInizialeContainer.instance.accessDenied[1].SetActive(false);
            verticalList[5].GetChild(0).GetChild(0).gameObject.SetActive(true);
            verticalList[5].GetChild(1).GetChild(0).gameObject.SetActive(true);
        }
        if (!isSlot3Avabile)
        {
            MenuInizialeContainer.instance.accessDenied[2].SetActive(true);
            verticalList[6].GetChild(0).GetChild(0).gameObject.SetActive(false);
            verticalList[6].GetChild(1).GetChild(0).gameObject.SetActive(false);
            _Slot3Index = 0;
        }
        else
        {
            MenuInizialeContainer.instance.accessDenied[2].SetActive(false);
            verticalList[6].GetChild(0).GetChild(0).gameObject.SetActive(true);
            verticalList[6].GetChild(1).GetChild(0).gameObject.SetActive(true);
        }
        if (!isSlot4Avabile)
        {
            MenuInizialeContainer.instance.accessDenied[3].SetActive(true);
            verticalList[7].GetChild(0).GetChild(0).gameObject.SetActive(false);
            verticalList[7].GetChild(1).GetChild(0).gameObject.SetActive(false);
            _Slot4Index = 0;
        }
        else
        {
            MenuInizialeContainer.instance.accessDenied[3].SetActive(false);
            verticalList[7].GetChild(0).GetChild(0).gameObject.SetActive(true);
            verticalList[7].GetChild(1).GetChild(0).gameObject.SetActive(true);
        }

    }

    private void ResetAccessory()
    {
        for (int i = 4; i < 8; i++)
        {
            int j = 0;

            switch (i)
            {
                case 4:
                    j = slot1Index;
                    slot1Index = 0;
                    
                    break;
                case 5:
                    j = slot2Index;
                    slot2Index = 0;
                    break;
                case 6:
                    j = _Slot3Index;
                    _Slot3Index = 0;
                    break;
                case 7:
                    j = _Slot4Index;
                    _Slot4Index = 0;
                    break;
            }

            verticalList[i].GetChild(0).GetChild(j).gameObject.SetActive(false);
            verticalList[i].GetChild(1).GetChild(j).gameObject.SetActive(false);

            j = 0;

            verticalList[i].GetChild(0).GetChild(j).gameObject.SetActive(true);
            verticalList[i].GetChild(1).GetChild(j).gameObject.SetActive(true);
        }
    }
    #endregion

    #region VehiclePersonalization

    private void ChangeLoom()
    {

        if(loomIndex == 0)
        {
            MenuInizialeContainer.instance.vehicles.transform.GetChild(loomIndex).gameObject.SetActive(true);
            MenuInizialeContainer.instance.vehicles.transform.GetChild(loomIndex +1).gameObject.SetActive(false);
        }

        else if(loomIndex  == 1)
        {
            MenuInizialeContainer.instance.vehicles.transform.GetChild(loomIndex).gameObject.SetActive(true);
            MenuInizialeContainer.instance.vehicles.transform.GetChild(loomIndex -1).gameObject.SetActive(false);
            MenuInizialeContainer.instance.vehicles.transform.GetChild(loomIndex + 1).gameObject.SetActive(false);
        }

        else if(loomIndex == 2)
        {
            MenuInizialeContainer.instance.vehicles.transform.GetChild(loomIndex).gameObject.SetActive(true);
            MenuInizialeContainer.instance.vehicles.transform.GetChild(loomIndex - 1).gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Telaio mancante");
        }

    }

    private void ChangeEngine()
    {
        if (engineIndex <= engines[0].childCount -1)
        {
            for (int i = 0; i < engines.Length; i++)
            {
                engines[i].GetChild(engineIndex).gameObject.SetActive(true);

                if (engineIndex < engines[0].childCount -1)
                {
                    engines[i].GetChild(engineIndex + 1).gameObject.SetActive(false);
                }
                if (engineIndex > 0)
                {

                    engines[i].GetChild(engineIndex - 1).gameObject.SetActive(false);
                }
                
            }

        }
        else
        {
            Debug.LogWarning("Non esiste");
        }

    }

    private void ChangeWheels()
    {
        if (wheelIndex <= wheels[0].childCount -1)
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].GetChild(wheelIndex).gameObject.SetActive(true);

                if (wheelIndex < wheels[i].childCount -1)
                {
                    wheels[i].GetChild(wheelIndex + 1).gameObject.SetActive(false);
                }
                if (wheelIndex > 0)
                {
                    wheels[i].GetChild(wheelIndex - 1).gameObject.SetActive(false);
                }

            }
        }
        else
        {
            Debug.LogWarning("Non esiste");
        }


    }

    private void ChangeAccessory(Transform[] slot, int index)
    {
        if (index < slot[0].childCount - 1)
        {
            slot[loomIndex].GetChild(index + 1).gameObject.SetActive(false);
        }
        if (index > 0)
        {

            slot[loomIndex].GetChild(index - 1).gameObject.SetActive(false);
        }


        slot[loomIndex].GetChild(index).gameObject.SetActive(true);

        //if (index <= slot[0].childCount - 1)
        //{
        //    for (int i = 0; i < slot.Length; i++)
        //    {
        //        slot[i].GetChild(index).gameObject.SetActive(true);

        //        if (index < slot[0].childCount - 1)
        //        {
        //            slot[i].GetChild(index + 1).gameObject.SetActive(false);
        //        }
        //        if (index > 0)
        //        {

        //            slot[i].GetChild(index - 1).gameObject.SetActive(false);
        //        }

        //    }

        //}


    }

    private void ResetAccessories(int loomIndex)
    {

        for(int i = 0; i < slot1Tr.Length; i++)
        {
            slot1Tr[i].GetChild(slot1Index).gameObject.SetActive(false);
            slot1Tr[i].GetChild(0).gameObject.SetActive(true);
        }

        for(int i = 0; i <slot2Tr.Length; i++)
        {
            slot2Tr[i].GetChild(slot2Index).gameObject.SetActive(false);
            slot2Tr[i].GetChild(0).gameObject.SetActive(true);
        }

        //for(int i = 0; i< slot3Tr.Length -1; i++)
        //{
        //    slot3Tr[i].GetChild(_Slot3Index).gameObject.SetActive(false);
        //    slot3Tr[i].GetChild(0).gameObject.SetActive(true);
        //}

    }

    private void ResetAccessory(Transform[] slot, int slotIndex)
    {
        slot[loomIndex].GetChild(slotIndex).gameObject.SetActive(false);
        slot[loomIndex].GetChild(0).gameObject.SetActive(true);
    }

    private void ChangeSkin()
    {
        if(loomIndex == 0)
        {
            skins[0].GetComponent<MeshRenderer>().material = _TricicloSkins[skinIndex];
        }
        else if(loomIndex == 1)
        {
            skins[1].GetComponent<MeshRenderer>().material = _ClassicaSkins[skinIndex];
        }
        else
        {
            switch(skinIndex)
            {
                case 0:
                    skins[2].GetChild(0).GetComponent<MeshRenderer>().material = _PickUpSkin1[0];
                    skins[2].GetChild(1).GetComponent<MeshRenderer>().material = _PickUpSkin1[1];
                    skins[2].GetChild(2).GetComponent<MeshRenderer>().material = _PickUpSkin1[2];
                    break;
                case 1:
                    skins[2].GetChild(0).GetComponent<MeshRenderer>().material = _PickUpSkin2[0];
                    skins[2].GetChild(1).GetComponent<MeshRenderer>().material = _PickUpSkin2[1];
                    skins[2].GetChild(2).GetComponent<MeshRenderer>().material = _PickUpSkin2[2];
                    break;
                case 2:
                    skins[2].GetChild(0).GetComponent<MeshRenderer>().material = _PickUpSkin3[0];
                    skins[2].GetChild(1).GetComponent<MeshRenderer>().material = _PickUpSkin3[1];
                    skins[2].GetChild(2).GetComponent<MeshRenderer>().material = _PickUpSkin3[2];
                    break;
            }
            
            
            
        }
    }

    private void ResetSkins()
    {
        verticalList[3].GetChild(0).GetChild(skinIndex).gameObject.SetActive(false);
        verticalList[3].GetChild(1).GetChild(skinIndex).gameObject.SetActive(false);

        skinIndex = 0;

        verticalList[3].GetChild(0).GetChild(skinIndex).gameObject.SetActive(true);
        verticalList[3].GetChild(1).GetChild(skinIndex).gameObject.SetActive(true);

        if (loomIndex == 0)
        {
            skins[0].GetComponent<MeshRenderer>().material = _TricicloSkins[0];
        }
        else if (loomIndex == 1)
        {
            skins[1].GetComponent<MeshRenderer>().material = _ClassicaSkins[0];
        }
        else
        {
            skins[2].GetChild(0).GetComponent<MeshRenderer>().material = _PickUpSkin1[0];
            skins[2].GetChild(1).GetComponent<MeshRenderer>().material = _PickUpSkin1[1];
            skins[2].GetChild(2).GetComponent<MeshRenderer>().material = _PickUpSkin1[2];
        }


    }

    private void StartManagment()
    {
        for (int i = 0; i < verticalList.Length; i++)
        {
            for (int j = 0; j < verticalList[i].GetChild(0).childCount - 1; j++)
            {
                switch (i)
                {
                    case 0:
                        m_MaxLoom++;
                        break;
                    case 1:
                        m_MaxEngine++;
                        break;
                    case 2:
                        m_MaxWheel++;
                        break;
                    case 3:
                        m_MaxSkin++;
                        break;
                    case 4:
                        m_MaxSlot1++;
                        break;
                    case 5:
                        m_MaxSlot2++;
                        break;
                    case 6:
                        m_MaxSlot3++;
                        break;
                    case 7:
                        m_MaxSlot4++;
                        break;
                }
            }
        }

        MenuInizialeContainer.instance.garageHighlightFrames[m_VerticalIndex].SetActive(true);
        MenuInizialeContainer.instance.garageNormalFrames[m_VerticalIndex].SetActive(false);

        verticalList[m_VerticalIndex].GetChild(1).gameObject.SetActive(true);
        verticalList[m_VerticalIndex].GetChild(0).gameObject.SetActive(false);
        verticalList[m_VerticalIndex].GetChild(0).GetChild(loomIndex).gameObject.SetActive(true);

        AccessoryManagment(true, true, false, false);
    }
    #endregion
    
    private void ResetWarning()
    {
        for (int i = 0; i < m_WarningIndex.Length; i++)
        {
            m_WarningIndex[i] = false;
        }
    }

    public string[] GetAccessoryName()
    {
        string[] accessoryNames = new string[4];
        accessoryNames[0] = verticalList[4].GetChild(0).GetChild(slot1Index).gameObject.name;
        accessoryNames[1] = verticalList[5].GetChild(0).GetChild(slot2Index).gameObject.name;
        accessoryNames[2] = verticalList[6].GetChild(0).GetChild(_Slot3Index).gameObject.name;
        accessoryNames[3] = verticalList[7].GetChild(0).GetChild(_Slot4Index).gameObject.name;
        return accessoryNames;
    }

    private void CameraMovement()
    {
        if (loomIndex == 0)
        {
            if (m_VerticalIndex == 0)
            {
                TricicloLoom.m_Priority = 11;
            }
            //else 
            //{
            //	TricicloLoom.m_Priority = 10;
            //}

            if (m_VerticalIndex == 1)
            {
                TricicloEngine.m_Priority = 11;
            }
            else
            {
                TricicloEngine.m_Priority = 10;
            }
            if (m_VerticalIndex == 2)
            {
                TricicloWheels.m_Priority = 11;
            }
            else
            {
                TricicloWheels.m_Priority = 10;
            }

            if (m_VerticalIndex == 3)
            {
                TricicloSkin.m_Priority = 11;
            }
            else
            {
                TricicloSkin.m_Priority = 10;
            }
            if (m_VerticalIndex == 4)
            {
                TricicloAcc1.m_Priority = 11;
            }
            else
            {
                TricicloAcc1.m_Priority = 10;
            }

            if (m_VerticalIndex == 5)
            {
                TricicloAcc2.m_Priority = 11;
            }
            else
            {
                TricicloAcc2.m_Priority = 10;
            }

        }
        else
        {
            if (m_VerticalIndex == 0)
            {
                TricicloLoom.m_Priority = 10;
            }
        }
        if (loomIndex == 1)
        {
            if (m_VerticalIndex == 0)
            {
                ClassicaLoom.m_Priority = 11;
            } //else {
              //ClassicaLoom.m_Priority = 10;
              //}

            if (m_VerticalIndex == 1)
            {
                ClassicaEngine.m_Priority = 11;
            }
            else
            {
                ClassicaEngine.m_Priority = 10;
            }
            if (m_VerticalIndex == 2)
            {
                ClassicaWheels.m_Priority = 11;
            }
            else
            {
                ClassicaWheels.m_Priority = 10;
            }

            if (m_VerticalIndex == 3)
            {
                ClassicaSkin.m_Priority = 11;
            }
            else
            {
                ClassicaSkin.m_Priority = 10;
            }
            if (m_VerticalIndex == 4)
            {
                ClassicaAcc1.m_Priority = 11;
            }
            else
            {
                ClassicaAcc1.m_Priority = 10;
            }

            if (m_VerticalIndex == 5)
            {
                ClassicaAcc2.m_Priority = 11;
            }
            else
            {
                ClassicaAcc2.m_Priority = 10;
            }

        }

        else
        {
            if (m_VerticalIndex == 0)
            {
                ClassicaLoom.m_Priority = 10;
            }
        }

        if (loomIndex == 2)
        {
            if (m_VerticalIndex == 0)
            {
                PickUpLoom.m_Priority = 11;
            }
            //else 
            //{
            //	PickUpLoom.m_Priority = 10;
            //}

            if (m_VerticalIndex == 1)
            {
                PickUpEngine.m_Priority = 11;
            }
            else
            {
                PickUpEngine.m_Priority = 10;
            }
            if (m_VerticalIndex == 2)
            {
                PickUpWheels.m_Priority = 11;
            }
            else
            {
                PickUpWheels.m_Priority = 10;
            }

            if (m_VerticalIndex == 3)
            {
                PickUpSkin.m_Priority = 11;
            }
            else
            {
                PickUpSkin.m_Priority = 10;
            }
            if (m_VerticalIndex == 4)
            {
                PickUpAcc1.m_Priority = 11;
            }
            else
            {
                PickUpAcc1.m_Priority = 10;
            }

            if (m_VerticalIndex == 5)
            {
                PickUpAcc2.m_Priority = 11;
            }
            else
            {
                PickUpAcc2.m_Priority = 10;
            }

        }
        else
        {
            if (m_VerticalIndex == 0)
            {
                PickUpLoom.m_Priority = 10;
            }
        }


    }

    private void OnEnable()
    {
        MenuInizialeContainer.instance.vehicles.SetActive(true);
    }

    private void OnDisable()
    {
        if(MenuInizialeContainer.instance.vehicles != null)
            MenuInizialeContainer.instance.vehicles.SetActive(false);

        StatsBar.instance.RefreshBars();

        if (m_Warning)
        {
            if (m_WarningIndex[0])
            {
                ResetAccessory(slot1Tr, slot1Index);
                WarningReset(verticalList[4], ref slot1Index);
            }
            else if (m_WarningIndex[1])
            {
                ResetAccessory(slot2Tr, slot2Index);
                WarningReset(verticalList[5], ref slot2Index);
            }
            else if (m_WarningIndex[2])
            {
                WarningReset(verticalList[6], ref _Slot3Index);
            }
            else if (m_WarningIndex[3])
            {
                WarningReset(verticalList[7], ref _Slot4Index);
            }


        }


    }
}