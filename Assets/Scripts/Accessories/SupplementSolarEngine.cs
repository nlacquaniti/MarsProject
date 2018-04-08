using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplementSolarEngine : Accessory
{

    float supplementValue = 1;

    IEnumerator mCooldown;
    //IEnumerator mSupplSolarOn;
    IEnumerator mTurnSupplSolarOn;

    void Start ()
    {
        
        fileManager = new FileManager();
        type = TypeOfAccessories.Solar_Propeller;
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
                
                isActive = false;
                StopCoroutine(mTurnSupplSolarOn);
                //StopCoroutine(mSupplSolarOn);

                StartCoroutine(mCooldown = Cooldown());

            }

            else if (accessoryPressed && !isActive)
            {
				accessoryPressed = false;
                StartCoroutine(mTurnSupplSolarOn = TurnSupplSolarOn());
                StartCoroutine(HUD.instance.SlotBarDecrease(slot, duration));

            }
        }

    }


    /*IEnumerator SupplSolarOn()
    {
        float timer = duration;
        print("Entro directional Arrow on");

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        print("Call turn off method");
        TurnSupplSolarOff();
        StartCoroutine(mCooldown = Cooldown());

    }*/

    IEnumerator TurnSupplSolarOn()
    {
        isActive = true;
		float timer = duration;
        
        while (timer > 0)
        {
            if (!vehicleController.isInShadow)
            {
                vehicleController.currentFuel += supplementValue * Time.deltaTime;
                if (vehicleController.currentFuel > 100) { vehicleController.currentFuel = 100; }
            }
			timer -= Time.deltaTime;
            yield return null;
        }

		isActive = false;
		StartCoroutine(mCooldown = Cooldown());
    }
}


