using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MapType")]
public class MapType : ScriptableObject
{
    public bool m_bNoTerrain, m_bFlatExtremes, m_bFloatingObstacles, m_bObstacleSizeVariation, m_bUniformObstacles, m_bRotatedObstacles;
    public int m_iMapSize;
    public float m_fMinSize, m_fMaxSize, m_fObstacleDensity;
    public int[] m_tObstacleCounts;
    public GameObject[] m_tObstacles;

    public void ChangeObstacleTransform(Transform _oTransform)
    {
        Vector3 vOriginalLocal = _oTransform.localScale;
        float fRandomX = Random.Range(m_fMinSize, m_fMaxSize);
        float fRandomY = Random.Range(m_fMinSize, m_fMaxSize);
        float fRandomZ = Random.Range(m_fMinSize, m_fMaxSize);
        if (m_bUniformObstacles)
        {
            fRandomY = fRandomX;
            fRandomZ = fRandomX;
        }
        _oTransform.localScale = new Vector3(vOriginalLocal.x * fRandomX, vOriginalLocal.y * fRandomY, vOriginalLocal.z * fRandomZ);
        if (m_bRotatedObstacles)
        {
            _oTransform.rotation = Random.rotation;
        }
    }
}
