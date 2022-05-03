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
        m_bFree = !CheckCollision();
        m_iLayerMask = ~(LayerMask.GetMask("Agent") | LayerMask.GetMask("Goal"));
    }

    public override bool CheckCollision()
    {
        return Physics.OverlapSphere(m_vPosition, m_fNodeRadius, m_iLayerMask).Length > 0;
    }
    public void ConnectNeighbours(List<Node> _tNodes)
    {
        for(int i = 0; i < _tNodes.Count; i ++)
        {
            if(!m_tNeighbours.Contains(_tNodes[i]))
            {
                Vector3 vDir = _tNodes[i].m_vPosition - m_vPosition;
                float fDistance = vDir.magnitude;
                if (fDistance < m_fNeighbourRadius)
                {
                    bool bCollision = Physics.Raycast(m_vPosition, vDir, fDistance, m_iLayerMask);
                    if (bCollision)
                    {
                        m_tNeighbours.Add(_tNodes[i]);
                        _tNodes[i].m_tNeighbours.Add(this);
                    }
                }
            }
        }
    }
}
