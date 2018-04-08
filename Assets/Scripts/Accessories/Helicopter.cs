using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : Accessory
{


    IEnumerator mCooldown;
    IEnumerator mHelicopterOn;
    IEnumerator mTurnHelicopterOn;
    float force = 0;

    Animator m_Animator;
    AudioSource audioSource = null;


    void Start ()
    {
        m_Animator = GetComponentInChildren<Animator>();
        audioSource = gameObject.AddComponent<AudioSource>();
        fileManager = new FileManager();
        type = TypeOfAccessories.Helicopter;
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
                m_Animator.SetBool("IsActive", false);
                isActive = false;
                StopCoroutine(mHelicopterOn);
                StartCoroutine(mCooldown = Cooldown());

            }

            else if (accessoryPressed && !isActive)
            {
				accessoryPressed = false;
                m_Animator.SetBool("IsActive", true);
                StartCoroutine(mHelicopterOn = HelicopterOn());
                StartCoroutine(HUD.instance.SlotBarDecrease(slot, duration));

                /* if(duration > 0)
                 {
                     vehicleController.rb.AddForceAtPosition(Vector3.up * force / 2500, vehicleController.transform.position, ForceMode.VelocityChange);
                 }
                 */
            }
        }
        if (isActive)
        {
            vehicleController.rb.AddForceAtPosition(Vector3.up * force * Time.deltaTime / 2500, vehicleController.transform.position, ForceMode.VelocityChange);
        }
    }

    IEnumerator HelicopterOn()
    {
        float timer = duration;
        print("Entro directional Arrow on");
        isActive = true;
        PlaySound();

        while (timer > 0)
        {
           
            timer -= Time.deltaTime;
            yield return null;
        }
        m_Animator.SetBool("IsActive", false);
        isActive = false;
        StartCoroutine(mCooldown = Cooldown());

    }

    void PlaySound()
    {
        int index = isActive ? 14 : 14;

        audioSource.clip = AudioManager.Audio.GetAccessoriesSoundSettings()[index].audioFile;
        audioSource.volume = AudioManager.Audio.GetAccessoriesSoundSettings()[index].volume;
        audioSource.pitch = AudioManager.Audio.GetAccessoriesSoundSettings()[index].pitch;
        audioSource.maxDistance = AudioManager.Audio.GetAccessoriesSoundSettings()[index].distance;
        audioSource.loop = false;
        audioSource.Play();
    }

}
