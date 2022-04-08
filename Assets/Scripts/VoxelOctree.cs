using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelOctree : MonoBehaviour
{
    OctreeNode m_oRoot;
    public float m_fSize = 2048;
    // Start is called before the first frame update
    void Start()
    {
        m_oRoot = new OctreeNode(Vector3.zero, m_fSize * 0.5f);
        m_oRoot.Subdivide(1);
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

    private Color m_oMinColor = new Color(1, 0, 0, 0.25f);
    private Color m_oMaxColor = new Color(0, 0f, 1f, 1f);

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
        Debug.Log("Created node at " + m_vPosition + " with half size " + _fHalfSize);
    }
    public bool Subdivide(float _fMinSize)
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
                oChildNode.m_iDepth = m_iDepth + 1;
                oChildNode.m_iLocalIndex = i;
                oChildNode.CalculateLevelCoords();
                oChildNode.Subdivide(_fMinSize);
                m_tSubNodes[i] = oChildNode;
            }
            return true;
        }
        else
        {
            m_bFree = true;
            return false;
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
        if(m_bFree && _oNode.m_bFree && !m_tNeighbors.Contains(_oNode))
        {
            m_tNeighbors.Add(_oNode);
            _oNode.m_tNeighbors.Add(this);
        }
    }
    public OctreeNode GetNodeFromCoordinates(Vector3Int _vLevelCoordinates, int _iDepth)
    {
        OctreeNode oNode = m_oRoot;
        for (int i = 0; i < _iDepth; i++)
        {
            if(!m_bFree)
            {
                int iBinaryDigit = (int)Mathf.Pow(2, _iDepth);
                int iFirstIndexDigit = (_vLevelCoordinates.x & iBinaryDigit) == iBinaryDigit ? 4 : 0;
                int iSecondIndexDigit = (_vLevelCoordinates.y & iBinaryDigit) == iBinaryDigit ? 2 : 0;
                int iThirdIndexDigit = (_vLevelCoordinates.z & iBinaryDigit) == iBinaryDigit ? 1 : 0;

                oNode = oNode.m_tSubNodes[iFirstIndexDigit + iSecondIndexDigit + iThirdIndexDigit];

            }
            else
            {
                i = _iDepth;
            }

        }
        return oNode;
    }
}