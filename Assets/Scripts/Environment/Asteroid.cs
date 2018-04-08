using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    ParticleSystem explosionParticle;


    void Start ()
    {
        explosionParticle = transform.GetChild(2).GetComponent<ParticleSystem>();
	}
    
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Terrain")
        {
            explosionParticle.Play();
        }
    }
}
