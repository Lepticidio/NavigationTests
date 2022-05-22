using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : PathEntity
{
    public bool m_bFollowingMode, m_bPathCreated;
    bool m_bMoving;
    float m_fMinDistance = 0.05f;
    public float m_fSpeed;
    Vector3 m_vCurrentGoal;
    public Transform m_oGoal;
    public Pathfinder m_oPathfinder;
    public List<Vector3> m_tCurrentPath;
    // Start is called before the first frame update

    public void StartPath(MapType _oMapType, NodeMap _oNodeMap)
    {
        m_bPathCreated = false;
        m_bMoving = false;
        RandomPosition(_oMapType, _oNodeMap);
        StartCoroutine( CreatePath(_oNodeMap));

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
                if(m_bPathCreated)
                {
                    MoveToNextPosition();
                }
            }
        }
    }
    public IEnumerator CreatePath(NodeMap _oNodeMap)
    {
        StartCoroutine( m_oPathfinder.GetPath(transform.position, m_oGoal.position, _oNodeMap));
        while(!m_oPathfinder.m_bPathFound)
        {
            yield return null;
        }
        m_tCurrentPath = m_oPathfinder.m_tPath;
        MoveToNextPosition();
        CalculatePathLength();
        m_bPathCreated = true;
    }
    public void MoveTo(Vector3 _vPosition)
    {
        m_vCurrentGoal = _vPosition;
        m_bMoving = true;
        transform.LookAt(m_vCurrentGoal);
    }
    public void MoveToNextPosition()
    {
        if(m_tCurrentPath.Count > 1)
        {
            MoveTo(m_tCurrentPath[m_tCurrentPath.Count - 1]);
            m_tCurrentPath.RemoveAt(m_tCurrentPath.Count - 1);
        }
    }
    public float CalculatePathLength()
    {
        float fResult = 0;
        Vector3 vTempPos = transform.position;
        for (int i = m_tCurrentPath.Count -1; i >= 0 ; i--)
        {
            fResult += (m_tCurrentPath[i] - vTempPos).magnitude;
            vTempPos = m_tCurrentPath[i];
        }
        return fResult;
    }
    void OnDrawGizmos()
    {
        for (int i = 0; i < m_oPathfinder.m_tCenters.Count - 1; i++)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(m_oPathfinder.m_tCenters[i], m_oPathfinder.m_tCenters[i + 1]);
        }
        for (int i = 0; i < m_tCurrentPath.Count - 1; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(m_tCurrentPath[i], m_tCurrentPath[i + 1]);
        }
        if(m_tCurrentPath.Count > 0)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(m_tCurrentPath[m_tCurrentPath.Count - 1], m_vCurrentGoal);
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, m_vCurrentGoal);
    }

}
