using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelOctree : MonoBehaviour
{
    OctreeNode m_oRoot;
    public List<OctreeNode> m_tFreeNodes = new List<OctreeNode>();
    public float m_fSize = 2048;
    // Start is called before the first frame update
    void Start()
    {
        m_oRoot = new OctreeNode(Vector3.zero, m_fSize * 0.5f);
        m_oRoot.Subdivide(1, m_tFreeNodes);
        ConnectNeighbours();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnDrawGizmos()
    {

        if(m_oRoot != null)
        {
            m_oRoot.Draw(m_fSize*0.5f);
        }
    }

    void ConnectNeighbours()
    {
        for(int i = 0; i < m_tFreeNodes.Count; i++)
        {
            m_tFreeNodes[i].ConnectNeighbours();
        }
    }
}
public class OctreeNode
{
    bool m_bFree = false;
    int m_iDepth = 0, m_iLocalIndex = 0;
    float m_fHalfSize;
    Vector3 m_vHalfExtents;
    Vector3 m_vPosition;
    public OctreeNode[] m_tSubNodes;
    public List<OctreeNode> m_tNeighbors = new List<OctreeNode>();
    public Vector3Int m_vLevelCoords;
    public OctreeNode m_oParent, m_oRoot;

    private Color m_oMinColor = new Color(1f, 1f, 1f, 0.25f);
    private Color m_oMaxColor = new Color(0f, 0f, 0f, 1f);

    public OctreeNode(Vector3 _vPosition, float _fHalfSize, OctreeNode _oParent = null)
    {
        m_vPosition = _vPosition;
        m_fHalfSize = _fHalfSize;
        m_vHalfExtents = new Vector3(_fHalfSize, _fHalfSize, _fHalfSize);
        m_oParent = _oParent;
        if(m_oParent != null)
        {
            m_oRoot = m_oParent.m_oRoot;
        }
        else
        {
            m_oRoot = this;
        }
    }
    public void Subdivide(float _fMinSize, List<OctreeNode> _tFreeList)
    {
        if (CheckCollision() && m_fHalfSize > _fMinSize)
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
        else
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
        if(m_oParent != null)
        {
            vParentCoords = m_oParent.m_vLevelCoords;
        }
        m_vLevelCoords = vParentCoords * 2 + new Vector3Int(GetIntLocalX(), GetIntLocalY(), GetIntLocalZ());

    }
    public void Draw(float _fMaxSize)
    {
        Gizmos.color = Color.Lerp(m_oMinColor, m_oMaxColor, Mathf.Log(m_fHalfSize,2) / Mathf.Log(_fMaxSize, 2));
        Gizmos.DrawWireCube(m_vPosition, Vector3.one * m_fHalfSize*2);
        if(m_tSubNodes != null)
        {
            foreach (OctreeNode oNode in m_tSubNodes)
            {
                oNode.Draw(_fMaxSize);
            }
        }
        Gizmos.color = new Color (Mathf.Lerp(-_fMaxSize, _fMaxSize, m_vPosition.x), Mathf.Lerp(-_fMaxSize, _fMaxSize, m_vPosition.y), Mathf.Lerp(-_fMaxSize, _fMaxSize, m_vPosition.z), 1f);
        for (int i = 0; i < m_tNeighbors.Count; i++)
        {
            Gizmos.DrawLine(m_vPosition, m_tNeighbors[i].m_vPosition);
        }
    }
    public Vector3 GetPosition()
    {
        return m_vPosition;
    }
    public bool CheckCollision()
    {
        return Physics.OverlapBox(m_vPosition, m_vHalfExtents).Length > 0;
    }
    public void Connect(OctreeNode _oNode)
    {
        if(_oNode != null && m_bFree && _oNode.m_bFree && _oNode != this && !m_tNeighbors.Contains(_oNode))
        {
            m_tNeighbors.Add(_oNode);
            _oNode.m_tNeighbors.Add(this);
            //Debug.Log("Connected nodes " + _oNode.m_iDepth + ", " + _oNode.m_vLevelCoords + " - " + m_iDepth + ", " + m_vLevelCoords);
        }
    }
    public OctreeNode GetFreeNodeFromCoordinates(Vector3Int _vLevelCoordinates, int _iDepth)
    {
        OctreeNode oTempNode = m_oRoot;
        OctreeNode oResult = null;

        for (int i = 0; i < _iDepth + 1; i++)
        {
            if(oTempNode != null)
            {
                if (oTempNode.m_bFree)
                {
                    oResult = oTempNode;
                }

                int iBinaryDigit = (int)Mathf.Pow(2, _iDepth - oTempNode.m_iDepth);
                int iIndex = (_vLevelCoordinates.x & iBinaryDigit) == iBinaryDigit ? 4 : 0;
                iIndex += (_vLevelCoordinates.y & iBinaryDigit) == iBinaryDigit ? 2 : 0;
                iIndex += (_vLevelCoordinates.z & iBinaryDigit) == iBinaryDigit ? 1 : 0;

                //Debug.Log("Searched index: " + iIndex + " for node " + _iDepth + ", " + _vLevelCoordinates + " at binary digit " + iBinaryDigit + " in node " + oTempNode.m_iDepth +", " + oTempNode.m_vLevelCoords + " has subnodes? " + (oTempNode.m_tSubNodes != null).ToString());

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
    public void ConnectNeighbours()
    {
        Connect(GetFreeNodeFromCoordinates(m_vLevelCoords + Vector3Int.up, m_iDepth));
        Connect(GetFreeNodeFromCoordinates(m_vLevelCoords + Vector3Int.down, m_iDepth));
        Connect(GetFreeNodeFromCoordinates(m_vLevelCoords + Vector3Int.right, m_iDepth));
        Connect(GetFreeNodeFromCoordinates(m_vLevelCoords + Vector3Int.left, m_iDepth));
        Connect(GetFreeNodeFromCoordinates(m_vLevelCoords + Vector3Int.forward, m_iDepth));
        Connect(GetFreeNodeFromCoordinates(m_vLevelCoords + Vector3Int.back, m_iDepth));
    }
}