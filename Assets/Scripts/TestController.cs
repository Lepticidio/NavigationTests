using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    public List<Test> m_tTestsToRun = new List<Test>();
    public Agent m_oAgent;
    public Goal m_oGoal;
    public MapGenerator m_oMapGen;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < m_tTestsToRun.Count; i++)
        {
            Debug.Log("Test: " + i.ToString());
            StartCoroutine(m_tTestsToRun[i].RunTests(m_oMapGen, m_oAgent, m_oGoal));
        }
    }
}
