using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PRM")]
public class ProbabilisticRoadMap : NodeMap
{
    public int m_iNumberNodes = 100;
    public float m_fNodeRadius, m_fConnectRadius;
    public override void GenerateMap(MapType _oMapType)
    {
        m_bGenerated = false;
        m_tFreeNodes.Clear();
        CalculateHalfSize(_oMapType);
        int iCounter = 0;
        int iCounterLimit = 10000;
        while(m_tFreeNodes.Count < m_iNumberNodes && iCounter < iCounterLimit)
        {
            PRMNode oNode = new PRMNode(new Vector3(Random.Range(-m_fMapHalfSize, m_fMapHalfSize), Random.Range(-m_fMapHalfSize, m_fMapHalfSize), Random.Range(-m_fMapHalfSize, m_fMapHalfSize)), m_fNodeRadius, m_fConnectRadius);
            oNode.m_bFree = !oNode.CheckCollision();
            if(oNode != null && oNode.m_bFree)
            {
                m_tFreeNodes.Add(oNode);
            }
            iCounter++;
        }
        if(!_oMapType.m_bNoTerrain)
        {
            ClearUnderTerrainNodes();
        }
        ConnectNeighbours();
        m_bGenerated = true;
    }
    public void ConnectNeighbours()
    {
        LayerMask iLayerMask = ~(LayerMask.GetMask("Agent") | LayerMask.GetMask("Goal"));
        for (int i = 0; i < m_tFreeNodes.Count; i++)
        {
            for (int j = i + 1; j < m_tFreeNodes.Count; j++)
            {
                Vector3 vDir = m_tFreeNodes[i].m_vPosition - m_tFreeNodes[j].m_vPosition;
                float fDistance = vDir.magnitude;
                if (fDistance < m_fConnectRadius)
                {
                    RaycastHit oInfo;
                    bool bCollision = Physics.SphereCast(m_tFreeNodes[j].m_vPosition, 0.5f, vDir, out oInfo, fDistance, iLayerMask);
                    if (!bCollision)
                    {
                        m_tFreeNodes[i].m_tNeighbours.Add(m_tFreeNodes[j]);
                        m_tFreeNodes[j].m_tNeighbours.Add(m_tFreeNodes[i]);
                    }
                }

            }
        }
    }
}
