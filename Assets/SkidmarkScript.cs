using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkidmarkScript : MonoBehaviour
{
    public WheelCollider wheelCollider;
    public TrailRenderer trailRenderer;


	void FixedUpdate()
    { 
        WheelHit hit;
        if (wheelCollider.GetGroundHit(out hit))
        {
            transform.position = hit.point + (wheelCollider.transform.up * 0.05f);
            trailRenderer.enabled = true;
        }
        else
        {
            trailRenderer.enabled = false;
        }
        
        /*Vector3 centerPoint = wheelCollider.transform.TransformPoint(wheelCollider.center);

        RaycastHit hit;
        if (Physics.Raycast(centerPoint, -wheelCollider.transform.up, out hit, wheelCollider.suspensionDistance + wheelCollider.radius))
        {
            transform.position = hit.point + (wheelCollider.transform.up * wheelCollider.radius);
        }
        else
        {
            transform.position = centerPoint - (wheelCollider.transform.up * wheelCollider.suspensionDistance);
        }*/

        //trailRenderer.enabled = !NewVehicleController.vehicleController.isFlying;
    }
}
