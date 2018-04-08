using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cooldown : MonoBehaviour {

    [HideInInspector] public float timer;
    [HideInInspector] public bool gameHasStarted = false;

    private Text HUDcd;

    void Start ()
    {
        HUDcd = GameObject.Find("321Go!").GetComponent<Text>();
        timer = 3.0f;
    }
	
	void Update ()
    {
	    if (timer > 0)
        {
            timer -= Time.deltaTime;
            HUDcd.text = ((int)timer+1).ToString();
        }
        else
        {
            StartCoroutine(SetGo());
        }
	}

    IEnumerator SetGo ()
    {
        gameHasStarted = true;
        HUDcd.text = "Go!";

        yield return new WaitForSeconds(0.5f);

        HUDcd.gameObject.SetActive(false);
        Quest.currentQuest.ChangeMissionStatus(Quest.MissionStatus.RUNNING);

        yield return null;
    }
}
