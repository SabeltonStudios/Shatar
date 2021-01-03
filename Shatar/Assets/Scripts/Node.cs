using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    public bool[] adjacencieNoAlcanzable = new bool[8];
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
    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);
        Handles.color = Color.red;
        Handles.Label(transform.position, this.gameObject.name);
        for (int i = 0; i < adjacencies.Length; i++)
        {
            if(adjacencies[i] == null)
            {
                return;
            }
        }
        //Gizmos.DrawLine(transform.position, adjacencies[1].transform.position);
    }
    */
    public void DrawAdjacencies(TipoPieza pieza, bool apertura, Color color)
    {
        seleccionables = new List<Node>();
        switch (pieza)
        {
            case TipoPieza.PEON:
                //Busca la casilla de delante
                if(adjacencies[1] != null && !adjacencieNoAlcanzable[1])
                {
                    seleccionables.Add(adjacencies[1]);
                }
                //Si es apertura, también selecciona la siguiente
                if (apertura)
                {
                    seleccionables.Add(adjacencies[1].adjacencies[1]);
                }
                if (adjacencies[0].pieza != null && adjacencies[0] != null && !adjacencieNoAlcanzable[0])
                {
                    seleccionables.Add(adjacencies[0]);
                }
                if(adjacencies[2].pieza != null && adjacencies[2] != null && !adjacencieNoAlcanzable[2])
                {
                    seleccionables.Add(adjacencies[2]);
                }
                break;

            case TipoPieza.ALFIL:
                //Busca las adyacencias en diagonal
                moverDiagonal(2);
                break;

            case TipoPieza.TORRE:
                //Busca las adyacencias rectas
                moverRecto(2);
                break;
            case TipoPieza.CABALLO:
                moverL();
                break;

            case TipoPieza.REINA:
                //Busca adyacencias rectas y en diagonal
                moverDiagonal(5);
                moverRecto(5);
                break;
            case TipoPieza.REY:
                //Busca adyacencias rectas y en diagonal
                moverDiagonal(1);
                moverRecto(1);
                break;
            default:
                break;
        }
        //Pone las casillas seleccionables a true y cambia el color
        setColor(color);
    }

    private void moverDiagonal(int casillas)
    {
        
        Node izqSuperior = this;
        Node izqInferior = this;
        Node derInferior = this;
        Node derSuperior = this;
        for (int i = 0; i < casillas; i++)
        {


            //Si hay adyacencia, si es alcanzable y si tiene una casilla anterior 
            if (adjacencies[0] != null && !adjacencieNoAlcanzable[0] && izqSuperior != null)
            {
                izqSuperior = izqSuperior.adjacencies[0];
                seleccionables.Add(izqSuperior);
            }
            else
            {
                izqSuperior = null;
            }


            //Si hay adyacencia, si es alcanzable y si tiene una casilla anterior 
            if (adjacencies[2] != null && !adjacencieNoAlcanzable[2] && derSuperior != null)
            {
                derSuperior = derSuperior.adjacencies[2];
                seleccionables.Add(derSuperior);
            }
            else
            {
                derSuperior = null;
            }

            //Si hay adyacencia, si es alcanzable y si tiene una casilla anterior 
            if (adjacencies[5] != null && !adjacencieNoAlcanzable[5] && izqInferior != null)
            {
                izqInferior = izqInferior.adjacencies[5];
                seleccionables.Add(izqInferior);
            }
            else
            {
                izqInferior = null;
            }

            //Si hay adyacencia, si es alcanzable y si tiene una casilla anterior 
            if (adjacencies[7] != null && !adjacencieNoAlcanzable[7] && derInferior != null)
            {
                derInferior = derInferior.adjacencies[7];
                seleccionables.Add(derInferior);
            }
            else
            {
                derInferior = null;
            }
        }
    }

    private void moverRecto(int casillas)
    {
        Node superior = this;
        Node inferior = this;
        Node derecha = this;
        Node izquierda = this;
        for(int i = 0; i < casillas; i++)
        {
            //Si hay adyacencia, si es alcanzable y si tiene una casilla anterior 
            if (adjacencies[1] != null && !adjacencieNoAlcanzable[1] && superior != null)
            {
                superior = superior.adjacencies[1];
                seleccionables.Add(superior);
            }
            else
            {
                superior = null;
            }

            //Si hay adyacencia, si es alcanzable y si tiene una casilla anterior 
            if (adjacencies[4] != null && !adjacencieNoAlcanzable[4] && derecha != null)
            {
                derecha = derecha.adjacencies[4];
                seleccionables.Add(derecha);
            }
            else
            {
                derecha = null;
            }

            //Si hay adyacencia, si es alcanzable y si tiene una casilla anterior 
            if (adjacencies[6] != null && !adjacencieNoAlcanzable[6] && inferior != null)
            {
                inferior = inferior.adjacencies[6];
                seleccionables.Add(inferior);

            }
            else
            {
                inferior = null;
            }

            if (adjacencies[3] != null && !adjacencieNoAlcanzable[3] && inferior != null)
            {
                izquierda = izquierda.adjacencies[3];
                seleccionables.Add(izquierda);
            }
            else
            {
                izquierda = null;
            }
        }
    }

    private void moverL()
    {
        Node superior = this;
        Node inferior = this;
        Node derecha = this;
        Node izquierda = this;
        //Hacia delante
        for (int i = 0; i < 3; i++)
        {
            if (superior.adjacencies[1] != null && superior != null)
            {
                superior = superior.adjacencies[1];
            }
            else
            {
                superior = null;
                break;
            }
        }
        if (superior != null)
        {
            if (superior.adjacencies[3] != null) { seleccionables.Add(superior.adjacencies[3]); }
            if (superior.adjacencies[4] != null) { seleccionables.Add(superior.adjacencies[4]); }
        }


        //Hacia detras
        for (int i = 0; i < 3; i++)
        {
            if (inferior.adjacencies[6] != null && inferior != null)
            {
                inferior = inferior.adjacencies[6];
            }
            else
            {
                inferior = null;
                break;
            }
        }
        if(inferior != null)
        {
            if (inferior.adjacencies[3] != null){seleccionables.Add(inferior.adjacencies[3]);}
            if (inferior.adjacencies[4] != null){seleccionables.Add(inferior.adjacencies[4]);}
        }
        

        //Hacia derecha
        for (int i = 0; i < 3; i++)
        {
            if (derecha.adjacencies[4] != null && derecha != null)
            {
                derecha = derecha.adjacencies[4];
            }
            else
            {
                derecha = null;
                break;
            }
        }
        if (derecha.adjacencies[3] != null) { seleccionables.Add(derecha.adjacencies[3]); }
        if (derecha.adjacencies[4] != null) { seleccionables.Add(derecha.adjacencies[4]); }

        //Hacia izquierda
        for (int i = 0; i < 3; i++)
        {
            if (derecha.adjacencies[4] != null && derecha != null)
            {
                izquierda = izquierda.adjacencies[3];
            }
            else
            {
                izquierda = null;
                break;
            }
        }
        if (izquierda.adjacencies[3] != null){ seleccionables.Add(izquierda.adjacencies[3]); }
        if (izquierda.adjacencies[4] != null) { seleccionables.Add(izquierda.adjacencies[4]); }

        //Recorriendo una casilla hacia delante y dos a los lados
        //Hacia delante
        superior = this;
        if (superior.adjacencies[1] != null)
        {
            superior = superior.adjacencies[1];
            //Hacia la derecha
            Node superiorDerecha = superior.adjacencies[4];
            if(superiorDerecha!= null && superiorDerecha.adjacencies[4] != null)
            {
                seleccionables.Add(superiorDerecha.adjacencies[4]);
            }

            //Hacia la izquierda
            Node superiorIzquierda = superior.adjacencies[3];
            if (superiorIzquierda != null && superiorIzquierda.adjacencies[3] != null)
            {
                seleccionables.Add(superiorIzquierda.adjacencies[3]);
            }
        }

        inferior = this;
        //Hacia detrás
        if (inferior.adjacencies[6] != null)
        {
            inferior = inferior.adjacencies[6];
            //Hacia la derecha
            Node inferiorDerecha = inferior.adjacencies[4];
            if (inferiorDerecha != null && inferiorDerecha.adjacencies[4] != null)
            {
                seleccionables.Add(inferiorDerecha.adjacencies[4]);
            }

            //Hacia la izquierda
            Node inferiorIzquierda = superior.adjacencies[3];
            if (inferiorIzquierda != null && inferiorIzquierda.adjacencies[3] != null)
            {
                seleccionables.Add(inferiorIzquierda.adjacencies[3]);
            }
        }

        derecha = this;
        //Hacia la derecha
        if (derecha.adjacencies[4] != null)
        {
            derecha = derecha.adjacencies[4];
            //Hacia arriba
            Node derechaSuperior = derecha.adjacencies[1];
            if (derechaSuperior != null && derechaSuperior.adjacencies[1] != null)
            {
                seleccionables.Add(derechaSuperior.adjacencies[1]);
            }

            //Hacia la izquierda
            Node derechaInferior = derecha.adjacencies[6];
            if (derechaInferior != null && derechaInferior.adjacencies[6] != null)
            {
                seleccionables.Add(derechaInferior.adjacencies[6]);
            }
        }

        izquierda = this;
        //Hacia la izquierda
        if (izquierda.adjacencies[3] != null)
        {
            izquierda = izquierda.adjacencies[3];
            //Hacia arriba
            Node izquierdaSuperior = izquierda.adjacencies[1];
            if (izquierdaSuperior != null && izquierdaSuperior.adjacencies[1] != null)
            {
                seleccionables.Add(izquierdaSuperior.adjacencies[1]);
            }

            //Hacia la izquierda
            Node izquierdaInferior = izquierda.adjacencies[6];
            if (izquierdaInferior != null && izquierdaInferior.adjacencies[6] != null)
            {
                seleccionables.Add(izquierdaInferior.adjacencies[6]);
            }
        }
    }
    private void setColor(Color color)
    {
        foreach (Node n in seleccionables)
        {
            //n.GetComponent<MeshRenderer>().material.color = color;
            n.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
            n.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", color);
            n.GetComponent<MeshRenderer>().material.color = color;
            n.seleccionable = true;
        }
    }
    public void UndrawAdjacencies()
    {
        foreach(Node nodo in seleccionables)
        {
            nodo.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
            nodo.GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }
}
