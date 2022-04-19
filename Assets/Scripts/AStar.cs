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

        Node oStartNode = _oMap.GetNodeFromPosition(_vStart);
        Node oEndNode = _oMap.GetNodeFromPosition(_vEnd);


        AStarNode oStartAStarNode = null;
        AStarNode oEndAStarNode = null;

        if(oStartNode != null)
        {
            oStartAStarNode = new AStarNode(oStartNode);
        }
        if(oEndNode != null)
        {
            oEndAStarNode = new AStarNode(oEndNode);
        }

        bool bEnd = oStartAStarNode == null || oEndAStarNode == null;

        if (!bEnd)
        {
            oStartAStarNode.CalculateH(_vEnd);
            oEndAStarNode.m_fH = 0;
            oStartAStarNode.m_fG = 0;
            oStartAStarNode.CalculateF();
            m_tOpen.Add(oStartAStarNode);
        }
        AStarNode oCurrent = null;
        while (!bEnd)
        {
            oCurrent = GetLowestOpenF();
            m_tOpen.Remove(oCurrent);
            m_tClosed.Add(oCurrent);
            
            if(oCurrent == oEndAStarNode)
            {
                bEnd = true;
            }
            else
            {
                for(int i = 0; i< oCurrent.GetNeighbours().Count; i++)
                {
                    AStarNode oNeighbour = oCurrent.GetNeighbourPathInfo(i) as AStarNode;
                    if(oNeighbour == null)
                    {
                        oNeighbour = new AStarNode(oCurrent.GetNeighbour(i));
                    }
                    if(oNeighbour.m_fH == Mathf.Infinity)
                    {
                        oNeighbour.CalculateH(_vEnd);
                    }
                    if(oCurrent.GetFree() && !m_tClosed.Contains(oNeighbour))
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

        OctreeNode oCurrentOctreeNode = oCurrent.m_oNode as OctreeNode;

        if (oCurrentOctreeNode != null)
        {
            Vector3 vPrevious = _vEnd;

            while (oCurrent.m_oParent != null)
            {
                m_tCenters.Add(oCurrent.GetPosition());
                vPrevious = oCurrentOctreeNode.GetClosestPointInNode(vPrevious);
                tResult.Add(vPrevious);
                oCurrent = oCurrent.m_oParent;
                oCurrentOctreeNode = oCurrent.m_oNode as OctreeNode;
            }
        }
        else
        {

            while (oCurrent.m_oParent != null)
            {
                tResult.Add(oCurrent.GetPosition());
                oCurrent = oCurrent.m_oParent;
            }
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
        return oResult;
    }
}
