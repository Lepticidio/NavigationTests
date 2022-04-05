using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public bool m_bFollowingMode;
    bool m_bMoving;
    float m_fMinDistance = 0.05f;
    public float m_fSpeed;
    Vector3 m_vCurrentGoal;
    public Transform m_oGoal;
    // Start is called before the first frame update


    private void Start()
    {
        MoveTo(m_oGoal.position);
    }
    private void Update()
    {
        if(m_bFollowingMode)
        {
            MoveTo(m_oGoal.position);
        }
        if(m_bMoving)
        {
            Vector3 vToGoal = m_vCurrentGoal - transform.position;
            transform.position = transform.position + vToGoal.normalized * m_fSpeed * Time.deltaTime;
            if(vToGoal.sqrMagnitude < m_fMinDistance)
            {
                m_bMoving = false;
            }
        }
    }
    public void MoveTo(Vector3 _vPosition)
    {
        m_vCurrentGoal = _vPosition;
        m_bMoving = true;
        transform.LookAt(m_vCurrentGoal);
    }
}
