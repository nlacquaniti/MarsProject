using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplementRadiationEngine : Accessory
{
    float supplementValue = 1;

    IEnumerator mCooldown;
    IEnumerator mTurnSupplRadOn;
    Animator m_Animator;
    AudioSource audioSource = null;


    void Start ()
    {
        m_Animator = GetComponentInChildren<Animator>();
        audioSource = gameObject.AddComponent<AudioSource>();
        fileManager = new FileManager();
        type = TypeOfAccessories.Radiation_Propeller;
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
                m_Animator.SetBool("IsGenerating", false);
                isActive = false;
                StopCoroutine(mTurnSupplRadOn);

                StartCoroutine(mCooldown = Cooldown());
            }

            else if (accessoryPressed && !isActive)
            {
				accessoryPressed = false;
                m_Animator.SetBool("IsGenerating", true);
                StartCoroutine(mTurnSupplRadOn = TurnSupplRadOn());
                StartCoroutine(HUD.instance.SlotBarDecrease(slot, duration));
            }
        }
    }


    IEnumerator TurnSupplRadOn()
    {
        isActive = true;
        PlaySound();

        float timer = duration;

		while (timer > 0)
        {
            foreach (EnvPropertyScript zone in vehicleController.currentInfluenceZones)
            {
                string influenceTag = zone.gameObject.tag;

                if (influenceTag == "EnvPropertyRadiation")
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
        int index = isActive ? 17 : 17;

        audioSource.clip = AudioManager.Audio.GetAccessoriesSoundSettings()[index].audioFile;
        audioSource.volume = AudioManager.Audio.GetAccessoriesSoundSettings()[index].volume;
        audioSource.pitch = AudioManager.Audio.GetAccessoriesSoundSettings()[index].pitch;
        audioSource.maxDistance = AudioManager.Audio.GetAccessoriesSoundSettings()[index].distance;
        audioSource.loop = false;
        audioSource.Play();
    }
}


