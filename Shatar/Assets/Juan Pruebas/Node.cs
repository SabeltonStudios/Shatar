using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    // The normal vector to the node/square
    public Vector3 orientation;

    // The list of adjacent nodes/squares
    public Node[] adjacencies;
    /*
    The adjacencies will be represented like shown:
    ^ [0][1][2] ^
    | [3][-][4] |
    | [5][6][7] |
    where [-] represents this node/square
    */

    public void InitializeAdjacencies()
    {
        adjacencies = new Node[8];
    }
}
