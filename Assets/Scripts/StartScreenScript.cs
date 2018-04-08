using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StartScreenScript : MonoBehaviour
{
    public GameObject loadingScreenCamera;
    public GameObject canvasStartScreen;
    public Text[] textButtons;
    public Color highlightColor;
    Color defaultColor;
    int selectedButton = 0;
    bool needToRelease = false;


    void Start()
    {
        defaultColor = textButtons[0].color;
        ChangeTextsColor();
    }
    
    void Update()
    {
        if (!needToRelease)
        {
            if (Input.GetAxis("VerticalHandling") > 0)
            {
                selectedButton -= selectedButton > 0 ? 1 : 0;
                AudioManager.Audio.PlayMenuSound(MenuSounds.ChangeSelection);
                ChangeTextsColor();
                needToRelease = true;
            }
            else if (Input.GetAxis("VerticalHandling") < 0)
            {
                selectedButton += selectedButton < textButtons.Length - 1 ? 1 : 0;
                AudioManager.Audio.PlayMenuSound(MenuSounds.ChangeSelection);
                ChangeTextsColor();
                needToRelease = true;
            }
        }
        else
        {
            if (Input.GetButtonUp("VerticalHandling") || Input.GetAxis("VerticalHandling") == 0)
            {
                needToRelease = false;
            }        
        }
                
        if (Input.GetAxis("Accessory2") > 0 || Input.GetKeyDown(KeyCode.Return))
        {
            AudioManager.Audio.PlayMenuSound(MenuSounds.MenuEnter);

            switch (selectedButton)
            {
               case 0:
                    
                    canvasStartScreen.SetActive(false);
                    loadingScreenCamera.SetActive(true);
                    StartCoroutine(SceneLoader.LoadSceneAsync("MenuSelezione")); break;

               case 1: Debug.Log("NO CREDITS :'-("); break;

               case 2:
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif
                    break;
            }
        }
	}

    void ChangeTextsColor()
    {
        for (int i = 0; i < textButtons.Length; i++)
        {
            textButtons[i].color = (i == selectedButton) ? highlightColor : defaultColor;
        }
    }
}
