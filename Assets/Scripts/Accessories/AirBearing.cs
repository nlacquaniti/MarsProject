using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirBearing : Accessory
{
    IEnumerator mCooldown;
    IEnumerator mAirBeeringOn;
    float force = 0;
    AudioSource audioSource = null;

    void Start ()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        fileManager = new FileManager();
        type = TypeOfAccessories.AirBearing;
        base.Start();
        vehicleController.rb.mass += float.Parse(fileManager.LoadAccessoriesValue(1, (int)type + 1));
        force = float.Parse(fileManager.LoadAccessoriesValue(7, (int)type + 1));
        force *= vehicleController.accessoriesForceMoltiplier;
    }

    void FixedUpdate()
    {
        if (!isCooldown)
        {
            if (accessoryPressed && isActive)
            {

                isActive = false;
                StopCoroutine(mAirBeeringOn);
                StartCoroutine(mCooldown = Cooldown());
                
            }

            else if (accessoryPressed && !isActive)
            {
				accessoryPressed = false;
                StartCoroutine(mAirBeeringOn = AirBeeringOn());
                StartCoroutine(HUD.instance.SlotBarDecrease(slot, duration));

                if (duration > 0)
                {
                    vehicleController.rb.AddForceAtPosition(Vector3.up * (force / 1000), vehicleController.transform.position, ForceMode.VelocityChange);
                }
               
            }
        }
    }

    IEnumerator AirBeeringOn()
    {
        float timer = duration;
        isActive = true;
        PlaySound();
        while (timer > 0)
        {

            timer -= Time.deltaTime;
            yield return null;
        }

        //TurnDirArrowOff();
        StartCoroutine(mCooldown = Cooldown());
    }

    void PlaySound()
    {
        int index = isActive ? 13 : 13;

        audioSource.clip = AudioManager.Audio.GetAccessoriesSoundSettings()[index].audioFile;
        audioSource.volume = AudioManager.Audio.GetAccessoriesSoundSettings()[index].volume;
        audioSource.pitch = AudioManager.Audio.GetAccessoriesSoundSettings()[index].pitch;
        audioSource.maxDistance = AudioManager.Audio.GetAccessoriesSoundSettings()[index].distance;
        audioSource.loop = false;
        audioSource.Play();
    }
}
