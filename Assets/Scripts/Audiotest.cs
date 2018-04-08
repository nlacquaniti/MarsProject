using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audiotest : MonoBehaviour
{


    public float rpm;
    public float delayUp;
    public float delayDown;
    public float gas;
    private NewVehicleController newVehicleController;


    void Start()
    {
        newVehicleController = GetComponent<NewVehicleController>();
      
    }

    private void LateUpdate()
    {
        rpmLimiter();

        rpmgrowth();

        gas = (Input.GetAxis("Triggers"));
    }


    void rpmLimiter()
    {
        if (rpm < 0)
        {
            rpm = 0;
        }
        if (rpm > 500)
        {
            rpm = 500;
        }
    }
    void rpmgrowth()
    {
        if (Input.GetAxis("Triggers") > 0 && rpm <= 500)
        {
            rpm = rpm + (delayUp * gas) * Time.deltaTime;
        }
        else
        {
            rpm = rpm - (delayDown) * Time.deltaTime;
        }
    }

    private void OnDestroy()
    {

    }
}
