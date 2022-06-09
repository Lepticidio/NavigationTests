using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarMap
{
    public bool m_bGenerated = false, m_bConnected = false;
    SpaceRepresentation m_oBaseMap;
    public List<AStarNode> m_tFreeNodes = new List<AStarNode>();
    public Dictionary<Node, List<AStarNode>> m_tNodeListDictionary = new Dictionary<Node, List<AStarNode>>();
    public Dictionary<Node, AStarNode> m_tNodeDictionary = new Dictionary<Node, AStarNode>();

    public AStarMap(SpaceRepresentation _oMap)
    {
        m_oBaseMap = _oMap;
    }

    public void GenerateMap()
    {
        m_bGenerated = false;
        int icount = 0;
        foreach (Node oNode in m_oBaseMap.m_tFreeNodes)
        {
            if (m_oBaseMap.m_bPathfindingInEdges)
            {
                foreach (Node oNeighbour in oNode.m_tNeighbours)
                {
                    AStarNode oAStarNode = new AStarNode(oNode, oNeighbour);
                    oAStarNode.m_tNodes.Add(oNode);
                    oAStarNode.m_tNodes.Add(oNeighbour);

                    if(!m_tNodeListDictionary.ContainsKey(oNode))
                    {
                        m_tNodeListDictionary[oNode] = new List<AStarNode>();
                        m_tNodeListDictionary[oNode].Add(oAStarNode);
                    }
                    else
                    {
                        for(int i = 0; i < m_tNodeListDictionary[oNode].Count; i++)
                        {
                            m_tNodeListDictionary[oNode][i].m_tNeighbours.Add(oAStarNode);
                            oAStarNode.m_tNeighbours.Add(m_tNodeListDictionary[oNode][i]);

                        }
                    }
                    if (!m_tNodeListDictionary.ContainsKey(oNeighbour))
                    {
                        m_tNodeListDictionary[oNeighbour] = new List<AStarNode>();
                        m_tNodeListDictionary[oNeighbour].Add(oAStarNode);
                    }
                    else
                    {
                        for (int i = 0; i < m_tNodeListDictionary[oNeighbour].Count; i++)
                        {
                            m_tNodeListDictionary[oNeighbour][i].m_tNeighbours.Add(oAStarNode);
                            oAStarNode.m_tNeighbours.Add(m_tNodeListDictionary[oNeighbour][i]);

                        }
                    }
                    m_tFreeNodes.Add(oAStarNode);
                }
            }
            else
            {
                AStarNode oAStarNode = new AStarNode(oNode);
                oAStarNode.m_tNodes.Add(oNode);

                for (int i = 0; i <  oNode.m_tNeighbours.Count; i++)
                {
                    if(m_tNodeDictionary.ContainsKey(oNode.m_tNeighbours[i]))
                    {
                        m_tNodeDictionary[oNode.m_tNeighbours[i]].m_tNeighbours.Add(oAStarNode);
                        oAStarNode.m_tNeighbours.Add(m_tNodeDictionary[oNode.m_tNeighbours[i]]);
                    }
                }

                m_tNodeDictionary.Add(oNode, oAStarNode);
                
                m_tFreeNodes.Add(oAStarNode);
            }
            icount++;
        }
        m_bGenerated = true;        
    }

    //public IEnumerator AddConnections()
    //{
    //    m_bConnected = false;
    //    Debug.Log("Adding connections in map with " + m_tFreeNodes.Count +" nodes");
    //    yield return new WaitForSeconds(3f);
    //    for (int i = 0; i < m_tFreeNodes.Count; i++)
    //    {
    //        for(int j = i + 1; j < m_tFreeNodes.Count; j++)
    //        {
    //            bool bCommonBaseNode = false;
    //            foreach (Node oBaseNodeA in m_tFreeNodes[i].m_tNodes)
    //            {
    //                if(i%2 == 0 && j%100000 == 0)
    //                {
    //                    Debug.Log("Checking connection between node " + i + " and " + j);
    //                    yield return new WaitForSeconds(0.01f);
    //                }
    //                foreach (Node oBaseNodeB in m_tFreeNodes[j].m_tNodes)
    //                {
    //                    if(m_oBaseMap.m_bPathfindingInEdges && oBaseNodeA == oBaseNodeB)
    //                    {
    //                        bCommonBaseNode = true;
    //                    }
    //                    else if(!m_oBaseMap.m_bPathfindingInEdges && oBaseNodeA.m_tNeighbours.Contains(oBaseNodeB))
    //                    {
    //                        bCommonBaseNode = true;
    //                    }
    //                }
    //            }
    //            if(bCommonBaseNode)
    //            {
    //                m_tFreeNodes[i].m_tNeighbours.Add(m_tFreeNodes[j]);
    //                m_tFreeNodes[j].m_tNeighbours.Add(m_tFreeNodes[i]);
    //            }
    //        }
    //    }
    //    m_bConnected = true;
    //}
}
