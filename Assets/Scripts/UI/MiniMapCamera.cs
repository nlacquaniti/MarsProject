using UnityEngine;
using System;



public class MiniMapCamera : MonoBehaviour
{
    private Camera _MiniMapCamera;
    private Vector3 _NewCameraPos;
    private float _DistanceFromPlayer;
    private float _CurrentDistanceFromPlayer;

    private void Start()
    {
        _MiniMapCamera = GetComponent<Camera>();

        if (NewVehicleController.vehicleController == null)
            return;

        Vector3 _FirstPos = NewVehicleController.vehicleController.transform.position;
        _FirstPos.y = this.transform.position.y;
        this.transform.position = _FirstPos;
        _DistanceFromPlayer = this.transform.position.y - NewVehicleController.vehicleController.transform.position.y;

    }


    private void LateUpdate()
    {
        _NewCameraPos = NewVehicleController.vehicleController.transform.position;
        _CurrentDistanceFromPlayer = this.transform.position.y - NewVehicleController.vehicleController.transform.position.y;

        if (_CurrentDistanceFromPlayer > _DistanceFromPlayer)
        {
            _NewCameraPos.y = this.transform.position.y - 1f;
        }
        else
        {
            _NewCameraPos.y = this.transform.position.y + 1f;
            
        }

        this.transform.eulerAngles = new Vector3 (90, 0, - NewVehicleController.vehicleController.transform.rotation.eulerAngles.y);
        this.transform.position = _NewCameraPos;

    }


}
