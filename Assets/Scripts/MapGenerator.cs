using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public bool m_bFlatExtremes;
    float m_fInverseWidth, m_fInverseHeight, m_fCellSizeFactor = 0.05f;
    public int m_iWidth, m_iHeight, m_iDepth;
    public float m_fScale, m_fOffsetX, m_fOffsetY, m_fSeaLevel, m_fSeaDepth;
    public Terrain m_oTerr;
    // Start is called before the first frame update
    void Start()
    {
        GenerateTerrain();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PreparateTerrain()
    {
        //m_oTerr.terrainData.SetHeights(0, 0, heights);
    }

    void GenerateTerrain()
    {


        m_oTerr.terrainData.size = new Vector3(m_iWidth * m_fCellSizeFactor, m_iDepth * m_fCellSizeFactor, m_iHeight * m_fCellSizeFactor);
        m_oTerr.gameObject.transform.position = new Vector3(-m_oTerr.terrainData.size.x * 0.5f, -m_oTerr.terrainData.size.y * m_fSeaLevel, -m_oTerr.terrainData.size.z * 0.5f);

        float[,] fTempHeightMap = new float[2049, 2049];

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
                if(!m_bFlatExtremes)
                {
                    fTempHeightMap[j, i] = fAltitude;
                }
                else if (fAltitude < m_fSeaLevel)
                {
                    if (remainderI == 0 && remainderJ == 0)
                    {
                        if (fAltitude > m_fSeaLevel - m_fSeaDepth)
                        {
                            fTempHeightMap[j, i] = fAltitude;
                        }
                        else
                        {
                            fTempHeightMap[j, i] = m_fSeaLevel - m_fSeaDepth;
                        }
                    }
                }
                else
                {
                    if (remainderI == 0 && remainderJ == 0)
                    {
                        fTempHeightMap[j, i] = m_fSeaLevel;
                    }
                }
            }
        }
        m_oTerr.terrainData.SetHeights(0, 0, fTempHeightMap);
    }

    float CalculateAltitude(int _iX, int _iY)
    {

        float fXPerl = (float)_iX * m_fInverseWidth * m_fScale + m_fOffsetX;
        float fYPerl = (float)_iY * m_fInverseHeight * m_fScale + m_fOffsetY;
        float fPerlinNoise = Mathf.PerlinNoise(fXPerl, fYPerl);
        return fPerlinNoise;
    }
}
