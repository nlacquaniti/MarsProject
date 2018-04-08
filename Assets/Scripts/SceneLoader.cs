using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public static class SceneLoader  
{
    public static void GoToScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public static IEnumerator LoadSceneAsync(string scene)
    {
        LoadingScreen.instance.EnableUI();
        AudioManager.Audio.PlayMenuSound(MenuSounds.MenuEnter);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        while (!asyncLoad.isDone)
        {
            LoadingScreen.instance.percentBar.fillAmount = asyncLoad.progress + 0.1f;
            yield return null;
        }

    }
}


