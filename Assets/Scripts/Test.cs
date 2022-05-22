using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            _oMapGen.GenerateMap(m_oMapType);
            yield return new WaitForSeconds(0.05f);
            for (int j = 0; j < m_tNodeMaps.Count; j++)
            {
                NodeMap oNodeMap = m_tNodeMaps[j];
                m_oLastNodeMap = oNodeMap;
                for (int k = 0; k < m_iRepetitionsPerMap; k++)
                {
                    oNodeMap.GenerateMap(m_oMapType);
                    _oGoal.RandomPosition(m_oMapType, oNodeMap);
                    _oAgent.StartPath(m_oMapType, oNodeMap);

                    Debug.Log("map " + i + " node map "+ j + " iteration " + k);
                    yield return new WaitForSeconds(5f);
                }
            }
        }
    }
}
