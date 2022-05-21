using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Test")]
public class Test : ScriptableObject
{

    public int m_iSize, m_iMapNumber, m_iRepetitionsPerMap;
    public MapType m_oMapType;
    public List<float> m_tResults = new List<float>();
    public NodeMap m_oNodeMap;

    public IEnumerator RunTests(MapGenerator _oMapGen, Agent _oAgent, Goal _oGoal)
    {
        for (int i = 0; i < m_iMapNumber; i++)
        {
            _oMapGen.GenerateMap(m_oMapType);
            Debug.Log("Map " + i + " generated" + " Algorithm: " + m_oNodeMap.name);
            for (int  j = 0; j < m_iRepetitionsPerMap; j++)
            {
                Debug.Log("Map " + i + " iteration " + j +" Algorithm: " + m_oNodeMap.name );
                yield return new WaitForSeconds(1);
                //yield return _oMapGen.StartCoroutine(m_oNodeMap.GenerateMap(m_oMapType));
                _oMapGen.StartCoroutine( m_oNodeMap.GenerateMap(m_oMapType));
                while (!m_oNodeMap.m_bGenerated)
                {
                    Debug.Log("waiting for map");
                    yield return new WaitForSeconds(0.05f);
                }
                Debug.Log("Finished generating map");
                yield return new WaitForSeconds(1f);
                _oGoal.RandomPosition(m_oMapType, m_oNodeMap);
                Debug.Log("Finished goal random Position");
                yield return new WaitForSeconds(0.1f);
                _oAgent.StartCoroutine(_oAgent.StartPath(m_oMapType, m_oNodeMap));
                while (!_oAgent.m_bPathCreated)
                {
                    Debug.Log("waiting for path");
                    yield return new WaitForSeconds(0.05f);
                }
                //Debug.Log("Map type: " + m_oMapType.name + " Map: " + i + " Iteration: " + j +" Algorithm: " + m_oNodeMap.name+   " Length: " + fResult);
                Debug.Log("ITERATION ENDS");

                yield return new WaitForSeconds(1f);
            }
        }
    }
}
