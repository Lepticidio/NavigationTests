using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NodeMap : MonoBehaviour
{
    public bool m_bGenerated= false;
    public bool m_bDebug;

    public List<Node> m_tFreeNodes = new List<Node>();
    public MapGenerator m_oMapGen;

    private void Update()
    {
        if (!m_bGenerated)
        {
            GenerateMap();
        }
    }
    public abstract void GenerateMap();
    public virtual Node GetNodeFromPosition(Vector3 _vPosition)
    {
        LayerMask iLayerMask = ~(LayerMask.GetMask("Agent") | LayerMask.GetMask("Goal"));
        Node oResult = null;
        float fClosestDistance = Mathf.Infinity;


        for (int i = 0; i < m_tFreeNodes.Count; i++)
        {
            Node oNode = m_tFreeNodes[i];
            Vector3 vDir = oNode.m_vPosition - _vPosition;
            float fDistance = vDir.magnitude;
            RaycastHit oInfo;
            bool bCollision = Physics.SphereCast(_vPosition, 0.5f, vDir, out oInfo, fDistance, iLayerMask);
            if (!bCollision && (oResult == null || fDistance < fClosestDistance))
            {
                oResult = oNode;
                fClosestDistance = fDistance;
            }
        }
        return oResult;
    }
    public virtual Node GetConnectedNodeFromPosition(Vector3 _vPosition, Node _oOtherNode)
    {
        LayerMask iLayerMask = ~(LayerMask.GetMask("Agent") | LayerMask.GetMask("Goal"));
        Node oResult = null;
        if(_oOtherNode != null)
        {
            float fClosestDistance = Mathf.Infinity;
            
            for (int i = 0; i < m_tFreeNodes.Count; i++)
            {
                Node oNode = m_tFreeNodes[i];
                Vector3 vDir = oNode.m_vPosition - _vPosition;
                bool bConnected = false;
                List<Node> tCheckedNodes = new List<Node>();
                oNode.CheckIfConnected(_oOtherNode, ref tCheckedNodes, ref bConnected);
                if (bConnected)
                {
                    oNode.m_bCheckedDebug = true;
                    float fDistance = vDir.magnitude;
                    RaycastHit oInfo;
                    bool bCollision = Physics.SphereCast(_vPosition, 0.5f, vDir, out oInfo, fDistance, iLayerMask);
                    if (!bCollision && (oResult == null || fDistance < fClosestDistance))
                    {
                        oResult = oNode;
                        fClosestDistance = fDistance;
                    }
                }
            }
        }
        return oResult;
    }
    public void ClearUnderTerrainNodes()
    {
        int iTerrainMask = LayerMask.GetMask("Terrain");
        for (int i = m_tFreeNodes.Count -1; i >= 0; i --)
        {
            if(!Misc.CheckOverTerrain(m_tFreeNodes[i].m_vPosition))
            {
                m_tFreeNodes.RemoveAt(i);
            }

        }
    }
}

