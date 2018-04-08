using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public GameObject wheelsParticleObj;
    public Color sandParticleColor;
    public Color iceParticleColor;
    private ParticleSystem rightParticle;
    private ParticleSystem leftParticle;
    private Vector3 particleDirection = Vector3.zero;

    private void Start()
    {
        GameObject rightParticleObj = Instantiate(wheelsParticleObj, NewVehicleController.vehicleController.transform);
        GameObject leftParticleObj = Instantiate(wheelsParticleObj, NewVehicleController.vehicleController.transform);

        rightParticleObj.transform.localPosition = new Vector3(1, -1, 0);
        leftParticleObj.transform.localPosition = new Vector3(-1, -1, 0);

        rightParticle = rightParticleObj.GetComponent<ParticleSystem>();
        leftParticle = leftParticleObj.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        SetParticleDirection();
    }

    public void SetRockParticle()
    {
        var rightMain = rightParticle.main;
        var rightEmission = rightParticle.emission;
        var leftMain = leftParticle.main;
        var leftEmission = leftParticle.emission;

        rightMain.startColor = sandParticleColor;
        leftMain.startColor = sandParticleColor;

        if (Mathf.Abs(NewVehicleController.vehicleController.currentSpeed) > 10)
        {
            rightEmission.rateOverDistance = Mathf.Lerp(rightEmission.rateOverDistance.constantMax, 1f, Time.deltaTime / 2);
            leftEmission.rateOverDistance = Mathf.Lerp(leftEmission.rateOverDistance.constantMax, 1f, Time.deltaTime / 2);
        }
        else
        {
            rightEmission.rateOverDistance = 0;
            leftEmission.rateOverDistance = 0;
        }
    }

    public void SetSandParticle()
    {
        var rightMain = rightParticle.main;
        var rightEmission = rightParticle.emission;
        var leftMain = leftParticle.main;
        var leftEmission = leftParticle.emission;

        rightMain.startColor = sandParticleColor;
        leftMain.startColor = sandParticleColor;
        
        if (Mathf.Abs(NewVehicleController.vehicleController.currentSpeed) > 10)
        {
            rightEmission.rateOverDistance = Mathf.Lerp(rightEmission.rateOverDistance.constantMax, 2f, Time.deltaTime / 2);
            leftEmission.rateOverDistance = Mathf.Lerp(leftEmission.rateOverDistance.constantMax, 2f, Time.deltaTime / 2);
        }
        else
        {
            rightEmission.rateOverDistance = 0;
            leftEmission.rateOverDistance = 0;
        }
    }

    public void SetIceParticle()
    {
        var rightMain = rightParticle.main;
        var rightEmission = rightParticle.emission;
        var leftMain = leftParticle.main;
        var leftEmission = leftParticle.emission;

        rightMain.startColor = iceParticleColor;
        leftMain.startColor = iceParticleColor;
        
        if (Mathf.Abs(NewVehicleController.vehicleController.currentSpeed) > 10)
        {
            rightEmission.rateOverDistance = Mathf.Lerp(rightEmission.rateOverDistance.constantMax, 1f, Time.deltaTime / 2);
            leftEmission.rateOverDistance = Mathf.Lerp(leftEmission.rateOverDistance.constantMax, 1f, Time.deltaTime / 2);
        }
        else
        {
            rightEmission.rateOverDistance = 0;
            leftEmission.rateOverDistance = 0;
        }
    }

    public void DeactivateParticle()
    {
        var rightEmission = rightParticle.emission;
        var leftEmission = leftParticle.emission;

        rightEmission.rateOverDistance = 0;
        leftEmission.rateOverDistance = 0;
    }

    public void SetParticleDirection()
    {
        if(Mathf.Abs(NewVehicleController.vehicleController.currentSpeed) > 10)
        {
            rightParticle.gameObject.transform.LookAt(NewVehicleController.vehicleController.transform.position - NewVehicleController.vehicleController.rb.velocity);
            leftParticle.gameObject.transform.LookAt(NewVehicleController.vehicleController.transform.position - NewVehicleController.vehicleController.rb.velocity);
        }
    }
}
