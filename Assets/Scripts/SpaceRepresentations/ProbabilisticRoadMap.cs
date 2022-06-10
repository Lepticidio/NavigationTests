using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructures.ViliWonka.KDTree;

[CreateAssetMenu(menuName = "PRM")]
public class ProbabilisticRoadMap : SpaceRepresentation
{
    public bool m_bUsesKTrees = false;
    public float m_fNodeRadius, m_fConnectRadius, m_fNodeDensity;
    public Dictionary<Vector3, PRMNode> m_tNodesByPosition = new Dictionary<Vector3, PRMNode>();
    public List<Vector3> m_tPositions = new List<Vector3>();
    public KDTree m_oKTree;
    public override void GenerateMap(MapType _oMapType)
    {
        m_bGenerated = false;
        m_tFreeNodes.Clear();
        m_tNodesByPosition.Clear();
        m_tPositions.Clear();
        CalculateHalfSize(_oMapType);
        int iCounter = 0;
        int iCounterLimit = 100000;
        while(m_tFreeNodes.Count < m_fNodeDensity * _oMapType.GetTridimensionalSize() && iCounter < iCounterLimit)
        {
            PRMNode oNode = new PRMNode(new Vector3(Random.Range(-m_fMapHalfSize, m_fMapHalfSize), Random.Range(-m_fMapHalfSize, m_fMapHalfSize), Random.Range(-m_fMapHalfSize, m_fMapHalfSize)), m_fNodeRadius, m_fConnectRadius);
            oNode.m_bFree = !oNode.CheckCollision();
            if(oNode != null && oNode.m_bFree)
            {
                m_tFreeNodes.Add(oNode);
                m_tNodesByPosition.Add(oNode.m_vPosition, oNode);
                m_tPositions.Add(oNode.m_vPosition);
            }
            iCounter++;
        }
        if(m_bUsesKTrees)
        {
            m_oKTree = new KDTree();
            m_oKTree.Build(m_tPositions);          
        }
        if (!_oMapType.m_bNoTerrain)
        {
            ClearUnderTerrainNodes();
        }
        ConnectNeighbours();
        m_bGenerated = true;
    }

    public override Node GetNodeFromPosition(Vector3 _vPosition)
    {
        if(m_bUsesKTrees)
        {
            KDQuery oQuery = new KDQuery();
            List<int> tIndexes = new List<int>();
            oQuery.ClosestPoint(m_oKTree, _vPosition, tIndexes);
            Vector3 vPoint = m_tPositions[tIndexes[0]];
            //if(m_tNodesByPosition.ContainsKey(vPoint))
            //{
                return m_tNodesByPosition[vPoint];
            //}
            //else
            //{
            //    return null;
            //}
        }
        else
        {
            return base.GetNodeFromPosition(_vPosition);
        }
    }


    public void ConnectNeighbours()
    {
        LayerMask iLayerMask = ~(LayerMask.GetMask("Agent") | LayerMask.GetMask("Goal"));
        if(m_bUsesKTrees)
        {
            for (int i = 0; i < m_tFreeNodes.Count; i++)
            {
                Node oNode = m_tFreeNodes[i];
                List<int> tIndexes = new List<int>();
                KDQuery oQuery = new KDQuery();
                oQuery.Radius(m_oKTree, oNode.m_vPosition, m_fConnectRadius, tIndexes);
                for (int j = 0; j < tIndexes.Count; j++)
                {
                    Vector3 vPoint = m_tPositions[tIndexes[j]];
                    if(vPoint != oNode.m_vPosition)
                    {
                        if(m_tNodesByPosition.ContainsKey(vPoint))
                        {
                            PRMNode oNeighbour = m_tNodesByPosition[vPoint];
                            if (!oNode.m_tNeighbours.Contains(oNeighbour))
                            {
                                Vector3 vDir = oNeighbour.m_vPosition - oNode.m_vPosition;
                                float fDistance = vDir.magnitude;
                                RaycastHit oInfo;
                                bool bCollision = Physics.SphereCast(oNode.m_vPosition, 0.5f, vDir, out oInfo, fDistance, iLayerMask);
                                if (!bCollision)
                                {
                                    oNode.m_tNeighbours.Add(oNeighbour);
                                    oNeighbour.m_tNeighbours.Add(oNode);
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < m_tFreeNodes.Count; i++)
            {
                for (int j = i + 1; j < m_tFreeNodes.Count; j++)
                {
                    Vector3 vDir = m_tFreeNodes[i].m_vPosition - m_tFreeNodes[j].m_vPosition;
                    float fDistance = vDir.magnitude;
                    if (fDistance < m_fConnectRadius)
                    {
                        RaycastHit oInfo;
                        bool bCollision = Physics.SphereCast(m_tFreeNodes[j].m_vPosition, 0.5f, vDir, out oInfo, fDistance, iLayerMask);
                        if (!bCollision)
                        {
                            m_tFreeNodes[i].m_tNeighbours.Add(m_tFreeNodes[j]);
                            m_tFreeNodes[j].m_tNeighbours.Add(m_tFreeNodes[i]);
                        }
                    }

                }
            }

        }
    }
}
