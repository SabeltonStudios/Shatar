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
    public bool isGoal;
    // The list of adjacent nodes/squares
    public Node[] adjacencies;
    public Node[] adjacencies90;
    public Node[] adjacencies180;
    public Node[] adjacencies270;
    public Node[] adjacenciesOrientadas;
    public bool[] adjacencieNoAlcanzable = new bool[8];
    public bool[] adjacencieNoAlcanzable90 = new bool[8];
    public bool[] adjacencieNoAlcanzable180 = new bool[8];
    public bool[] adjacencieNoAlcanzable270 = new bool[8];
    public bool[] adjacencieNoAlcanzableOrientada = new bool[8];
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
    public void Start()
    {
        adjacencies90 = new Node[8];
        adjacencies180 = new Node[8];
        adjacencies270 = new Node[8];
        adjacenciesOrientadas = adjacencies;

        adjacencies90[0] = adjacencies[2];
        adjacencies90[1] = adjacencies[4];
        adjacencies90[2] = adjacencies[7];
        adjacencies90[3] = adjacencies[1];
        adjacencies90[4] = adjacencies[6];
        adjacencies90[5] = adjacencies[0];
        adjacencies90[6] = adjacencies[3];
        adjacencies90[7] = adjacencies[5];

        adjacencies180[0] = adjacencies[7];
        adjacencies180[1] = adjacencies[6];
        adjacencies180[2] = adjacencies[5];
        adjacencies180[3] = adjacencies[4];
        adjacencies180[4] = adjacencies[3];
        adjacencies180[5] = adjacencies[2];
        adjacencies180[6] = adjacencies[1];
        adjacencies180[7] = adjacencies[0];

        adjacencies270[0] = adjacencies[5];
        adjacencies270[1] = adjacencies[3];
        adjacencies270[2] = adjacencies[0];
        adjacencies270[3] = adjacencies[6];
        adjacencies270[4] = adjacencies[1];
        adjacencies270[5] = adjacencies[7];
        adjacencies270[6] = adjacencies[4];
        adjacencies270[7] = adjacencies[2];

        adjacencieNoAlcanzableOrientada = adjacencieNoAlcanzable;

        adjacencieNoAlcanzable90[0] = adjacencieNoAlcanzable[2];
        adjacencieNoAlcanzable90[1] = adjacencieNoAlcanzable[4];
        adjacencieNoAlcanzable90[2] = adjacencieNoAlcanzable[7];
        adjacencieNoAlcanzable90[3] = adjacencieNoAlcanzable[1];
        adjacencieNoAlcanzable90[4] = adjacencieNoAlcanzable[6];
        adjacencieNoAlcanzable90[5] = adjacencieNoAlcanzable[0];
        adjacencieNoAlcanzable90[6] = adjacencieNoAlcanzable[3];
        adjacencieNoAlcanzable90[7] = adjacencieNoAlcanzable[5];

        adjacencieNoAlcanzable180[0] = adjacencieNoAlcanzable[7];
        adjacencieNoAlcanzable180[1] = adjacencieNoAlcanzable[6];
        adjacencieNoAlcanzable180[2] = adjacencieNoAlcanzable[5];
        adjacencieNoAlcanzable180[3] = adjacencieNoAlcanzable[4];
        adjacencieNoAlcanzable180[4] = adjacencieNoAlcanzable[3];
        adjacencieNoAlcanzable180[5] = adjacencieNoAlcanzable[2];
        adjacencieNoAlcanzable180[6] = adjacencieNoAlcanzable[1];
        adjacencieNoAlcanzable180[7] = adjacencieNoAlcanzable[0];

        adjacencieNoAlcanzable270[0] = adjacencieNoAlcanzable[5];
        adjacencieNoAlcanzable270[1] = adjacencieNoAlcanzable[3];
        adjacencieNoAlcanzable270[2] = adjacencieNoAlcanzable[0];
        adjacencieNoAlcanzable270[3] = adjacencieNoAlcanzable[6];
        adjacencieNoAlcanzable270[4] = adjacencieNoAlcanzable[1];
        adjacencieNoAlcanzable270[5] = adjacencieNoAlcanzable[7];
        adjacencieNoAlcanzable270[6] = adjacencieNoAlcanzable[4];
        adjacencieNoAlcanzable270[7] = adjacencieNoAlcanzable[2];


    }
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
            orientarAdjacencies(izqSuperior);
            if (adjacencies[0] != null && !adjacencieNoAlcanzableOrientada[0] && izqSuperior != null)
            {
                izqSuperior = izqSuperior.adjacencies[0];
                seleccionables.Add(izqSuperior);
            }
            else
            {
                izqSuperior = null;
            }

            orientarAdjacencies(derSuperior);
            //Si hay adyacencia, si es alcanzable y si tiene una casilla anterior 
            if (adjacencies[2] != null && !adjacencieNoAlcanzableOrientada[2] && derSuperior != null)
            {
                derSuperior = derSuperior.adjacencies[2];
                seleccionables.Add(derSuperior);
            }
            else
            {
                derSuperior = null;
            }

            orientarAdjacencies(izqInferior);
            //Si hay adyacencia, si es alcanzable y si tiene una casilla anterior 
            if (adjacencies[5] != null && !adjacencieNoAlcanzableOrientada[5] && izqInferior != null)
            {
                izqInferior = izqInferior.adjacencies[5];
                seleccionables.Add(izqInferior);
            }
            else
            {
                izqInferior = null;
            }

            orientarAdjacencies(derInferior);
            //Si hay adyacencia, si es alcanzable y si tiene una casilla anterior 
            if (adjacencies[7] != null && !adjacencieNoAlcanzableOrientada[7] && derInferior != null)
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
            orientarAdjacencies(superior);
            //Si hay adyacencia, si es alcanzable y si tiene una casilla anterior 
            if (adjacencies[1] != null && !adjacencieNoAlcanzableOrientada[1] && superior != null)
            {
                superior = superior.adjacencies[1];
                seleccionables.Add(superior);
            }
            else
            {
                superior = null;
            }

            orientarAdjacencies(derecha);
            //Si hay adyacencia, si es alcanzable y si tiene una casilla anterior 
            if (adjacencies[4] != null && !adjacencieNoAlcanzableOrientada[4] && derecha != null)
            {
                derecha = derecha.adjacencies[4];
                seleccionables.Add(derecha);
            }
            else
            {
                derecha = null;
            }

            orientarAdjacencies(inferior);
            //Si hay adyacencia, si es alcanzable y si tiene una casilla anterior 
            if (adjacencies[6] != null && !adjacencieNoAlcanzableOrientada[6] && inferior != null)
            {
                inferior = inferior.adjacencies[6];
                seleccionables.Add(inferior);

            }
            else
            {
                inferior = null;
            }

            orientarAdjacencies(izquierda);
            if (adjacencies[3] != null && !adjacencieNoAlcanzableOrientada[3] && izquierda != null)
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
            orientarAdjacencies(superior);
            if (superior.adjacenciesOrientadas[1] != null && superior != null)
            {
                superior = superior.adjacenciesOrientadas[1];
            }
            else
            {
                superior = null;
                break;
            }
        }
        orientarAdjacencies(superior);
        if (superior != null)
        {
            if (superior.adjacenciesOrientadas[3] != null) { seleccionables.Add(superior.adjacenciesOrientadas[3]); }
            if (superior.adjacenciesOrientadas[4] != null) { seleccionables.Add(superior.adjacenciesOrientadas[4]); }
        }


        //Hacia detras
        for (int i = 0; i < 3; i++)
        {
            orientarAdjacencies(inferior);
            if (inferior.adjacenciesOrientadas[6] != null && inferior != null)
            {
                inferior = inferior.adjacenciesOrientadas[6];
            }
            else
            {
                inferior = null;
                break;
            }
        }
        orientarAdjacencies(inferior);
        if (inferior != null)
        {
            if (inferior.adjacenciesOrientadas[3] != null){seleccionables.Add(inferior.adjacenciesOrientadas[3]);}
            if (inferior.adjacenciesOrientadas[4] != null){seleccionables.Add(inferior.adjacenciesOrientadas[4]);}
        }
        

        //Hacia derecha
        for (int i = 0; i < 3; i++)
        {
            orientarAdjacencies(derecha);
            if (derecha.adjacenciesOrientadas[4] != null && derecha != null)
            {
                derecha = derecha.adjacenciesOrientadas[4];
            }
            else
            {
                derecha = null;
                break;
            }
        }
        orientarAdjacencies(derecha);
        if (derecha.adjacenciesOrientadas[3] != null) { seleccionables.Add(derecha.adjacenciesOrientadas[3]); }
        if (derecha.adjacenciesOrientadas[4] != null) { seleccionables.Add(derecha.adjacenciesOrientadas[4]); }

        //Hacia izquierda
        for (int i = 0; i < 3; i++)
        {
            orientarAdjacencies(izquierda);
            if (derecha.adjacenciesOrientadas[4] != null && derecha != null)
            {
                izquierda = izquierda.adjacenciesOrientadas[3];
            }
            else
            {
                izquierda = null;
                break;
            }
        }
        orientarAdjacencies(izquierda);
        if (izquierda.adjacenciesOrientadas[3] != null){ seleccionables.Add(izquierda.adjacenciesOrientadas[3]); }
        if (izquierda.adjacenciesOrientadas[4] != null) { seleccionables.Add(izquierda.adjacenciesOrientadas[4]); }

        //Recorriendo una casilla hacia delante y dos a los lados
        //Hacia delante
        superior = this;
        if (superior.adjacenciesOrientadas[1] != null)
        {
            superior = superior.adjacenciesOrientadas[1];
            //Hacia la derecha
            orientarAdjacencies(superior);
            Node superiorDerecha = superior.adjacenciesOrientadas[4];
            orientarAdjacencies(superiorDerecha);
            if(superiorDerecha!= null && superiorDerecha.adjacenciesOrientadas[4] != null)
            {
                seleccionables.Add(superiorDerecha.adjacenciesOrientadas[4]);
            }

            //Hacia la izquierda
            orientarAdjacencies(superior);
            Node superiorIzquierda = superior.adjacenciesOrientadas[3];
            orientarAdjacencies(superiorIzquierda);
            if (superiorIzquierda != null && superiorIzquierda.adjacenciesOrientadas[3] != null)
            {
                seleccionables.Add(superiorIzquierda.adjacenciesOrientadas[3]);
            }
        }

        inferior = this;
        //Hacia detrás
        if (inferior.adjacenciesOrientadas[6] != null)
        {
            inferior = inferior.adjacenciesOrientadas[6];
            //Hacia la derecha
            orientarAdjacencies(inferior);
            Node inferiorDerecha = inferior.adjacenciesOrientadas[4];
            orientarAdjacencies(inferior);
            if (inferiorDerecha != null && inferiorDerecha.adjacenciesOrientadas[4] != null)
            {
                seleccionables.Add(inferiorDerecha.adjacenciesOrientadas[4]);
            }

            //Hacia la izquierda
            orientarAdjacencies(inferior);
            Node inferiorIzquierda = superior.adjacenciesOrientadas[3];
            orientarAdjacencies(inferiorIzquierda);
            if (inferiorIzquierda != null && inferiorIzquierda.adjacenciesOrientadas[3] != null)
            {
                seleccionables.Add(inferiorIzquierda.adjacenciesOrientadas[3]);
            }
        }

        derecha = this;
        //Hacia la derecha
        if (derecha.adjacencies[4] != null)
        {
            derecha = derecha.adjacencies[4];
            //Hacia arriba
            orientarAdjacencies(derecha);
            Node derechaSuperior = derecha.adjacencies[1];
            orientarAdjacencies(derechaSuperior);
            if (derechaSuperior != null && derechaSuperior.adjacencies[1] != null)
            {
                seleccionables.Add(derechaSuperior.adjacencies[1]);
            }

            //Hacia la izquierda
            orientarAdjacencies(derecha);
            Node derechaInferior = derecha.adjacencies[6];
            orientarAdjacencies(derechaInferior);
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
            orientarAdjacencies(izquierda);
            Node izquierdaSuperior = izquierda.adjacencies[1];
            orientarAdjacencies(izquierdaSuperior);
            if (izquierdaSuperior != null && izquierdaSuperior.adjacencies[1] != null)
            {
                seleccionables.Add(izquierdaSuperior.adjacencies[1]);
            }

            //Hacia la izquierda
            orientarAdjacencies(izquierda);
            Node izquierdaInferior = izquierda.adjacencies[6];
            orientarAdjacencies(izquierdaInferior);
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

    private void orientarAdjacencies(Node adyacencia)
    {
        //Calcular ángulo
        
       
        if(adyacencia!= null)
        {
            int angulo = 0;
            Transform transformAdy = Instantiate(adyacencia.transform, adyacencia.transform.position, adyacencia.transform.rotation);
            Vector3 diferenciaNormal = this.orientation - adyacencia.orientation;
            if (!adyacencia.orientation.Equals(this.orientation))
            {
                Debug.Log("no es igual");
            }
            transformAdy.rotation = Quaternion.Euler(adyacencia.orientation + diferenciaNormal);
            float calculoAngulo = Vector3.Angle(this.transform.forward, transformAdy.forward);
            angulo = (int)calculoAngulo;
            if (calculoAngulo > 0)
            {
                Debug.Log("orientando");
            }
            switch (angulo)
            {
                case 0:
                    adjacenciesOrientadas = adjacencies;
                    adjacencieNoAlcanzableOrientada = adjacencieNoAlcanzable;
                    break;
                case 89:
                    adjacenciesOrientadas = adjacencies90;
                    adjacencieNoAlcanzableOrientada = adjacencieNoAlcanzable90;
                    break;
                case 180:
                    adjacenciesOrientadas = adjacencies180;
                    adjacencieNoAlcanzableOrientada = adjacencieNoAlcanzable180;
                    break;
                case 270:
                    adjacenciesOrientadas = adjacencies270;
                    adjacencieNoAlcanzableOrientada = adjacencieNoAlcanzable270;
                    break;
                default:
                    break;

            }
        }
    }
}
