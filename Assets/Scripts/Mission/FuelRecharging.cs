using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelRecharging : MonoBehaviour
{
    public float recharge = 10f;

    private bool _CollisionTrigger = true;
    private MeshRenderer _Mesh;

    private void Awake()
    {
        _Mesh = this.gameObject.GetComponent<MeshRenderer>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Vehicle") && _CollisionTrigger)
        {
            _CollisionTrigger = false;

            _Mesh.enabled = false;
            transform.position = new Vector3(0, -10000, 0);

            if (NewVehicleController.vehicleController.currentFuel + recharge >= 100)
            {
                NewVehicleController.vehicleController.currentFuel = 100;
            }
            else
            {
                NewVehicleController.vehicleController.currentFuel += recharge;
            }


        }
    }

}
