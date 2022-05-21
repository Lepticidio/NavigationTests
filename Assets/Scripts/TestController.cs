using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    public List<Test> m_tTestsToRun = new List<Test>();
    public Agent m_oAgent;
    public Goal m_oGoal;
    Test m_oLastTest;
    public MapGenerator m_oMapGen;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < m_tTestsToRun.Count; i++)
        {
            Debug.Log("Test: " + i.ToString());
            m_oLastTest = m_tTestsToRun[i];
            StartCoroutine(m_tTestsToRun[i].RunTests(m_oMapGen, m_oAgent, m_oGoal));
        }
    }
    void OnDrawGizmos()
    {
        if(m_oLastTest != null)
        {
            if (m_oLastTest.m_oNodeMap.m_bPathfindingInEdges)
            {
                VoxelOctree oOctree = m_oLastTest.m_oNodeMap as VoxelOctree;
                if (oOctree != null && oOctree.m_oRoot != null && oOctree.m_bDebug)
                {
                    oOctree.m_oRoot.Draw(oOctree.m_fMapHalfSize);
                }
            }
            else
            {
                ProbabilisticRoadMap oPRM = m_oLastTest.m_oNodeMap as ProbabilisticRoadMap;
                if (oPRM.m_bDebug)
                {
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
