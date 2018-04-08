using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;


public class NewVehicleController : MonoBehaviour
{
    public bool isInShadow = false;
    public List<WheelInfo> wheelInfos;
    [HideInInspector] public bool isFlying = false;
    [HideInInspector] public Rigidbody rb;
    private float forwardStiffness;
    private float sidewaysStiffness;
    private Transform[] caterpillarModels;
    private TerrainCheck terrainCheck = null;
    FileManager fileManager = new FileManager();
    string wheelSettingsDebug = "";
    bool showDebugValues = false;
    bool isDebugShowed = false;
    public List<EnvPropertyScript> currentInfluenceZones = new List<EnvPropertyScript>();
    public static NewVehicleController vehicleController;
    Text debugText = null;
    private Texture contactTexture;
    private float fuelLastTime;
    [HideInInspector] public float maxSpeed;
    GameObject sunLight = null;
    bool flyModeAlternative = false;
    string wheelsTerrainContactDebug = "";
    float lastTemperatureTemp = 0;
    [HideInInspector] public bool flyingControlEnabled = true;
    private ParticleManager pm;
    Terrain currentTerrain = null;
    FPSCounter fpsCounter = null;
    List<GameObject> dronesBlades = new List<GameObject>();


    #region Sound

    AudioSource engineAudioSource = null;
    AudioSource resistenceAirAudioSource = null;

    float minEngineVolume = 0;
    float maxEngineVolume = 0;
    float minEnginePitch = 0;
    float maxEnginePitch = 0;

    float minResistenceAirVolume = 0;
    float maxResistenceAirVolume = 0;
    float minResistenceAirPitch = 0;
    float maxResistenceAirPitch = 0;

    AudioClip wheelContactClipIce = null;
    float minWheelContactVolumeIce = 0;
    float maxWheelContactVolumeIce = 0;
    float minWheelContactPitchIce = 0;
    float maxWheelContactPitchIce = 0;

    AudioClip wheelContactClipSand = null;
    float minWheelContactVolumeSand = 0;
    float maxWheelContactVolumeSand = 0;
    float minWheelContactPitchSand = 0;
    float maxWheelContactPitchSand = 0;

    AudioClip wheelContactClipRock = null;
    float minWheelContactVolumeRock = 0;
    float maxWheelContactVolumeRock = 0;
    float minWheelContactPitchRock = 0;
    float maxWheelContactPitchRock = 0;

    AudioClip wheelContactClipFlying = null;
    float minWheelContactVolumeFlying = 0;
    float maxWheelContactVolumeFlying = 0;
    float minWheelContactPitchFlying = 0;
    float maxWheelContactPitchFlying = 0;

    #endregion

    #region Car Components
    // ENUM
    public enum TypeOfChassis { Trycicle = 0, Classic = 1, Pickup = 2, Truck = 3, Drop = 4, Geodude = 5 };
    public enum TypeOfEngine { Magnetic = 0, Nuclear = 1, PhotoSynthesis = 2, DryIce = 3, ThermalExcursion = 4 };
    public enum TypeOfWheel { Suckers = 0, Hobnailed = 1, MonsterTruck = 2, Caterpillar = 9999, Sphere = 3, Drone = 4, Propeller = 5, Dragonfly = 6, Arts = 7 };
    public enum TypeOfAccessorie
    {
        Not_Equipped = -1, IndicatorArrow = 0, Turbo = 1, Glider = 2, Brights = 3, AeolianPropeller = 4, SolarPropeller = 5,
        RadiationPropeller = 6, HydraulicSuspensions = 7, Shielding = 8, WeightMod = 9, AirBearing = 10, Helicopter = 11, Grapple = 12
    };

    [HideInInspector] public TypeOfChassis chosenChassis = TypeOfChassis.Trycicle;
    [HideInInspector] public TypeOfEngine chosenEngine = TypeOfEngine.Magnetic;
    [HideInInspector] public TypeOfWheel chosenWheel = TypeOfWheel.MonsterTruck;
    [HideInInspector] public TypeOfAccessorie chosenAccessory1 = TypeOfAccessorie.Not_Equipped;
    [HideInInspector] public TypeOfAccessorie chosenAccessory2 = TypeOfAccessorie.Not_Equipped;
    [HideInInspector] public TypeOfAccessorie chosenAccessory3 = TypeOfAccessorie.Not_Equipped;
    [HideInInspector] public TypeOfAccessorie chosenAccessory4 = TypeOfAccessorie.Not_Equipped;
    #endregion Car Components

    #region Stats

    private float currentMotorTorque;
    private float defaulMotorTorque;
    private float motorTorqueMultiplierSun;
    private float motorTorqueMultiplierRadiation;

    private float brakeTorque;
    private float steeringAngle;

    /*[HideInInspector] public FlyMode currentFlyMode;*/
    [HideInInspector] public Vector3 airSensibility;

    private int vehicleMass;
    private float rock_forwardStiffness;
    private float sand_forwardStiffness;
    private float ice_forwardStiffness;
    private float rock_sidewaysStiffness;
    private float sand_sidewaysStiffness;
    private float ice_sidewaysStiffness;
    private float handBrakeStiffnessFactor;
    private float slopeSensibilityValue = 0.25f;

    //[Header("Sospensioni")]
    //private float suspensionDistance = 0;
    //private float suspensionSpring = 0;
    //private float suspensionDamper = 0;

    /*[Header("Turbo")]
    private float turboForce;
    private float turboConsumption;
    private float turboRechargeRate;
    [HideInInspector] public float turboQuantity = 100;*/

    [Header("Carburante")]
    [HideInInspector]
    public float currentFuelConsumption;
    [HideInInspector] public float currentFuel = 100;
    private float defaultFuelConsumption;
    private float fuelConsumptionMultiplierIce;
    private float fuelConsumptionMultiplierSun;

    //[Header("Baricentro")]
    //public Vector3 centerOfMass = new Vector3(0, -2, 0);

    [HideInInspector] public float currentSpeed;
    [HideInInspector] public float accessoriesForceMoltiplier = 1;
    private float thermalEngineVectorForce = 1;
    #endregion Stats

    private string accelerateAxis = "Triggers";
    private string triggerL = "TriggerL";
    private string triggerR = "TriggerR";
    private string bumperL = "BumperL";
    private string bumperR = "BumperR";
    private string sphereL = "SphereL";
    private string sphereR = "SphereR";
    private string horizontalInput = "HorizontalHandling";
    private string verticalInput = "VerticalHandling";
    private string accessoryDown = "Accessory1";
    private string accessoryUp = "Accessory3";

    [HideInInspector] public int currentEngineBonus = 0;
    [HideInInspector] public int ambientTemperature = int.MaxValue;
    [HideInInspector] public float currentTemperature = 0;
    [HideInInspector] public bool currentRadiation = false;
    [HideInInspector] public float currentLightQuantity = 0;
    [HideInInspector] public float currentWindSpeed = 0;
    [HideInInspector] public bool isShielded = false;
    [HideInInspector] public Vector3 currentWindDirection = Vector3.zero;
    public Material[] skidmarkMaterials;


    void Awake()
    {
        vehicleController = this;
        wheelInfos = new List<WheelInfo>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        LoadSettingsFromFiles();
        InitializeVehicleSoundSystem();
        InitializeWheelsSkidmarksSystem();
        StartCoroutine(CheckWheelContactSurface(wheelInfos));
        StartCoroutine(CheckIfVehicleIsInLight());

        /*if (chosenWheel == TypeOfWheel.Caterpillar)
        {
            var _Models = transform.Find("Wheels").Find("Cingoli").Find("Models");
            caterpillarModels = new Transform[_Models.childCount];
            for (int i = 0; i < _Models.childCount; i++)
            {
                caterpillarModels[i] = _Models.GetChild(i);
            }
        }*/

        if (chosenWheel == TypeOfWheel.Drone)
        {
            Transform modelParent = wheelInfos[0].wheelCollider.gameObject.transform.parent;

            foreach (Transform wheelModel in modelParent.Find("Models"))
            {
                dronesBlades.Add(wheelModel.GetChild(0).GetChild(2).gameObject);
            }
        }

        ambientTemperature = int.MaxValue;
        fpsCounter = gameObject.AddComponent<FPSCounter>();
    }

    private void InitializeWheelsSkidmarksSystem()
    {
        if (chosenWheel != TypeOfWheel.Propeller && chosenWheel != TypeOfWheel.Drone)
        {
            foreach (WheelInfo wheel in wheelInfos)
            {
                GameObject skidMarkObject = Instantiate(new GameObject("Skidmark"));
                skidMarkObject.transform.parent = wheel.wheelCollider.gameObject.transform;

                wheel.trailRenderer = skidMarkObject.AddComponent<TrailRenderer>();
                wheel.trailRenderer.material = skidmarkMaterials[0];
                wheel.trailRenderer.startWidth = 0.25f;
                wheel.trailRenderer.endWidth = 0f;
                wheel.trailRenderer.time = 2;
                wheel.trailRenderer.minVertexDistance = 0.1f;
                wheel.trailRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                SkidmarkScript skidmarkScript = skidMarkObject.AddComponent<SkidmarkScript>();
                skidmarkScript.trailRenderer = wheel.trailRenderer;
                skidmarkScript.wheelCollider = wheel.wheelCollider;
            }
        }
    }

    void LoadSettingsFromFiles()
    {
        ////// SETTAGGI CHASSIS //////
        rb.mass = Int32.Parse(fileManager.LoadChassisValue(1, (int)chosenChassis + 1));
        rb.centerOfMass = new Vector3(0, (float.Parse(fileManager.LoadChassisValue(3, (int)chosenChassis + 1))) + float.Parse(fileManager.LoadWheelsValue(10, (int)chosenWheel + 1)), 0);
        accessoriesForceMoltiplier = float.Parse(fileManager.LoadChassisValue(7, (int)chosenChassis + 1));
        airSensibility.y = float.Parse(fileManager.LoadChassisValue(8, (int)chosenChassis + 1));
        airSensibility.x = float.Parse(fileManager.LoadChassisValue(9, (int)chosenChassis + 1));
        airSensibility.z = float.Parse(fileManager.LoadChassisValue(10, (int)chosenChassis + 1));
        //////////////////////////////


        ////// SETTAGGI ENGINE ///////
        rb.mass += Int32.Parse(fileManager.LoadEngineValue(1, (int)chosenEngine + 1));
        currentMotorTorque = float.Parse(fileManager.LoadEngineValue(2, (int)chosenEngine + 1));
        currentMotorTorque *= float.Parse(fileManager.LoadChassisValue(2, (int)chosenChassis + 1));
        currentMotorTorque *= float.Parse(fileManager.LoadWheelsValue(11, (int)chosenWheel + 1));
        brakeTorque = float.Parse(fileManager.LoadEngineValue(3, (int)chosenEngine + 1));
        brakeTorque *= float.Parse(fileManager.LoadWheelsValue(12, (int)chosenWheel + 1));
        maxSpeed = Int32.Parse(fileManager.LoadEngineValue(4, (int)chosenEngine + 1));
        defaultFuelConsumption = float.Parse(fileManager.LoadEngineValue(5, (int)chosenEngine + 1));
        thermalEngineVectorForce = float.Parse(fileManager.LoadEngineValue(6, (int)chosenEngine + 1));
        motorTorqueMultiplierSun = float.Parse(fileManager.LoadEngineValue(7, (int)chosenEngine + 1));
        motorTorqueMultiplierRadiation = float.Parse(fileManager.LoadEngineValue(10, (int)chosenEngine + 1));
        fuelConsumptionMultiplierSun = float.Parse(fileManager.LoadEngineValue(9, (int)chosenEngine + 1));
        fuelConsumptionMultiplierIce = float.Parse(fileManager.LoadEngineValue(8, (int)chosenEngine + 1));
        //////////////////////////////


        ////// SETTAGGI WHEELS ///////           
        foreach (WheelInfo wheelInfo in wheelInfos)
        {
            wheelInfo.wheelCollider.mass = Int32.Parse(fileManager.LoadWheelsValue(1, (int)chosenWheel + 1));
            steeringAngle = Int32.Parse(fileManager.LoadWheelsValue(2, (int)chosenWheel + 1));
            wheelInfo.wheelCollider.suspensionDistance = float.Parse(fileManager.LoadChassisValue(4, (int)chosenChassis + 1));
            JointSpring wheelSpring = wheelInfo.wheelCollider.suspensionSpring;
            wheelSpring.spring = float.Parse(fileManager.LoadChassisValue(5, (int)chosenChassis + 1));
            wheelSpring.damper = float.Parse(fileManager.LoadChassisValue(6, (int)chosenChassis + 1));
            wheelInfo.wheelCollider.suspensionSpring = wheelSpring;
            
            wheelSettingsDebug = "\n\nWHEEL MASS: " + wheelInfo.wheelCollider.mass +
            "\nWHEEL SUSPENSION DISTANCE: " + wheelInfo.wheelCollider.suspensionDistance +
            "\nWHEEL SPRING: " + wheelSpring.spring +
            "\nWHEEL DAMPER: " + wheelSpring.damper;
        }

        rock_forwardStiffness = float.Parse(fileManager.LoadWheelsValue(3, (int)chosenWheel + 1));
        rock_sidewaysStiffness = float.Parse(fileManager.LoadWheelsValue(4, (int)chosenWheel + 1));
        sand_forwardStiffness = float.Parse(fileManager.LoadWheelsValue(5, (int)chosenWheel + 1));
        sand_sidewaysStiffness = float.Parse(fileManager.LoadWheelsValue(6, (int)chosenWheel + 1));
        ice_forwardStiffness = float.Parse(fileManager.LoadWheelsValue(7, (int)chosenWheel + 1));
        ice_sidewaysStiffness = float.Parse(fileManager.LoadWheelsValue(8, (int)chosenWheel + 1));
        defaultFuelConsumption += float.Parse(fileManager.LoadWheelsValue(9, (int)chosenWheel + 1));
        slopeSensibilityValue = float.Parse(fileManager.LoadWheelsValue(13, (int)chosenWheel + 1));
        ////////////////////////////////

        defaulMotorTorque = currentMotorTorque;
        flyModeAlternative = (chosenWheel == TypeOfWheel.Caterpillar || chosenWheel == TypeOfWheel.Sphere || chosenWheel == TypeOfWheel.Drone);
    }

    void InitializeVehicleSoundSystem()
    {
        if (engineAudioSource == null)
        {
            engineAudioSource = gameObject.AddComponent<AudioSource>();
        }
        
        engineAudioSource.clip = AudioManager.Audio.GetVehicleSoundSettings()[(int)chosenEngine].audioFile;
        engineAudioSource.spatialBlend = AudioManager.Audio.GetVehicleSoundSettings()[(int)chosenEngine].from2dTO3d;
        engineAudioSource.loop = AudioManager.Audio.GetVehicleSoundSettings()[(int)chosenEngine].loop;
        engineAudioSource.minDistance = AudioManager.Audio.GetVehicleSoundSettings()[(int)chosenEngine].minDistance;
        engineAudioSource.maxDistance = AudioManager.Audio.GetVehicleSoundSettings()[(int)chosenEngine].maxDistance;
        engineAudioSource.volume = 0;
        minEngineVolume = AudioManager.Audio.GetVehicleSoundSettings()[(int)chosenEngine].minVolume;
        maxEngineVolume = AudioManager.Audio.GetVehicleSoundSettings()[(int)chosenEngine].maxVolume;
        minEnginePitch = AudioManager.Audio.GetVehicleSoundSettings()[(int)chosenEngine].minPitch;
        maxEnginePitch = AudioManager.Audio.GetVehicleSoundSettings()[(int)chosenEngine].maxPitch;

        if (chosenWheel != TypeOfWheel.Drone && chosenWheel != TypeOfWheel.Propeller)
        {
            wheelContactClipRock = AudioManager.Audio.GetVehicleSoundSettings()[5].audioFile;
            minWheelContactVolumeRock = AudioManager.Audio.GetVehicleSoundSettings()[5].minVolume;
            maxWheelContactVolumeRock = AudioManager.Audio.GetVehicleSoundSettings()[5].maxVolume;
            minWheelContactPitchRock = AudioManager.Audio.GetVehicleSoundSettings()[5].minPitch;
            maxWheelContactPitchRock = AudioManager.Audio.GetVehicleSoundSettings()[5].maxPitch;

            wheelContactClipSand = AudioManager.Audio.GetVehicleSoundSettings()[6].audioFile;
            minWheelContactVolumeSand = AudioManager.Audio.GetVehicleSoundSettings()[6].minVolume;
            maxWheelContactVolumeSand = AudioManager.Audio.GetVehicleSoundSettings()[6].maxVolume;
            minWheelContactPitchSand = AudioManager.Audio.GetVehicleSoundSettings()[6].minPitch;
            maxWheelContactPitchSand = AudioManager.Audio.GetVehicleSoundSettings()[6].maxPitch;

            wheelContactClipIce = AudioManager.Audio.GetVehicleSoundSettings()[7].audioFile;
            minWheelContactVolumeIce = AudioManager.Audio.GetVehicleSoundSettings()[7].minVolume;
            maxWheelContactVolumeIce = AudioManager.Audio.GetVehicleSoundSettings()[7].maxVolume;
            minWheelContactPitchIce = AudioManager.Audio.GetVehicleSoundSettings()[7].minPitch;
            maxWheelContactPitchIce = AudioManager.Audio.GetVehicleSoundSettings()[7].maxPitch;
        }
        else
        {
            int flyingIndex = chosenWheel == TypeOfWheel.Drone ? 8 : 9;
            wheelContactClipFlying = AudioManager.Audio.GetVehicleSoundSettings()[flyingIndex].audioFile;
            minWheelContactVolumeFlying = AudioManager.Audio.GetVehicleSoundSettings()[flyingIndex].minVolume;
            maxWheelContactVolumeFlying = AudioManager.Audio.GetVehicleSoundSettings()[flyingIndex].maxVolume;
            minWheelContactPitchFlying = AudioManager.Audio.GetVehicleSoundSettings()[flyingIndex].minPitch;
            maxWheelContactPitchFlying = AudioManager.Audio.GetVehicleSoundSettings()[flyingIndex].maxPitch;
        }

        foreach (WheelInfo wheel in wheelInfos)
        {
            wheel.wheelCollider.gameObject.AddComponent<AudioSource>();
            AudioSource wheelAudioSource = wheel.wheelCollider.gameObject.GetComponent<AudioSource>();
            wheelAudioSource.clip = wheelContactClipSand;
            wheelAudioSource.spatialBlend = AudioManager.Audio.GetVehicleSoundSettings()[5].from2dTO3d;
            wheelAudioSource.loop = AudioManager.Audio.GetVehicleSoundSettings()[5].loop;
            wheelAudioSource.minDistance = AudioManager.Audio.GetVehicleSoundSettings()[5].minDistance;
            wheelAudioSource.maxDistance = AudioManager.Audio.GetVehicleSoundSettings()[5].maxDistance;
            wheelAudioSource.volume = 0;
            wheel.audioSource = wheelAudioSource;
        }

        if (resistenceAirAudioSource == null)
        {
            resistenceAirAudioSource = gameObject.AddComponent<AudioSource>();
        }
        
        resistenceAirAudioSource.clip = AudioManager.Audio.GetVehicleSoundSettings()[10].audioFile;
        resistenceAirAudioSource.spatialBlend = AudioManager.Audio.GetVehicleSoundSettings()[10].from2dTO3d;
        resistenceAirAudioSource.loop = AudioManager.Audio.GetVehicleSoundSettings()[10].loop;
        resistenceAirAudioSource.minDistance = AudioManager.Audio.GetVehicleSoundSettings()[10].minDistance;
        resistenceAirAudioSource.maxDistance = AudioManager.Audio.GetVehicleSoundSettings()[10].maxDistance;
        resistenceAirAudioSource.volume = 0;
        minResistenceAirVolume = AudioManager.Audio.GetVehicleSoundSettings()[10].minVolume;
        maxResistenceAirVolume = AudioManager.Audio.GetVehicleSoundSettings()[10].maxVolume;
        minResistenceAirPitch = AudioManager.Audio.GetVehicleSoundSettings()[10].minPitch;
        maxResistenceAirPitch = AudioManager.Audio.GetVehicleSoundSettings()[10].maxPitch;
    }

    void ApplyLocalPositionToVisuals(WheelInfo wheelInfo)
    {
        WheelCollider wheelCollider = wheelInfo.wheelCollider;
        Vector3 position;
        Quaternion rotation;

        if (chosenWheel != TypeOfWheel.Drone && chosenWheel != TypeOfWheel.Propeller)
        {
            Transform visualWheel = wheelCollider.transform.GetChild(0);
            wheelCollider.GetWorldPose(out position, out rotation);
            visualWheel.transform.position = position;
            visualWheel.transform.rotation = rotation;
        }
        else if (chosenWheel == TypeOfWheel.Drone)
        {
            foreach (GameObject blade in dronesBlades)
            {
                if (Input.GetAxis(accelerateAxis) > 0)
                {
                    blade.transform.Rotate(Vector3.forward, 3f + Input.GetAxis(accelerateAxis)*22);
                }
                else
                {
                    blade.transform.Rotate(Vector3.back, 3f + Mathf.Abs(Input.GetAxis(accelerateAxis)*22));
                }
            }     
        }
    }

    void FixedUpdate()
    {
        if (ambientTemperature == int.MaxValue)
        {
            GameObject globalTemperatureObject = GameObject.Find("GlobalEnvTemperature");
            if (globalTemperatureObject != null)
            {
                ambientTemperature = (int)globalTemperatureObject.GetComponent<EnvPropertyScript>().value;
            }
        }

        if (currentTerrain == null)
        {
            if (FindObjectOfType<Terrain>() != null)
            {
                currentTerrain = FindObjectOfType<Terrain>();
                terrainCheck = FindObjectOfType<TerrainCheck>();
            }
        }

        if (Input.GetKeyDown(KeyCode.Insert))
        {
            showDebugValues = !showDebugValues;
            InitializeVehicleSoundSystem();
        }

        if (debugText != null)
        {
            debugText.text = "FPS: " + fpsCounter.GetFPS() + " ";
        }
        else
        {
            if (transform.Find("Canvas") != null)
            {
                debugText = transform.Find("Canvas").GetComponent<Text>();
                debugText.color = Color.green;
                debugText.fontSize = 12;
                debugText.fontStyle = FontStyle.Bold;
            }
        }

        if (showDebugValues)
        {
            if (debugText != null)
            {
                string debugString = "";
                debugString = "(REALTIME) CAR DEBUGING: " +
                "\n\nCHASSIS TYPE: " + chosenChassis +
                "\nENGINE TYPE: " + chosenEngine +
                "\nWHEEL TYPE: " + chosenWheel +
                "\n\nACCESS 1: " + chosenAccessory1 +
                "\nACCESS 2: " + chosenAccessory2 +
                "\nACCESS 3: " + chosenAccessory3 +
                "\nACCESS 4: " + chosenAccessory4 +
                "\n\nVEHICLE MASS: " + rb.mass +
                "\nCENTER OF MASS Y: " + rb.centerOfMass.y +
                "\n\nAIR SENSIBILITY: " + airSensibility +
                "\n\nMOTOR TORQUE: " + currentMotorTorque +
                "\nBRAKE TORQUE: " + brakeTorque +
                "\n\nMAX SPEED: " + maxSpeed +
                "\nFUEL CONSUMPTION: " + currentFuelConsumption +
                "\n\nROCK FORWARD: " + rock_forwardStiffness +
                "\nROCK SIDEWAY: " + rock_sidewaysStiffness +
                "\nSAND FORWARD: " + sand_forwardStiffness +
                "\nSAND SIDEWAY: " + sand_sidewaysStiffness +
                "\nICE FORWARD: " + ice_forwardStiffness +
                "\nICE SIDEWAY: " + ice_sidewaysStiffness + wheelSettingsDebug +
                "\n\nWHEELS CONTACTS: " + wheelsTerrainContactDebug;
                debugText.text += debugString;
            }
        }
        else
        {
            if (isDebugShowed)
            {
                if (debugText != null)
                {
                    debugText.text = "";
                }

                isDebugShowed = false;
            }
        }

        foreach (WheelInfo wheelInfo in wheelInfos)
        {
            if (Quest.currentQuest != null)
            {
                if (Quest.currentQuest.CurrentQuestStatus != Quest.MissionStatus.WAITING)
                {
                    Acceleration(wheelInfo);
                    Steering(wheelInfo);
                    DecelerationAndBraking(wheelInfo);
                }
            }

            ApplyLocalPositionToVisuals(wheelInfo);
        }

        ApplyInfluencesBasedOnEngineType();
        CheckIfVehicleIsFlying();
        FlyingControl();
        StartCoroutine(FuelControl());
        CalculateSpeed();
        CalculateEngineSoundModulation();
        CalculateResistenceAirModulation();
    }

    /*void SetWheelsProperties(WheelInfo wheelInfo)
    {
        wheelInfo.wheelCollider.mass = wheels[(int)chosenWheel].weight;
        wheelInfo.wheelCollider.suspensionDistance = suspensionDistance;

        JointSpring wheelSpring = wheelInfo.wheelCollider.suspensionSpring;
        wheelSpring.spring = suspensionSpring;
        wheelSpring.damper = suspensionDamper;

        wheelInfo.wheelCollider.suspensionSpring = wheelSpring;
    }*/

    void ApplyInfluencesBasedOnEngineType()
    {
        currentMotorTorque = defaulMotorTorque;
        currentFuelConsumption = defaultFuelConsumption;
        currentEngineBonus = 0;
        float excursionDifference = 0;

        foreach (EnvPropertyScript influenceZone in currentInfluenceZones)
        {
            currentTemperature = ambientTemperature;
            currentTemperature -= isInShadow ? 40 : 0;
            currentRadiation = false;

            string influenceTag = influenceZone.gameObject.tag;

            if (influenceTag == "EnvPropertyRadiation" && !isShielded)
            {
                currentRadiation = true; //influenceZone.value;
                currentMotorTorque *= motorTorqueMultiplierRadiation;
            }

            if (influenceTag == "EnvPropertyTemperature")
            {
                float newTemperatureTemp = isInShadow ? influenceZone.value - 40 : influenceZone.value;

                if (currentTemperature >= newTemperatureTemp)
                {
                    excursionDifference = Mathf.Abs(currentTemperature) - newTemperatureTemp;
                }
                else
                {
                    excursionDifference = Mathf.Abs(newTemperatureTemp) - currentTemperature;
                }

                currentTemperature = newTemperatureTemp;
            }
        }

        if (chosenEngine == TypeOfEngine.Magnetic)
        {
            if (currentRadiation)
            {
                if (!isShielded)
                {
                    currentMotorTorque *= motorTorqueMultiplierRadiation;
                    currentEngineBonus = -1;
                }
            }
            else
            {
                currentEngineBonus = 1;
            }
        }
        else if (chosenEngine == TypeOfEngine.PhotoSynthesis)
        {
            if (!isInShadow)
            {
                currentMotorTorque *= motorTorqueMultiplierSun;
                currentFuelConsumption *= fuelConsumptionMultiplierSun;
                currentEngineBonus = 1;
            }
            else
            {
                currentEngineBonus = -1;
            }

            currentMotorTorque *= currentLightQuantity == 0 ? 1 : currentLightQuantity;
        }
        else if (chosenEngine == TypeOfEngine.ThermalExcursion)
        {
            if (lastTemperatureTemp != currentTemperature)
            {
                lastTemperatureTemp = currentTemperature;
                excursionDifference += excursionDifference == 0 ? 40 : 0;

                if (currentSpeed != 0)
                {
                    rb.AddForce(transform.forward * (excursionDifference * thermalEngineVectorForce), ForceMode.VelocityChange);
                    currentEngineBonus = 1;
                }
                else
                {
                    currentEngineBonus = -1;
                }
            }
            else
            {
                currentEngineBonus = -1;
            }   
        }
        else if (chosenEngine == TypeOfEngine.DryIce)
        {
            if (!isInShadow)
            {
                currentMotorTorque *= motorTorqueMultiplierSun;
            }

            currentMotorTorque *= (1 + ((currentTemperature + 60) * 0.01f));

            if (currentTemperature < -60)
            {
                currentEngineBonus = -1;
            }
            else if (currentTemperature >= -60 && currentTemperature <= -20)
            {
                currentEngineBonus = 0;
            }
            else if (currentTemperature > -20)
            {
                currentEngineBonus = 1;
            }
        }
        else if (chosenEngine == TypeOfEngine.Nuclear)
        {
            currentEngineBonus = 1;
        }  
    }

    void Acceleration(WheelInfo wheelInfo)
    {
        if (!isFlying)
        {
            if (currentFuel > 0)
            {
                float motor = 0;

                if (chosenWheel != TypeOfWheel.Caterpillar && chosenWheel != TypeOfWheel.Sphere)
                {
                    motor = currentMotorTorque * Input.GetAxis(accelerateAxis);

                    if (wheelInfo.motor)
                    {
                        wheelInfo.wheelCollider.motorTorque = motor;
                    }
                }
                else if (chosenWheel == TypeOfWheel.Caterpillar)
                {
                    float catMotorL = currentMotorTorque * Input.GetAxis(triggerL);
                    float catMotorR = currentMotorTorque * Input.GetAxis(triggerR);
                    float catRearMotorL = currentMotorTorque * Input.GetAxis(bumperL);
                    float catRearMotorR = currentMotorTorque * Input.GetAxis(bumperR);

                    if (catMotorL + catMotorR + catRearMotorL + catRearMotorR != 0)
                    {
                        wheelInfo.wheelCollider.brakeTorque = 0;
                    }

                    if (wheelInfo.wheelCollider.tag == "WheelL" && wheelInfo.motor)
                    {
                        if (Input.GetAxis(triggerL) > 0)
                        {
                            wheelInfo.wheelCollider.motorTorque = catMotorL;
                        }

                        if (Input.GetAxis(bumperL) > 0)
                        {
                            wheelInfo.wheelCollider.motorTorque = -(catRearMotorL);
                        }
                    }
                    else if (wheelInfo.wheelCollider.tag == "WheelR"
                            && Input.GetAxis(triggerL) > 0
                            && Input.GetAxis(triggerR) == 0
                            && currentSpeed > 5)
                    {
                        wheelInfo.wheelCollider.brakeTorque = 1000;
                    }

                    if (wheelInfo.wheelCollider.tag == "WheelR" && wheelInfo.motor)
                    {
                        if (Input.GetAxis(triggerR) > 0)
                        {
                            wheelInfo.wheelCollider.motorTorque = catMotorR;
                        }

                        if (Input.GetAxis(bumperR) > 0)
                        {
                            wheelInfo.wheelCollider.motorTorque = -(catRearMotorR);
                        }
                    }
                    else if (wheelInfo.wheelCollider.tag == "WheelL"
                            && Input.GetAxis(triggerR) > 0
                            && Input.GetAxis(triggerL) == 0
                            && currentSpeed > 5)
                    {
                        wheelInfo.wheelCollider.brakeTorque = 1000;
                    }
                }
                else if (chosenWheel == TypeOfWheel.Sphere)
                {
                    motor = (currentMotorTorque * Input.GetAxis(accelerateAxis));

                    if (wheelInfo.motor)
                    {
                        wheelInfo.wheelCollider.motorTorque = motor;
                    }

                    if (Input.GetAxis(sphereL) == 0 && Input.GetAxis(sphereR) == 0 && Input.GetAxis(horizontalInput) == 0 && Input.GetAxis(verticalInput) == 0)
                    {
                        wheelInfo.wheelCollider.steerAngle = 0;
                    }
                }
            }
            else
            {
                wheelInfo.wheelCollider.motorTorque = 0;
            }

            if (currentSpeed >= maxSpeed || currentSpeed <= -maxSpeed || (Input.GetAxis(accelerateAxis) == 0 &&
                (Input.GetAxis(triggerL) + Input.GetAxis(triggerR) + Input.GetAxis(bumperL) + Input.GetAxis(bumperR) == 0)))
            {
                wheelInfo.wheelCollider.motorTorque = 0;
            }
        }
        else
        {
            wheelInfo.wheelCollider.motorTorque = 0;

            if (wheelInfo.wheelCollider.rpm != 0)
            {
                wheelInfo.wheelCollider.brakeTorque = 1000;
            }
            else
            {
                wheelInfo.wheelCollider.brakeTorque = 0;
            }
        }
    }

    void Steering(WheelInfo wheelInfo)
    {
        if (chosenWheel != TypeOfWheel.Caterpillar)
        {
            float steering = (steeringAngle - (currentSpeed / 8)) * Input.GetAxis(horizontalInput);

            if (chosenWheel == TypeOfWheel.Sphere)
            {
                if (!Input.GetButton(sphereL) && !Input.GetButton(sphereR))
                {
                    if (Input.GetAxis(verticalInput) >= 0.9f && Input.GetAxis(horizontalInput) >= 0.9f)
                    {
                        steering = 45;
                    }
                    else if (Input.GetAxis(verticalInput) == 0 && Input.GetAxis(horizontalInput) >= 0.9f)
                    {
                        steering = 90;
                    }
                    else if (Input.GetAxis(verticalInput) <= -0.9f && Input.GetAxis(horizontalInput) >= 0.9f)
                    {
                        steering = 135;
                    }
                    else if (Input.GetAxis(verticalInput) <= -0.9f && Input.GetAxis(horizontalInput) <= -0.9f)
                    {
                        steering = -135;
                    }
                    else if (Input.GetAxis(verticalInput) == 0 && Input.GetAxis(horizontalInput) <= -0.9f)
                    {
                        steering = -90;
                    }
                    else if (Input.GetAxis(verticalInput) >= 0.9f && Input.GetAxis(horizontalInput) <= -0.9f)
                    {
                        steering = -45;
                    }
                }
                else
                {
                    if (wheelInfo.steering)
                    {
                        if (Input.GetAxis(sphereL) > 0)
                        {
                            steering = -steeringAngle;
                        }
                        else if (Input.GetAxis(sphereR) > 0)
                        {
                            steering = steeringAngle;
                        }
                    }
                }
                wheelInfo.wheelCollider.steerAngle = steering;
            }
            else
            {
                if (wheelInfo.steering)
                {
                    wheelInfo.wheelCollider.steerAngle = steering;
                }
            }
        }
    }

    void HandBrake(WheelInfo wheelInfo)
    {
        if (wheelInfo.handbraking)
        {
            if (Input.GetButton("Jump"))
            {
                wheelInfo.wheelCollider.brakeTorque = brakeTorque;

                if (currentSpeed != 0)
                {
                    SetWheelFriction(wheelInfo.wheelCollider, forwardStiffness / handBrakeStiffnessFactor, sidewaysStiffness / handBrakeStiffnessFactor);
                }
            }
        }
    }

    void DecelerationAndBraking(WheelInfo wheelInfo)
    {
        if (chosenWheel != TypeOfWheel.Caterpillar && chosenWheel != TypeOfWheel.Sphere)
        {
            if (Input.GetAxis(accelerateAxis) == 0)
            {
                wheelInfo.wheelCollider.motorTorque = 0;
            }
            else if (Input.GetAxis(accelerateAxis) < 0 && currentSpeed > 0)
            {
                wheelInfo.wheelCollider.motorTorque = 0;
                wheelInfo.wheelCollider.brakeTorque = brakeTorque;
            }
            else
            {
                wheelInfo.wheelCollider.brakeTorque = 0;
            }
        }
        else
        {
            if (Input.GetAxis(sphereL) >= 0 || Input.GetAxis(sphereR) >= 0)
            {
                wheelInfo.wheelCollider.brakeTorque = 0;
            }
        }
    }

    void SetWheelFriction(WheelCollider wheelCollider, float forwardStiffness, float sidewaysStiffness)
    {
        WheelFrictionCurve tempForwardFriction = (WheelFrictionCurve)wheelCollider.forwardFriction;
        tempForwardFriction.stiffness = forwardStiffness;
        wheelCollider.forwardFriction = tempForwardFriction;

        WheelFrictionCurve tempSidewaysFriction = (WheelFrictionCurve)wheelCollider.sidewaysFriction;
        tempSidewaysFriction.stiffness = sidewaysStiffness;
        wheelCollider.sidewaysFriction = tempSidewaysFriction;
    }

    void CalculateSpeed()
    {
        currentSpeed = Mathf.Round(transform.InverseTransformDirection(rb.velocity).z * 3.6f);
    }

    void CalculateEngineSoundModulation()
    {
        if (Input.GetAxis(accelerateAxis) != 0)
        {
            float pitchEngineValue = (((maxEnginePitch - minEnginePitch) * Mathf.Abs(currentSpeed)) / maxSpeed) + minEnginePitch;
            float volumeEngineValue = (((maxEngineVolume - minEngineVolume) * Mathf.Abs(currentSpeed)) / maxSpeed) + minEngineVolume;

            AudioManager.Audio.SetPitch(engineAudioSource, pitchEngineValue);
            AudioManager.Audio.SetVolume(engineAudioSource, volumeEngineValue);
        }
        else
        {
            AudioManager.Audio.SetPitchOverTime(engineAudioSource, -1f * Time.deltaTime, minEnginePitch, 0);
            AudioManager.Audio.SetVolumeOverTime(engineAudioSource, -1f * Time.deltaTime, minEngineVolume, 0);
        }
    }

    void CalculateResistenceAirModulation()
    {
        float pitchAirValue = (((maxResistenceAirPitch - minResistenceAirPitch) * Mathf.Abs(currentSpeed)) / maxSpeed) + minResistenceAirPitch;
        float volumeAirValue = (((maxResistenceAirVolume - minResistenceAirVolume) * Mathf.Abs(currentSpeed)) / maxSpeed) + minResistenceAirVolume;

        AudioManager.Audio.SetPitch(resistenceAirAudioSource, pitchAirValue);
        AudioManager.Audio.SetVolume(resistenceAirAudioSource, volumeAirValue);
    }

    IEnumerator CheckWheelContactSurface(List<WheelInfo> wheels)
    {
        while (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == ScenesContainer.MENU_SELEZIONE)
        {
            yield return null;
        }
                
        pm = Quest.currentQuest.gameObject.GetComponent<ParticleManager>();

        while (true)
        {
            if (showDebugValues)
            {
                wheelsTerrainContactDebug = "";
            }

            int wheelIndexCount = 0;
            
            foreach (WheelInfo wheelInfo in wheels)
            {
                int terrainIndex = 1;
                WheelCollider wheel = wheelInfo.wheelCollider;
                WheelHit hit;
                if (wheel.GetGroundHit(out hit))
                {
                    if (hit.collider.tag == "Terrain")
                    {
                        terrainCheck = hit.collider.GetComponent<TerrainCheck>();

                        if (terrainCheck != null)
                        {
                            currentFuelConsumption = defaultFuelConsumption;

                            contactTexture = terrainCheck.GetTerrainTextureAt(hit.point, currentTerrain);

                            if (terrainCheck.RockTexture.Contains(contactTexture))
                            {
                                forwardStiffness = rock_forwardStiffness;
                                sidewaysStiffness = rock_sidewaysStiffness;
                                terrainIndex = 0;

                                pm.SetRockParticle();  
                            }
                            else if (terrainCheck.SandTexture.Contains(contactTexture))
                            {
                                forwardStiffness = sand_forwardStiffness;
                                sidewaysStiffness = sand_sidewaysStiffness;
                                terrainIndex = 1;

                                pm.SetSandParticle();                                    
                            }
                            else if (terrainCheck.IceTexture.Contains(contactTexture))
                            {
                                forwardStiffness = ice_forwardStiffness;
                                sidewaysStiffness = ice_sidewaysStiffness;
                                terrainIndex = 2;

                                pm.SetIceParticle();

                                currentFuelConsumption *= 0.5f;
                                if (chosenEngine == TypeOfEngine.DryIce)
                                {
                                    currentFuelConsumption *= fuelConsumptionMultiplierIce;
                                }
                            }
                            else
                            {
                                forwardStiffness = sand_forwardStiffness;
                                sidewaysStiffness = sand_sidewaysStiffness;
                            }
                        }

                        if (chosenWheel != TypeOfWheel.Suckers)
                        {
                            float slopeValue = GetWheelSurfaceSlope(wheel.transform.up);
                            
                            if (slopeValue < 0.8f)
                            {
                                slopeValue -= slopeSensibilityValue;

                                forwardStiffness *= slopeValue;
                                sidewaysStiffness *= slopeValue;
                            }
                        }

                        SetWheelFriction(wheel, forwardStiffness, sidewaysStiffness);

                        if (showDebugValues)
                        {
                            switch (terrainIndex)
                            {
                                case 0: wheelsTerrainContactDebug += "\n" + wheel.name + " on ROCK"; break;
                                case 1: wheelsTerrainContactDebug += "\n" + wheel.name + " on SAND"; break;
                                case 2: wheelsTerrainContactDebug += "\n" + wheel.name + " on ICE"; break;
                                default: wheelsTerrainContactDebug += "\n" + wheel.name + " on NOTHING"; break;
                            }

                            wheelsTerrainContactDebug += "\nForward Stiff: " + forwardStiffness;
                            wheelsTerrainContactDebug += "\nSideway Stiff: " + sidewaysStiffness;
                        }
                    }
                }
                else
                {
                    pm.DeactivateParticle();

                    if (showDebugValues)
                    {
                        wheelsTerrainContactDebug += "\n" + wheel.name + " on FLYING";
                    }
                }

                SetWheelContactSound(wheelInfo, terrainIndex);

                if (wheelInfo.trailRenderer != null)
                {
                    wheelInfo.trailRenderer.material = (terrainIndex >= 0 && terrainIndex <= 2) ? skidmarkMaterials[terrainIndex] : skidmarkMaterials[0];
                }
                                
                wheelIndexCount = wheelIndexCount == wheelInfos.Count ? 0 : wheelIndexCount += 1;
            }

            yield return new WaitForSeconds(0.1f);
        }

        yield return null;
    }

    void SetWheelContactSound(WheelInfo wheelInfo, int surfaceIndex)
    {
        float maxWheelContactVolume = 0;
        float minWheelContactVolume = 0;
        float maxWheelContactPitch = 0;
        float minWheelContactPitch = 0;

        switch (surfaceIndex)
        {
            case 0:
                wheelInfo.audioSource.clip = wheelContactClipRock;
                maxWheelContactVolume = maxWheelContactVolumeRock;
                minWheelContactVolume = minWheelContactVolumeRock;
                maxWheelContactPitch = maxWheelContactPitchRock;
                minWheelContactPitch = minWheelContactPitchRock;
                break;
            case 1:
                wheelInfo.audioSource.clip = wheelContactClipSand;
                maxWheelContactVolume = maxWheelContactVolumeSand;
                minWheelContactVolume = minWheelContactVolumeSand;
                maxWheelContactPitch = maxWheelContactPitchSand;
                minWheelContactPitch = minWheelContactPitchSand;
                break;
            case 2:
                wheelInfo.audioSource.clip = wheelContactClipIce;
                maxWheelContactVolume = maxWheelContactVolumeIce;
                minWheelContactVolume = minWheelContactVolumeIce;
                maxWheelContactPitch = maxWheelContactPitchIce;
                minWheelContactPitch = minWheelContactPitchIce;
                break;
            case 3:
                wheelInfo.audioSource.clip = wheelContactClipFlying;
                maxWheelContactVolume = maxWheelContactVolumeFlying;
                minWheelContactVolume = minWheelContactVolumeFlying;
                maxWheelContactPitch = maxWheelContactPitchFlying;
                minWheelContactPitch = minWheelContactPitchFlying;
                break;
        }

        float wheelVolume = (((maxWheelContactVolume - minWheelContactVolume) * Mathf.Abs(currentSpeed)) / maxSpeed) + minWheelContactVolume;
        float wheelPitch = (((maxWheelContactPitch - minWheelContactPitch) * Mathf.Abs(currentSpeed)) / maxSpeed) + minWheelContactPitch;

        if (!isFlying && currentSpeed != 0)
        {
            AudioManager.Audio.SetPitch(wheelInfo.audioSource, wheelPitch);
            AudioManager.Audio.SetVolume(wheelInfo.audioSource, wheelVolume);
        }
        else
        {
            AudioManager.Audio.SetVolumeOverTime(wheelInfo.audioSource, -2.5f * Time.deltaTime, 0, maxWheelContactVolume);
        }
    }

    float GetWheelSurfaceSlope(Vector3 contactPoint)
    {
        float slopeValue = Vector3.Dot(currentTerrain.transform.up, contactPoint);
        return slopeValue; //< 0 ? slopeValue*-1 : 1;
    }

    void CheckIfVehicleIsFlying()
    {
        int wheelHittingCount = 0;

        foreach (WheelInfo wheelInfo in wheelInfos)
        {
            WheelHit hit;
            if (wheelInfo.wheelCollider.GetGroundHit(out hit))
            {
                wheelHittingCount++;
            }
        }

        if (isFlying != (wheelHittingCount == 0))
        {
            isFlying = (wheelHittingCount == 0);
        }
    }

    void FlyingControl()
    {
        if (isFlying && flyingControlEnabled)
        {
            float axes1 = 0, axes2 = 0, axes3 = 0;

            if (!flyModeAlternative)
            {
                axes1 = Input.GetAxis(verticalInput) * (airSensibility.y / 5); //beccheggio Y
                axes2 = Input.GetAxis(horizontalInput) * (airSensibility.x / 5); //imbardata Z
                axes3 += Input.GetAxis(bumperL); //rollio X
                axes3 -= Input.GetAxis(bumperR);
                axes3 *= (airSensibility.z / 5);
            }
            else
            {
                axes1 = Input.GetAxis(verticalInput) * (airSensibility.y / 5); //beccheggio Y
                axes3 = Input.GetAxis(horizontalInput) * (airSensibility.x / 5); //rollio X
                axes2 += Input.GetAxis(bumperR); //imbardata Z
                axes2 -= Input.GetAxis(bumperL);
                axes2 *= (airSensibility.z / 5);
            }

            rb.AddRelativeTorque(axes1, axes2, axes3, ForceMode.VelocityChange);
        }
    }

    IEnumerator FuelControl()
    {
        if (Input.GetAxis(accelerateAxis) != 0)
        {
            if ((Input.GetAxis(accelerateAxis) > 0 && currentSpeed >= 0) ||
                (Input.GetAxis(accelerateAxis) < 0 && currentSpeed <= 0))
            {
                if (currentFuel > 0)
                {
                    if (Time.time >= fuelLastTime + 1)
                    {
                        currentFuel -= currentFuelConsumption;

                        if (currentFuel < 0)
                        {
                            currentFuel = 0;
                        }

                        fuelLastTime = Time.time;
                    }
                }
            }
        }

        yield return null;
    }

    IEnumerator CheckIfVehicleIsInLight()
    {
        while (true)
        {
            if (sunLight != null)
            {
                float sunAngle = sunLight.transform.rotation.eulerAngles.x;
                Vector3 sunDirection = -sunLight.transform.forward * 1000;
                currentLightQuantity = sunAngle / 90;
                bool wasInShadow = isInShadow;
                isInShadow = false;

                RaycastHit hit;
                if (Physics.Raycast(transform.position, sunDirection, out hit, LayerMask.GetMask("InfluenceZone")))
                {
                    if (hit.collider.gameObject.tag == "Terrain")
                    {
                        isInShadow = true;
                        currentLightQuantity = 0;
                    }
                }

                if (wasInShadow != isInShadow)
                {
                    if (isInShadow)
                    {
                        currentTemperature -= 40;
                    }
                    else
                    {
                        currentTemperature += 40;
                    }

                    lastTemperatureTemp = int.MaxValue;
                    wasInShadow = isInShadow;
                }

                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                sunLight = GameObject.Find("Directional Light");
                yield return null;
            }
        }
    }

    /*private void OnDrawGizmos() // X TEST CHECK LIGHT
    {
        if (sunLight != null)
        {
            Vector3 sunDirection = -sunLight.transform.forward * 1000;
            Gizmos.DrawRay(transform.position, sunDirection);
            Gizmos.DrawSphere(transform.position + transform.TransformVector(rb.centerOfMass), 0.5f);
        }
    }*/

    [Serializable]
    public class WheelInfo
    {
        public WheelCollider wheelCollider;
        public bool motor;
        public bool steering;
        public bool handbraking;
        public AudioSource audioSource = null;
        public TrailRenderer trailRenderer = null;
    }
}

