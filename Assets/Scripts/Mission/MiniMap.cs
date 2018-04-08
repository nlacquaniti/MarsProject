using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MiniMap : MonoBehaviour
{
    public static MiniMap instance;

    private const int ICON_OFFSET = 60; 
    private const int ICON_POS = 532;

    public MiniMapIcons[] _Icons;
    public SelectMap mainMap = null;
    private Camera _MiniMapCamera;
    private Transform _Hud;
    private int i = 0;


    private void Start()
    {
        instance = this;
        _MiniMapCamera = GameObject.Find("MiniMapCamera").GetComponent<Camera>();
        _Hud = GameObject.Find("IconLayer").transform;

        var _TargetsCount = GameObject.FindGameObjectsWithTag("Target").Length;
        var _WindEnv = GameObject.FindGameObjectsWithTag("EnvPropertyWind").Length;
        var _RadEnv = GameObject.FindGameObjectsWithTag("EnvPropertyRadiation").Length;

        _Icons = new MiniMapIcons[_TargetsCount + _WindEnv * 2 + _RadEnv * 2 + 2];
    }



    public void GetIcon(Image img, GameObject target, bool isClamped)
    {
        if (img != null)
        {
            Image _Img = Instantiate(img);

            _Icons[i] = new MiniMapIcons { Icon = _Img, TargetPos = target, IsClamped = isClamped };

            if (!isClamped)
            {
                _Icons[i].Icon.transform.SetParent(this.transform.parent);
            }
            else
            {
                _Icons[i].Icon.transform.SetParent(_Hud);
            }

            i++;

            if (i >= _Icons.Length)
                StartCoroutine(DrawIcons());
        }
    }

    public IEnumerator RemoveIcon(GameObject obj)
    {
        for (int i = 0; i < _Icons.Length; i++)
        {
            if (_Icons[i].TargetPos == obj)
            {
                _Icons[i].Icon.enabled = false;
                if (mainMap != null) { mainMap.UpdateIconsVisibility(i, 0); }
                break;
            }
            yield return null;
        }
    }

    public IEnumerator SetIconVisible(GameObject obj)
    {
        for (int i = 0; i < _Icons.Length; i++)
        {
            if (_Icons[i].TargetPos == obj)
            {
                _Icons[i].Icon.enabled = true;

                if (mainMap != null) { mainMap.UpdateIconsVisibility(i, 1); }
                
                break;
            }
            yield return null;

        }
    }

    private IEnumerator DrawIcons()
    {
        while (true)
        {
            for (int i = 0; i < _Icons.Length; i++)
            {
                if (_Icons[i].Icon.enabled && _Icons[i].IsClamped) //TARGET MISSIONS
                {
                    Vector3 _ScreenPos = _Icons[i].TargetPos.transform.position - NewVehicleController.vehicleController.transform.position;

                    Vector3 _TargetNormalized = _Icons[i].TargetPos.transform.position;

                    _TargetNormalized.y = NewVehicleController.vehicleController.transform.position.y;

                    float _DistanceToIcon = Vector3.Distance(NewVehicleController.vehicleController.transform.position, _TargetNormalized)
                                                             * (this.transform.position.x / ICON_POS);

                    _DistanceToIcon = Mathf.Clamp(_DistanceToIcon, 0, this.transform.position.x - ICON_OFFSET / 1.3f);
                    float _DeltaY = Mathf.Atan2(_ScreenPos.x, _ScreenPos.z) * Mathf.Rad2Deg - 270 - _MiniMapCamera.transform.eulerAngles.y;

                    _ScreenPos.x = _DistanceToIcon * Mathf.Cos(_DeltaY * Mathf.Deg2Rad) * -1;
                    _ScreenPos.y = _DistanceToIcon * Mathf.Sin(_DeltaY * Mathf.Deg2Rad);

                    _Icons[i].Icon.transform.position = new Vector3(_ScreenPos.x, _ScreenPos.y, 0) + this.transform.position;

                }
                else if(!_Icons[i].IsClamped && _Icons[i].Icon.enabled) //VEHICLE, ENV
                {
                    Vector3 _ScreenPos = _Icons[i].TargetPos.transform.position - NewVehicleController.vehicleController.transform.position;

                    Vector3 _TargetNormalized = _Icons[i].TargetPos.transform.position;

                    _TargetNormalized.y = NewVehicleController.vehicleController.transform.position.y;

                    float _DistanceToIcon = Vector3.Distance(NewVehicleController.vehicleController.transform.position, _TargetNormalized)
                                                             * (this.transform.position.x / ICON_POS);

                    float _DeltaY = Mathf.Atan2(_ScreenPos.x, _ScreenPos.z) * Mathf.Rad2Deg - 270 - _MiniMapCamera.transform.eulerAngles.y;

                    _ScreenPos.x = _DistanceToIcon * Mathf.Cos(_DeltaY * Mathf.Deg2Rad) * -1; ;
                    _ScreenPos.y = _DistanceToIcon * Mathf.Sin(_DeltaY * Mathf.Deg2Rad); 

                    _Icons[i].Icon.transform.position = new Vector3(_ScreenPos.x, _ScreenPos.y, 0) + this.transform.position;
                }
            }

            yield return null;
        }
    }

}


