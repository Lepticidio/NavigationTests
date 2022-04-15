using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode : Node
{
    public float m_fG = Mathf.Infinity, m_fH = Mathf.Infinity, m_fF = Mathf.Infinity;
    public AStarNode m_oParent;

    public AStarNode (Vector3 _vPosition, AStarNode _oParent, float _fG = Mathf.Infinity, float _fH = Mathf.Infinity, float _fF = Mathf.Infinity)
    {
        m_vPosition = _vPosition;
        m_fG = _fG;
        m_fH = _fH;
        m_fF = _fF;
        m_oParent = _oParent;
    }

    public void CalculateH(Vector3 _vGoal)
    {
        m_fH = (_vGoal - m_vPosition).magnitude;
    }
    public void CalculateG(AStarNode _oPrevious)
    {
        float fCandidateG = _oPrevious.m_fG + (m_vPosition - _oPrevious.m_vPosition).magnitude;

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
