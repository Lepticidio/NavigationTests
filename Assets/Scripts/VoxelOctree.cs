using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "VoxelOctree")]
public class VoxelOctree : NodeMap
{
    OctreeNode m_oRoot;
    // Start is called before the first frame update


    public override IEnumerator GenerateMap(MapType _oMapType)
    {
        Debug.Log("Genereting octree map");
        m_bGenerated = false;
        yield return new WaitForSeconds(0.1f);
        m_tFreeNodes.Clear();
        m_oRoot = new OctreeNode(Vector3.zero, m_fMapHalfSize );
        m_oRoot.Subdivide(1, m_tFreeNodes);
        if (!_oMapType.m_bNoTerrain)
        {
            ClearUnderTerrainNodes();
        }
        ConnectNeighbours();
        Debug.Log("Map generated with " + m_tFreeNodes.Count + " nodes");
        m_bGenerated = true;
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