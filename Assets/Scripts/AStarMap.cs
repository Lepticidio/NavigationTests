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
        Debug.Log("Generating map from map with " + m_oBaseMap.m_tFreeNodes.Count + " nodes");
        List<Node> tCheckedNodes = new List<Node>();
        int icount = 0;
        foreach (Node oNode in m_oBaseMap.m_tFreeNodes)
        {
            if (m_oBaseMap.m_bPathfindingInEdges)
            {
                Debug.Log("base map is edge - based");
                foreach (Node oNeighbour in oNode.m_tNeighbours)
                {
                    if (!tCheckedNodes.Contains(oNeighbour))
                    {
                        AStarNode oAStarNode = new AStarNode(oNode, oNeighbour);
                        Debug.Log("AStarNode " + icount + " has " + oAStarNode.m_tNodes.Count + " base nodes");
                        m_tFreeNodes.Add(oAStarNode);
                    }
                }
            }
            else
            {
                Debug.Log("base map is not edge - based");
                AStarNode oAStarNode = new AStarNode(oNode);
                Debug.Log("AStarNode " + icount + " has " + oAStarNode.m_tNodes.Count + " base nodes");
                m_tFreeNodes.Add(oAStarNode);
            }
            icount++;
        }      
        
    }

    public void AddConnections()
    {
        Debug.Log("Adding connections");
        for (int i = 0; i < m_tFreeNodes.Count; i++)
        {
            for(int j = i + 1; j < m_tFreeNodes.Count; j++)
            {
                bool bCommonBaseNode = false;
                Debug.Log("Conn: AStarNode " + i + " has " + m_tFreeNodes[i].m_tNodes.Count + " base nodes");
                foreach (Node oBaseNodeA in m_tFreeNodes[i].m_tNodes)
                {
                    if(oBaseNodeA == null)
                    {
                        Debug.Log("BaseNode A is NULL");
                    }
                    else
                    {
                        Debug.Log("BaseNode A NOT null, has " + oBaseNodeA.m_tNeighbours.Count + " neighbours");
                    }
                    foreach (Node oBaseNodeB in m_tFreeNodes[j].m_tNodes)
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
                    m_tFreeNodes[i].m_tNeighbours.Add(m_tFreeNodes[j]);
                    m_tFreeNodes[j].m_tNeighbours.Add(m_tFreeNodes[i]);
                }
            }
        }
    }
}
