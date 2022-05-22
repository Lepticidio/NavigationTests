using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : Pathfinder
{
    public List<AStarNode> m_tOpen = new List<AStarNode>();
    public List<AStarNode> m_tClosed = new List<AStarNode>();

    // Start is called before the first frame update

    public override IEnumerator GetPath(Vector3 _vStart, Vector3 _vEnd, NodeMap _oMap)
    {
        Debug.Log("start getting path");
        yield return new WaitForSeconds(1f);

        m_bPathFound = false;
        m_tCenters.Clear();
        m_tClosed.Clear();
        m_tOpen.Clear();

        Debug.Log("lists cleared");
        yield return new WaitForSeconds(0.05f);

        m_tPath = new List<Vector3>();
        m_tPath.Add(_vEnd);

        Debug.Log("before creating map");
        yield return new WaitForSeconds(0.05f);

        AStarMap oMap = new AStarMap(_oMap);
        Debug.Log("before generating map");
        yield return new WaitForSeconds(0.05f);
        StartCoroutine(  oMap.GenerateMap());
        while(!oMap.m_bGenerated)
        {
            Debug.Log("waiting for AStarMap");
            yield return null;
        }
        Debug.Log("after generating map");
        yield return new WaitForSeconds(0.05f);

        AStarNode oStartAStarNode = null;
        AStarNode oEndAStarNode = null;

        oStartAStarNode = new AStarNode(_oMap, _vStart);
        oMap.m_tFreeNodes.Add(oStartAStarNode);
        oStartAStarNode.ReviewConnections(oMap);
        if (oStartAStarNode != null)
        {
            oEndAStarNode = new AStarNode(_oMap, oStartAStarNode.m_tNodes[0], _vEnd);
            oMap.m_tFreeNodes.Add(oEndAStarNode);
            oEndAStarNode.ReviewConnections(oMap);
        }
        

        bool bEnd = oStartAStarNode == null || oEndAStarNode == null;

        Debug.Log("bEnd? " + bEnd);
        yield return new WaitForSeconds(0.05f);
        if (!bEnd)
        {
            oStartAStarNode.CalculateH(_vEnd);
            oEndAStarNode.m_fH = 0;
            oStartAStarNode.m_fG = 0;
            oStartAStarNode.CalculateF();
            m_tOpen.Add(oStartAStarNode);
        }
        AStarNode oCurrent = null;
        int iCounter = 0;
        Debug.Log("Start node " + oStartAStarNode.m_vPosition + " end node " + oEndAStarNode.m_vPosition);
        yield return new WaitForSeconds(0.05f);

        while (!bEnd && iCounter < 1000000)
        {
            oCurrent = GetLowestOpenF();
            m_tOpen.Remove(oCurrent);
            m_tClosed.Add(oCurrent);
            if(iCounter %1000 == 0)
            {
                Debug.Log(iCounter + " Current node: " + oCurrent.m_vPosition + " open " + m_tOpen.Count + " closed " + m_tClosed.Count + " neighbours " + oCurrent.m_tNeighbours.Count);
                yield return new WaitForSeconds(0.05f);
            }
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
        while(oCurrent != null && iCounter < 100000)
        {
            Debug.Log("Path node: " + oCurrent.m_vPosition);
            m_tCenters.Add(oCurrent.m_vPosition);
            m_tPath.Add(oCurrent.m_vPosition);
            oCurrent = oCurrent.m_oParent;
            iCounter++;
        }
        Debug.Log("original path has " + m_tPath.Count + " nodes");
        OptimizePath(ref m_tPath);
        Debug.Log("optimized path has " + m_tPath.Count + " nodes");
        m_bPathFound = true;
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
