using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeadShielding : Accessory
{
    IEnumerator mLeadShieldingOn;
    IEnumerator mCooldown;
    AudioSource audioSource = null;


    void Start ()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        fileManager = new FileManager();
        type = TypeOfAccessories.Shielding;
        base.Start();
        vehicleController.rb.mass += float.Parse(fileManager.LoadAccessoriesValue(1, (int)type + 1));
        //TODO
    }

    void FixedUpdate()
    {
        if (!isCooldown)
        {
            if (accessoryPressed && isActive)
            {
                isActive = false;

                StopCoroutine(mLeadShieldingOn);
                StartCoroutine(mCooldown = Cooldown());
            }

            else if (accessoryPressed && !isActive)
            {
                isActive = true;
                accessoryPressed = false;
                PlaySound();

                StartCoroutine(mLeadShieldingOn = LeadShieldingOn());
                StartCoroutine(HUD.instance.SlotBarDecrease(slot, duration));
            }
        }

        if (isActive)
        {
            NewVehicleController.vehicleController.isShielded = true;
        }
        else
        {
            NewVehicleController.vehicleController.isShielded = false;
        }
    }

    IEnumerator LeadShieldingOn()
    {
        float timer = duration;
        isActive = true;
        PlaySound();

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        isActive = false;
        PlaySound();

        StartCoroutine(mCooldown = Cooldown());
    }

    void PlaySound()
    {
        int index = isActive ? 11 : 12;

        audioSource.clip = AudioManager.Audio.GetAccessoriesSoundSettings()[index].audioFile;
        audioSource.volume = AudioManager.Audio.GetAccessoriesSoundSettings()[index].volume;
        audioSource.pitch = AudioManager.Audio.GetAccessoriesSoundSettings()[index].pitch;
        audioSource.maxDistance = AudioManager.Audio.GetAccessoriesSoundSettings()[index].distance;
        audioSource.loop = false;
        audioSource.Play();
    }
}
