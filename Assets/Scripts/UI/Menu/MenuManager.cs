using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    static public bool pauseMenuActive = false;


    private int _InterfaceI = 0;
    private int _PauseI = 0;
   
    private bool m_Trigger = false;
    private bool _MenuTrigger = true;

    private float _StartingXScale;
    private float _IncreasedXScale;

    private const float _XScaleMultiplier = 0.3f;

    private Image[] _PauseButtons = new Image[2];
    private Sprite _LightBlue = null;
    private Sprite _DarkBlue = null;


    private System.Collections.IEnumerator _LerpCoroutineStart = null;
    private System.Collections.IEnumerator _LerpCoroutineIncrese = null;

    private void Awake()
    {
        var vehicle = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject go in vehicle)
        {
            if(go.name.Contains("Clone"))
            {
                Destroy(go);
                break;
            }
        }

        StartCoroutine(WaitingCanvas());
    }

   

    private void Update()
    {

        if (m_Trigger)
        {
            if (!pauseMenuActive)
            {
                if (Input.GetAxis(InputContainer.ACCESSORY2) > 0 && _InterfaceI < MenuInizialeContainer.instance.interfaces.Length - 1)
                {
                    if (!MissionSelection.invalidZone)
                    {
                        MenuInizialeContainer.instance.interfaces[_InterfaceI].SetActive(false);

                        if (_InterfaceI == 1)
                        {
                            StatsBar.instance.ChangeParent("Recap&Go");
                        }

                        _InterfaceI++;
                        MenuInizialeContainer.instance.interfaces[_InterfaceI].SetActive(true);


                        if (AudioManager.Audio != null)
                        {
                            AudioManager.Audio.PlayMenuSound(MenuSounds.MenuSlide);
                        }
                    }
                    else
                    {
                        MissionSelection.ShakeLockIcon();
                    }
                }

                else if (Input.GetAxis(InputContainer.ACCESSORYRIGHT) > 0 && _InterfaceI > 0)
                {
                    MenuInizialeContainer.instance.interfaces[_InterfaceI].SetActive(false);

                    if (_InterfaceI == 2)
                    {
                        StatsBar.instance.ChangeParent("Garage");
                    }

                    _InterfaceI--;
                    MenuInizialeContainer.instance.interfaces[_InterfaceI].SetActive(true);

                    if (AudioManager.Audio != null)
                    {
                        AudioManager.Audio.PlayMenuSound(MenuSounds.MenuSlide);
                    }
                }
               
                else if (Input.GetButtonDown(InputContainer.MENU))
                {
                    StartCoroutine(_LerpCoroutineIncrese = ScaleMenu(_PauseButtons[1].transform, false));
                    pauseMenuActive = true;
                    MenuInizialeContainer.instance.pauseBox.SetActive(true);
                }
            }
            else
            {
                if (_MenuTrigger)
                {
                   
                    _MenuTrigger = false;
                    StartCoroutine(_LerpCoroutineIncrese = ScaleMenu(_PauseButtons[0].transform, true));
                }

                if (Input.GetAxisRaw(InputContainer.HORIZONTAL) > 0 && _PauseI < _PauseButtons.Length - 1)
                {
                    _PauseButtons[_PauseI].sprite = _LightBlue;
                    

                    StartCoroutine(_LerpCoroutineStart = ScaleMenu(_PauseButtons[_PauseI].transform, false));
                    _PauseI++;
                    StartCoroutine(_LerpCoroutineIncrese = ScaleMenu(_PauseButtons[_PauseI].transform, true));

                    _PauseButtons[_PauseI].sprite = _DarkBlue;
                    

                }
                else if (Input.GetAxisRaw(InputContainer.HORIZONTAL) < 0 && _PauseI > 0)
                {
                    _PauseButtons[_PauseI].sprite = _LightBlue;
                    

                    StartCoroutine(_LerpCoroutineStart = ScaleMenu(_PauseButtons[_PauseI].transform, false));
                    _PauseI--;
                    StartCoroutine(_LerpCoroutineIncrese = ScaleMenu(_PauseButtons[_PauseI].transform, true));

                    _PauseButtons[_PauseI].sprite = _DarkBlue;
                    
                }

                if (Input.GetButtonDown(InputContainer.SUBMIT))
                {
                    if (_PauseI == 0)
                    {
                        Application.Quit();
                    }
                    else
                    {
                        _PauseI = 0;
                        MenuInizialeContainer.instance.pauseBox.SetActive(false);

                        StartCoroutine(SetMenuTriggerTrue());
                        _MenuTrigger = true;

                        _PauseButtons[0].transform.localScale = new Vector3(_StartingXScale, 1, 1);

                        _PauseButtons[0].sprite = _DarkBlue;
                        _PauseButtons[1].sprite = _LightBlue;


                    }

                }

            }
        }

        if(Input.GetAxis(InputContainer.ACCESSORY2) == 0 && Input.GetAxis(InputContainer.ACCESSORYRIGHT) == 0)
        {
            m_Trigger = true;
        }
        else
        {
            m_Trigger = false;
        }
    }

    private System.Collections.IEnumerator SetMenuTriggerTrue()
    {
        yield return new WaitForSeconds(0.1f);
        pauseMenuActive = false;
    }

    private System.Collections.IEnumerator ScaleMenu(Transform t, bool increase)
    {

        while (true)
        {
            if (increase)
            {
                t.localScale = new Vector3(t.localScale.x + 0.1f, t.localScale.y, t.localScale.z);
                if (t.localScale.x >= _IncreasedXScale)
                {

                    t.localScale = new Vector3(_IncreasedXScale, t.localScale.y, t.localScale.z);
                    break;
                }
            }
            else
            {
                t.localScale = new Vector3(t.localScale.x - 0.1f, t.localScale.y, t.localScale.z);
                if (t.localScale.x <= _StartingXScale)
                {

                    t.localScale = new Vector3(_StartingXScale, t.localScale.y, t.localScale.z);
                    break;
                }
            }
            yield return null;
        }

    }

    private System.Collections.IEnumerator WaitingCanvas()
    {
        while(GameObject.Find("Canvas").transform.Find("Pause") == null)
        {
            yield return null;
        }

        _PauseButtons[0] = GameObject.Find("Canvas").transform.Find("Pause").GetChild(0).Find("Yes").GetComponent<Image>();
        _PauseButtons[1] = GameObject.Find("Canvas").transform.Find("Pause").GetChild(0).Find("No").GetComponent<Image>();

        _LightBlue = _PauseButtons[1].sprite;
        _DarkBlue = _PauseButtons[0].sprite;

        _StartingXScale = _PauseButtons[0].transform.localScale.x;
        _IncreasedXScale = _PauseButtons[0].transform.localScale.x + _XScaleMultiplier;

    }

}
