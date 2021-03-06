using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class TestController : MonoBehaviour
{
    public bool m_bDebug;
    public List<Test> m_tTestsToRun = new List<Test>();
    public Agent m_oAgent;
    public Goal m_oGoal;
    Test m_oLastTest;
    public MapGenerator m_oMapGen;
    public List<TestResult> m_tTestResults = new List<TestResult>();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RunTests());
    }

    public IEnumerator RunTests()
    {
        DateTime oInitialTime = HighResolutionDateTime.UtcNow;
        for (int i = 0; i < m_tTestsToRun.Count; i++)
        {
            Debug.Log("Test: " + i.ToString());
            m_oLastTest = m_tTestsToRun[i];
            yield return StartCoroutine(m_tTestsToRun[i].RunTests(i, m_oMapGen, m_oAgent, m_oGoal, oInitialTime));
            m_tTestResults.AddRange(m_tTestsToRun[i].m_tResults);
        }

        string sToExport = "";
        for (int i = 0; i < m_tTestResults.Count; i++)
        {
            sToExport += m_tTestResults[i].ToString();
        }

        ExporterCSV.SaveToFile(sToExport);
        Debug.Log("Info exported");
    }
    void OnDrawGizmos()
    {
        if(m_bDebug)
        {
            if (m_oLastTest != null && m_oLastTest.m_oLastSpaceRepresentation != null)
            {
                if (m_oLastTest.m_oLastSpaceRepresentation.m_bPathfindingInEdges)
                {
                    VoxelOctree oOctree = m_oLastTest.m_oLastSpaceRepresentation as VoxelOctree;
                    if (oOctree != null && oOctree.m_oRoot != null)
                    {
                        oOctree.m_oRoot.Draw(oOctree.m_fMapHalfSize);
                    }
                    else
                    {
                        GridMap oGrid = m_oLastTest.m_oLastSpaceRepresentation as GridMap;
                        if(oGrid != null)
                        {
                            for (int i = 0; i < oGrid.m_tNodes.Count; i++)
                            {
                                oGrid.m_tNodes[i].Draw(0.5f);
                            }
                        }
                    }
                }
                else
                {
                    ProbabilisticRoadMap oPRM = m_oLastTest.m_oLastSpaceRepresentation as ProbabilisticRoadMap;

                    for (int i = 0; i < oPRM.m_tFreeNodes.Count; i++)
                    {
                        PRMNode oNode = oPRM.m_tFreeNodes[i] as PRMNode;
                        oNode.Draw();
                    }
                    
                }
            }
        }
    }
}
