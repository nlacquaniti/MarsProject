using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMovement : MonoBehaviour
{
    private List<Vector3> m_Position;
    private List<Quaternion> m_Rotation;

    [Header("Disattivami per salvare il percorso")] //Da calcellare, non serve
    [SerializeField] private bool m_IsRecording = true; //Togliere il SerializeField è inutile
    private int m_Index = 0;

    public bool isRecording { get { return m_IsRecording; } }

    void Start ()
    {
        m_Position = new List<Vector3>();
        m_Rotation = new List<Quaternion>();
    }

	void Update ()
    {
        if (m_IsRecording)
        {
            RecordMovement();
        }
    }

    private void RecordMovement()
    {
        m_Position.Insert(m_Index, transform.position);
        m_Rotation.Insert(m_Index, transform.rotation);
        m_Index++;
    }

    public void StoreMovement(List<Vector3> position, List<Quaternion> rotation)
    {
        position.AddRange(m_Position);
        rotation.AddRange(m_Rotation);
    }
}
