using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pathfinder : MonoBehaviour
{
    public abstract List<Vector3> GetPath(Vector3 _vStart, Vector3 _vEnd, NodeMap _oMap);
    public List<Vector3> m_tCenters = new List<Vector3>();

    public void OptimizePath(ref List<Vector3> _tPath)
    {
        LayerMask iLayerMask = ~(LayerMask.GetMask("Agent") | LayerMask.GetMask("Goal"));
        for (int i = 0; i < _tPath.Count; i++)
        {
            Debug.Log("tPath i " + i.ToString() + " at " + _tPath[i]);
            bool bRemoveInBetween = false;
            for (int j = _tPath.Count - 1; j > i; j--)
            {
                Debug.Log("tPath j " + j.ToString() + " at " + _tPath[j]);
                if (!bRemoveInBetween)
                {
                    Vector3 vDir = _tPath[j] - _tPath[i];
                    RaycastHit oInfo;
                    bool bCollision = Physics.SphereCast(_tPath[i], 0.5f, vDir, out oInfo, vDir.magnitude, iLayerMask);

                    if (!bCollision)
                    {
                        Debug.Log("No collsion between i at" + _tPath[i] + " and j at " + _tPath[j] + " going to remove");
                        bRemoveInBetween = true;
                    }
                }
                else
                {
                    Debug.Log("Removing " + j.ToString() + " at " + _tPath[j]);
                    _tPath.RemoveAt(j);
                }
            }
        }
    }
}
