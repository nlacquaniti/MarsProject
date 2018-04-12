using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SupplementAeolianEngine : Accessory
{

    float supplementValue = 1;

    IEnumerator mCooldown;
    IEnumerator mSupplAeolianOn;
    Animator m_Animator;
    AudioSource audioSource = null;


    void Start ()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        m_Animator = GetComponentInChildren<Animator>();
        fileManager = new FileManager();
        type = TypeOfAccessories.Aeolian_Propeller;
        base.Start();
        vehicleController.rb.mass += float.Parse(fileManager.LoadAccessoriesValue(1, (int)type + 1));
        supplementValue = float.Parse(fileManager.LoadAccessoriesValue(4, (int)type + 1));
    }
	
	void FixedUpdate ()
    {
        //Debug.Log("ACCESSORY: " + (int)vehicleController.chosenAccessory1);

        if (!isCooldown)
        {
            if (accessoryPressed && isActive)
            {
                m_Animator.SetBool("IsGenerating", false);
                isActive = false;
                //StopCoroutine(mTurnSupplAeolianOn);
                StopCoroutine(mSupplAeolianOn);

                StartCoroutine(mCooldown = Cooldown());
            }

            else if (accessoryPressed && !isActive)
            {
				isActive = true;
				accessoryPressed = false;
                m_Animator.SetBool("IsGenerating", true);
				StartCoroutine(mSupplAeolianOn = SupplAeolianOn());
                StartCoroutine(HUD.instance.SlotBarDecrease(slot, duration));
            }
        }
    }

    IEnumerator SupplAeolianOn()
    {
		isActive = true;
        PlaySound();

        float timer = duration;

        while (timer > 0)
        {
            foreach (EnvPropertyScript zone in vehicleController.currentInfluenceZones)
            {
                string influenceTag = zone.gameObject.tag;

                if (influenceTag == "EnvPropertyWind")
                {
                    vehicleController.currentFuel += supplementValue * Time.deltaTime;
                    if (vehicleController.currentFuel > 100) { vehicleController.currentFuel = 100; }
                    break;
                }
            }

            timer -= Time.deltaTime;
            yield return null;
        }
        m_Animator.SetBool("IsGenerating", false);
        isActive = false;
        StartCoroutine(mCooldown = Cooldown());
    }

    void PlaySound()
    {
        int index = isActive ? 15 : 15;

        audioSource.clip = AudioManager.Audio.GetAccessoriesSoundSettings()[index].audioFile;
        audioSource.volume = AudioManager.Audio.GetAccessoriesSoundSettings()[index].volume;
        audioSource.pitch = AudioManager.Audio.GetAccessoriesSoundSettings()[index].pitch;
        audioSource.maxDistance = AudioManager.Audio.GetAccessoriesSoundSettings()[index].distance;
        audioSource.loop = false;
        audioSource.Play();
    }
}
