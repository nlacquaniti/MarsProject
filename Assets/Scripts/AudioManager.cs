using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;



public enum ObjectiveSounds { TimerBlink, LostMedal, CollectAchivement, BronzeGoal, SilverGoal, GoldGoal, MissionFailed, RefillFuel, CollectPoint }
public enum MenuSounds { ChangeSelection, ConfirmSelection, MenuSlide, MenuEnter, MenuBack, MenuSwitch, AccessoryAlreadySelectedAlert }

[Serializable]
public class SoundSettings
{
    public AudioClip audioFile = null;
    [Range(0, 1)] public float minVolume = 0;
    [Range(0, 1)] public float maxVolume = 1;
    [Range(0, 2)] public float minPitch = 0.75f;
    [Range(0, 2)] public float maxPitch = 1.5f;
    [Range(0, 1)] public float from2dTO3d = 0;
    public bool loop = true;
    public float minDistance = 1;
    public float maxDistance = 750;
}

[Serializable]
public class SoundSettingsSemplified
{
    public AudioClip audioFile = null;
    [Range(0, 1)] public float volume = 1;
    [Range(0, 2)] public float pitch = 1;
    [Range(0, 1)] public float from2dTO3d = 0;
    public bool loop = true;
    public float distance = 750;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Audio;
    private AudioSource audioSource = null;
    private AudioSource musicSource = null;

    [Header("[SOUND SETTINGS]")]
    [Header(" - VEHICLE ENGINES")]
    [SerializeField] private SoundSettings vehicleEngineMagnetic;
    [SerializeField] private SoundSettings vehicleEngineNuclear;
    [SerializeField] private SoundSettings vehicleEnginePhotoSynthesis;
    [SerializeField] private SoundSettings vehicleEngineDryIce;
    [SerializeField] private SoundSettings vehicleEngineThermic;

    [Header(" - VEHICLE WHEEL")]
    [SerializeField] private SoundSettings vehicleWheelSurfaceRock;
    [SerializeField] private SoundSettings vehicleWheelSurfaceSand;
    [SerializeField] private SoundSettings vehicleWheelSurfaceIce;
    [SerializeField] private SoundSettings vehicleWheelDrone;
    [SerializeField] private SoundSettings vehicleWheelPropulsor;

    [Header(" - VEHICLE ACCESSORIES")]
    [SerializeField] private SoundSettingsSemplified gliderON;
    [SerializeField] private SoundSettingsSemplified gliderOFF;
    [SerializeField] private SoundSettingsSemplified pointerON;
    [SerializeField] private SoundSettingsSemplified pointerOFF;
    [SerializeField] private SoundSettingsSemplified weightModON;
    [SerializeField] private SoundSettingsSemplified weightModOFF;
    [SerializeField] private SoundSettingsSemplified turbo;
    [SerializeField] private SoundSettingsSemplified hydraulicSuspensionON;
    [SerializeField] private SoundSettingsSemplified hydraulicSuspensionOFF;
    [SerializeField] private SoundSettingsSemplified brightsON;
    [SerializeField] private SoundSettingsSemplified brightsOFF;
    [SerializeField] private SoundSettingsSemplified shieldingON;
    [SerializeField] private SoundSettingsSemplified shieldingOFF;
    [SerializeField] private SoundSettingsSemplified airBearing;
    [SerializeField] private SoundSettingsSemplified helicopterON;
    private SoundSettingsSemplified[] accessoriesSoundSettings;
    
    [Header(" - VEHICLE EXTRA")]
    [SerializeField] private SoundSettings vehicleResistenceAir;
    private SoundSettings[] vehicleSoundSettings;

    [Header(" - INFLUENCE ZONES")]
    [SerializeField] private SoundSettingsSemplified sandStormZone;
    [SerializeField] private SoundSettingsSemplified radiationZone;
    [SerializeField] private SoundSettingsSemplified radiationAlert;
    private SoundSettingsSemplified[] influenceZonesSoundSettings;

    [Header(" - OBJECTIVES & ACHIVEMENTS")]
    [SerializeField] private SoundSettingsSemplified timerBlink;
    [SerializeField] private SoundSettingsSemplified lostMedal;
    [SerializeField] private SoundSettingsSemplified collectAchivement;
    [SerializeField] private SoundSettingsSemplified bronzeGoal;
    [SerializeField] private SoundSettingsSemplified silverGoal;
    [SerializeField] private SoundSettingsSemplified goldGoal;
    [SerializeField] private SoundSettingsSemplified missionFailed;
    [SerializeField] private SoundSettingsSemplified refillFuel;
    [SerializeField] private SoundSettingsSemplified collectPoint;
    private SoundSettingsSemplified[] objectivesSoundSettings;

    [Header(" - MENUS")]
    [SerializeField] private SoundSettingsSemplified changeSelection;
    [SerializeField] private SoundSettingsSemplified confirmSelection;
    [SerializeField] private SoundSettingsSemplified menuSlide;
    [SerializeField] private SoundSettingsSemplified menuEnter;
    [SerializeField] private SoundSettingsSemplified menuBack;
    [SerializeField] private SoundSettingsSemplified menuGarageSwitch;
    [SerializeField] private SoundSettingsSemplified accessoryAlreadySelectedAlert;
    private SoundSettingsSemplified[] menuSoundSettings;

    [Header(" - MUSIC")]
    [SerializeField] private SoundSettingsSemplified[] musicArray;
    float currentMusicMinVolume = 0;
    float currentMusicMinPitch = 0;


    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Audio = this;

        foreach (AudioSource source in GetComponents<AudioSource>())
        {
            Destroy(source);
        } 

        InitializeSound();
    }

    void InitializeSound()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.playOnAwake = false;
        }

        LoadSoundSettingsArrays();
    }

    void Update()
    {
        if (!musicSource.isPlaying)
        {
            StartCoroutine(PlayNextMusicTrack());
        }

        if (Input.GetKeyDown(KeyCode.Insert))
        {
            InitializeSound();
        }
    }

    void LoadSoundSettingsArrays()
    {
        vehicleSoundSettings = new SoundSettings[11]
        {
            vehicleEngineMagnetic,
            vehicleEngineNuclear,
            vehicleEnginePhotoSynthesis,
            vehicleEngineDryIce,
            vehicleEngineThermic,
            vehicleWheelSurfaceRock,
            vehicleWheelSurfaceSand,
            vehicleWheelSurfaceIce,
            vehicleWheelDrone,
            vehicleWheelPropulsor,
            vehicleResistenceAir
        };

        accessoriesSoundSettings = new SoundSettingsSemplified[15]
        {
            gliderON,
            gliderOFF,
            pointerON,
            pointerOFF,
            weightModON,
            weightModOFF,
            turbo,
            hydraulicSuspensionON,
            hydraulicSuspensionOFF,
            brightsON,
            brightsOFF,
            shieldingON,
            shieldingOFF,
            airBearing,
            helicopterON
        };

        influenceZonesSoundSettings = new SoundSettingsSemplified[3]
        {
            sandStormZone,
            radiationZone,
            radiationAlert
        };

        objectivesSoundSettings = new SoundSettingsSemplified[9]
        {
            timerBlink,
            lostMedal,
            collectAchivement,
            bronzeGoal,
            silverGoal,
            goldGoal,
            missionFailed,
            refillFuel,
            collectPoint
        };

        menuSoundSettings = new SoundSettingsSemplified[7]
        {
            changeSelection,
            confirmSelection,
            menuSlide,
            menuEnter,
            menuBack,
            menuGarageSwitch,
            accessoryAlreadySelectedAlert
        };
    }

    public SoundSettings[] GetVehicleSoundSettings()
    {
        return vehicleSoundSettings;
    }

    public SoundSettingsSemplified[] GetAccessoriesSoundSettings()
    {
        return accessoriesSoundSettings;
    }

    public SoundSettingsSemplified[] GetInfluenceZonesSoundSettings()
    {
        return influenceZonesSoundSettings;
    }

    public void SetVolumeOverTime(AudioSource audioSource, float speed, float minVolume, float maxVolume)
    {
        if (!audioSource.isPlaying) { audioSource.Play(); }

        if ((audioSource.volume + speed) <= maxVolume && (audioSource.volume + speed) >= minVolume)
        {
            audioSource.volume += speed;
        }

        if (audioSource.volume > maxVolume)
        {
            audioSource.volume = maxVolume;
        }

        if (audioSource.volume < minVolume)
        {
            audioSource.volume = minVolume;
        }
    }

    public void SetVolume(AudioSource audioSource, float value)
    {
        if (!audioSource.isPlaying) { audioSource.Play(); }
        audioSource.volume = value;
    }

    public void SetPitchOverTime(AudioSource audioSource, float speed, float minPitch, float maxPitch)
    {
        if (!audioSource.isPlaying) { audioSource.Play(); }

        if ((audioSource.pitch + speed) <= maxPitch && (audioSource.pitch + speed) >= minPitch)
        {
            audioSource.pitch += speed;
        }

        if (audioSource.pitch > maxPitch)
        {
            audioSource.pitch = maxPitch;
        }

        if (audioSource.pitch < minPitch)
        {
            audioSource.pitch = minPitch;
        }
    }

    public void SetPitch(AudioSource audioSource, float value)
    {
        if (!audioSource.isPlaying) { audioSource.Play(); }
        audioSource.pitch = value;
    }

    public void PlayObjectivesSound(ObjectiveSounds soundIndex)
    {
        audioSource.clip = objectivesSoundSettings[(int)soundIndex].audioFile;
        audioSource.volume = objectivesSoundSettings[(int)soundIndex].volume;
        audioSource.pitch = objectivesSoundSettings[(int)soundIndex].pitch;
        audioSource.maxDistance = objectivesSoundSettings[(int)soundIndex].distance;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void PlayMenuSound(MenuSounds soundIndex)
    {
        audioSource.clip = menuSoundSettings[(int)soundIndex].audioFile;
        audioSource.volume = menuSoundSettings[(int)soundIndex].volume;
        audioSource.pitch = menuSoundSettings[(int)soundIndex].pitch;
        audioSource.maxDistance = menuSoundSettings[(int)soundIndex].distance;
        audioSource.loop = false;
        audioSource.Play();
    }

    public void StopAllSounds()
    {
        foreach (AudioSource source in FindObjectsOfType<AudioSource>())
        {
            if (source != musicSource)
            {
                source.Stop();
            }
        }
    }

    IEnumerator PlayNextMusicTrack()
    {
        if (musicSource.isPlaying)
        {
            while (musicSource.volume > 0.01f)
            {
                musicSource.volume -= 0.5f * Time.deltaTime;
                yield return null;
            }

            musicSource.Stop();
            StopCoroutine(PlayNextMusicTrack());
            StartCoroutine(PlayNextMusicTrack());
        }
        else
        {
            int musicIndex = UnityEngine.Random.Range(0, musicArray.Length);
            musicSource.clip = musicArray[musicIndex].audioFile;
            musicSource.volume = 0;
            musicSource.pitch = musicArray[musicIndex].pitch;
            musicSource.spatialBlend = 0;
            musicSource.loop = false;
            musicSource.Play();

            while (musicSource.volume < musicArray[musicIndex].volume)
            {
                musicSource.volume += 0.15f * Time.deltaTime;
                yield return null;
            }

            musicSource.volume = musicArray[musicIndex].volume;
        }

        yield return null;
    }

    public void PlayPauseMenuChange(bool isOpened)
    {
        if (isOpened)
        {
            StopAllSounds();
            musicSource.volume /= 2; 
        }
        else
        {
            musicSource.volume *= 2;
        }
    }

    void OnLevelWasLoaded(int level)
    {
        StopAllSounds();
        StartCoroutine(PlayNextMusicTrack());
    }
}
