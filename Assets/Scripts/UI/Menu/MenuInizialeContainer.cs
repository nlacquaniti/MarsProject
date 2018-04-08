using UnityEngine;
using UnityEngine.UI;

public class MenuInizialeContainer : MonoBehaviour
{
    [Header("Info Texts")]
    public Text missionInfo;
    public Text userInfo;



    [Header("Icone")]
    public GameObject zoneFrameHighlight;
    public GameObject zoneFrameNormal;
    public GameObject missioniFrameHighlight;
    public GameObject missioniFrameNormal;

    [Header("Immagini Garage")]
    public GameObject[] garageHighlightFrames;
    public GameObject[] garageNormalFrames;
    public GameObject[] accessDenied;

    [Header("Interfaces")]
    public GameObject[] interfaces;
    public GameObject pauseBox;

    [Header("Veicoli")]
    public GameObject vehicles;

    [Header("Mappa")]
    public string missionName;
    public string zoneName;

    
    public static MenuInizialeContainer instance;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }
}
