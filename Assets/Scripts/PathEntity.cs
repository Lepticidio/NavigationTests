using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathEntity : MonoBehaviour
{
    public bool m_bRandomPosition;
    public bool m_bDependentOnSize, m_bPossitive;
    public Vector3 m_vOffset;
    public float m_fRandomMargin, m_fObstacleFreeRadius;
    public MapGenerator m_oMapGen;

    public void RandomPosition()
    {
        if(m_bRandomPosition)
        {
            int iLayerMask = ~(LayerMask.GetMask("Agent") | LayerMask.GetMask("Goal"));
            bool bValidPosition = false;
            if (m_bDependentOnSize)
            {
                m_vOffset = new Vector3(m_oMapGen.m_oNodeMap.m_fMapHalfSize, 0, 0);
            }
            if (!m_bPossitive)
            {
                m_vOffset = -m_vOffset;
            }
            while (!bValidPosition)
            {

                float fX = Random.Range(-m_fRandomMargin, m_fRandomMargin);
                float fY = Random.Range(-m_fRandomMargin, m_fRandomMargin);
                float fZ = Random.Range(-m_fRandomMargin, m_fRandomMargin);

                transform.position = m_vOffset + new Vector3(fX, fY, fZ);

                bValidPosition = !Physics.CheckSphere(transform.position, m_fObstacleFreeRadius, iLayerMask);


                if (bValidPosition && !m_oMapGen.m_oCurrentType.m_bNoTerrain)
                {
                    bValidPosition = Misc.CheckOverTerrain(transform.position);

                }
            }
        }

    }
}
