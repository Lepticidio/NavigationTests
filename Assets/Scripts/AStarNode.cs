using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode : NodePathfindingInfo
{
    public float m_fG = Mathf.Infinity, m_fH = Mathf.Infinity, m_fF = Mathf.Infinity;
    public AStarNode m_oParent;

    public AStarNode (Node _oNode)
    {
        m_oNode = _oNode;
        m_oNode.m_oPathInfo = this;
    }

    public void CalculateH(Vector3 _vGoal)
    {
        m_fH = (_vGoal - GetPosition()).magnitude;
    }
    public void CalculateG(AStarNode _oPrevious)
    {
        float fCandidateG = _oPrevious.m_fG + (GetPosition() - _oPrevious.GetPosition()).magnitude;

        if(fCandidateG < m_fG)
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
