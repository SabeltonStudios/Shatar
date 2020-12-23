using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TipoPieza
{
    PEON,
    TORRE,
    ALFIL,
    CABALLO,
    REINA,
    REY
}

public class Node : MonoBehaviour
{
    // The normal vector to the node/square
    public Vector3 orientation;

    // The list of adjacent nodes/squares
    public Node[] adjacencies;
    Color colorSeleccionable = new Color(0.75f, 1, 0, 1);
    public List<Node> seleccionables;
    public bool seleccionable = false;

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);
    }

    public void DrawAdjacencies(TipoPieza pieza, bool apertura)
    {
        switch (pieza)
        {
            case TipoPieza.PEON:
                seleccionables = new List<Node>();
                seleccionables.Add(adjacencies[1]);
                adjacencies[1].seleccionable = true;
                if (apertura)
                {
                    adjacencies[1].GetComponent<MeshRenderer>().material.color = colorSeleccionable;
                    adjacencies[1].adjacencies[1].GetComponent<MeshRenderer>().material.color = colorSeleccionable;
                    seleccionables.Add(adjacencies[1].adjacencies[1]);
                    adjacencies[1].adjacencies[1].seleccionable = true;
                }
                else
                {
                    adjacencies[1].GetComponent<MeshRenderer>().material.color = colorSeleccionable;
                }
                
                break;
            case TipoPieza.ALFIL:
                break;
            case TipoPieza.TORRE:
                break;
            case TipoPieza.CABALLO:
                break;
            case TipoPieza.REINA:
                break;
            case TipoPieza.REY:
                break;
            default:
                break;
        }
    }

    public void UndrawAdjacencies()
    {
        foreach(Node nodo in seleccionables)
        {
            nodo.GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }
}
