using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public bool m_bFollowingMode, m_bPathCreated;
    bool m_bMoving;
    float m_fMinDistance = 0.05f;
    public float m_fSpeed;
    Vector3 m_vCurrentGoal;
    public Transform m_oGoal;
    public Pathfinder m_oPathfinder;
    public NodeMap m_oMap;
    public List<Vector3> m_tCurrentPath;
    // Start is called before the first frame update

    private void Update()
    {
        if(!m_bPathCreated)
        {
            m_tCurrentPath = m_oPathfinder.GetPath(transform.position, m_oGoal.position, m_oMap);
            m_bPathCreated = true;
        }
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
    public void MoveToNextPosition()
    {
        MoveTo(m_tCurrentPath[m_tCurrentPath.Count - 1]);
        m_tCurrentPath.RemoveAt(m_tCurrentPath.Count - 1);
    }
    void OnDrawGizmos()
    {
        //for(int i = 0; i < m_tCurrentPath.Count - 1; i++)
        //{
        //    Gizmos.color = Color.green;
        //    Gizmos.DrawLine(m_tCurrentPath[i], m_tCurrentPath[i + 1]);
        //}
    }

}
