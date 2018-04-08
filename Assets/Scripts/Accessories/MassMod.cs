using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassMod : Accessory
{
    float force;

    IEnumerator mCooldown;
    IEnumerator mMassModOn;
    AudioSource audioSource = null;


    void Start ()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); 
        fileManager = new FileManager();
        type = TypeOfAccessories.WeightMod;
        base.Start();
        vehicleController.rb.mass += float.Parse(fileManager.LoadAccessoriesValue(1, (int)type + 1));
        force = float.Parse(fileManager.LoadAccessoriesValue(6, (int)type + 1));
        force *= vehicleController.accessoriesForceMoltiplier;
    }

    void FixedUpdate()
    {
        if (!isCooldown)
        {
            if (accessoryPressed && isActive)
            {
                isActive = false;
                PlaySound();
                StopCoroutine(mMassModOn);
                StartCoroutine(mCooldown = Cooldown());
            }

            else if (accessoryPressed && !isActive)
            {
				isActive = true;
                PlaySound();
                accessoryPressed = false;
                StartCoroutine(mMassModOn = MassModOn());
                StartCoroutine(HUD.instance.SlotBarDecrease(slot, duration));

                /*if(duration > 0)
                {
                    vehicleController.rb.AddForce(Vector3.down * force);
                }
                */
            }
        }
        if (isActive) {

            vehicleController.rb.AddForce(Vector3.down * force * Time.deltaTime);
        }

    }

    IEnumerator MassModOn()
    {
        isActive = true;
		float timer = duration;

        while (timer > 0)
        {
            vehicleController.rb.AddForce(Vector3.down * force * Time.deltaTime);
            timer -= Time.deltaTime;
            yield return null;

        }
        isActive = false;
        StartCoroutine(mCooldown = Cooldown());
    }

    void PlaySound()
    {
        int index = isActive ? 4 : 5;

        audioSource.clip = AudioManager.Audio.GetAccessoriesSoundSettings()[index].audioFile;
        audioSource.volume = AudioManager.Audio.GetAccessoriesSoundSettings()[index].volume;
        audioSource.pitch = AudioManager.Audio.GetAccessoriesSoundSettings()[index].pitch;
        audioSource.maxDistance = AudioManager.Audio.GetAccessoriesSoundSettings()[index].distance;
        audioSource.loop = false;
        audioSource.Play();
    }
}

