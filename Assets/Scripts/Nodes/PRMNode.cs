using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PRMNode : Node
{

    private LayerMask m_iLayerMask;
    float m_fNodeRadius, m_fNeighbourRadius;
    public PRMNode(Vector3 _vPosition, float _fRadius, float _fNeighbourRadius)
    {
        m_vPosition = _vPosition;
        m_fNodeRadius = _fRadius;
        m_fNeighbourRadius = _fNeighbourRadius;
        m_iLayerMask = ~(LayerMask.GetMask("Agent") | LayerMask.GetMask("Goal"));
    }

    public bool CheckCollision()
    {
        //return Physics.OverlapBox(m_vPosition, new Vector3(m_fNodeRadius, m_fNodeRadius, m_fNodeRadius), Quaternion.identity, m_iLayerMask).Length > 0;
        return Physics.OverlapSphere(m_vPosition, m_fNodeRadius, m_iLayerMask).Length > 0;
    }
    public void ConnectNeighbours(List<Node> _tNodes, SpaceRepresentation _oRepresentation)
    {
        for(int i = 0; i < _tNodes.Count; i ++)
        {
            if (!m_tNeighbours.Contains(_tNodes[i]))
            {
                Vector3 vDir = _tNodes[i].m_vPosition - m_vPosition;
                float fDistance = vDir.magnitude;
                if (fDistance < m_fNeighbourRadius)
                {
                    bool bCollision = Physics.Raycast(m_vPosition, vDir, fDistance, m_iLayerMask);
                    if (!bCollision)
                    {
                        Connect(_tNodes[i], _oRepresentation);
                    }
                }
            }
        }
    }
    public void Draw()
    {
        //Gizmos.color = Color.Lerp(m_oMinColor, m_oMaxColor, Mathf.Log(m_fHalfSize, 2) / Mathf.Log(_fMaxSize, 2));
        Gizmos.color = new Color(1, 0, 1, 0.5f);
        if (m_bCheckedDebug)
        {
            Gizmos.color = new Color(0, 1, 0, 0.5f);
        }
        Gizmos.DrawSphere(m_vPosition, m_fNodeRadius);
        Gizmos.color = new Color(1, 0, 0, 0.125f);
        for (int i = 0; i < m_tNeighbours.Count; i++)
        {
            Gizmos.DrawLine(m_vPosition, m_tNeighbours[i].m_vPosition);
        }


    }
}
