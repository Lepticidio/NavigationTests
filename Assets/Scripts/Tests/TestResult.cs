using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestResult
{
    public string m_sSpaceRepresentationName;
    public int m_iTestID, m_iMapID, m_iSpaceRepresentationID, m_iIterationID, m_iNumberNodes, m_iNumberConnections, m_iNumberObstacles, m_iMapSize;
    public float m_fPathLength;
    public double m_dMapGenTime, m_dNodeGenTime, m_dPathGenTime, m_dTestTime, m_dTotalTime;


    public TestResult (int _iTestID, int _iMapID, int _iSpaceRepresentationID, int _iIterationID, float _fPathLength, double _dMapGenTime, double _dNodeGenTime, double _dPathGenTime)
    {
        m_iTestID = _iTestID;
        m_iMapID = _iMapID;
        m_iSpaceRepresentationID = _iSpaceRepresentationID;
        m_iIterationID = _iIterationID;
        m_fPathLength = _fPathLength;
        m_dMapGenTime = _dMapGenTime;
        m_dNodeGenTime = _dNodeGenTime;
        m_dPathGenTime = _dPathGenTime;
    }

    public override string ToString()
    {
        string sResult = "\n";
        sResult += m_iTestID.ToString();
        sResult += ",";
        sResult += m_iMapID.ToString();
        sResult += ",";
        sResult += "Map Size";
        sResult += ",";
        sResult += m_iMapSize.ToString();
        sResult += ",";
        sResult += "Obstacles";
        sResult += ",";
        sResult += m_iNumberObstacles.ToString();
        sResult += ",";
        sResult += m_sSpaceRepresentationName;
        sResult += ",";
        sResult += m_iSpaceRepresentationID.ToString();
        sResult += ",";
        sResult += m_iIterationID.ToString();
        sResult += ",";
        sResult += "Nodes";
        sResult += ",";
        sResult += m_iNumberNodes.ToString();
        sResult += ",";
        sResult += "Connections";
        sResult += ",";
        sResult += m_iNumberConnections.ToString();
        sResult += ",";
        sResult += "Path Length";
        sResult += ",";
        sResult += m_fPathLength.ToString();
        sResult += ",";
        sResult += m_dMapGenTime.ToString();
        sResult += ",";
        sResult += "Repr Time";
        sResult += ",";
        sResult += m_dNodeGenTime.ToString();
        sResult += ",";
        sResult += "Path Time";
        sResult += ",";
        sResult += m_dPathGenTime.ToString();
        sResult += ",";
        sResult += "Test Time";
        sResult += ",";
        sResult += (m_dTestTime/1000).ToString();
        sResult += ",";
        sResult += "Total Time";
        sResult += ",";
        sResult += (m_dTotalTime/1000).ToString();

        return sResult;
    }
}
