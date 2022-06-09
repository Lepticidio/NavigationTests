using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Misc
{
    public static bool CheckOverTerrain(Vector3 _vPosition)
    {
        int iTerrainMask = LayerMask.GetMask("Terrain");
        return Physics.Raycast(_vPosition, Vector3.down, Mathf.Infinity, iTerrainMask) ;

    }
}
