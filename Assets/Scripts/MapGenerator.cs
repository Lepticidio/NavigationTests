using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public bool m_bRandomGoalPosition, m_bMapGenerated;
    float m_fInverseWidth, m_fInverseHeight, m_fCellSizeFactor = 0.05f;
    public int m_iWidth, m_iHeight, m_iDepth;
    public float[,] m_tAltitudes;
    public float m_fScale, m_fOffsetX, m_fOffsetY, m_fSeaLevel, m_fSeaDepth;
    public Transform oObstacleParent;
    public Terrain m_oTerr;

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
        m_oTerr.terrainData.size = new Vector3(m_iWidth * m_fCellSizeFactor, m_iDepth * m_fCellSizeFactor, m_iHeight * m_fCellSizeFactor);
        m_oTerr.gameObject.transform.position = new Vector3(-m_oTerr.terrainData.size.x * 0.5f, -m_oTerr.terrainData.size.y * m_fSeaLevel, -m_oTerr.terrainData.size.z * 0.5f);

        m_tAltitudes = new float[m_iWidth + 1, m_iHeight + 1];

        float fHeightMapWidth = m_oTerr.terrainData.heightmapResolution;
        float fInverseHmWidth = 1f / (float)(fHeightMapWidth - 1);

        m_fInverseWidth = 1f / (float)m_iWidth;
        m_fInverseHeight = 1f / (float)m_iHeight;


        for (int i = 0; i < m_iWidth; i++)
        {
            for (int j = 0; j < m_iHeight; j++)
            {
               float fAltitude = CalculateAltitude(i, j) * 0.8f + 0.1f;

                int remainderI = (int)((float)i % (m_iWidth * fInverseHmWidth));
                int remainderJ = (int)((float)j % (m_iHeight * fInverseHmWidth));
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
        for (int i = 0; i < _oType.m_tObstacleCounts.Length; i++)
        {
            GameObject oPrefab = _oType.m_tObstacles[i];

            for (int j = 0; j < _oType.m_tObstacleCounts[i] * _oType.m_iObstacleAbundance; j++)
            {
                int iX = Random.Range(0, m_iWidth + 1);
                int iZ = Random.Range(0, m_iHeight + 1);

                float fY = Random.Range(0f, 1f);
                if (!_oType.m_bFloatingObstacles)
                {
                    fY = m_tAltitudes[iX, iZ];
                }

                GameObject oItemGo = Instantiate(oPrefab, new Vector3((iX - m_iWidth/2) * m_fCellSizeFactor, fY * m_iDepth * m_fCellSizeFactor - (m_iDepth*m_fCellSizeFactor)/2, (iZ - m_iHeight/2) * m_fCellSizeFactor), Quaternion.identity);
                oItemGo.transform.SetParent(oObstacleParent);

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
