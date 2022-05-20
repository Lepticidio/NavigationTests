using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelOctree : NodeMap
{
    OctreeNode m_oRoot;
    // Start is called before the first frame update


    public override void GenerateMap()
    {
        m_oRoot = new OctreeNode(Vector3.zero, m_fMapHalfSize );
        m_oRoot.Subdivide(1, m_tFreeNodes);
        if (!m_oMapGen.m_oCurrentType.m_bNoTerrain)
        {
            ClearUnderTerrainNodes();
        }
        ConnectNeighbours();
        m_bGenerated = true;
        Debug.Log("Map generated with " + m_tFreeNodes.Count + " nodes");
    }

    void OnDrawGizmos()
    {
        if(m_oRoot != null && m_bDebug)
        {
            m_oRoot.Draw(m_fMapHalfSize);
        }
    }

    void ConnectNeighbours()
    {
        for(int i = 0; i < m_tFreeNodes.Count; i++)
        {
            OctreeNode oNode = m_tFreeNodes[i] as OctreeNode;
            oNode.ConnectNeighbours();
        }
    }
    public override Node GetNodeFromPosition(Vector3 _vPosition)
    {
        OctreeNode oResult = null;
        for (int i = 0; i < m_tFreeNodes.Count; i++)
        {
            OctreeNode oNode = m_tFreeNodes[i] as OctreeNode;
            Vector3 vNodePosition = oNode.m_vPosition;

            bool bInVoxelX = _vPosition.x >= vNodePosition.x - oNode.m_fHalfSize && _vPosition.x <= vNodePosition.x + oNode.m_fHalfSize;
            bool bInVoxelY = _vPosition.y >= vNodePosition.y - oNode.m_fHalfSize && _vPosition.y <= vNodePosition.y + oNode.m_fHalfSize;
            bool bInVoxelZ = _vPosition.z >= vNodePosition.z - oNode.m_fHalfSize && _vPosition.z <= vNodePosition.z + oNode.m_fHalfSize;
         
            if(bInVoxelX && bInVoxelY && bInVoxelZ)
            {
                oResult = oNode;
                i = m_tFreeNodes.Count;
            }
        }
        return oResult;
    }
    public override Node GetConnectedNodeFromPosition(Vector3 _vPosition, Node _oOtherNode)
    {
        OctreeNode oResult = null;
        if (_oOtherNode != null)
        {
            for (int i = 0; i < m_tFreeNodes.Count; i++)
            {
                OctreeNode oNode = m_tFreeNodes[i] as OctreeNode;
                Vector3 vNodePosition = oNode.m_vPosition;

                bool bInVoxelX = _vPosition.x >= vNodePosition.x - oNode.m_fHalfSize && _vPosition.x <= vNodePosition.x + oNode.m_fHalfSize;
                bool bInVoxelY = _vPosition.y >= vNodePosition.y - oNode.m_fHalfSize && _vPosition.y <= vNodePosition.y + oNode.m_fHalfSize;
                bool bInVoxelZ = _vPosition.z >= vNodePosition.z - oNode.m_fHalfSize && _vPosition.z <= vNodePosition.z + oNode.m_fHalfSize;

                if (bInVoxelX && bInVoxelY && bInVoxelZ)
                {
                    oResult = oNode;
                    i = m_tFreeNodes.Count;
                }
            }
        }
        return oResult;
    }
}