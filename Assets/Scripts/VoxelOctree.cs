using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "VoxelOctree")]
public class VoxelOctree : NodeMap
{
    public OctreeNode m_oRoot;
    // Start is called before the first frame update


    public override void GenerateMap(MapType _oMapType)
    {
        m_bGenerated = false;
        m_tFreeNodes.Clear();
        m_oRoot = new OctreeNode(Vector3.zero, m_fMapHalfSize );
        m_oRoot.Subdivide(1, m_tFreeNodes);
        if (!_oMapType.m_bNoTerrain)
        {
            ClearUnderTerrainNodes();
        }
        ConnectNeighbours();
        m_bGenerated = true;
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

        Debug.Log("Getting a node out of " + m_tFreeNodes.Count);
        OctreeNode oResult = null;
        //OctreeNode oClosest = null;
        //float fDistance = Mathf.Infinity;
        //float fClosest = Mathf.Infinity;

        for (int i = 0; i < m_tFreeNodes.Count; i++)
        {
            OctreeNode oNode = m_tFreeNodes[i] as OctreeNode;
            Vector3 vNodePosition = oNode.m_vPosition;
            //float fX = 0;
            //float fY = 0;
            //float fZ = 0;

            bool bInVoxelX = _vPosition.x >= vNodePosition.x - oNode.m_fHalfSize && _vPosition.x <= vNodePosition.x + oNode.m_fHalfSize;

            bool bInVoxelY = _vPosition.y >= vNodePosition.y - oNode.m_fHalfSize && _vPosition.y <= vNodePosition.y + oNode.m_fHalfSize;

            bool bInVoxelZ = _vPosition.z >= vNodePosition.z - oNode.m_fHalfSize && _vPosition.z <= vNodePosition.z + oNode.m_fHalfSize;

            Debug.Log("Obtained node pos " + oNode.m_vPosition + " size " + oNode.m_fHalfSize);

            //Debug.Log(" x: " + bInVoxelX + " y: " + bInVoxelY + " z: " + bInVoxelZ);
            if(bInVoxelX && bInVoxelY && bInVoxelZ)
            {
                oResult = oNode;
                i = m_tFreeNodes.Count;
            }
            //else if(oResult == null)
            //{
            //    if (!bInVoxelX)
            //    {
            //        if (_vPosition.x < vNodePosition.x)
            //        {
            //            fX = vNodePosition.x - oNode.m_fHalfSize - _vPosition.x;
            //        }
            //        else
            //        {
            //            fX = _vPosition.x - vNodePosition.x + oNode.m_fHalfSize;
            //        }
            //    }
            //    if (!bInVoxelY)
            //    {
            //        if (_vPosition.y < vNodePosition.y)
            //        {
            //            fY = vNodePosition.y - oNode.m_fHalfSize - _vPosition.y;
            //        }
            //        else
            //        {
            //            fY = _vPosition.y - vNodePosition.y + oNode.m_fHalfSize;
            //        }
            //    }
            //    if (!bInVoxelZ)
            //    {
            //        if (_vPosition.z < vNodePosition.z)
            //        {
            //            fZ = vNodePosition.z - oNode.m_fHalfSize - _vPosition.z;
            //        }
            //        else
            //        {
            //            fZ = _vPosition.z - vNodePosition.z + oNode.m_fHalfSize;
            //        }
            //    }
            //    fDistance = new Vector3(fX, fY, fZ).sqrMagnitude;
            //    if(fDistance < fClosest)
            //    {
            //        fClosest = fDistance;
            //        oClosest = oNode;
            //    }

            //}
        }
        //if(oResult == null)
        //{
        //    oResult = oClosest;
        //}
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