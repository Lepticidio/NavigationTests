using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool m_bFree = false;
    public Vector3 m_vPosition;
    public List<Node> m_tNeighbors = new List<Node>();
}
