using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;

public class VehicleCamera : MonoBehaviour
{
    private GameObject vehicle;
    private Vector3 wantedPosition;

    [Header("Posizione Generica")]
    [Tooltip("La distanza della camera dal veicolo")]
    [SerializeField]private float distance = 5.5f;
    [Tooltip("L'altezza della camera rispetto al veicolo")]
    [Range(0.05f, 1f)][SerializeField] private float height;
    [Tooltip("L'orientamento della camera rispetto al veicolo")]
    [Range(0, 50f)] [SerializeField] private float orientation;

    //[Header("Prospettiva")]
    private bool lookFromBehind = true;

    [Header("Tempo riposizionamento minimo")]
    [Tooltip("La velocità di riposizionamento quando il veicolo raggiunge la massima velocità")]
    public float minRepositionTime;

    [Header("Tempo riposizionamento massimo")]
    [Tooltip("La velocità di riposizionamento quando il veicolo viaggia a velocità minima")]
    public float maxRepositionTime;

    [Header("Cap Velocità riposizionamento")]
    [Tooltip("Il cap di velocità oltre il quale la velocità di posizionamento si autoimposta su Minima")]
    public float repositionSpeedReach;

    [Header("Sensibilità massima")]
    [Tooltip("La sensibilità manuale della Camera ad inizio movimento")]
    [Range(1, 2)]
    public float cameraMaxSensibility;

    [Header("Sensibilità minima")]
    [Tooltip("La sensibilità manuale della Camera della sua fase di forzatura massima")]
    [Range(0.1f, 0.5f)]
    public float cameraMinSensibility;

    [Header("Cap Velocità inversione cambiomarcia")]
    [Tooltip("La velocità al quale si attiva l'inversione della camera in caso di cambio marcia")]
    [Range(0f, 50f)]
    public float minInvertSpeed;

    [Header("Altezza massima camera Verticale")]
    [Tooltip("L'altezza massima alla quale è possibile spostare verticalmente la camera")]
    [Range(0f, 1f)]
    public float maxVerticalDistance;

    [Header("Settaggi camera in volo")]
    [SerializeField] [Range (0, 5)] private int flyingCameraHeight = 4;
    [SerializeField] [Range (1, 10)] private int flyingCameraDistance = 3;
    [SerializeField] [Range(1, 5)] private int heightToActivate;

    public LayerMask carMask;

    private Transform vehicleCamera;
    private float timer;
    private float tmpDistance;
    private float cameraSensibility;
    private float repositionCamTimer = 0.2f;
    private float repositionFlyinCamTimer = 6f;
    private bool flyingDistance = false;
    private Vector3 offset;
    private bool flyinOffsetChecked = false;
    private bool offsetChecked = false;
    [HideInInspector] public Vector3[] clipPointPositionArray;
    NewVehicleController vehicleController;
    private float cameraRepositionTime;
    private Vector3 cameraDirection;
    private Vector3 vehicleForward;
    private Vector3 vehiclePoint;
    private int nWheels;
    private Transform rayPoint;
    public Vector3[] pinPoints;

    private CinemachineVirtualCamera generalCamera;
    private CinemachineVirtualCamera firstPersonCamera;
    private CinemachineVirtualCamera airCamera;
    private CinemachineVirtualCamera upperCamera;
    private CinemachineVirtualCamera lowerCamera;

    private Vector3 cameraDestination;

    private string verticalInput = "VerticalCamera";
    private string horizontalInput = "HorizontalCamera";
    private string cameraInvert = "CameraInvert";
    
    void Start()
    {
        vehicle = GameObject.FindGameObjectWithTag("Player");
        vehicleController = vehicle.GetComponent<NewVehicleController>();
        vehicleCamera = transform.GetChild(0);
        clipPointPositionArray = new Vector3[5];
        offset = transform.position - vehicle.transform.position;
        vehiclePoint = vehicle.transform.position;

        checkVirtualCamera();
        checkCollisionStartPoints();
        generalCameraSettings();

        transform.eulerAngles = new Vector3(0, vehicle.transform.rotation.eulerAngles.y, 0);
    }

    void LateUpdate()
    {
        if (FlyingCheck() /*&& vehicleController.currentFlyMode != VehicleController.FlyMode.GLIDE*/)
        {
            AirCamera();
        }
        else
        {
            GeneralCamera();
            vehiclePoint = new Vector3(vehicle.transform.localPosition.x, vehicle.transform.localPosition.y + 2, vehicle.transform.localPosition.z);
        }
    }

    void FixedUpdate()
    {
        if (!flyinOffsetChecked)
        {
            flyingDistance = CameraChange();
        }
    }

    void GeneralCamera()
    {
        setCameraPriority(airCamera, 1);

        flyinOffsetChecked = false;

        if (!offsetChecked)
        {
            offsetChecked = true;
            repositionFlyinCamTimer = 6f;
        }

        float tmpDistance = -distance;

        if (!lookFromBehind)
        {
            tmpDistance *= -2;
        }
        
        cameraDirection = (vehicleCamera.position - vehicle.transform.position).normalized;
        vehicleForward = (vehicle.transform.forward).normalized;

        wantedPosition = vehicle.transform.position;
        transform.DOMove(wantedPosition, 0.2f);
        
        cameraRepositionTime = (100 - ((vehicleController.currentSpeed * 100) / repositionSpeedReach)) / 100;

        if (Input.GetButton(cameraInvert))
        {
            if (vehicleController.currentSpeed >= -minInvertSpeed)
            {
                transform.eulerAngles = (new Vector3(0, vehicle.transform.rotation.eulerAngles.y + 180, 0));
            }
            else if (vehicleController.currentSpeed < -minInvertSpeed)
            {
                transform.eulerAngles = (new Vector3(0, vehicle.transform.rotation.eulerAngles.y, 0));
            }
        }
        else if (Input.GetAxis("HorizontalCamera") == 0 && Input.GetAxis("VerticalCamera") == 0 && vehicleController.currentSpeed != 0)
        {
            if (vehicleController.currentSpeed >= minInvertSpeed)
            {
                transform.DOLocalRotate(new Vector3(0, vehicle.transform.rotation.eulerAngles.y, 0), ((maxRepositionTime - minRepositionTime) * cameraRepositionTime) + minRepositionTime);
            }
            else if (vehicleController.currentSpeed <= -minInvertSpeed)
            {
                transform.DOLocalRotate(new Vector3(0, vehicle.transform.rotation.eulerAngles.y + 180, 0), ((maxRepositionTime - minRepositionTime) * cameraRepositionTime) + minRepositionTime);
            }
        }
        else
        {
            if (vehicleController.currentSpeed >= 0)
            {
                cameraSensibility = (Vector3.Dot(cameraDirection, vehicleForward) * -cameraMaxSensibility) + (cameraMaxSensibility + cameraMinSensibility);
            }
            else
            {
                cameraSensibility = (Vector3.Dot(cameraDirection, -vehicleForward) * -cameraMaxSensibility) + (cameraMaxSensibility + cameraMinSensibility);
            }

            float cameraY = (Vector3.Dot(cameraDirection, vehicle.transform.up) * -cameraMaxSensibility) + cameraMaxSensibility;

            transform.DOLocalRotate(new Vector3(0, (Input.GetAxis("HorizontalCamera") * cameraSensibility) *10, 0), 0.5f, RotateMode.LocalAxisAdd);
        }

        if ((Input.GetAxis("VerticalCamera") == -1) /*|| (vehicle.transform.eulerAngles.x > 60 && vehicle.transform.eulerAngles.x < 300)*/)
        {
            Debug.Log("Vehicle Rotation X = " + vehicle.transform.eulerAngles.x);
            setCameraPriority(lowerCamera, 5);
            setCameraPriority(upperCamera, 0);
        }
        else if ((Input.GetAxis("VerticalCamera") == 1))
        {
            setCameraPriority(upperCamera, 5);
            setCameraPriority(lowerCamera, 0);
        }
        else
        {
            setCameraPriority(lowerCamera, 0);
            setCameraPriority(upperCamera, 0);
        }

        wallBlock();
    }

    void AirCamera()
    {
        if(!flyinOffsetChecked)
        {
            flyinOffsetChecked = true;
        }

        offsetChecked = false;

        //setCameraPriority(airCamera, 6);

        transform.DOMove(vehicle.transform.position, 0.4f);
        transform.DOLookAt(vehicle.transform.position, 5f);
        
        transform.DOLocalRotate(new Vector3(0, (Input.GetAxis("HorizontalCamera") * cameraSensibility) * 5, 0), 0.5f, RotateMode.LocalAxisAdd);
    }

    void CameraInvert()
    {
        transform.DOLocalRotate(new Vector3(0, vehicle.transform.rotation.eulerAngles.y + 180, 0), 0.2f);
    }

    #region Start settings methods

    private void generalCameraSettings ()
    {
        transform.localScale = new Vector3(distance, distance, distance);
        generalCamera.transform.localPosition = new Vector3(0, height, -0.5f);
        generalCamera.transform.eulerAngles = new Vector3(orientation, 0, 0);
        rayPoint.transform.localPosition = new Vector3(0, height, -0.5f);
        upperCamera.transform.localPosition = new Vector3(0, maxVerticalDistance, -0.3f);
    }

    private void checkVirtualCamera ()
    {
        generalCamera = transform.Find("GeneralCamera").GetComponent<CinemachineVirtualCamera>();
        firstPersonCamera = transform.Find("FirstPersonCamera").GetComponent<CinemachineVirtualCamera>();
        airCamera = transform.Find("FlyinCamera").GetComponent<CinemachineVirtualCamera>();
        upperCamera = transform.Find("UpperCamera").GetComponent<CinemachineVirtualCamera>();
        lowerCamera = transform.Find("LowerCamera").GetComponent<CinemachineVirtualCamera>();
        upperCamera.m_LookAt = vehicle.transform;
        airCamera.m_LookAt = vehicle.transform;
    }

    private void checkCollisionStartPoints()
    {
        rayPoint = transform.GetChild(5);
        nWheels = vehicleController.wheelInfos.Count;
        pinPoints = new Vector3[nWheels];
    }

    #endregion

    #region Runtime check methods

    public bool FlyingCheck()
    {
        return (vehicle.GetComponent<NewVehicleController>().isFlying && flyingDistance);
    }

    private bool CameraChange()
    {
        RaycastHit hit;
        Debug.DrawRay(vehicle.transform.position, -Vector3.up, Color.yellow);

        if (Physics.Raycast(vehicle.transform.position, -Vector3.up, out hit))
        {
            if (hit.distance > heightToActivate) return true;
            else return false;
        }
        return true;
    }

    private void FlyinCameraRepositioning()
    {
        if (repositionFlyinCamTimer > 2.5f)
        {
            repositionFlyinCamTimer -= Time.deltaTime;
        }

        if (repositionFlyinCamTimer < 2.5f)
        {
            repositionFlyinCamTimer = 2.5f;
        }
    }

    void wallBlock()
    {
        RaycastHit hit;
        Vector3 dir = (vehicleCamera.position - vehicle.transform.position);

        int n = 0;

        foreach (NewVehicleController.WheelInfo wheelInfo in vehicleController.wheelInfos)
        {
            pinPoints[n] = (wheelInfo.wheelCollider.transform.position);
            pinPoints[n].y += 0.5f;
            Debug.DrawRay(pinPoints[n], (vehicleCamera.position - pinPoints[n]), Color.red);
            n++;
        }
        
        float distanceScale = -0.5f;
        tmpDistance = -0.5f;

        for (int i = 0; i < pinPoints.Length; i++)
        {
            if (Physics.Raycast(pinPoints[i], (rayPoint.position - pinPoints[i]).normalized, out hit, distance) && hit.collider.gameObject.tag == ("Terrain"))
            {
                float wallDistance = hit.distance;

                if (wallDistance >= (distance * 0.6f))
                {
                    tmpDistance = -0.33f;
                }
                else if (wallDistance < (distance * 0.6f) && wallDistance >= (distance * 0.25f))
                {
                    tmpDistance = -0.25f;
                }
                else if (wallDistance < (distance * 0.25f))
                {
                    tmpDistance = -0.2f;
                }
            }

            if (tmpDistance > distanceScale)
            {
                distanceScale = tmpDistance;
                //Debug.Log(distanceScale);
            }
        }

        if (distanceScale < -0.21f)
        {
            setCameraPriority(firstPersonCamera, 0);
        }
        else
        {
            setCameraPriority(firstPersonCamera, 10);
        }
        
        transform.GetChild(1).DOLocalMoveZ(distanceScale, 0.5f);
    }

    private void setCameraPriority(CinemachineVirtualCamera actualCamera, int priority)
    {
        actualCamera.Priority = priority;
    }

    #endregion
}

