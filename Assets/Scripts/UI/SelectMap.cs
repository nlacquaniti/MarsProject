using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SelectMap : MonoBehaviour
{
    GameObject HUDObject = null;
    Image mapImage = null;
    Camera mapCamera = null;
    Image mapBackground = null;
    Image mapBorder = null;
    GameObject selectMapObject = null;
    GameObject playerTarget = null;
    GameObject playerIcon = null;
    
    List<Image> imagesIcons = new List<Image>();
    bool iconCreated = false;
    int mapSize = 900;
    public Sprite[] mapsPreviews;



    void Start()
    {
        HUDObject = GameObject.Find("HUD");

        GameObject mapBorderObject = new GameObject("MapBorder");
        mapBorderObject.transform.parent = HUDObject.transform;
        mapBorderObject.transform.localScale = new Vector3(1, 1, 1);
        //mapBorder = mapBorderObject.AddComponent<Image>();
        //mapBorder.color = Color.black;
        //mapBorder.rectTransform.sizeDelta = new Vector2(mapSize + 16, mapSize + 16);
        //mapBorder.rectTransform.localScale = new Vector3(1, 1, 1);
        //mapBorder.enabled = false;
        mapBorderObject.transform.localPosition = Vector3.zero;

        mapBackground = HUDObject.GetComponent<Image>();

        selectMapObject = new GameObject("SelectMap");
        selectMapObject.transform.parent = mapBorderObject.transform;

        GameObject selectMapCamera = new GameObject("MapCamera");
        selectMapCamera.transform.position = Vector3.zero;

        RenderTexture renderTexture = new RenderTexture(mapSize, mapSize, 1, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
        //Material mapMaterial = new Material(Shader.Find("Unlit/Texture"));
        //mapMaterial.mainTexture = renderTexture;

        mapCamera = selectMapCamera.AddComponent<Camera>();
        mapCamera.orthographic = true;
        mapCamera.orthographicSize = 3000;
        mapCamera.nearClipPlane = 0;
        mapCamera.farClipPlane = 7500;
        mapCamera.clearFlags = CameraClearFlags.SolidColor;
        mapCamera.backgroundColor = Color.black;
        mapCamera.cullingMask = 9 << 0;
        mapCamera.targetTexture = renderTexture;

        Quaternion cameraRot = mapCamera.transform.rotation;
        cameraRot.eulerAngles = new Vector3(90, 0, 0);
        mapCamera.transform.rotation = cameraRot;

        Vector3 cameraPos = new Vector3(3000, 5000, 3000);
        mapCamera.transform.position = cameraPos;
        mapCamera.enabled = false;

        mapImage = selectMapObject.AddComponent<Image>();
        mapImage.rectTransform.sizeDelta = new Vector2(mapSize, mapSize);
        mapImage.rectTransform.localScale = new Vector3(1, 1, 1);
        
        if (SceneManager.GetActiveScene().name.Contains("VallesMarineris"))
        {
            mapImage.sprite = mapsPreviews[0];
        }
        else if (SceneManager.GetActiveScene().name.Contains("OlympusMons"))
        {
            mapImage.sprite = mapsPreviews[1];
        }
        else if (SceneManager.GetActiveScene().name.Contains("Polonord"))
        {
            mapImage.sprite = mapsPreviews[2];
        }
        else if (SceneManager.GetActiveScene().name.Contains("Crateri"))
        {
            mapImage.sprite = mapsPreviews[3];
        }

        //mapImage //mapImage.material = mapMaterial;
        mapImage.enabled = false;
        selectMapObject.transform.localPosition = Vector3.zero;

        StartCoroutine(CreateIcons());
    }

    void Update()
    {
        if (Input.GetButtonDown("Map") || Time.timeScale < 1)
        {
            if (Time.timeScale < 1)
            {
                mapImage.enabled = false;
            }
            else
            {
                mapImage.enabled = !mapImage.enabled;
            }
                        
            //mapCamera.enabled = mapImage.enabled;
            mapBackground.enabled = mapImage.enabled;
            //mapBorder.enabled = mapImage.enabled;
            EnabledIcons(mapImage.enabled);
        }
    }

    public void UpdateIconsVisibility(int iconIndex, int enabled)
    {
        imagesIcons[iconIndex].color = new Color(1, 1, 1, enabled);
    }

    void LateUpdate()
    {
        if (iconCreated)
        {
            Vector3 targetPos = playerTarget.transform.position;
            Vector3 position = selectMapObject.transform.localPosition;

            int offsetX = (int)((mapSize * targetPos.x) / 6000) - mapSize / 2;
            int offsetZ = (int)((mapSize * targetPos.z) / 6000) - mapSize / 2;

            position += new Vector3(offsetX, offsetZ, 0);
            playerIcon.transform.localPosition = position;

            Vector3 rotation = Vector3.zero;
            rotation.z = -playerTarget.transform.rotation.eulerAngles.y;

            playerIcon.transform.rotation = Quaternion.Euler(rotation);
        }
    }

    IEnumerator CreateIcons()
    {
        yield return new WaitForSeconds(0.5f);

        if (MiniMap.instance != null)
        {
            for (int i = 0; i < MiniMap.instance._Icons.Length; i++)
            {
                if (MiniMap.instance._Icons[i].Icon.sprite != null)
                {
                    GameObject target = MiniMap.instance._Icons[i].TargetPos;

                    string iconName = "Icon";
                    if (target.tag == "Player")
                    {
                        playerTarget = MiniMap.instance._Icons[i].TargetPos;
                        iconName = "PlayerIcon";
                    }                  

                    GameObject iconObject = new GameObject(iconName);
                    iconObject.transform.parent = selectMapObject.transform;
                    iconObject.transform.localScale = new Vector3(1f, 1f, 1f);
                                        
                    Vector3 targetPos = target.transform.position;
                    Vector3 position = selectMapObject.transform.localPosition;

                    float offsetX = ((mapSize * targetPos.x) / 6000) - mapSize / 2;
                    float offsetZ = ((mapSize * targetPos.z) / 6000) - mapSize / 2;

                    position += new Vector3(offsetX, offsetZ, 0);
                    iconObject.transform.localPosition = position;

                    Image img = iconObject.AddComponent<Image>();
                    img.sprite = MiniMap.instance._Icons[i].Icon.sprite;
                    img.rectTransform.sizeDelta = new Vector2(32, 32);

                    if (target.tag.Contains("EnvProperty"))
                    {
                        if (!MiniMap.instance._Icons[i].Icon.name.Contains("ICON"))
                        {
                            float scale = 0.16f;
                            img.rectTransform.sizeDelta = new Vector2(target.transform.lossyScale.x * scale, target.transform.lossyScale.z * scale);
                        }
                        /*else
                        {
                            if (target.transform.lossyScale.x < target.transform.lossyScale.z)
                            {
                                img.rectTransform.sizeDelta = new Vector2(target.transform.lossyScale.x / 15, target.transform.lossyScale.x / 15);
                            }
                            else
                            {
                                img.rectTransform.sizeDelta = new Vector2(target.transform.lossyScale.z / 15, target.transform.lossyScale.z / 15);
                            }
                        }*/
                    }

                    img.enabled = false;
                    imagesIcons.Add(img);

                    if (playerIcon == null)
                    {
                        playerIcon = GameObject.Find("PlayerIcon");
                    }
                }
            }

            MiniMap.instance.mainMap = this;
        }

        iconCreated = true;
    }

    void EnabledIcons(bool enable)
    {
        foreach (Image icon in imagesIcons)
        {
            icon.enabled = enable;
        }
    }
}
