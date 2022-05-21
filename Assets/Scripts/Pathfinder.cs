using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pathfinder : MonoBehaviour
{
    public bool m_bPathFound = false;
    public abstract IEnumerator GetPath (Vector3 _vStart, Vector3 _vEnd, NodeMap _oMap);
    public List<Vector3> m_tCenters = new List<Vector3>();
    public List<Vector3> m_tPath = new List<Vector3>();

    public void OptimizePath(ref List<Vector3> _tPath)
    {
        LayerMask iLayerMask = ~(LayerMask.GetMask("Agent") | LayerMask.GetMask("Goal"));
        for (int i = 0; i < _tPath.Count; i++)
        {
            bool bRemoveInBetween = false;
            for (int j = _tPath.Count - 1; j > i; j--)
            {
                if (!bRemoveInBetween)
                {
                    Vector3 vDir = _tPath[j] - _tPath[i];
                    RaycastHit oInfo;
                    bool bCollision = Physics.SphereCast(_tPath[i], 0.5f, vDir, out oInfo, vDir.magnitude, iLayerMask);

                    if (!bCollision)
                    {
                        bRemoveInBetween = true;
                    }
                }
                else
                {
                    _tPath.RemoveAt(j);
                }
            }
        }
    }
}
