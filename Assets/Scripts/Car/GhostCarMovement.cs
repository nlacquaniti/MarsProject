using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCarMovement : MonoBehaviour
{
    public MovementStorage storedMovement;

    [Header("Attivami per avviare i movimenti della macchina fantasma")] //Da eliminare, è inutile
    [SerializeField] private bool m_HasRecord = false; //Togliere il Serialize Field è inutile
    private int m_Index = 0;

    void Update()
    {
        if(m_HasRecord)
        {
            Move();
        }
    }

    private void Move()
    {
        if (m_Index < storedMovement.Position.Count -1)
        {
            transform.position = storedMovement.Position[m_Index];
            transform.rotation = storedMovement.Rotation[m_Index];
            m_Index++;
        }
    }
}
