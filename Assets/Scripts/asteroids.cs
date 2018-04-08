using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asteroids : MonoBehaviour 
{
	//the radius you want GameObjects to spawn in
	public float radius;
	public int puntoDiRaggio;
	public int altezzaDiSpawn;
	public int numberOfAsteroids;
	private int asteroidsInGame;
	public GameObject asteroid;
	public float dimension;
	private float dim;
	private Vector3 randomSize;


	void Start () 
	{	
		for (asteroidsInGame = 0; asteroidsInGame < numberOfAsteroids; asteroidsInGame++) 
		{
		    CreateAsteroids ();
		}			
	}


	void CreateAsteroids ()
	{
	
		Vector3 rngVector = new Vector3 (Random.Range (-radius, radius), Random.Range (-altezzaDiSpawn,altezzaDiSpawn), Random.Range (-radius, radius));
		rngVector.Normalize ();
		rngVector *= Random.Range (puntoDiRaggio, radius);
		GameObject.Instantiate (asteroid, new Vector3 () + rngVector, Random.rotation);
		dim = Random.Range(1,dimension);
		randomSize = new Vector3(1,1,1)*dim; 
		asteroid.transform.localScale = randomSize;

	}
}
