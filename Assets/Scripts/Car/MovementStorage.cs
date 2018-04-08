using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementStorage : MonoBehaviour
{
    public SaveMovement SaveMovement;

    private static List<Vector3> m_PositionRecord = new List<Vector3>();
    private static List<Quaternion> m_RotationRecord = new List<Quaternion>();

    public List<Vector3> Position { get { return m_PositionRecord; } }
    public List<Quaternion> Rotation { get { return m_RotationRecord; } }

    private bool m_Save = true;

	void Update ()
    {
        if(!SaveMovement.isRecording && m_Save)
        {
            m_Save = false;
            SaveMovement.StoreMovement(m_PositionRecord, m_RotationRecord);
        }
		
	}
}
