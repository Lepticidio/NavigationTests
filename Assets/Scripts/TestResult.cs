using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestResult
{
    public int m_iTestID, m_iMapID, m_iNodeMapID, m_iIterationID;
    public float m_fPathLength;
    public double m_dMapGenTime, m_dNodeGenTime, m_dPathGenTime;


    public TestResult (int _iTestID, int _iMapID, int _iNodeMapID, int _iIterationID, float _fPathLength, double _dMapGenTime, double _dNodeGenTime, double _dPathGenTime)
    {
        m_iTestID = _iTestID;
        m_iMapID = _iMapID;
        m_iNodeMapID = _iNodeMapID;
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
        sResult += m_iNodeMapID.ToString();
        sResult += ",";
        sResult += m_iIterationID.ToString();
        sResult += ",";
        sResult += m_fPathLength.ToString();
        sResult += ",";
        sResult += m_dMapGenTime.ToString();
        sResult += ",";
        sResult += m_dNodeGenTime.ToString();
        sResult += ",";
        sResult += m_dPathGenTime.ToString();

        return sResult;
    }
}
