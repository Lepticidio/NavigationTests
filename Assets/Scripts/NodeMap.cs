using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NodeMap : MonoBehaviour
{
    public List<Node> m_tFreeNodes = new List<Node>();

    public abstract Node GetNodeFromPosition(Vector3 _vPosition);
}

