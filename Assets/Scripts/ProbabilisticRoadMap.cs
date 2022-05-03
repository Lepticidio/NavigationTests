using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbabilisticRoadMap : NodeMap
{
    public int m_iNumberNodes = 100;
    public float m_fNodeRadius, m_fConnectRadius;
    public void GenerateMap(Vector3 _vPosition, float _fMapHalfSize)
    {
        m_tFreeNodes.Clear();
        while(m_tFreeNodes.Count < m_iNumberNodes)
        {
            PRMNode oNode = new PRMNode(new Vector3(Random.Range(-_fMapHalfSize, _fMapHalfSize), Random.Range(-_fMapHalfSize, _fMapHalfSize), Random.Range(-_fMapHalfSize, _fMapHalfSize)), m_fNodeRadius, m_fConnectRadius);
            
            if(oNode != null && oNode.m_bFree)
            {
                m_tFreeNodes.Add(oNode);
                oNode.ConnectNeighbours(m_tFreeNodes);
            }
        }
    }
}
