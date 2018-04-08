using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Glider : Accessory
{    
    public int gliderDuration;
    public int gliderCooldown;
    [Range(0, 45)] public int maxAngleX;
    [Range(0, 45)] public int maxAngleY;
    public float verticalRotSensibility = 7.5f;
    public float veerRotSensibility = 5;
    public float horizontalRotSensibility = 10;
    public float outAngleReturnForce = 0.1f;
    public float lateralForceMultiplier = 250;
    public float forwardForceMultiplier = 250;
    public float backwardForceMultiplier = 500;
    public float upGliderForce = 10000;

    [HideInInspector] public float airSensibilityX;
    [HideInInspector] public float airSensibilityY;
    [HideInInspector] public float airSensibilityZ;

    IEnumerator mCooldown;
    IEnumerator mGliderOn;

    Animator m_Animator = null;
    AudioSource audioSource = null;


    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        m_Animator = gameObject.GetComponent<Animator>();
        fileManager = new FileManager();
        type = TypeOfAccessories.Glider;
        base.Start();

        vehicleController.rb.mass += float.Parse(fileManager.LoadAccessoriesValue(1, (int)type + 1));

        gliderDuration = Int32.Parse(fileManager.LoadAccessoriesValue(2, (int)type + 1));
        gliderCooldown = Int32.Parse(fileManager.LoadAccessoriesValue(3, (int)type + 1));

        airSensibilityX = vehicleController.airSensibility.x;
        airSensibilityY = vehicleController.airSensibility.y;
        airSensibilityZ = vehicleController.airSensibility.z;

        maxAngleX = int.Parse(fileManager.LoadAccessoriesValue(9, (int)type + 1));
        maxAngleY = int.Parse(fileManager.LoadAccessoriesValue(10, (int)type + 1));

        verticalRotSensibility = float.Parse(fileManager.LoadAccessoriesValue(11, (int)type + 1));
        //verticalRotSensibility *= vehicleController.accessoriesForceMoltiplier;

        veerRotSensibility = float.Parse(fileManager.LoadAccessoriesValue(12, (int)type + 1));
        //veerRotSensibility *= vehicleController.accessoriesForceMoltiplier;

        horizontalRotSensibility = float.Parse(fileManager.LoadAccessoriesValue(13, (int)type + 1));
        //horizontalRotSensibility *= vehicleController.accessoriesForceMoltiplier;

        outAngleReturnForce = float.Parse(fileManager.LoadAccessoriesValue(14, (int)type + 1));
        //outAngleReturnForce *= vehicleController.accessoriesForceMoltiplier;

        lateralForceMultiplier =  float.Parse(fileManager.LoadAccessoriesValue(15, (int)type + 1));
        //lateralForceMultiplier *= vehicleController.accessoriesForceMoltiplier;

        forwardForceMultiplier = float.Parse(fileManager.LoadAccessoriesValue(16, (int)type + 1));
        //forwardForceMultiplier *= vehicleController.accessoriesForceMoltiplier;

        backwardForceMultiplier = float.Parse(fileManager.LoadAccessoriesValue(17, (int)type + 1));
        //backwardForceMultiplier *= vehicleController.accessoriesForceMoltiplier;

        upGliderForce = float.Parse(fileManager.LoadAccessoriesValue(18, (int)type + 1));
        //upGliderForce *= vehicleController.accessoriesForceMoltiplier;
    }
    

    void FixedUpdate()
    {
        if (!isCooldown)
        {
            if (accessoryPressed && isActive)
            {
                m_Animator.SetBool("IsActive", false);
                isActive = false;
                PlaySound();
                vehicleController.flyingControlEnabled = true;
                StopCoroutine(mGliderOn);
                StartCoroutine(mCooldown = Cooldown());

            }
            else if (accessoryPressed && !isActive)
            {
				accessoryPressed = false;
                m_Animator.SetBool("IsActive", true);
                StartCoroutine(mGliderOn = GliderOn());
                StartCoroutine(HUD.instance.SlotBarDecrease(slot, duration));
            }
        }

        if (isActive && vehicleController.isFlying)
        {
            int angleH = Mathf.RoundToInt(vehicleController.transform.localEulerAngles.z);
            bool inLeftRange = ((angleH >= 0 || angleH == 360) && angleH <= maxAngleX);
            bool inRightRange = ((angleH <= 360 || angleH == 0) && angleH >= (360 - maxAngleX));

            int angleV = Mathf.RoundToInt(vehicleController.transform.localEulerAngles.x);
            bool inUpRange = ((angleV >= 0 || angleV == 360) && angleV <= maxAngleY);
            bool inDownRange = ((angleV <= 360 || angleV == 0) && angleV >= (360 - maxAngleY));

            float xAxes = Input.GetAxis("HorizontalHandling") * (airSensibilityX / 5);
            float yAxes = Input.GetAxis("VerticalHandling") * (airSensibilityY / 5);

            
            if (inLeftRange || inRightRange)
            {
                vehicleController.rb.AddRelativeTorque(0, 0, -xAxes * horizontalRotSensibility, ForceMode.VelocityChange);
            }
            else if (angleH < 180 && angleH > maxAngleX)
            {
                vehicleController.rb.AddTorque(0, xAxes * veerRotSensibility, 0, ForceMode.VelocityChange);
                vehicleController.rb.AddRelativeTorque(0, 0, -outAngleReturnForce, ForceMode.VelocityChange);
            }
            else if (angleH > 180 && angleH < (360 - maxAngleX))
            {
                vehicleController.rb.AddTorque(0, xAxes * veerRotSensibility, 0, ForceMode.VelocityChange);
                vehicleController.rb.AddRelativeTorque(0, 0, outAngleReturnForce, ForceMode.VelocityChange);
            }

            if (inUpRange || inDownRange)
            {
                
                vehicleController.rb.AddRelativeTorque(yAxes * verticalRotSensibility, 0, 0, ForceMode.VelocityChange);
            }
            else if (angleV < 180 && angleV > maxAngleY)
            {
                vehicleController.rb.AddRelativeTorque(-outAngleReturnForce, 0, 0, ForceMode.VelocityChange);
            }
            else if (angleV > 180 && angleV < (360 - maxAngleY))
            {
                vehicleController.rb.AddRelativeTorque(outAngleReturnForce, 0, 0, ForceMode.VelocityChange);
            }

            float totalAngleH = 0;
            if (angleH > 0 && angleH < 180)
            {
                totalAngleH = angleH;
                if (totalAngleH > 45) { totalAngleH = 45; }

                vehicleController.rb.AddRelativeForce(Vector3.left * (totalAngleH * lateralForceMultiplier), ForceMode.VelocityChange);
            }
            else if (angleH < 360 && angleH > 180)
            {
                totalAngleH = 360 - angleH;
                if (totalAngleH > 45) { totalAngleH = 45; }
                vehicleController.rb.AddRelativeForce(-Vector3.left * (totalAngleH * lateralForceMultiplier), ForceMode.VelocityChange);
            }

            float totalAngleV = 0;
            if (angleV > 0 && angleV < 180)
            {
                totalAngleV = angleV;
                if (totalAngleV > 45) { totalAngleV = 45; }
                vehicleController.rb.AddRelativeForce(Vector3.forward * (totalAngleV * forwardForceMultiplier), ForceMode.VelocityChange);
            }
            else if (angleV < 360 && angleV > 180)
            {
                totalAngleV = 360 - angleV;
                if (totalAngleV > 45) { totalAngleV = 45; }
                vehicleController.rb.AddRelativeForce(Vector3.forward * (totalAngleV * backwardForceMultiplier), ForceMode.VelocityChange);
            }

            vehicleController.rb.AddForce(Vector3.up * upGliderForce, ForceMode.VelocityChange);
        }
    }

    IEnumerator GliderOn()
    {
        float timer = duration;
        isActive = true;
        PlaySound();
        vehicleController.flyingControlEnabled = false;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        
        TurnGliderOff();
        StartCoroutine(mCooldown = Cooldown());       
    }

    void TurnGliderOff()
    {
        m_Animator.SetBool("IsActive", false);
        isActive = false;
        PlaySound();

        vehicleController.flyingControlEnabled = true;
    }

    void PlaySound()
    {
        int index = isActive ? 0 : 1;

        audioSource.clip = AudioManager.Audio.GetAccessoriesSoundSettings()[index].audioFile;
        audioSource.volume = AudioManager.Audio.GetAccessoriesSoundSettings()[index].volume;
        audioSource.pitch = AudioManager.Audio.GetAccessoriesSoundSettings()[index].pitch;
        audioSource.maxDistance = AudioManager.Audio.GetAccessoriesSoundSettings()[index].distance;
        audioSource.loop = false;
        audioSource.Play();
    }
}

