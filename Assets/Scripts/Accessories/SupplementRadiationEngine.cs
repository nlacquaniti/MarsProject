using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplementRadiationEngine : Accessory
{

    float supplementValue = 1;

    IEnumerator mCooldown;
    //IEnumerator mSupplRadOn;
    IEnumerator mTurnSupplRadOn;

    Animator m_Animator;


    void Start ()
    {
        m_Animator = GetComponentInChildren<Animator>();
        fileManager = new FileManager();
        type = TypeOfAccessories.Radiation_Propeller;
        base.Start();
        vehicleController.rb.mass += float.Parse(fileManager.LoadAccessoriesValue(1, (int)type + 1));
        supplementValue = float.Parse(fileManager.LoadAccessoriesValue(4, (int)type + 1));
    }

    void FixedUpdate()
    {
        //Debug.Log("ACCESSORY: " + (int)vehicleController.chosenAccessory1);
        //print("Accessory: " + (int)vehicleController.chosenAccessory2);

        if (!isCooldown)
        {
            if (accessoryPressed && isActive)
            {
                m_Animator.SetBool("IsGenerating", false);
                isActive = false;
                StopCoroutine(mTurnSupplRadOn);
                //StopCoroutine(mSupplRadOn);

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

    /*IEnumerator SupplRadOn()
    {
        float timer = duration;
        
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        m_Animator.SetBool("IsGenerating", false);
        isActive = false;
        StartCoroutine(mCooldown = Cooldown());
    }*/

    IEnumerator TurnSupplRadOn()
    {
        isActive = true;
		float timer = duration;
        //StartCoroutine(mSupplRadOn = SupplRadOn());
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
}


