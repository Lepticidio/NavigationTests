using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : Pathfinder
{
    public List<AStarNode> m_tOpen = new List<AStarNode>();
    public List<AStarNode> m_tClosed = new List<AStarNode>();
    public Dictionary<Node, AStarNode> m_tAll = new Dictionary<Node, AStarNode>();
    // Start is called before the first frame update

    public override List<Vector3> GetPath (Vector3 _vStart, Vector3 _vEnd, NodeMap _oMap)
    {
        List<Vector3> tResult = new List<Vector3>();
        if(m_tAll.Count == 0)
        {
            for (int i = 0; i < _oMap.m_tFreeNodes.Count; i++)
            {
                m_tAll.Add( AStarNode( _oMap.m_tFreeNodes[i] as AStarNode));
            }
        }

        Debug.Log("Tries to get path");

        AStarNode oStartNode = _oMap.GetNodeFromPosition(_vStart) as AStarNode;
        AStarNode oEndNode = _oMap.GetNodeFromPosition(_vEnd) as AStarNode;

        bool bEnd = oStartNode == null || oEndNode == null;

        Debug.Log("Start null?: " + (oStartNode == null).ToString());
        Debug.Log(" End null?: " + (oEndNode == null).ToString());

        if (!bEnd)
        {
            oStartNode.CalculateH(_vEnd);
            oEndNode.m_fH = 0;
            oStartNode.m_fG = 0;
            oStartNode.CalculateF();
            m_tOpen.Add(oStartNode);
        }
        AStarNode oCurrent = null;
        while (!bEnd)
        {
            Debug.Log("In loop");
            oCurrent = GetLowestOpenF();
            m_tOpen.Remove(oCurrent);
            m_tClosed.Add(oCurrent);
            
            if(oCurrent == oEndNode)
            {
                bEnd = true;
            }
            else
            {
                for(int i = 0; i< oCurrent.m_tNeighbors.Count; i++)
                {
                    AStarNode oNeighbour = oCurrent.m_tNeighbors[i] as AStarNode;
                    if(oNeighbour.m_fH == Mathf.Infinity)
                    {
                        oNeighbour.CalculateH(_vEnd);
                    }
                    if(oCurrent.m_bFree && !m_tClosed.Contains(oNeighbour))
                    {
                        float fPreviousG = oNeighbour.m_fG;
                        oNeighbour.CalculateG(oCurrent);
                        if(oNeighbour.m_fG < fPreviousG||!m_tOpen.Contains(oNeighbour))
                        {
                            oNeighbour.m_oParent = oCurrent;
                            if(!m_tOpen.Contains(oNeighbour))
                            {
                                m_tOpen.Add(oNeighbour);
                            }
                        }
                    }
                }
            }
            if(m_tOpen.Count == 0)
            {
                bEnd = true;
            }
        }

        while (oCurrent.m_oParent != null)
        {
            tResult.Add(oCurrent.m_vPosition);
            oCurrent = oCurrent.m_oParent;
        }

        return tResult;
    }


    AStarNode GetLowestOpenF()
    {
        AStarNode oResult = null;
        for (int i = 0; i < m_tOpen.Count; i++)
        {
            if(oResult == null || m_tOpen[i].m_fF < oResult.m_fF)
            {
                oResult = m_tOpen[i];
            }
        }
        Debug.Log("LowestF null?: " + oResult == null);
        return oResult;
    }
}
