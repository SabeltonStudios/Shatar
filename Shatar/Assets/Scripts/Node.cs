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
    public List<Node> seleccionables;
    public bool seleccionable = false;
    public GameObject pieza;

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
        for(int i = 0; i < adjacencies.Length; i++)
        {
            if(adjacencies[i] == null)
            {
                return;
            }
        }
        Gizmos.DrawLine(transform.position, adjacencies[1].transform.position);
    }

    public void DrawAdjacencies(TipoPieza pieza, bool apertura, Color color)
    {
        seleccionables = new List<Node>();
        Node superior = this;
        Node inferior = this;
        Node derecha = this;
        Node izquierda = this;
        Node izqSuperior = this;
        Node izqInferior = this;
        Node derInferior = this;
        Node derSuperior = this;
        switch (pieza)
        {
            case TipoPieza.PEON:
                //Busca la casilla de delante
                seleccionables.Add(adjacencies[1]);
                //Si es apertura, también selecciona la siguiente
                if (apertura)
                {
                    seleccionables.Add(adjacencies[1].adjacencies[1]);
                }
                break;

            case TipoPieza.ALFIL:
                //Busca las adyacencias en diagonal
                for(int i = 0; i < 2; i++)
                {
                    izqSuperior = izqSuperior.adjacencies[0];
                    seleccionables.Add(izqSuperior);

                    derSuperior = derSuperior.adjacencies[2];
                    seleccionables.Add(derSuperior);

                    izqInferior = izqInferior.adjacencies[5];
                    seleccionables.Add(izqInferior);

                    derInferior = derInferior.adjacencies[7];
                    seleccionables.Add(derInferior);
                }
                break;

            case TipoPieza.TORRE:
                //Busca las adyacencias rectas
                for (int i = 0; i < 2; i++)
                {
                    superior = superior.adjacencies[1];
                    seleccionables.Add(superior);

                    derecha = derecha.adjacencies[4];
                    seleccionables.Add(derecha);

                    inferior = inferior.adjacencies[6];
                    seleccionables.Add(inferior);

                    izquierda = izquierda.adjacencies[3];
                    seleccionables.Add(izquierda);
                    
                }
                break;
            case TipoPieza.CABALLO:
                //Hacia delante
                for (int i = 0; i < 3; i++)
                {
                    superior = superior.adjacencies[1];
                    
                }
                seleccionables.Add(superior.adjacencies[3]);
                seleccionables.Add(superior.adjacencies[4]);

                //Hacia detras
                for (int i = 0; i < 3; i++)
                {
                    inferior = inferior.adjacencies[6];

                }
                seleccionables.Add(inferior.adjacencies[3]);
                seleccionables.Add(inferior.adjacencies[4]);

                //Hacia derecha
                for (int i = 0; i < 3; i++)
                {
                    derecha = derecha.adjacencies[4];

                }
                seleccionables.Add(derecha.adjacencies[3]);
                seleccionables.Add(derecha.adjacencies[4]);

                //Hacia izquierda
                for (int i = 0; i < 3; i++)
                {
                    izquierda = izquierda.adjacencies[3];

                }
                seleccionables.Add(izquierda.adjacencies[3]);
                seleccionables.Add(izquierda.adjacencies[4]);
                break;

            case TipoPieza.REINA:
                //Busca adyacencias rectas y en diagonal
                for (int i = 0; i < 2; i++)
                {
                    superior = superior.adjacencies[1];
                    seleccionables.Add(superior);

                    derecha = derecha.adjacencies[4];
                    seleccionables.Add(derecha);

                    inferior = inferior.adjacencies[6];
                    seleccionables.Add(inferior);

                    izquierda = izquierda.adjacencies[3];
                    seleccionables.Add(izquierda);

                    izqSuperior = izqSuperior.adjacencies[0];
                    seleccionables.Add(izqSuperior);

                    derSuperior = derSuperior.adjacencies[2];
                    seleccionables.Add(derSuperior);

                    izqInferior = izqInferior.adjacencies[5];
                    seleccionables.Add(izqInferior);

                    derInferior = derInferior.adjacencies[7];
                    seleccionables.Add(derInferior);
                }
                break;
            case TipoPieza.REY:
                //Busca adyacencias rectas y en diagonal
                for (int i = 0; i < 1; i++)
                {
                    superior = superior.adjacencies[1];
                    seleccionables.Add(superior);

                    derecha = derecha.adjacencies[4];
                    seleccionables.Add(derecha);

                    inferior = inferior.adjacencies[6];
                    seleccionables.Add(inferior);

                    izquierda = izquierda.adjacencies[3];
                    seleccionables.Add(izquierda);

                    izqSuperior = izqSuperior.adjacencies[0];
                    seleccionables.Add(izqSuperior);

                    derSuperior = derSuperior.adjacencies[2];
                    seleccionables.Add(derSuperior);

                    izqInferior = izqInferior.adjacencies[5];
                    seleccionables.Add(izqInferior);

                    derInferior = derInferior.adjacencies[7];
                    seleccionables.Add(derInferior);
                }
                break;
            default:
                break;
        }
        //Pone las casillas seleccionables a true y cambia el color
        setColor(color);
    }

    private void setColor(Color color)
    {
        foreach (Node n in seleccionables)
        {
            n.GetComponent<MeshRenderer>().material.color = color;
            n.seleccionable = true;
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
