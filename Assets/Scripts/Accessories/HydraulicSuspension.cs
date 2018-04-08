using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydraulicSuspension : Accessory
{
    float multiplier = 4;
    float defaultSpringValue = 0;
    float defaultDamperValue = 0;
    float defaultSuspensionDistance = 0;
    float newSuspensionDistance;

    IEnumerator mCooldown;
    IEnumerator mHSOn;
    AudioSource audioSource = null;


    void Start ()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        fileManager = new FileManager();
        type = TypeOfAccessories.Hydraulic_Suspensions;
        base.Start();
        vehicleController.rb.mass += float.Parse(fileManager.LoadAccessoriesValue(1, (int)type + 1));

        defaultSuspensionDistance = float.Parse(fileManager.LoadChassisValue(4, (int)vehicleController.chosenChassis + 1));
        defaultSpringValue = float.Parse(fileManager.LoadChassisValue(5, (int)vehicleController.chosenChassis + 1));
        defaultDamperValue = float.Parse(fileManager.LoadChassisValue(6, (int)vehicleController.chosenChassis + 1));
        newSuspensionDistance = float.Parse(fileManager.LoadAccessoriesValue(8, (int)type + 1));

        //SetWheelSuspensionsMod(newSuspensionDistance, defaultSpringValue * multiplier, defaultDamperValue * multiplier);
    }

    void SetWheelSuspensionsMod(float distanceValue, float springValue, float damperValue)
    {
        foreach (NewVehicleController.WheelInfo wheelInfo in vehicleController.wheelInfos)
        {
            wheelInfo.wheelCollider.suspensionDistance = distanceValue;
            JointSpring wheelSpring = wheelInfo.wheelCollider.suspensionSpring;
            wheelSpring.spring = springValue;
            wheelSpring.damper = damperValue;
            wheelInfo.wheelCollider.suspensionSpring = wheelSpring;
        }
    }

    void FixedUpdate()
    {
        //Debug.Log("ACCESSORY: " + (int)vehicleController.chosenAccessory1);

        if (!isCooldown)
        {
            if (accessoryPressed && isActive)
            {
                SetWheelSuspensionsMod(defaultSuspensionDistance, defaultSpringValue, defaultDamperValue);
                isActive = false;
                PlaySound();
				StopCoroutine(mHSOn);
                StartCoroutine(mCooldown = Cooldown());
               
            }

            else if (accessoryPressed && !isActive)
            {
				accessoryPressed = false;
                SetWheelSuspensionsMod(newSuspensionDistance, defaultSpringValue * multiplier, defaultDamperValue * multiplier);
                StartCoroutine(mHSOn = HsOn());
                StartCoroutine(HUD.instance.SlotBarDecrease(slot, duration));

            }
        }

    }

    IEnumerator HsOn()
    {
        isActive = true;
        PlaySound();
        float timer = duration;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

		isActive = false;
        SetWheelSuspensionsMod(defaultSuspensionDistance, defaultSpringValue, defaultDamperValue);
        StartCoroutine(mCooldown = Cooldown());
    }

    void PlaySound()
    {
        int index = isActive ? 7 : 8;

        audioSource.clip = AudioManager.Audio.GetAccessoriesSoundSettings()[index].audioFile;
        audioSource.volume = AudioManager.Audio.GetAccessoriesSoundSettings()[index].volume;
        audioSource.pitch = AudioManager.Audio.GetAccessoriesSoundSettings()[index].pitch;
        audioSource.maxDistance = AudioManager.Audio.GetAccessoriesSoundSettings()[index].distance;
        audioSource.loop = false;
        audioSource.Play();
    }
}
