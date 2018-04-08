using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasStation : MonoBehaviour
{
    [SerializeField] private float m_RecarghingRate;
    [SerializeField] private float m_DistanceToCharge;

    private GameObject m_Car;
    private float m_Distance;

    private void Start()
    {
        m_Car = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        m_Distance = Vector3.Distance(m_Car.transform.position, transform.position);

        if(m_Distance < m_DistanceToCharge)
        {
            RechargeCar();
        }
    }

    private void RechargeCar()
    {
        if (m_Car.GetComponent<NewVehicleController>().currentFuel >= 0 && m_Car.GetComponent<NewVehicleController>().currentFuel <= 100)
        {
            m_Car.GetComponent<NewVehicleController>().currentFuel += m_RecarghingRate * Time.deltaTime;
        }
    }


}
