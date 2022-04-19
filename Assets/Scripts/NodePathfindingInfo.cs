using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePathfindingInfo
{
    public Node m_oNode;
    
    public Vector3 GetPosition ()
    {
        return m_oNode.m_vPosition;
    }
    public bool GetFree()
    {
        return m_oNode.m_bFree;
    }

    public List<Node> GetNeighbours()
    {
        return m_oNode.m_tNeighbours;
    }
    public Node GetNeighbour(int _iIndex)
    {
        return m_oNode.m_tNeighbours[_iIndex];
    }
    public NodePathfindingInfo GetNeighbourPathInfo(int _iIndex)
    {
        return m_oNode.m_tNeighbours[_iIndex].m_oPathInfo;
    }
}
