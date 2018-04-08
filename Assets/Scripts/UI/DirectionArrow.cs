using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    public GameObject arrow;
    public Transform target;

    private Vector3 m_Direction;

	void LateUpdate ()
    {
        m_Direction = (arrow.transform.position - target.position).normalized;
        arrow.transform.forward = m_Direction;
	}
}
