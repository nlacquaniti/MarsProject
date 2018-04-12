using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelRecharging : MonoBehaviour
{
    public float recharge = 10f;

    private bool _CollisionTrigger = true;
    private MeshRenderer _Mesh;

    private UnityEngine.UI.Image _StationIcon = null;

    private void Awake()
    {
        _Mesh = this.gameObject.GetComponent<MeshRenderer>();
        _StationIcon = Resources.Load<UnityEngine.UI.Image>("MinimapIcons/" + "StationIcon");
        StartCoroutine(WaitingMinimap());
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

            StartCoroutine(MiniMap.instance.RemoveIcon(this.gameObject));


        }
    }

    private System.Collections.IEnumerator WaitingMinimap()
    {
        while (MiniMap.instance == null)
        {
            yield return null;
        }

        MiniMap.instance.GetIcon(_StationIcon, this.gameObject, false);
    }

}
