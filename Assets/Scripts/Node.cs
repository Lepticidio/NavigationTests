using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{
    public bool m_bFree = false;
    public bool m_bCheckedDebug = false;
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
    public abstract bool CheckCollision();

    public bool CheckIfConnected(Node _oOtherNode, ref List<Node> _tCheckedNodes, ref bool _bResult)
    {
        if(!_bResult)
        {
            if (this == _oOtherNode)
            {
                m_bCheckedDebug = true;
                _bResult = true;
            }
            else
            {
                _tCheckedNodes.Add(this);
                for (int i = 0; i < m_tNeighbours.Count; i++)
                {
                    if (!_tCheckedNodes.Contains(m_tNeighbours[i]))
                    {
                        if (m_tNeighbours[i].CheckIfConnected(_oOtherNode, ref _tCheckedNodes, ref _bResult))
                        {
                            _bResult = true;
                            break;
                        }
                    }
                }
            }
        }
        if(_bResult)
        {
            m_bCheckedDebug = true;
        }
        return _bResult;
    }
}