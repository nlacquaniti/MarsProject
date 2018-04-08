using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class EnvPropertyScript : MonoBehaviour
{
    public enum PropertyType { LIGHT, TEMPERATURE, RADIATION, WIND }
    public PropertyType propertyType;
    public float value = 0;
    bool onWind, onRadiation = false;
    bool windCoroutineIsPlaying, radiationCoroutineIsPlaying = false;
    AudioSource audioSource = null;
    private HUDComponentList HUDList;
    private float defaultAlpha;
    public bool hasShielding;
    public Color mapRender;

    void Start()
    {
        HUDList = GameObject.Find("HUD").GetComponent<HUDComponentList>();
        //defaultAlpha = HUDList.HUDImages[0].color.a;
        mapRender = HUDList.miniMap.color;

        switch (propertyType)
        {
            case PropertyType.LIGHT: tag = "EnvPropertyLight"; break;
            case PropertyType.TEMPERATURE: tag = "EnvPropertyTemperature"; break;
            case PropertyType.RADIATION: tag = "EnvPropertyRadiation"; break;
            case PropertyType.WIND: tag = "EnvPropertyWind"; break;
        }

        if (gameObject.GetComponent<EnvIcon>() == null && propertyType != PropertyType.LIGHT && propertyType != PropertyType.TEMPERATURE)
        {
            this.gameObject.AddComponent<EnvIcon>();
        }


        foreach (AudioSource source in GetComponents<AudioSource>())
        {
            Destroy(source);
        }

        InitializeSound();
    }

    void InitializeSound()
    {
        if (propertyType == PropertyType.WIND)
        {
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            audioSource.maxDistance = transform.lossyScale.x / 1.25f;
            audioSource.loop = true;
            audioSource.spatialBlend = 1;
            audioSource.clip = AudioManager.Audio.GetInfluenceZonesSoundSettings()[0].audioFile;
            audioSource.volume = AudioManager.Audio.GetInfluenceZonesSoundSettings()[0].volume;
            audioSource.pitch = AudioManager.Audio.GetInfluenceZonesSoundSettings()[0].pitch;
        }
        else if (propertyType == PropertyType.RADIATION)
        {
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            audioSource.maxDistance = transform.lossyScale.x / 1.25f;
            audioSource.loop = true;
            audioSource.spatialBlend = 1;
            audioSource.clip = AudioManager.Audio.GetInfluenceZonesSoundSettings()[1].audioFile;
            audioSource.volume = AudioManager.Audio.GetInfluenceZonesSoundSettings()[1].volume;
            audioSource.pitch = AudioManager.Audio.GetInfluenceZonesSoundSettings()[1].pitch;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Insert))
        {
            InitializeSound();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Vehicle")
        {
            if (gameObject.tag != "GlobalEnvTemperature")
            {
                if (!NewVehicleController.vehicleController.currentInfluenceZones.Contains(this))
                {
                    NewVehicleController.vehicleController.currentInfluenceZones.Add(this);
                }
            }

            if (propertyType == PropertyType.WIND)
            {
                onWind = true;

                if (!windCoroutineIsPlaying)
                {
                    StartCoroutine(AddWindForce());
                    windCoroutineIsPlaying = true;
                    HUD.instance.WindEffect = true;
                }
            }

            if (propertyType == PropertyType.RADIATION)
            {
                onRadiation = true;

                if (!radiationCoroutineIsPlaying)
                {
                    StartCoroutine(AddRadiation());
                    radiationCoroutineIsPlaying = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Vehicle")
        {
            if (NewVehicleController.vehicleController.currentInfluenceZones.Contains(this))
            {
                NewVehicleController.vehicleController.currentInfluenceZones.RemoveAt(NewVehicleController.vehicleController.currentInfluenceZones.IndexOf(this));
            }

            if (propertyType == PropertyType.WIND)
            {
                onWind = false;
                windCoroutineIsPlaying = false;
                HUD.instance.WindEffect = false;
            }

            if (propertyType == PropertyType.RADIATION)
            {
                onRadiation = false;
                radiationCoroutineIsPlaying = false;
                StopRadiation();
            }
        }
    }

    private IEnumerator AddWindForce()
    {
        NewVehicleController.vehicleController.currentWindSpeed = 0;

        while (onWind)
        {
            float distanceFromCenter = (NewVehicleController.vehicleController.transform.position - transform.position).magnitude;
            float finalForce = 1;
            finalForce *= (((value * 50000)) / (distanceFromCenter));
            Vector3 force = -transform.right * finalForce;
            NewVehicleController.vehicleController.rb.AddForce(force, ForceMode.Force);
            NewVehicleController.vehicleController.currentWindDirection = force.normalized;
            NewVehicleController.vehicleController.currentWindSpeed = Mathf.Round(finalForce / (100f * distanceFromCenter));
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator AddRadiation()
    {
        while (onRadiation && !NewVehicleController.vehicleController.isShielded)
        {
            Color newAlpha = Color.white;

            for (int i = 0; i < HUDList.HUDImages.Count; i++)
            {
                newAlpha = HUDList.HUDImages[i].color;
                newAlpha.a = Random.Range(0, 0.1f);
                HUDList.HUDImages[i].color = newAlpha;

            }

            for (int i = 0; i < HUDList.HUDTexts.Count; i++)
            {
                newAlpha = HUDList.HUDTexts[i].color;
                newAlpha.a = Random.Range(0, 0.1f);
                HUDList.HUDTexts[i].color = newAlpha;
            }

            newAlpha = HUDList.miniMap.color;
            newAlpha.a = Random.Range(0, 0.1f);
            HUDList.miniMap.color = newAlpha;

            newAlpha.b = Random.Range(0, 1f);
            newAlpha.r = Random.Range(0, 1f);
            newAlpha.g = Random.Range(0, 1f);

            HUDList.disturb.color = newAlpha;

            yield return new WaitForSeconds(0.05f);
        }

        StopRadiation();
        yield return null;
    }

    void StopRadiation ()
    {
        Color newAlpha = Color.white;

        for (int i = 0; i < HUDList.HUDImages.Count; i++)
        {
            newAlpha = HUDList.HUDImages[i].color;
            newAlpha.a = 1f;
            HUDList.HUDImages[i].color = newAlpha;
        }

        for (int i = 0; i < HUDList.HUDTexts.Count; i++)
        {
            newAlpha = HUDList.HUDTexts[i].color;
            newAlpha.a = 1f;
            HUDList.HUDTexts[i].color = newAlpha;
        }

        newAlpha = HUDList.miniMap.color;
        newAlpha.a = 0f;
        HUDList.disturb.color = newAlpha;
        HUDList.miniMap.color = mapRender;
    }
}
