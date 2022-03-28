using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializeLevel()
    {
        world = GameObject.Find("Data").GetComponent<World>();
        music = world.GetComponent<MusicControl>();
        biome = world.currentBiome;
        GetComponent<MaterialController>().Initialize(world);


        surface = GetComponent<NavMeshSurface>();
        offsetX = Random.Range(0f, 99999f);
        offsetZ = Random.Range(0f, 99999f);

        sealevel = biome.sealevel;

        TerrainData terrainData = m_oSandyEarth;
        if (biome.humidity >= 300 && biome.sealevel < 1)
        {
            scale *= 2;
        }
        seaDepth = seaDepth * sealevel;

        if (biome.temperature < 5)
        {
            terrainData = m_oSandyTundra;
        }
        else
        {
            if (biome.humidity > 200)
            {
                terrainData = m_oSandyMud;
            }
            else if (biome.temperature < 10)
            {
                terrainData = m_oSnowyEarth;
            }
            else if (biome.humidity > 100)
            {
                terrainData = m_oMossyEarth;
            }
        }

        terrainWidth = terrainCells.GetLength(0);
        terrainHeight = terrainCells.GetLength(1);

        GameObject terrGO = Terrain.CreateTerrainGameObject(terrainData);
        terr = terrGO.GetComponent<Terrain>();
        terrGO.name = "Terrain";
        terrGO.layer = 8;
        terr.materialType = Terrain.MaterialType.Custom;
        terr.materialTemplate = terrainMaterial;
        terr.terrainData.size = new Vector3(width * CellConversor.factor, depth * CellConversor.factor, height * CellConversor.factor);
        terrGO.transform.position = new Vector3(-terr.terrainData.size.x * 0.5f, -terr.terrainData.size.y * sealevel, -terr.terrainData.size.z * 0.5f);
        hmWidth = terr.terrainData.heightmapWidth;
        terr.terrainData.SetHeights(0, 0, heights);

        itemData = GameObject.Find("Data").GetComponent<ItemData>();
        plants = GameObject.Find("Plants").transform;
        decorations = GameObject.Find("Decorations").transform;
        foods = GameObject.Find("Foods").transform;
        animals = GameObject.Find("Animals").transform;
        rockGenerator = GetComponent<RockGenerator>();

        grid.depth = depth;
        grid.sizeY = terr.terrainData.size.y;
        grid.sealevel = sealevel;
        grid.seaDepth = seaDepth;

    }

    float CalculateAltitude(int _iX, int _iY)
    {

        float fXPerl = (float)_iX * inverseWidth * scale + offsetX;
        float fYPerl = (float)_iY * inverseHeight * scale + offsetZ;
        float fPerlinNoise = Mathf.PerlinNoise(fXPerl, fYPerl);
        return fPerlinNoise;
    }
}
