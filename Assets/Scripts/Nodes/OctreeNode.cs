using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OctreeNode : VoxelNode
{
    public int m_iDepth = 0, m_iLocalIndex = 0;
    public OctreeNode[] m_tSubNodes;
    public Vector3Int m_vLevelCoords;
    public OctreeNode m_oParent, m_oRoot;

    public OctreeNode(Vector3 _vPosition, float _fHalfSize, OctreeNode _oParent = null) : base(_vPosition, _fHalfSize)
    {
        m_vPosition = _vPosition;
        m_fHalfSize = _fHalfSize;
        m_vHalfExtents = new Vector3(_fHalfSize, _fHalfSize, _fHalfSize);
        m_oParent = _oParent;
        if (m_oParent != null)
        {
            m_oRoot = m_oParent.m_oRoot;
        }
        else
        {
            m_oRoot = this;
        }
        m_iLayerMask = ~(LayerMask.GetMask("Agent") | LayerMask.GetMask("Goal"));
    }
    public void Subdivide(float _fMinSize, List<Node> _tFreeList)
    {
        bool bFree = !CheckCollision();
        if (!bFree && m_fHalfSize >= _fMinSize)
        {
            m_tSubNodes = new OctreeNode[8];
            float fNewHalfSize = m_fHalfSize * 0.5f;
            for (int i = 0; i < m_tSubNodes.Length; i++)
            {
                Vector3 vNewPos = m_vPosition;
                if (GetXFromInt(i))
                {
                    vNewPos.x += fNewHalfSize;
                }
                else
                {
                    vNewPos.x -= fNewHalfSize;
                }

                if (GetYFromInt(i))
                {
                    vNewPos.y += fNewHalfSize;
                }
                else
                {
                    vNewPos.y -= fNewHalfSize;
                }

                if (GetZFromInt(i))
                {
                    vNewPos.z += fNewHalfSize;
                }
                else
                {
                    vNewPos.z -= fNewHalfSize;
                }

                OctreeNode oChildNode = new OctreeNode(vNewPos, fNewHalfSize, this);
                m_tSubNodes[i] = oChildNode;
                oChildNode.m_iDepth = m_iDepth + 1;
                oChildNode.m_iLocalIndex = i;
                oChildNode.CalculateLevelCoords();
                oChildNode.Subdivide(_fMinSize, _tFreeList);
            }
        }
        else if (bFree)
        {
            m_bFree = true;
            _tFreeList.Add(this);
        }
    }

    bool GetXFromInt(int _iIndex)
    {
        return (_iIndex & 4) == 4;
    }

    bool GetLocalX()
    {
        return GetXFromInt(m_iLocalIndex);
    }
    bool GetYFromInt(int _iIndex)
    {
        return (_iIndex & 2) == 2;
    }

    bool GetLocalY()
    {
        return GetYFromInt(m_iLocalIndex);
    }
    bool GetZFromInt(int _iIndex)
    {
        return (_iIndex & 1) == 1;
    }

    bool GetLocalZ()
    {
        return GetZFromInt(m_iLocalIndex);
    }

    int GetIntLocalX()
    {
        return GetLocalX() ? 1 : 0;
    }
    int GetIntLocalY()
    {
        return GetLocalY() ? 1 : 0;
    }
    int GetIntLocalZ()
    {
        return GetLocalZ() ? 1 : 0;
    }
    public void CalculateLevelCoords()
    {
        Vector3Int vParentCoords = Vector3Int.zero;
        if (m_oParent != null)
        {
            vParentCoords = m_oParent.m_vLevelCoords;
        }
        m_vLevelCoords = vParentCoords * 2 + new Vector3Int(GetIntLocalX(), GetIntLocalY(), GetIntLocalZ());
    }
    public void Draw(float _fMaxSize)
    {
        //Gizmos.color = Color.Lerp(m_oMinColor, m_oMaxColor, Mathf.Log(m_fHalfSize, 2) / Mathf.Log(_fMaxSize, 2));
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(m_vPosition, Vector3.one * m_fHalfSize * 2);
        if (m_bFree)
        {
                Gizmos.color = new Color(0f, 0f, 1f, 0.5f);
        }
        else if (m_tSubNodes == null || m_tSubNodes.Length == 0 || m_tSubNodes[0] == null)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
            Gizmos.DrawCube(m_vPosition, Vector3.one * m_fHalfSize * 2);
        }
        //if (m_bCheckedDebug)
        //{
        //    Gizmos.color = new Color(1f, 1f, 0f, 0.25f);
        //    Gizmos.DrawCube(m_vPosition, Vector3.one * m_fHalfSize * 2);
        //}
        //Gizmos.color = m_oRandom;

        if (m_tSubNodes != null)
        {
            foreach (OctreeNode oNode in m_tSubNodes)
            {
                oNode.Draw(_fMaxSize);
            }
        }

        

        //Gizmos.color = new Color(Mathf.Lerp(-_fMaxSize, _fMaxSize, m_vPosition.x), Mathf.Lerp(-_fMaxSize, _fMaxSize, m_vPosition.y), Mathf.Lerp(-_fMaxSize, _fMaxSize, m_vPosition.z), 0.125f);
        //for (int i = 0; i < m_tNeighbors.Count; i++)
        //{
        //    Gizmos.DrawLine(m_vPosition, m_tNeighbors[i].m_vPosition);
        //}

    }

    public OctreeNode GetFreeNodeFromCoordinates(Vector3Int _vLevelCoordinates, int _iDepth)
    {
        OctreeNode oTempNode = m_oRoot;
        OctreeNode oResult = null;

        for (int i = 0; i < _iDepth + 1; i++)
        {
            if (oTempNode != null)
            {
                if (oTempNode.m_bFree)
                {
                    oResult = oTempNode;
                }

                int iBinaryDigit = (int)Mathf.Pow(2, _iDepth - (oTempNode.m_iDepth + 1));
                int iIndex = (_vLevelCoordinates.x & iBinaryDigit) == iBinaryDigit ? 4 : 0;
                iIndex += (_vLevelCoordinates.y & iBinaryDigit) == iBinaryDigit ? 2 : 0;
                iIndex += (_vLevelCoordinates.z & iBinaryDigit) == iBinaryDigit ? 1 : 0;

                if (oTempNode.m_tSubNodes != null && oTempNode.m_tSubNodes.Length > iIndex)
                {
                    oTempNode = oTempNode.m_tSubNodes[iIndex];
                }
            }
            else
            {
                i = _iDepth;
            }
        }
        return oResult;
    }
    public void ConnectNeighbours(SpaceRepresentation _oRepresentation)
    {
        float fMaxSize = Mathf.Pow(2, m_iDepth);
        if (m_vLevelCoords.x < fMaxSize - 1)
        {
            Connect(GetFreeNodeFromCoordinates(m_vLevelCoords + Vector3Int.right, m_iDepth), _oRepresentation);
        }
        if (m_vLevelCoords.x > 0)
        {
            Connect(GetFreeNodeFromCoordinates(m_vLevelCoords + Vector3Int.left, m_iDepth), _oRepresentation);

        }
        if (m_vLevelCoords.y < fMaxSize - 1)
        {
            Connect(GetFreeNodeFromCoordinates(m_vLevelCoords + Vector3Int.up, m_iDepth), _oRepresentation);
        }
        if (m_vLevelCoords.y > 0)
        {
            Connect(GetFreeNodeFromCoordinates(m_vLevelCoords + Vector3Int.down, m_iDepth), _oRepresentation);
        }
        if (m_vLevelCoords.z < fMaxSize - 1)
        {
            Connect(GetFreeNodeFromCoordinates(m_vLevelCoords + Vector3Int.forward, m_iDepth), _oRepresentation);
        }
        if (m_vLevelCoords.z > 0)
        {
            Connect(GetFreeNodeFromCoordinates(m_vLevelCoords + Vector3Int.back, m_iDepth), _oRepresentation);
        }
    }

    public Vector3 GetClosestPointInNode(Vector3 _vPosition)
    {
        float fX = _vPosition.x;

        if (fX > m_vPosition.x)
        {
            fX = Mathf.Min(fX, m_vPosition.x + m_fHalfSize);
        }
        else if (fX < m_vPosition.x)
        {
            fX = Mathf.Max(fX, m_vPosition.x - m_fHalfSize);
        }

        float fY = _vPosition.y;

        if (fY > m_vPosition.y)
        {
            fY = Mathf.Min(fY, m_vPosition.y + m_fHalfSize);
        }
        else if (fY < m_vPosition.y)
        {
            fY = Mathf.Max(fY, m_vPosition.y - m_fHalfSize);
        }

        float fZ = _vPosition.z;

        if (fZ > m_vPosition.z)
        {
            fZ = Mathf.Min(fZ, m_vPosition.z + m_fHalfSize);
        }
        else if (fZ < m_vPosition.x)
        {
            fZ = Mathf.Max(fZ, m_vPosition.z - m_fHalfSize);
        }

        return new Vector3(fX, fY, fZ);
    }
}
