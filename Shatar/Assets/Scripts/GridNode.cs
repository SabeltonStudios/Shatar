using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode
{
    public Vector3 pos;
    public GridNode(Vector3 p)
    {
        pos.x = p.x;
        pos.y = p.y;
        pos.z = p.z;
    }
}
