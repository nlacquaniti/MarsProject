using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsInspector : MonoBehaviour
{
    /*[Header("CHECK VEICOLO")]
    [Space(10)]
    [SerializeField] private bool m_StaVolando;
    [SerializeField] private bool m_StaFrenandoInAria;
    [SerializeField] private bool m_PropulsoreAttivo;
    [Space(10)]
    [SerializeField] private string m_TerrenoAttuale;
    [Space(10)]
    [SerializeField] private int m_PressioneAccellerazione;
    [SerializeField] private int m_PressioneFrenata;
    [Space(10)]
    [SerializeField] private float m_Accellerazione;
    [SerializeField] private int m_NRuoteATerra;
    
    private float m_Time;
    private float m_Input;
    private VehicleController m_Vehicle;
    private string m_VerticalInput = "Triggers";


    private void Start()
    {
        m_Vehicle = GameObject.FindGameObjectWithTag("Player").GetComponent<VehicleController>();
        m_Time = Time.deltaTime;
        
    }


    private void Update()
    {
        m_Input = Input.GetAxis(m_VerticalInput);

        m_StaVolando = m_Vehicle.IsFlying;
        m_NRuoteATerra = m_Vehicle.WheelHittingCount;
        m_TerrenoAttuale = m_Vehicle.ContactTexture;
        m_PropulsoreAttivo = m_Vehicle.IsTurboActive;

        if (m_Input > 0)
        {
            m_PressioneAccellerazione = (int)m_Input * 100;
            m_PressioneFrenata = 0;
        }
        else if (m_Input < 0)
        {
            m_PressioneFrenata = (int)m_Input * 100;
            m_PressioneAccellerazione = 0;
        }

        if(m_Input < 0 && m_Vehicle.IsFlying)
        {
            m_StaFrenandoInAria = true;
        }
        else
        {
            m_StaFrenandoInAria = false;
        }

        m_Accellerazione = (m_Vehicle.CurrentSpeed * 100) / m_Vehicle.maxSpeed;
    
    }*/

}
