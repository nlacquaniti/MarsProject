using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotazioneasteroidi : MonoBehaviour 
{

	public GameObject sun;
	public float speed;
	public Vector3 axes;

	// Use this for initialization
	void Start () 
	{
		

	}

	// Update is called once per frame
	void Update () 
	{
		OrbitAround ();//questo serve per far ruotare il pianeta x intorno al sole
	}

	void OrbitAround ()
	{
		transform.RotateAround (sun.transform.position, axes, speed * Time.deltaTime);
	}


}
