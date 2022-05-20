using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode
{
    public Vector3 m_vPosition;
    public float m_fG = Mathf.Infinity, m_fH = Mathf.Infinity, m_fF = Mathf.Infinity;
    public AStarNode m_oParent;
    public List<Node> m_tNodes = new List<Node>();
    public List<AStarNode> m_tNeighbours = new List<AStarNode>();

    public AStarNode(Node _oNode)
    {
        if(_oNode == null)
        {
            Debug.Log("AStarNode from NULL node");
        }
        m_vPosition = _oNode.m_vPosition;
        m_tNodes.Add(_oNode);
    }

    public AStarNode (Node _oNodeA, Node _oNodeB)
    {
        if (_oNodeA == null || _oNodeB == null)
        {
            Debug.Log("AStarNode from NULL nodes");
        }
        m_tNodes.Add(_oNodeA);
        m_tNodes.Add(_oNodeB);

        OctreeNode oOctreeA = _oNodeA as OctreeNode;
        if(oOctreeA != null)
        {
            OctreeNode oOctreeB = _oNodeB as OctreeNode;

            OctreeNode oSmallerOctree = oOctreeA;
            OctreeNode oBiggerOctree = oOctreeB;
            if(oSmallerOctree.m_fHalfSize > oBiggerOctree.m_fHalfSize)
            {
                oSmallerOctree = oOctreeB;
                oBiggerOctree = oOctreeA;
            }
            m_vPosition = oSmallerOctree.m_vPosition;

            Vector3 m_vDifference = oBiggerOctree.m_vPosition - m_vPosition;
            float fX = Mathf.Abs(m_vDifference.x);
            float fY = Mathf.Abs(m_vDifference.y);
            float fZ = Mathf.Abs(m_vDifference.z);
            
            if(fX > fY)
            {
                if(fX > fZ)
                {
                    m_vPosition = m_vPosition +
                        new Vector3(oSmallerOctree.m_fHalfSize * Mathf.Sign(m_vDifference.x), 0, 0);
                }
                else
                {
                    m_vPosition = m_vPosition +
                        new Vector3(0, 0, oSmallerOctree.m_fHalfSize * Mathf.Sign(m_vDifference.z));
                }
            }
            else
            {
                if (fY > fZ)
                {
                    m_vPosition = m_vPosition +
                        new Vector3(0, oSmallerOctree.m_fHalfSize * Mathf.Sign(m_vDifference.y), 0);
                }
                else
                {
                    m_vPosition = m_vPosition +
                        new Vector3(0, 0, oSmallerOctree.m_fHalfSize * Mathf.Sign(m_vDifference.z));
                }
            }
        }
        else
        {
            m_vPosition = (_oNodeA.m_vPosition + _oNodeB.m_vPosition) * 0.5f;
        }
    }
    public AStarNode (NodeMap _oMap, Vector3 _vPosition)
    {
        m_vPosition = _vPosition;
        m_tNodes.Add(_oMap.GetNodeFromPosition(_vPosition));
    }
    public AStarNode(NodeMap _oMap, Node _oNode, Vector3 _vPosition)
    {
        m_vPosition = _vPosition;
        m_tNodes.Add(_oMap.GetConnectedNodeFromPosition(_vPosition, _oNode));
    }
    public void CalculateH(Vector3 _vGoal)
    {
        m_fH = (_vGoal - m_vPosition).magnitude;        
    }
    public void CalculateG(AStarNode _oPrevious)
    {

        float fCandidateG = _oPrevious.m_fG + (m_vPosition - _oPrevious.m_vPosition).magnitude;

        if (fCandidateG < m_fG)
        {
            m_fG = fCandidateG;
            CalculateF();
        }        
    }
    public void CalculateF()
    {
        m_fF = m_fG + m_fH;
    }
}
