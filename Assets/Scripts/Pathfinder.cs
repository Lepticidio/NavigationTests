using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pathfinder : MonoBehaviour
{
    public abstract List<Vector3> GetPath(Vector3 _vStart, Vector3 _vEnd, NodeMap _oMap);
    public List<Vector3> m_tCenters = new List<Vector3>();
}
