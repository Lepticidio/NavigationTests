using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode : NodePathfindingInfo
{
    public float m_fG = Mathf.Infinity, m_fH = Mathf.Infinity, m_fF = Mathf.Infinity;
    public AStarNode m_oParent;
    public Vector3 m_vPointOfEntry;
    public AStarNode (Node _oNode)
    {
        m_oNode = _oNode;
        m_oNode.m_oPathInfo = this;
    }

    public void CalculateH(Vector3 _vGoal)
    {
        OctreeNode oOctreeNode = m_oNode as OctreeNode;
        if(oOctreeNode != null)
        {
            m_fH =(_vGoal -  oOctreeNode.GetClosestPointInNode(_vGoal)).magnitude;
        }
        else
        {
            m_fH = (_vGoal - GetPosition()).magnitude;
        }
    }
    public void CalculateG(AStarNode _oPrevious)
    {
        OctreeNode oOctreeNode = m_oNode as OctreeNode;
        if (oOctreeNode != null)
        {
            Vector3 vTempPointOfEntry = oOctreeNode.GetClosestPointInNode(_oPrevious.m_vPointOfEntry);
            float fCandidateG = _oPrevious.m_fG + (vTempPointOfEntry- _oPrevious.m_vPointOfEntry).magnitude ;

            if (fCandidateG < m_fG)
            {
                m_fG = fCandidateG;
                m_vPointOfEntry = vTempPointOfEntry;
                CalculateF();
            }
        }
        else
        {
            float fCandidateG = _oPrevious.m_fG + (GetPosition() - _oPrevious.GetPosition()).magnitude;

            if (fCandidateG < m_fG)
            {
                m_fG = fCandidateG;
                CalculateF();
            }
        }
    }
    public void CalculateF()
    {
        m_fF = m_fG + m_fH;
    }
}
