using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : Pathfinder
{
    public List<AStarNode> m_tOpen = new List<AStarNode>();
    public List<AStarNode> m_tClosed = new List<AStarNode>();

    // Start is called before the first frame update

    public override List<Vector3> GetPath (Vector3 _vStart, Vector3 _vEnd, NodeMap _oMap)
    {
        m_tCenters.Clear();
        m_tClosed.Clear();
        m_tOpen.Clear();

        List<Vector3> tResult = new List<Vector3>();
        tResult.Add(_vEnd);


        AStarMap oMap = new AStarMap(_oMap);

        AStarNode oStartAStarNode = null;
        AStarNode oEndAStarNode = null;

        oStartAStarNode = new AStarNode(_oMap, _vStart);
        if (oStartAStarNode != null)
        {
            oEndAStarNode = new AStarNode(_oMap, oStartAStarNode.m_tNodes[0], _vEnd);
            oMap.m_tFreeNodes.Add(oStartAStarNode);
            oMap.m_tFreeNodes.Add(oEndAStarNode);
        }
        

        bool bEnd = oStartAStarNode == null || oEndAStarNode == null;

        if (!bEnd)
        {
            oMap.AddConnections();            
            oStartAStarNode.CalculateH(_vEnd);
            oEndAStarNode.m_fH = 0;
            oStartAStarNode.m_fG = 0;
            oStartAStarNode.CalculateF();
            m_tOpen.Add(oStartAStarNode);
        }
        AStarNode oCurrent = null;
        int iCounter = 0;
        while (!bEnd && iCounter < 1000)
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
                for (int i = 0; i< oCurrent.m_tNeighbours.Count; i++)
                {
                    AStarNode oNeighbour = oCurrent.m_tNeighbours[i];
                    

                    if(oNeighbour.m_fH == Mathf.Infinity)
                    {
                        oNeighbour.CalculateH(_vEnd);
                    }
                    if(!m_tClosed.Contains(oNeighbour))
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
            iCounter++;
        }
        iCounter = 0;
        while(oCurrent != null && iCounter < 1000)
        {
            m_tCenters.Add(oCurrent.m_vPosition);
            tResult.Add(oCurrent.m_vPosition);
            oCurrent = oCurrent.m_oParent;
            iCounter++;
        }
        OptimizePath(ref tResult);
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
