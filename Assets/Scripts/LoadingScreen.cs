using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen instance;
    private GameObject _PercentBar;
    private GameObject _Background;
    private GameObject _LoadingBar;
    private GameObject _LoadingAnim;
    public Image percentBar;
    

    private void Awake()
    {
        instance = this;
        percentBar = transform.Find("PercentBar").GetComponent<Image>();
        _LoadingAnim = transform.Find("LoadingAnimation").gameObject;
        _PercentBar = transform.Find("PercentBar").gameObject;
        _Background = transform.Find("Background").gameObject;
        _LoadingBar = transform.Find("LoadingBar").gameObject;

    }


    public void EnableUI()
    {
        _LoadingAnim.SetActive(true);
        _PercentBar.SetActive(true);
        _Background.SetActive(true);
        _LoadingBar.SetActive(true);
    }




    

}
