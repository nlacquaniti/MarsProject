using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Brights : Accessory
{
    public Light light;
    
    IEnumerator mCooldown;
    IEnumerator mLightsOn;
    AudioSource audioSource = null;


    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        fileManager = new FileManager();
        type = TypeOfAccessories.Brights;
        base.Start();
        //vehicleController.rb.mass += float.Parse(fileManager.LoadAccessoriesValue(1, (int)type + 1));

        if (GameObject.Find("Faro").GetComponent<Light>() !=  null)
        {
            light = vehicleController.transform.Find("Lights").GetChild(0).GetComponent<Light>();
        }
    }

    void FixedUpdate()
    {
        //Debug.Log("ACCESSORY: " + (int)vehicleController.chosenAccessory1);

        if (!isCooldown)
        {
            if (accessoryPressed && isActive)
            {
                StopCoroutine(mLightsOn);
                TurnLightsOff();
            }

            else if (accessoryPressed && !isActive)
            {
				accessoryPressed = false;
                TurnLightsOn();
                StartCoroutine(HUD.instance.SlotBarDecrease(slot, duration));
            }
        }

    }

    private void TurnLightsOn()
    {
        isActive = true;
        PlaySound();
        light.intensity = 8;
        light.range = 200;
        light.spotAngle = 100;

        StartCoroutine(mLightsOn = LightsOn());
    }

    private void TurnLightsOff()
    {
        isActive = false;
        PlaySound();
        light.intensity = 3;
        light.range = 100;
        light.spotAngle = 60;
        StartCoroutine(mCooldown = Cooldown());
        //print("fine turnlightsoff");
    }

    IEnumerator LightsOn()
    {
		isActive = true;
        float timer = duration;

        while (timer > 0)
        {
            //print("lightson loop");
            timer -= Time.deltaTime;
            yield return null;
        }
		isActive = false;
        TurnLightsOff();
		StartCoroutine(mCooldown = Cooldown());
    }

    void PlaySound()
    {
        int index = isActive ? 9 : 10;

        audioSource.clip = AudioManager.Audio.GetAccessoriesSoundSettings()[index].audioFile;
        audioSource.volume = AudioManager.Audio.GetAccessoriesSoundSettings()[index].volume;
        audioSource.pitch = AudioManager.Audio.GetAccessoriesSoundSettings()[index].pitch;
        audioSource.maxDistance = AudioManager.Audio.GetAccessoriesSoundSettings()[index].distance;
        audioSource.loop = false;
        audioSource.Play();
    }
}
