using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public bool m_bRandomGoalPosition, m_bMapGenerated;
    public static float m_fCellSizeFactor = 0.05f;
    float m_fInverseWidth, m_fInverseHeight;
    public float[,] m_tAltitudes;
    public float m_fScale, m_fOffsetX, m_fOffsetY, m_fSeaLevel, m_fSeaDepth;
    public Transform oObstacleParent;
    public Terrain m_oTerr;
    public List<GameObject> m_tObstacles = new List<GameObject>();

    // Start is called before the first frame update
    public void GenerateMap(MapType _oType)
    {
        m_bMapGenerated = false;
        m_oTerr.gameObject.SetActive(!_oType.m_bNoTerrain);
        if(!_oType.m_bNoTerrain)
        {
            GenerateTerrain(_oType);
        }
        GenerateObstacles(_oType);
        m_bMapGenerated = true;
    }

    void GenerateTerrain(MapType _oType)
    {
        int iDepth = _oType.m_iMapSize / 2;


        m_oTerr.terrainData.size = new Vector3(_oType.m_iMapSize * m_fCellSizeFactor, iDepth * m_fCellSizeFactor, _oType.m_iMapSize * m_fCellSizeFactor);
        m_oTerr.gameObject.transform.position = new Vector3(-m_oTerr.terrainData.size.x * 0.5f, -m_oTerr.terrainData.size.y * m_fSeaLevel, -m_oTerr.terrainData.size.z * 0.5f);

        m_tAltitudes = new float[_oType.m_iMapSize + 1, _oType.m_iMapSize + 1];

        float fHeightMapWidth = m_oTerr.terrainData.heightmapResolution;
        float fInverseHmWidth = 1f / (float)(fHeightMapWidth - 1);

        m_fInverseWidth = 1f / (float)_oType.m_iMapSize;
        m_fInverseHeight = 1f / (float)_oType.m_iMapSize;


        for (int i = 0; i < _oType.m_iMapSize; i++)
        {
            for (int j = 0; j < _oType.m_iMapSize; j++)
            {
               float fAltitude = CalculateAltitude(i, j) * 0.8f + 0.1f;

                int remainderI = (int)((float)i % (_oType.m_iMapSize * fInverseHmWidth));
                int remainderJ = (int)((float)j % (_oType.m_iMapSize * fInverseHmWidth));
                if(!_oType.m_bFlatExtremes)
                {
                    m_tAltitudes[j, i] = fAltitude;
                }
                else if (fAltitude < m_fSeaLevel)
                {
                    if (remainderI == 0 && remainderJ == 0)
                    {
                        if (fAltitude > m_fSeaLevel - m_fSeaDepth)
                        {
                            m_tAltitudes[j, i] = fAltitude;
                        }
                        else
                        {
                            m_tAltitudes[j, i] = m_fSeaLevel - m_fSeaDepth;
                        }
                    }
                }
                else
                {
                    if (remainderI == 0 && remainderJ == 0)
                    {
                        m_tAltitudes[j, i] = m_fSeaLevel;
                    }
                }
            }
        }
        m_oTerr.terrainData.SetHeights(0, 0, m_tAltitudes);
    }
    public void GenerateObstacles(MapType _oType)
    {
        for (int i = m_tObstacles.Count -1; i > -1; i--)
        {
            Destroy(m_tObstacles[i]);
        }
        m_tObstacles.Clear();
        int iObstacleAbundance = (int)(_oType.m_fObstacleDensity * (float)_oType.m_iMapSize);
        for (int i = 0; i < _oType.m_tObstacleCounts.Length; i++)
        {
            GameObject oPrefab = _oType.m_tObstacles[i];

            for (int j = 0; j < _oType.m_tObstacleCounts[i] * iObstacleAbundance; j++)
            {
                int iX = Random.Range(0, _oType.m_iMapSize + 1);
                int iZ = Random.Range(0, _oType.m_iMapSize + 1);

                float fY = Random.Range(0f, 1f);
                if (!_oType.m_bFloatingObstacles)
                {
                    fY = m_tAltitudes[iX, iZ];
                }

                GameObject oItemGo = Instantiate(oPrefab, new Vector3((iX - _oType.m_iMapSize / 2) * m_fCellSizeFactor, fY * _oType.m_iMapSize * m_fCellSizeFactor - (_oType.m_iMapSize * m_fCellSizeFactor)/2, (iZ - _oType.m_iMapSize / 2) * m_fCellSizeFactor), Quaternion.identity);
                oItemGo.transform.SetParent(oObstacleParent);
                m_tObstacles.Add(oItemGo);

                if (_oType.m_bObstacleSizeVariation)
                {
                    _oType.ChangeObstacleTransform(oItemGo.transform);
                }                
            }
        }
    }
    float CalculateAltitude(int _iX, int _iY)
    {

        float fXPerl = (float)_iX * m_fInverseWidth * m_fScale + m_fOffsetX;
        float fYPerl = (float)_iY * m_fInverseHeight * m_fScale + m_fOffsetY;
        float fPerlinNoise = Mathf.PerlinNoise(fXPerl, fYPerl);
        return fPerlinNoise;
    }
}
