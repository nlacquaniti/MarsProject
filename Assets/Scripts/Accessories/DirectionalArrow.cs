using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DirectionalArrow : Accessory
{
    public GameObject testObject;
    public GameObject target;
    public GameObject vehicle;
    IEnumerator mCooldown;
    IEnumerator mDirectionalArrow;
    IEnumerator mTurnDirArrowOn;
    AudioSource audioSource = null;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); 
        // target = GameObject.Find("Target0");
        // vehicle = GameObject.Find("Classica");
        fileManager = new FileManager();
        type = TypeOfAccessories.Indicator_Arrow;
        base.Start();

        vehicleController.rb.mass += float.Parse(fileManager.LoadAccessoriesValue(1, (int)type + 1));

    }

    void FixedUpdate()
    {
        if (!isCooldown)
        {
            if (accessoryPressed && isActive)
            {
                isActive = false;
                PlaySound();
                TurnDirArrowOff();
                StopCoroutine(mTurnDirArrowOn);
                StopCoroutine(mDirectionalArrow);

                StartCoroutine(mCooldown = Cooldown());
            }

            else if (accessoryPressed && !isActive)
            {
				accessoryPressed = false;
                StartCoroutine(mTurnDirArrowOn = TurnDirArrowOn());
                StartCoroutine(HUD.instance.SlotBarDecrease(slot, duration));

            }
        }

    }


    IEnumerator DirectionalArrowOn()
    {
        float timer = duration;
        print("Entro directional Arrow on");

        while (timer > 0)
        {

            timer -= Time.deltaTime;
            yield return null;
        }
        print("Call turn off method");
        TurnDirArrowOff();
        StartCoroutine(mCooldown = Cooldown());

    }

    IEnumerator TurnDirArrowOn()
    {
        isActive = true;
        PlaySound();

        print(isActive);
        StartCoroutine(mDirectionalArrow = DirectionalArrowOn());
        testObject.GetComponent<MeshRenderer>().enabled = true;
        while (duration > 0)
        {

            
            // testObject.transform.SetParent(vehicle.transform);
            Vector3 dir = (testObject.transform.position - target.transform.position).normalized;
            testObject.transform.forward = new Vector3(dir.x, 0, dir.z);
            yield return null;

        }
    }

    void TurnDirArrowOff()
    {
        isActive = false;
        PlaySound();
        testObject.GetComponent<Renderer>().enabled = false;
    }

    void PlaySound()
    {
        int index = isActive ? 2 : 3;

        audioSource.clip = AudioManager.Audio.GetAccessoriesSoundSettings()[index].audioFile;
        audioSource.volume = AudioManager.Audio.GetAccessoriesSoundSettings()[index].volume;
        audioSource.pitch = AudioManager.Audio.GetAccessoriesSoundSettings()[index].pitch;
        audioSource.maxDistance = AudioManager.Audio.GetAccessoriesSoundSettings()[index].distance;
        audioSource.loop = false;
        audioSource.Play();
    }
}
