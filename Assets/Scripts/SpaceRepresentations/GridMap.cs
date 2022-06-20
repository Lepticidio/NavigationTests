using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GridMap")]
public class GridMap : VoxelMap
{
    public List<VoxelNode> m_tNodes = new List<VoxelNode>();
    public override void GenerateMap(MapType _oMapType)
    {
        base.GenerateMap(_oMapType);
        m_tNodes.Clear();

        for (int x = -(int)m_fMapHalfSize; x < (int)m_fMapHalfSize; x++)
        {
            for (int y = -(int)m_fMapHalfSize; y < (int)m_fMapHalfSize; y++)
            {
                for (int z = -(int)m_fMapHalfSize; z < (int)m_fMapHalfSize; z++)
                {
                    VoxelNode oNode = new VoxelNode(new Vector3(x, y, z), 0.5f);
                    m_tNodes.Add( oNode );
                    oNode.m_bFree = !oNode.CheckCollision();

                    if(oNode.m_bFree)
                    {
                        m_tFreeNodes.Add(oNode);
                        if (x > -m_fMapHalfSize)
                        {
                            if (y > -m_fMapHalfSize)
                            {
                                if (z > -m_fMapHalfSize)
                                {
                                    VoxelNode oNeighbour = GetNodeFromCoordinates(x - 1, y - 1, z - 1);
                                    oNode.Connect(oNeighbour, this);

                                }
                                VoxelNode oNeighbour2 = GetNodeFromCoordinates(x - 1, y - 1, z);
                                oNode.Connect(oNeighbour2, this);                                
                            }
                            if (z > -m_fMapHalfSize)
                            {
                                VoxelNode oNeighbour2 = GetNodeFromCoordinates(x - 1, y, z - 1);
                                oNode.Connect(oNeighbour2, this);
                            }
                            VoxelNode oNeighbour3 = GetNodeFromCoordinates(x - 1, y, z);
                            oNode.Connect(oNeighbour3, this);                            
                        }
                        if (y > -m_fMapHalfSize)
                        {
                            if (z > -m_fMapHalfSize)
                            {
                                VoxelNode oNeighbour = GetNodeFromCoordinates(x, y - 1, z - 1);
                                oNode.Connect(oNeighbour, this);
                            }
                            VoxelNode oNeighbour2 = GetNodeFromCoordinates(x, y - 1, z);
                            oNode.Connect(oNeighbour2, this);
                        }
                        if (z > -m_fMapHalfSize)
                        {
                            VoxelNode oNeighbour = GetNodeFromCoordinates(x, y, z - 1);
                            oNode.Connect(oNeighbour, this);
                        }
                    }
                }
            }
        }
        if (!_oMapType.m_bNoTerrain)
        {
            ClearUnderTerrainNodes();
        }
        m_bGenerated = true;
    }
    public VoxelNode GetNodeFromCoordinates(int _iX, int _iY, int _iZ)
    {
        int iMapSize = (int)(m_fMapHalfSize + m_fMapHalfSize);

        int iIndex = (_iX + (int)m_fMapHalfSize) * iMapSize * iMapSize +
            (_iY + (int)m_fMapHalfSize) * iMapSize  +
             (_iZ + (int)m_fMapHalfSize);

        return m_tNodes[iIndex];
    }
}
