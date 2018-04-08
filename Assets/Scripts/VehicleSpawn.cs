using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawn : MonoBehaviour
{
    private void Awake()
    {
        NewVehicleController.vehicleController.GetComponent<Rigidbody>().isKinematic = false;
        NewVehicleController.vehicleController.transform.forward = this.transform.forward;
        NewVehicleController.vehicleController.transform.position = this.transform.position;

    }
}
