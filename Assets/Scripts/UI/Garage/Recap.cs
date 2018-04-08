using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recap : MonoBehaviour
{
    public Garage garage;

    private bool _Trigger = true;

    private void Update()
    {

        #region GoForward
        if (!MenuManager.pauseMenuActive)
        {
            if (Input.GetButtonDown(InputContainer.SUBMIT) && _Trigger)
            {
                _Trigger = false;
                StartCoroutine(VehicleBuilding());
            }
        }
        #endregion

    }

    private IEnumerator VehicleBuilding()
    {
        GameObject vehicle = null;

        if (MenuInizialeContainer.instance.zoneName != "" && MenuInizialeContainer.instance.missionName != "")
        {
            foreach (Transform tr in MenuInizialeContainer.instance.vehicles.transform)
            {
                if (tr.gameObject.activeInHierarchy)
                {
                    vehicle = Instantiate(tr.gameObject, new Vector3(1000, 1000, 1000), Quaternion.identity);
                    break;
                }
                yield return null;
            }

            if (vehicle != null)
            {
                NewVehicleController nvc = vehicle.GetComponent<NewVehicleController>();
                nvc.chosenChassis = (NewVehicleController.TypeOfChassis)garage.loomIndex;
                nvc.chosenEngine = (NewVehicleController.TypeOfEngine)garage.engineIndex;
                nvc.chosenWheel = (NewVehicleController.TypeOfWheel)garage.wheelIndex;
                var accessory = garage.GetAccessoryName();
                nvc.chosenAccessory1 = (NewVehicleController.TypeOfAccessorie)Enum.Parse(typeof(NewVehicleController.TypeOfAccessorie), accessory[0]);
                
                nvc.chosenAccessory2 = (NewVehicleController.TypeOfAccessorie)Enum.Parse(typeof(NewVehicleController.TypeOfAccessorie), accessory[1]);
             
                nvc.chosenAccessory3 = (NewVehicleController.TypeOfAccessorie)Enum.Parse(typeof(NewVehicleController.TypeOfAccessorie), accessory[2]);
                nvc.chosenAccessory4 = (NewVehicleController.TypeOfAccessorie)Enum.Parse(typeof(NewVehicleController.TypeOfAccessorie), accessory[3]);
                yield return null;

                /* TO DO 
                * Skin assignment
                */

                foreach (Transform wheels in vehicle.transform.Find("Wheels"))
                {
                    if (wheels.gameObject.activeInHierarchy)
                    {
                        for (int i = 0; i < wheels.transform.childCount; i++)
                        {
                            if (wheels.GetChild(i).GetComponent<WheelCollider>() != null)
                            {
                                NewVehicleController.WheelInfo wi = new NewVehicleController.WheelInfo();
                                wi.wheelCollider = wheels.GetChild(i).GetComponent<WheelCollider>();
                                if (i == 0 || i == 1)
                                {
                                    wi.steering = true;
                                }
                                wi.motor = true;
                                nvc.wheelInfos.Add(wi);
                                yield return null;
                            }
                        }

                    }
                    yield return null;
                }


                
                nvc.enabled = true;
                vehicle.GetComponent<VehicleControllerIcon>().enabled = true;
                yield return null;

                foreach (Transform acc in vehicle.transform.Find("Acc1"))
                {
                    if (acc.gameObject.activeInHierarchy)
                    {
						if(acc.GetComponent<Accessory>() != null)
                        	acc.GetComponent<Accessory>().enabled = true;

                    }
                    yield return null;
                }

                foreach (Transform acc in vehicle.transform.Find("Acc2"))
                {
                    if (acc.gameObject.activeInHierarchy)
                    {
                        if(acc.GetComponent<Accessory>() != null)
                            acc.GetComponent<Accessory>().enabled = true;

                    }
                    yield return null;
                }


                
            }
            else
            {
                Debug.LogWarning("There's not an instance of vehicle");
            }

            DontDestroyOnLoad(vehicle);

            StartCoroutine(SceneLoader.LoadSceneAsync(MenuInizialeContainer.instance.zoneName + MenuInizialeContainer.instance.missionName));
        }
        else
        {
            Debug.LogWarning(MenuInizialeContainer.instance.zoneName + " " + MenuInizialeContainer.instance.missionName + " Missing");
        }
    }
    
    private void OnEnable()
    {
        MenuInizialeContainer.instance.vehicles.SetActive(true);
    }
    private void OnDisable()
    {
        if(MenuInizialeContainer.instance.vehicles != null)
            MenuInizialeContainer.instance.vehicles.SetActive(false);
    }
}