using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
[CreateAssetMenu(menuName = "Test")]
public class Test : ScriptableObject
{
    public int m_iSize, m_iMapNumber, m_iRepetitionsPerMap;
    public MapType m_oMapType;
    public List<float> m_tResults = new List<float>();
    public List<NodeMap> m_tNodeMaps = new List<NodeMap>();
    public NodeMap m_oLastNodeMap;


    public IEnumerator RunTests(MapGenerator _oMapGen, Agent _oAgent, Goal _oGoal)
    {
        for (int i = 0; i < m_iMapNumber; i++)
        {
            DateTime oBeforeGenerateMap = HighResolutionDateTime.UtcNow;
            _oMapGen.GenerateMap(m_oMapType);
            DateTime oAfterGenerateMap = HighResolutionDateTime.UtcNow;
            Debug.Log("Map " + i +"-Time to generate map: " + (oAfterGenerateMap - oBeforeGenerateMap).TotalMilliseconds);
            yield return new WaitForSeconds(0.05f);
            for (int j = 0; j < m_tNodeMaps.Count; j++)
            {
                NodeMap oNodeMap = m_tNodeMaps[j];
                m_oLastNodeMap = oNodeMap;
                for (int k = 0; k < m_iRepetitionsPerMap; k++)
                {
                    DateTime oBeforeGenerateNodes = HighResolutionDateTime.UtcNow;
                    oNodeMap.GenerateMap(m_oMapType);
                    DateTime oAfterGenerateNodes = HighResolutionDateTime.UtcNow;
                    Debug.Log("Map " + i + " algorithm "+ j + " iteration " + k+ "-Time to generate nodes: " + (oAfterGenerateNodes - oBeforeGenerateNodes).TotalMilliseconds);

                    _oGoal.RandomPosition(m_oMapType, oNodeMap);
                    DateTime oBeforeGeneratePath = HighResolutionDateTime.UtcNow;
                    _oAgent.StartPath(m_oMapType, oNodeMap);
                    DateTime oAfterGeneratePath = HighResolutionDateTime.UtcNow;
                    Debug.Log("Map " + i + " algorithm " + j + " iteration " + k + "-Time to generate path: " + (oAfterGeneratePath - oBeforeGeneratePath).TotalMilliseconds);

                    yield return new WaitForSeconds(5f);
                }
            }
        }
    }
}
