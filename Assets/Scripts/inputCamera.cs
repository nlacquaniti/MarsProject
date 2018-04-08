using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class inputCamera : MonoBehaviour {

	public PlayableDirector myDirector;
	[Header("Timelines")]
	public PlayableAsset play;
	public PlayableAsset playToSetting;
	public PlayableAsset settingsToPlay;
	public PlayableAsset playToMultiplayer;
	public PlayableAsset multiplayerToPlay;
	public PlayableAsset settingsToCredits;
	public PlayableAsset creditsToSetting;
	public PlayableAsset multiplayerToExit;
	public PlayableAsset exitToMultiplayer;
	public PlayableAsset creditsToExit;
	public PlayableAsset exitToCredits;


	public int zoneController;


	// Use this for initialization
	void Start () 
	{
		myDirector = GetComponent<PlayableDirector> ();
		myDirector.playableAsset = play;
		zoneController = 1;
		myDirector.Play ();

//		myDirector.playableAsset.duration.(myDirector.time);


	}

	// Update is called once per frame
	void Update () 
	{
		if (myDirector.playableAsset.duration == (myDirector.time))
		
		{
			
			if (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetButtonDown ("BumperL")) 
			
			{
				LeftBumperPressed ();

			} 

			else if (Input.GetKeyDown (KeyCode.RightArrow) || Input.GetButtonDown ("BumperR")) 
			
			{
				RightBumperPressed ();

			}
		}
	}

	void LeftBumperPressed()
	{
//		if (zoneController == 1)
//		{
//			myDirector.playableAsset = playToSetting;
//			zoneController = 5;
//			myDirector.Play ();
//		}
//		else if (zoneController == 5)
//		{
//			myDirector.playableAsset = settingsToCredits;
//			zoneController = 4;
//			myDirector.Play ();
//		}
//		else if (zoneController == 4)
//		{
//			myDirector.playableAsset = creditsToExit;
//			zoneController = 3;
//			myDirector.Play ();
//		}
//		else if (zoneController == 3)
//		{
//			myDirector.playableAsset = exitToMultiplayer;
//			zoneController = 2;
//			myDirector.Play ();
//		}
//		 else if (zoneController == 2)
//		{
//			myDirector.playableAsset = multiplayerToPlay;
//			zoneController = 1;
//			myDirector.Play ();
//		}
		switch (zoneController) 
		{
		case 1:
			myDirector.playableAsset = playToSetting;
			zoneController = 5;
			myDirector.Play ();
			break;
		case 5:
			myDirector.playableAsset = settingsToCredits;
			zoneController = 4;
			myDirector.Play ();
			break;
		case 4:
			myDirector.playableAsset = creditsToExit;
			zoneController = 3;
			myDirector.Play ();
			break;
		case 3:
			myDirector.playableAsset = exitToMultiplayer;
			zoneController = 2;
			myDirector.Play ();
			break;
		case 2:
			myDirector.playableAsset = multiplayerToPlay;
			zoneController = 1;
			myDirector.Play ();
			break;

		}


	}


	void RightBumperPressed()
	{
//		if (zoneController ==1)
//		{
//			myDirector.playableAsset = playToMultiplayer;
//			zoneController = 2;
//			myDirector.Play ();
//		}
//		else if (zoneController == 2)
//		{
//			myDirector.playableAsset = multiplayerToExit;
//			zoneController = 3;
//			myDirector.Play ();
//		}
//		else if (zoneController == 3)
//		{
//			myDirector.playableAsset = exitToCredits;
//			zoneController = 4;
//			myDirector.Play ();
//		}
//		else if (zoneController == 4)
//		{
//			myDirector.playableAsset = creditsToSetting;
//			zoneController = 5;
//			myDirector.Play ();
//		}
//		else if (zoneController == 5)
//		{
//			myDirector.playableAsset = settingsToPlay;
//			zoneController = 1;
//			myDirector.Play ();
//		}

		switch (zoneController) 
		{
		case 1:
			myDirector.playableAsset = playToMultiplayer;
			zoneController = 2;
			myDirector.Play ();
			break;
		case 2:
			myDirector.playableAsset = multiplayerToExit;
			zoneController = 3;
			myDirector.Play ();
			break;
		case 3:
			myDirector.playableAsset = exitToCredits;
			zoneController = 4;
			myDirector.Play ();
			break;
		case 4:
			myDirector.playableAsset = creditsToSetting;
			zoneController = 5;
			myDirector.Play ();
			break;
		case 5:
			myDirector.playableAsset = settingsToPlay;
			zoneController = 1;
			myDirector.Play ();
			break;
		}

	}

}
