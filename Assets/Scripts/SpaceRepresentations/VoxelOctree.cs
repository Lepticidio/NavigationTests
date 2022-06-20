using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "VoxelOctree")]
public class VoxelOctree : VoxelMap
{
    public OctreeNode m_oRoot;
    // Start is called before the first frame update


    public override void GenerateMap(MapType _oMapType)
    {
        base.GenerateMap(_oMapType);
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
            oNode.ConnectNeighbours(this);
        }
    }
}