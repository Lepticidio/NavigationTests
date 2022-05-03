using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbabilisticRoadMap : NodeMap
{
    public int m_iNumberNodes = 100;
    public float m_fNodeRadius, m_fConnectRadius;
    public float m_fMapHalfSize;
    public override void GenerateMap()
    {
        m_tFreeNodes.Clear();
        int iCounter = 0;
        int iCounterLimit = 10000;
        while(m_tFreeNodes.Count < m_iNumberNodes|| iCounter > iCounterLimit)
        {
            PRMNode oNode = new PRMNode(new Vector3(Random.Range(-m_fMapHalfSize, m_fMapHalfSize), Random.Range(-m_fMapHalfSize, m_fMapHalfSize), Random.Range(-m_fMapHalfSize, m_fMapHalfSize)), m_fNodeRadius, m_fConnectRadius);
            
            if(oNode != null && oNode.m_bFree)
            {
                oNode.ConnectNeighbours(m_tFreeNodes);
                m_tFreeNodes.Add(oNode);
            }
            iCounter++;
        }
        Debug.Log("Counter: " + iCounter);
        m_bGenerated = true;
    }

    void OnDrawGizmos()
    {
        if (m_bDebug)
        {
            for (int i = 0; i < m_tFreeNodes.Count; i++)
            {
                PRMNode oNode = m_tFreeNodes[i] as PRMNode;
                oNode.Draw();
            }
        }
    }
}
