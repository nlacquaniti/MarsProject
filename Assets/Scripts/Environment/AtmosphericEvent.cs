using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtmosphericEvent : MonoBehaviour
{
    public enum EventType { SandStorm = 0, MeteorStorm = 1 };
    public EventType eventType;
    public float eventForce = 100;
    public EllipsoidParticleEmitter[] sandStormEmitters;
    public GameObject meteorPrefab;

    //Private variables for optimization
    EventType currentEventType;
    float currentForce = 0;
    List<GameObject> meteorsList = new List<GameObject>();
    ////////////////////////////////////


    void Awake()
    {
       // GetComponent<MeshRenderer>().enabled = false;
        currentEventType = eventType;
    }

    void Update()
    {
        PlayEffect(eventType);
    }


    void PlayEffect(EventType type)
    {
        if (currentEventType != type || currentForce != eventForce)
        {
            foreach (EllipsoidParticleEmitter emitter in sandStormEmitters)
            {
                emitter.emit = type == EventType.SandStorm;
                emitter.localVelocity = new Vector3(-eventForce, 0, 0);
                emitter.minSize = transform.localScale.x / 5f;
                emitter.maxSize = transform.localScale.x / 1.5f;
            }

            currentEventType = type;
            currentForce = eventForce;

            if (type == EventType.MeteorStorm)
            {
                meteorsList.Clear();
                InvokeRepeating("PlayMeteorStorm", 200 / eventForce, 200 / eventForce);
            }
        }
    }


    void PlayMeteorStorm()
    {
        if (meteorsList.Count < Mathf.Round(eventForce / 10))
        {
            for (int i = 0; i < Mathf.Round(eventForce / 10); i++)
            {
                GameObject newMeteor = Instantiate(meteorPrefab, transform.position, transform.rotation);
                newMeteor.SetActive(false);
                newMeteor.transform.parent = transform;
                meteorsList.Add(newMeteor);
            }
        }

        foreach (GameObject m in meteorsList)
        {
            if (m.transform.position.y+250 < transform.position.y)
            {
                Vector3 newMeteorPos = transform.position;
                newMeteorPos.x += Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2);
                newMeteorPos.z += Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2);
                newMeteorPos.y += Random.Range(transform.localScale.x, transform.localScale.x*3);
                m.GetComponent<ConstantForce>().force = new Vector3(0, Random.Range(-30000, -10000), 0);
                m.transform.position = newMeteorPos;
                m.SetActive(false);
            }
            else if (!m.activeSelf)
            {
                m.SetActive(true);
            }
        }
    }


    void OnTriggerStay(Collider other)
    {
        if (eventType == EventType.SandStorm)
        {
            if (other.tag == "Player")
            {
                Rigidbody playerRb = other.gameObject.GetComponent<Rigidbody>();
                float totalWindForce = eventForce * (5000 / Vector3.Distance(transform.position, other.gameObject.transform.position));
                playerRb.AddForce(new Vector3(-totalWindForce, 0, 0));
            }
        }
    }
}
