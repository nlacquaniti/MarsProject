using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAsteroids : MonoBehaviour
{
    private Renderer m_Render;
    public GameObject asteroid;

	// Use this for initialization
	void Start ()
    {
        m_Render = GetComponent<Renderer>();
        InvokeRepeating("SpawnAsteroid", 1f, 0.5f);
	}
	
	// Update is called once per frame
	void Update ()
    {
        Debug.Log(m_Render.bounds.min.z);
	}

    private void SpawnAsteroid ()
    {
        Vector3 min = m_Render.bounds.min;
        Vector3 max = m_Render.bounds.max;
        GameObject clone = asteroid;
        clone = Instantiate(clone);
        clone.transform.position = new Vector3(Random.Range(min.x, max.x), max.y, Random.Range(min.z, max.z));
        Debug.Log(clone.transform.position);
    }
}
