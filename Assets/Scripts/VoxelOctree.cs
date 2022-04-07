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

public enum OctreeIndex
{
    BottomLeftFront = 0, //000,
    BottomRightFront = 2, //010,
    BottomRightBack = 3, //011,
    BottomLeftBack = 1, //001,
    TopLeftFront = 4, //100,
    TopRightFront = 6, //110,
    TopRightBack = 7, //111,
    TopLeftBack = 5, //101,
}
public class OctreeNode
{
    bool m_bFree = false;
    float m_fHalfSize;
    Vector3 m_vHalfExtents;
    Vector3 m_vPosition;
    public OctreeNode[] m_tSubNodes;

    private Color m_oMinColor = new Color(1, 0, 0, 0.25f);
    private Color m_oMaxColor = new Color(0, 0f, 1f, 1f);

    public OctreeNode(Vector3 _vPosition, float _fHalfSize)
    {
        m_vPosition = _vPosition;
        m_fHalfSize = _fHalfSize;
        m_vHalfExtents = new Vector3(_fHalfSize, _fHalfSize, _fHalfSize);
        Debug.Log("Created node at " + m_vPosition + " with half size " + _fHalfSize);
    }
    public void Subdivide(float _fMinSize)
    {
        if (CheckCollision() && m_fHalfSize > _fMinSize)
        {
            m_tSubNodes = new OctreeNode[8];
            float fNewHalfSize = m_fHalfSize * 0.5f;
            for (int i = 0; i < m_tSubNodes.Length; i++)
            {

                Vector3 vNewPos = m_vPosition;
                if ((i & 4) == 4)
                {
                    vNewPos.y += fNewHalfSize;
                }
                else
                {
                    vNewPos.y -= fNewHalfSize;
                }

                if ((i & 2) == 2)
                {
                    vNewPos.x += fNewHalfSize;
                }
                else
                {
                    vNewPos.x -= fNewHalfSize;
                }

                if ((i & 1) == 1)
                {
                    vNewPos.z += fNewHalfSize;
                }
                else
                {
                    vNewPos.z -= fNewHalfSize;
                }

                OctreeNode oChildNode = new OctreeNode(vNewPos, fNewHalfSize);
                oChildNode.Subdivide(_fMinSize);
                m_tSubNodes[i] = oChildNode;
            }
        }
        else
        {
            m_bFree = true;
        }
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
}