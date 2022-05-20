using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarMap
{
    NodeMap m_oBaseMap;
    public List<AStarNode> m_tFreeNodes = new List<AStarNode>();

    public AStarMap(NodeMap _oMap)
    {
        m_oBaseMap = _oMap;
        GenerateMap();
    }

    public void GenerateMap()
    {
        List<Node> tCheckedNodes = new List<Node>();

        foreach (Node oNode in m_oBaseMap.m_tFreeNodes)
        {
            if (m_oBaseMap.m_bPathfindingInEdges)
            {
                foreach (Node oNeighbour in oNode.m_tNeighbours)
                {
                    if (!tCheckedNodes.Contains(oNeighbour))
                    {
                        AStarNode oAStarNode = new AStarNode(oNode, oNeighbour);
                        m_tFreeNodes.Add(oAStarNode);
                    }
                }
            }
            else
            {
                AStarNode oAStarNode = new AStarNode(oNode);
                m_tFreeNodes.Add(oAStarNode);
            }
        }      
        
    }

    public void AddConnections()
    {
        for (int i = 0; i < m_tFreeNodes.Count; i++)
        {
            for(int j = i + 1; j < m_tFreeNodes.Count; j++)
            {
                AStarNode oNodeA = m_tFreeNodes[i];
                AStarNode oNodeB = m_tFreeNodes[j];

                bool bCommonBaseNode = false;
                foreach (Node oBaseNodeA in oNodeA.m_tNodes)
                {
                    foreach (Node oBaseNodeB in oNodeB.m_tNodes)
                    {
                        if(m_oBaseMap.m_bPathfindingInEdges && oBaseNodeA == oBaseNodeB)
                        {
                            bCommonBaseNode = true;
                        }
                        else if(!m_oBaseMap.m_bPathfindingInEdges && oBaseNodeA.m_tNeighbours.Contains(oBaseNodeB))
                        {
                            bCommonBaseNode = true;
                        }
                    }
                }

                if(bCommonBaseNode)
                {
                    oNodeA.m_tNeighbours.Add(oNodeB);
                    oNodeB.m_tNeighbours.Add(oNodeA);
                }
            }
        }
    }
}
