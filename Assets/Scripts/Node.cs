using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{
    public bool m_bFree = false;
    public Vector3 m_vPosition;
    public List<Node> m_tNeighbours = new List<Node>();
    public NodePathfindingInfo m_oPathInfo;

    public override bool Equals (object obj)
    {
        if((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            return m_vPosition == ((Node)obj).m_vPosition;
        }
    }
}