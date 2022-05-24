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
    public List<NodeMap> m_tNodeMaps = new List<NodeMap>();
    public NodeMap m_oLastNodeMap;
    public List<TestResult> m_tResults = new List<TestResult>();


    public IEnumerator RunTests(int _iID, MapGenerator _oMapGen, Agent _oAgent, Goal _oGoal)
    {
        for (int i = 0; i < m_iMapNumber; i++)
        {
            DateTime oBeforeGenerateMap = HighResolutionDateTime.UtcNow;
            _oMapGen.GenerateMap(m_oMapType);
            DateTime oAfterGenerateMap = HighResolutionDateTime.UtcNow;

            double dMapTime = (oAfterGenerateMap - oBeforeGenerateMap).TotalMilliseconds;

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
                    double dNodeMapTime = (oAfterGenerateNodes - oBeforeGenerateNodes).TotalMilliseconds;

                    _oGoal.RandomPosition(m_oMapType, oNodeMap);
                    DateTime oBeforeGeneratePath = HighResolutionDateTime.UtcNow;
                    _oAgent.StartPath(m_oMapType, oNodeMap);
                    DateTime oAfterGeneratePath = HighResolutionDateTime.UtcNow;
                    double dPathGenerationTime = (oAfterGeneratePath - oBeforeGeneratePath).TotalMilliseconds;
                    float fLength = _oAgent.CalculatePathLength();


                    TestResult oResult = new TestResult(_iID, i, j, k, fLength, dMapTime, dNodeMapTime, dPathGenerationTime);
                    m_tResults.Add(oResult);

                    Debug.Log("Finished test " + _iID + " map " + i + " nodeMap " + j + " iteration " + k);
                    yield return new WaitForSeconds(5f);
                }
            }
        }
    }
}