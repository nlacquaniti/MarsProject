using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplementSolarEngine : Accessory
{
    float supplementValue = 1;

    IEnumerator mCooldown;
    IEnumerator mTurnSupplSolarOn;
    AudioSource audioSource = null;
    

    void Start ()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        fileManager = new FileManager();
        type = TypeOfAccessories.Solar_Propeller;
        base.Start();
        vehicleController.rb.mass += float.Parse(fileManager.LoadAccessoriesValue(1, (int)type + 1));
        supplementValue = float.Parse(fileManager.LoadAccessoriesValue(4, (int)type + 1));
    }

    void FixedUpdate()
    {
        if (!isCooldown)
        {
            if (accessoryPressed && isActive)
            {
                
                isActive = false;
                StopCoroutine(mTurnSupplSolarOn);
                StartCoroutine(mCooldown = Cooldown());

            }

            else if (accessoryPressed && !isActive)
            {
				accessoryPressed = false;
                StartCoroutine(mTurnSupplSolarOn = TurnSupplSolarOn());
                StartCoroutine(HUD.instance.SlotBarDecrease(slot, duration));

            }
        }

    }


    IEnumerator TurnSupplSolarOn()
    {
        isActive = true;
        PlaySound();

        float timer = duration;
        
        while (timer > 0)
        {
            if (!vehicleController.isInShadow)
            {
                vehicleController.currentFuel += supplementValue * Time.deltaTime;
                if (vehicleController.currentFuel > 100) { vehicleController.currentFuel = 100; }
            }
			timer -= Time.deltaTime;
            yield return null;
        }

		isActive = false;
		StartCoroutine(mCooldown = Cooldown());
    }


    void PlaySound()
    {
        int index = isActive ? 16 : 16;

        audioSource.clip = AudioManager.Audio.GetAccessoriesSoundSettings()[index].audioFile;
        audioSource.volume = AudioManager.Audio.GetAccessoriesSoundSettings()[index].volume;
        audioSource.pitch = AudioManager.Audio.GetAccessoriesSoundSettings()[index].pitch;
        audioSource.maxDistance = AudioManager.Audio.GetAccessoriesSoundSettings()[index].distance;
        audioSource.loop = false;
        audioSource.Play();
    }
}


