using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NodeMap : MonoBehaviour
{
    public bool m_bGenerated= false;

    public List<Node> m_tFreeNodes = new List<Node>();

    public virtual Node GetNodeFromPosition(Vector3 _vPosition)
    {
        Node oResult = null;
        float fClosestDistance = Mathf.Infinity;
        for (int i = 0; i < m_tFreeNodes.Count; i++)
        {
            Node oNode = m_tFreeNodes[i];
            float fDistance = (oNode.m_vPosition - _vPosition).sqrMagnitude;
            if (oResult == null || fDistance < fClosestDistance)
            {
                oResult = oNode;
                fClosestDistance = fDistance;
            }
        }
        return oResult;
    }
}

