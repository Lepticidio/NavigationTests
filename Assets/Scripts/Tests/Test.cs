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
    public List<SpaceRepresentation> m_tSpaceRepresentations = new List<SpaceRepresentation>();
    public SpaceRepresentation m_oLastSpaceRepresentation;
    public List<TestResult> m_tResults = new List<TestResult>();


    public IEnumerator RunTests(int _iID, MapGenerator _oMapGen, Agent _oAgent, Goal _oGoal, DateTime _oInitialTime)
    {
        for (int i = 0; i < m_iMapNumber; i++)
        {
            DateTime oBeforeGenerateMap = HighResolutionDateTime.UtcNow;
            _oMapGen.GenerateMap(m_oMapType);
            DateTime oAfterGenerateMap = HighResolutionDateTime.UtcNow;

            double dMapTime = (oAfterGenerateMap - oBeforeGenerateMap).TotalMilliseconds;

            yield return new WaitForSeconds(0.05f);
            for (int j = 0; j < m_tSpaceRepresentations.Count; j++)
            {
                SpaceRepresentation oSpaceRepresentation = m_tSpaceRepresentations[j];
                m_oLastSpaceRepresentation = oSpaceRepresentation;
                for (int k = 0; k < m_iRepetitionsPerMap; k++)
                {
                    DateTime oBeforeGenerateNodes = HighResolutionDateTime.UtcNow;
                    oSpaceRepresentation.GenerateMap(m_oMapType);
                    DateTime oAfterGenerateNodes = HighResolutionDateTime.UtcNow;
                    double dSpaceRepresentationTime = (oAfterGenerateNodes - oBeforeGenerateNodes).TotalMilliseconds;

                    _oGoal.RandomPosition(m_oMapType, oSpaceRepresentation);
                    DateTime oBeforeGeneratePath = HighResolutionDateTime.UtcNow;
                    _oAgent.StartPath(m_oMapType, oSpaceRepresentation);
                    DateTime oAfterGeneratePath = HighResolutionDateTime.UtcNow;
                    double dPathGenerationTime = (oAfterGeneratePath - oBeforeGeneratePath).TotalMilliseconds;
                    float fLength = _oAgent.CalculatePathLength();


                    TestResult oResult = new TestResult(_iID, i, j, k, fLength, dMapTime, dSpaceRepresentationTime, dPathGenerationTime);
                    oResult.m_iNumberObstacles = _oMapGen.m_tObstacles.Count;
                    oResult.m_iMapSize = m_oMapType.m_iMapSize;
                    oResult.m_iNumberNodes = oSpaceRepresentation.m_tFreeNodes.Count;
                    oResult.m_sSpaceRepresentationName = oSpaceRepresentation.ToString();

                    DateTime oEndTestTime = HighResolutionDateTime.UtcNow;
                    double dTestTime = (oEndTestTime - oBeforeGenerateNodes).TotalMilliseconds;
                    double dTotalTime = (oEndTestTime - _oInitialTime).TotalMilliseconds;
                    oResult.m_dTestTime = dTestTime;
                    oResult.m_dTotalTime = dTotalTime;

                    m_tResults.Add(oResult);

                    Debug.Log("Finished test " + _iID + " map " + i + " SpaceRepresentation " + j + " iteration " + k);
                    yield return new WaitForSeconds(5f);
                }
            }
        }
    }
}