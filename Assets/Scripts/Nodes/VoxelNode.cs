using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelNode : Node
{
    public float m_fHalfSize;
    protected Vector3 m_vHalfExtents;

    protected LayerMask m_iLayerMask;
    // Start is called before the first frame update

    public VoxelNode(Vector3 _vPosition, float _fHalfSize)
    {
        m_vPosition = _vPosition;
        m_fHalfSize = _fHalfSize;
        m_vHalfExtents = new Vector3(_fHalfSize, _fHalfSize, _fHalfSize);

        m_iLayerMask = ~(LayerMask.GetMask("Agent") | LayerMask.GetMask("Goal"));
    }
    public bool CheckCollision()
    {
        return Physics.OverlapBox(m_vPosition, m_vHalfExtents, Quaternion.identity, m_iLayerMask).Length > 0;
    }
    public void Draw(float _fMaxSize)
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(m_vPosition, Vector3.one * m_fHalfSize * 2);
        if (m_bFree)
        {
            Gizmos.color = new Color(0f, 0f, 1f, 0.5f);
        }
        else
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
            Gizmos.DrawCube(m_vPosition, Vector3.one * m_fHalfSize * 2);
        }
    }
}
