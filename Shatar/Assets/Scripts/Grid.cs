using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private float size = 1f;
    [SerializeField]
    private int gridSize = 3;
    private List<GridNode> GridNodes = new List<GridNode>();
    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {
        Vector3 res;
        float nearD = float.MaxValue;
        GridNode aux= null;
        foreach(GridNode n in GridNodes)
        {
            if (Vector3.Distance(position, n.pos) < nearD)
            {
                nearD = Vector3.Distance(position, n.pos);
                aux = n;
            }
        }
        res = aux.pos;
        return res;
        /*
        //position -= transform.position;
        Debug.Log(position.x + " " + position.y + " " + position.z);
        float xCount =(float) Mathf.RoundToInt(position.x / size);
        if (position.x -(int) position.x == 0.5f) { xCount += 1; }

        if (!(position.x < size / 2 || position.x > gridSize - size - size / 2)) {
            if (position.x > xCount) { xCount += 0.5f; }
            else { xCount -= 0.5f; } }

        float yCount = (float)Mathf.RoundToInt(position.y / size);
        if (position.y - (int)position.y == 0.5f) { yCount += 1; }

        if (!(position.y < size / 2 || position.y > gridSize - size - size / 2))
        { if (position.y > yCount) { yCount += 0.5f; } else { yCount -= 0.5f; } }

        float zCount = (float)Mathf.RoundToInt(position.z / size);
        if (position.z - (int)position.z == 0.5f) { zCount += 1; }

        if (!(position.z < size / 2 || position.z > gridSize - size - size / 2))
        { if (position.z > zCount) { zCount += 0.5f; } else { zCount -= 0.5f; } }

        Debug.Log(xCount + "" + yCount + "" + zCount);
        Vector3 result = new Vector3(
            (float)xCount * size,
            (float)yCount * size,
            (float)zCount * size);

        //result += transform.position;
        return result;*/
    }
    public Vector3 CreatePoints(Vector3 position)
    {
        position -= transform.position;
        Vector3 result = new Vector3(
            (float)position.x * size,
            (float)position.y * size,
            (float)position.z * size);

        result += transform.position;
        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (size < gridSize && size > 0)
        {
            for (float x = 0; x < gridSize- size / 2; x += size/2 )
            {
                for (float z = 0; z < gridSize - size / 2; z += size/2 )
                {
                    for (float y = 0; y < gridSize - size / 2; y += size / 2)
                    {
                        if (((z == 0 || z==gridSize-size) && (x % size == size / 2)&& (y % size == size / 2))
                            || ((x == 0 || x == gridSize - size) && (z % size == size / 2) && (y % size == size / 2))
                            || ((y == 0 || y == gridSize - size) && (x % size == size / 2) && (z % size == size / 2)))
                        {
                            var p = CreatePoints(new Vector3(x, y, z));
                            GridNode point = new GridNode(p);
                            GridNodes.Add(point);
                            Gizmos.DrawSphere(p, 0.1f);
                        }
                    }
                }
            }
        }
    }
}
