using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Accessory : MonoBehaviour
{
    public bool isCooldown = false;
    protected int duration = 50;
    protected float cooldown = 10;
    public bool isActive = false;
    public bool accessoryPressed;
    [HideInInspector]
    public FileManager fileManager;
    [HideInInspector]
    public NewVehicleController vehicleController;
    protected int slot;

    public enum TypeOfAccessories
    {
        Not_Equipped = -1, Indicator_Arrow = 0, Turbo = 1, Glider = 2, Brights = 3, Aeolian_Propeller = 4, Solar_Propeller = 5,
        Radiation_Propeller = 6, Hydraulic_Suspensions = 7, Shielding = 8, WeightMod = 9, AirBearing = 10, Helicopter = 11, Grapple = 12
    };

    [HideInInspector]
    public TypeOfAccessories type;

    protected void Start()
    {
        vehicleController = NewVehicleController.vehicleController;
        duration = Int32.Parse(fileManager.LoadAccessoriesValue(2, (int)type + 1));
        cooldown = Int32.Parse(fileManager.LoadAccessoriesValue(3, (int)type + 1));

        if (transform.parent.name == "Acc1")
        {
            slot = 1;
            StartCoroutine(WaitHUD());
        }
        else
        {
            slot = 2;
            StartCoroutine(WaitHUD());
        }
        //print("slot " + slot + " duration " + duration);
    }

    protected void Update()
    {
        string input = "";
		accessoryPressed = false;

        if ((int)vehicleController.chosenAccessory1 == (int)type)
        {
            input = "Accessory1";
        }
        else if((int)vehicleController.chosenAccessory2 == (int)type)
        {
            input = "Accessory2";
        }
        else if ((int)vehicleController.chosenAccessory3 == (int)type)
        {
            input = "Accessory3";
        }
        else if ((int)vehicleController.chosenAccessory4 == (int)type)
        {
            input = "Accessory4";
        }

		if (input != "") {
			if (!isCooldown) {
				accessoryPressed = Input.GetButtonDown (input);
			}
		}
    }

    protected IEnumerator Cooldown()
    {
        float timer = cooldown;
        //print("Accessory cooldown value " + cooldown);
        StartCoroutine(HUD.instance.SlotBarCooldown(slot, cooldown));
        isActive = false;
        isCooldown = true;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        isCooldown = false;
    }

    private IEnumerator WaitHUD()
    {
        while (HUD.instance == null)
        {
            yield return null;
        }
        HUD.instance.SetAccessoryName(slot, type.ToString());

    }


}
