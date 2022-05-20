using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Test")]
public class Test : ScriptableObject
{

    int m_iSize, m_iMapNumber, m_iRepetitionsPerMap;
    public MapType m_oMapType;
    public MapGenerator m_oMapGen;
    public List<float> m_oResults;
    public NodeMap m_oNodeMap;



    // Start is called before the first frame update
    //IEnumerator RunTests()
    //{

    //   for (int i = 0; i < m_iMapNumber; i++)
    //   {
    //        m_oMapGen.gen
    //   }
    //}
}
