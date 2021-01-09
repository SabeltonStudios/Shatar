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
    public Vector3 nodeForward;
    public bool teletransport;
    public bool isGoal;
    public bool buttonGoal;
    public bool showAdjacencies;
    // The list of adjacent nodes/squares
    public Node[] adjacencies;
    Node[] adjacencies90;
    Node[] adjacencies180;
    Node[] adjacencies270;
    public Node[] adjacenciesOrientadas;
    public bool[] adjacencieNoAlcanzable = new bool[8];
    bool[] adjacencieNoAlcanzable90 = new bool[8];
    bool[] adjacencieNoAlcanzable180 = new bool[8];
    bool[] adjacencieNoAlcanzable270 = new bool[8];
    public bool[] adjacencieNoAlcanzableOrientada = new bool[8];
    public List<Node> seleccionables;
    public bool seleccionable = false;
    public GameObject pieza;
    public GameController gameController;
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
        /*
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);
        Handles.color = Color.red;
        Handles.Label(transform.position, this.gameObject.name);
        for (int i = 0; i < adjacencies.Length; i++)
        {
            if(adjacencies[i] == null)
            {
                break; ;
            }
        }*/
        //Gizmos.DrawLine(transform.position, adjacencies[1].transform.position);

        float scaleFactor = 4.0f;
        Gizmos.color = Color.blue;
        // Draw normal vector
        Vector3 normalVector = orientation.normalized * scaleFactor;
        DrawArrow(transform.position, transform.position + normalVector);
        // Draw forward vector
        Gizmos.color = Color.red;
        Vector3 forwardVector = nodeForward.normalized * scaleFactor;
        DrawArrow(transform.position, transform.position + forwardVector);

        Gizmos.color = Color.green;
        if (showAdjacencies)
        {
            foreach (Node adj in adjacencies)
            {
                if (adj)
                {
                    DrawArrow(transform.position, adj.transform.position);
                }
            }
        }
    }

    void DrawArrow(Vector3 pointA, Vector3 pointB)
    {
        Gizmos.DrawLine(pointA, pointB);
        Vector3 pointC = transform.position + (pointB - pointA) * 0.75f;
        Vector3 rotatingAxis = Vector3.Dot((pointB - pointA), transform.right) == 0 ? transform.right : Vector3.Dot((pointB - pointA), transform.up) == 0 ? transform.up : transform.forward;
        Vector3 cross = Vector3.Cross(rotatingAxis.normalized * 0.25f, (pointB - pointA) * 0.75f);
        Gizmos.DrawLine(pointB, pointC + cross);
        Gizmos.DrawLine(pointB, pointC - cross);
    }
    
    public void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        nodeForward = GetComponentInParent<FaceGridCreator>().forward;
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
                if (adjacencies[1] != null && !adjacencieNoAlcanzable[1])
                {
                    //Añade la casilla si es la meta y está abierta, o si otra casilla
                    if((adjacencies[1].isGoal && gameController.goalOpen) || !adjacencies[1].isGoal)
                    {
                        seleccionables.Add(adjacencies[1]);
                    }
                    
                }
                //Si es apertura, también selecciona la siguiente
                if (apertura)
                {
                    if(adjacencies[1].adjacencies[1] != null)
                    seleccionables.Add(adjacencies[1].adjacencies[1]);
                }
                if(adjacencies[0] != null)
                {
                    if (adjacencies[0].pieza != null && !adjacencieNoAlcanzable[0])
                    {
                        seleccionables.Add(adjacencies[0]);
                    }
                }
                if(adjacencies[2] != null)
                {
                    if (adjacencies[2].pieza != null && !adjacencieNoAlcanzable[2])
                    {
                        seleccionables.Add(adjacencies[2]);
                    }
                }
                break;

            case TipoPieza.ALFIL:
                //Busca las adyacencias en diagonal
                moverDiagonal(2);
                //Se elimina la casilla meta de las seleccionables, ya que solo se puede acceder con el peon
                removeSeleccionableMeta();
                break;
            case TipoPieza.TORRE:
                //Busca las adyacencias rectas
                moverRecto(5);
                //Se elimina la casilla meta de las seleccionables, ya que solo se puede acceder con el peon
                removeSeleccionableMeta();
                break;
            case TipoPieza.CABALLO:
                moverL();
                //Se elimina la casilla meta de las seleccionables, ya que solo se puede acceder con el peon
                removeSeleccionableMeta();
                break;
            case TipoPieza.REINA:
                //Busca adyacencias rectas y en diagonal
                moverDiagonal(5);
                moverRecto(5);
                //Se elimina la casilla meta de las seleccionables, ya que solo se puede acceder con el peon
                removeSeleccionableMeta();
                break;
            case TipoPieza.REY:
                //Busca adyacencias rectas y en diagonal
                moverDiagonal(1);
                moverRecto(1);
                //Se elimina la casilla meta de las seleccionables, ya que solo se puede acceder con el peon
                removeSeleccionableMeta();
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
        Node izqSuperiorAnterior = this;
        Node izqInferior = this;
        Node izqInferiorAnterior = this;
        Node derInferior = this;
        Node derInferiorAnterior = this;
        Node derSuperior = this;
        Node derSuperiorAnterior = this;
        float anguloizqSuperior = 0;
        float anguloizqInferior = 0;
        float anguloderSuperior = 0;
        float anguloderInferior = 0;

        for (int i = 0; i < casillas; i++)
        {
            //Si hay adyacencia, si es alcanzable y si tiene una casilla anterior 

            if (izqSuperior != null)
            {
                //Orientamos las caras calculada entre el nodo actual y el anterior, guardando los angulos acumulativos
                anguloizqSuperior = orientarAdjacencies(izqSuperiorAnterior, izqSuperior, anguloizqSuperior);
                if (izqSuperior.adjacenciesOrientadas[0] != null && !izqSuperior.adjacencieNoAlcanzableOrientada[0])
                {
                    izqSuperiorAnterior = izqSuperior;
                    izqSuperior = izqSuperior.adjacenciesOrientadas[0];
                    seleccionables.Add(izqSuperior);
                    
                }
                else
                {
                    izqSuperior = null;
                }
            }


            if (derSuperior != null)
            {
                //Orientamos las caras calculada entre el nodo actual y el anterior, guardando los angulos acumulativos
                anguloderSuperior = orientarAdjacencies(derSuperiorAnterior, derSuperior, anguloderSuperior);
                //Si hay adyacencia, si es alcanzable y si tiene una casilla anterior 
                if (derSuperior.adjacenciesOrientadas[2] != null && !derSuperior.adjacencieNoAlcanzableOrientada[2])
                {
                    derSuperiorAnterior = derSuperior;
                    derSuperior = derSuperior.adjacenciesOrientadas[2];
                    seleccionables.Add(derSuperior);
                    
                }
                else
                {
                    derSuperior = null;
                }
            }


            if (izqInferior != null)
            {
                //Orientamos las caras calculada entre el nodo actual y el anterior, guardando los angulos acumulativos
                anguloizqInferior = orientarAdjacencies(izqInferiorAnterior, izqInferior, anguloizqInferior);
                //Si hay adyacencia, si es alcanzable y si tiene una casilla anterior 
                if (izqInferior.adjacenciesOrientadas[5] != null && !izqInferior.adjacencieNoAlcanzableOrientada[5])
                {
                    izqInferiorAnterior = izqInferior;
                    izqInferior = izqInferior.adjacenciesOrientadas[5];
                    seleccionables.Add(izqInferior);
                    
                }
                else
                {
                    izqInferior = null;
                }
            }

            if (derInferior != null)
            {
                //Orientamos las caras calculada entre el nodo actual y el anterior, guardando los angulos acumulativos
                anguloderInferior = orientarAdjacencies(derInferiorAnterior, derInferior, anguloderInferior);
                //Si hay adyacencia, si es alcanzable y si tiene una casilla anterior 
                if (derInferior.adjacenciesOrientadas[7] != null && !derInferior.adjacencieNoAlcanzableOrientada[7])
                {
                    derInferiorAnterior = derInferior;
                    derInferior = derInferior.adjacenciesOrientadas[7];
                    seleccionables.Add(derInferior);
                    
                }
                else
                {
                    derInferior = null;
                }
            }

        }
    }

    private void moverRecto(int casillas)
    {
        
        Node superior = this;
        Node superiorAnterior = this;
        Node inferior = this;
        Node inferiorAnterior = this;
        Node derecha = this;
        Node derechaAnterior = this;
        Node izquierda = this;
        Node izquierdaAnterior = this;
        float anguloSuperior = 0;
        float anguloInferior = 0;
        float anguloDerecha = 0;
        float anguloIzquierda = 0;
        for (int i = 0; i < casillas; i++)
        {
            if (superior != null)
            {
                //Orientamos las caras calculada entre el nodo actual y el anterior, guardando los angulos acumulativos
                anguloSuperior = orientarAdjacencies(superiorAnterior, superior, anguloSuperior);
                //Si hay adyacencia, si es alcanzable y si tiene una casilla anterior 
                if (superior.adjacenciesOrientadas[1] != null && !superior.adjacencieNoAlcanzableOrientada[1])
                {
                    superiorAnterior = superior;
                    superior = superior.adjacenciesOrientadas[1];
                    seleccionables.Add(superior);
                    
                }
                else
                {
                    superior = null;
                }
            }
            
            if (derecha != null)
            {
                //Orientamos las caras calculada entre el nodo actual y el anterior, guardando los angulos acumulativos
                anguloDerecha = orientarAdjacencies(derechaAnterior, derecha, anguloDerecha);
                if (derecha.adjacenciesOrientadas[4] != null && !derecha.adjacencieNoAlcanzableOrientada[4])
                {
                    derechaAnterior = derecha;
                    derecha = derecha.adjacenciesOrientadas[4];
                    seleccionables.Add(derecha);
                }
                else
                {
                    derecha = null;
                }
            }

            if (inferior != null)
            {
                //Orientamos las caras calculada entre el nodo actual y el anterior, guardando los angulos acumulativos
                anguloInferior = orientarAdjacencies(inferiorAnterior, inferior, anguloInferior);
                //Si hay adyacencia, si es alcanzable y si tiene una casilla anterior 
                if (inferior.adjacenciesOrientadas[6] != null && !inferior.adjacencieNoAlcanzableOrientada[6])
                {
                    inferiorAnterior = inferior;
                    inferior = inferior.adjacenciesOrientadas[6];
                    seleccionables.Add(inferior);
                }
                else
                {
                    inferior = null;
                }
            }

            if (izquierda != null)
            {
                //Orientamos las caras calculada entre el nodo actual y el anterior, guardando los angulos acumulativos
                anguloIzquierda = orientarAdjacencies(izquierdaAnterior, izquierda, anguloIzquierda);
                if (izquierda.adjacenciesOrientadas[3] != null && !izquierda.adjacencieNoAlcanzableOrientada[3])
                {
                    izquierdaAnterior = izquierda;
                    izquierda = izquierda.adjacenciesOrientadas[3];
                    seleccionables.Add(izquierda);
                }
                else
                {
                    izquierda = null;
                }
            }

        }
    }

    private void moverL()
    {
        Node superior = this;
        Node superiorAnterior = this;
        Node inferior = this;
        Node inferiorAnterior = this;
        Node derecha = this;
        Node derechaAnterior = this;
        Node izquierda = this;
        Node izquierdaAnterior = this;
        float anguloSuperior = 0;
        float anguloInferior = 0;
        float anguloDerecha = 0;
        float anguloIzquierda = 0;
        //Hacia delante
        adjacenciesOrientadas = adjacencies;
        for (int i = 0; i < 2; i++)
        {
            if (superior!= null)
            {
                if (superior.adjacenciesOrientadas[1] != null)
                {
                    superiorAnterior = superior;
                    superior = superior.adjacenciesOrientadas[1];
                    //Orientamos las caras calculada entre el nodo actual y el anterior, guardando los angulos acumulativos
                    anguloSuperior = orientarAdjacencies(superiorAnterior, superior, anguloSuperior);
                }
                else
                {
                    //Si no hay adyacencia, se pone a null para dejar de comprobar por ese lado
                    superior = null;
                    break;
                }
            }
        }
        //Si el nodo no es null, almacenamos las casillas seleccionables de la izquierda y derecha
        if (superior != null)
        {
            if (superior.adjacenciesOrientadas[3] != null) { seleccionables.Add(superior.adjacenciesOrientadas[3]); }
            if (superior.adjacenciesOrientadas[4] != null) { seleccionables.Add(superior.adjacenciesOrientadas[4]); }
        }


        //Hacia detras
        adjacenciesOrientadas = adjacencies;
        for (int i = 0; i < 2; i++)
        {
            if (inferior != null)
            {
                if (inferior.adjacenciesOrientadas[6] != null)
                {
                    inferiorAnterior = inferior;
                    inferior = inferior.adjacenciesOrientadas[6];
                    //Orientamos las caras calculada entre el nodo actual y el anterior, guardando los angulos acumulativos
                    anguloInferior = orientarAdjacencies(inferiorAnterior, inferior, anguloInferior);
                }
                else
                {
                    //Si no hay adyacencia, se pone a null para dejar de comprobar por ese lado
                    inferior = null;
                    break;
                }
            }
        }
        //Si el nodo no es null, almacenamos las casillas seleccionables de la izquierda y derecha
        if (inferior != null)
        {
            if (inferior.adjacenciesOrientadas[3] != null){seleccionables.Add(inferior.adjacenciesOrientadas[3]);}
            if (inferior.adjacenciesOrientadas[4] != null){seleccionables.Add(inferior.adjacenciesOrientadas[4]);}
        }


        //Hacia derecha
        adjacenciesOrientadas = adjacencies;
        for (int i = 0; i < 2; i++)
        {
            if(derecha!= null)
            {
                if (derecha.adjacenciesOrientadas[4] != null)
                {
                    derechaAnterior = derecha;
                    derecha = derecha.adjacenciesOrientadas[4];
                    //Orientamos las caras calculada entre el nodo actual y el anterior, guardando los angulos acumulativos
                    anguloDerecha = orientarAdjacencies(derechaAnterior, derecha, anguloDerecha);
                }
                else
                {
                    //Si no hay adyacencia, se pone a null para dejar de comprobar por ese lado
                    derecha = null;
                    break;
                }
            }
        }
        //Si el nodo no es null, almacenamos las casillas seleccionables de arriba y abajo
        if (derecha!= null)
        {
            if (derecha.adjacenciesOrientadas[1] != null) { seleccionables.Add(derecha.adjacenciesOrientadas[1]); }
            if (derecha.adjacenciesOrientadas[6] != null) { seleccionables.Add(derecha.adjacenciesOrientadas[6]); }
        }

        //Hacia izquierda
        adjacenciesOrientadas = adjacencies;
        for (int i = 0; i < 2; i++)
        {
            if(izquierda!= null)
            {
                if (izquierda.adjacenciesOrientadas[4] != null)
                {
                    izquierdaAnterior = izquierda;
                    izquierda = izquierda.adjacenciesOrientadas[3];
                    //Orientamos las caras calculada entre el nodo actual y el anterior, guardando los angulos acumulativos
                    anguloIzquierda = orientarAdjacencies(izquierdaAnterior, izquierda, anguloIzquierda);
                }
                else
                {
                    //Si no hay adyacencia, se pone a null para dejar de comprobar por ese lado
                    izquierda = null;
                    break;
                }
            }
        }
        //Si el nodo no es null, almacenamos las casillas seleccionables de arriba y abajo
        if (izquierda != null)
        {
            if (izquierda.adjacenciesOrientadas[1] != null) { seleccionables.Add(izquierda.adjacenciesOrientadas[1]); }
            if (izquierda.adjacenciesOrientadas[6] != null) { seleccionables.Add(izquierda.adjacenciesOrientadas[6]); }
        }

        
        //Recorriendo una casilla hacia delante y dos a los lados
        //Hacia delante
        
        superior = this;
        adjacenciesOrientadas = adjacencies;
        if (superior.adjacenciesOrientadas[1] != null)
        {
            anguloSuperior = 0;
            superior = superior.adjacenciesOrientadas[1];
            //Hacia la derecha
            //Orientamos las caras calculada entre el nodo actual y el anterior, guardando los angulos acumulativos
            anguloSuperior = orientarAdjacencies(this, superior, anguloSuperior);
            //Almacenamos el nodo derecha
            Node superiorDerecha = superior.adjacenciesOrientadas[4];
            //Orientamos las caras calculada entre el nodo actual y el anterior, guardando los angulos acumulativos
            anguloSuperior = orientarAdjacencies(superior, superiorDerecha, anguloSuperior);
            //Si el nodo derecha no es null, lo añadimos como seleccionable
            if (superiorDerecha != null && superiorDerecha.adjacenciesOrientadas[4] != null)
            {
                seleccionables.Add(superiorDerecha.adjacenciesOrientadas[4]);
            }

            anguloSuperior = 0;
            //Hacia la izquierda
            //Orientamos las caras calculada entre el nodo actual y el anterior, guardando los angulos acumulativos
            anguloSuperior = orientarAdjacencies(this, superior, anguloSuperior);
            //Almacenamos el nodo izquierda
            Node superiorIzquierda = superior.adjacenciesOrientadas[3];
            //Orientamos las caras calculada entre el nodo actual y el anterior, guardando los angulos acumulativos
            anguloSuperior = orientarAdjacencies(superior, superiorIzquierda, anguloSuperior);
            //Si el nodo izquierda no es null, lo añadimos como seleccionable
            if (superiorIzquierda != null && superiorIzquierda.adjacenciesOrientadas[3] != null)
            {
                seleccionables.Add(superiorIzquierda.adjacenciesOrientadas[3]);
            }
        }

        
        inferior = this;
        //Hacia detrás
        adjacenciesOrientadas = adjacencies;
        if (inferior.adjacenciesOrientadas[6] != null)
        {
            
            anguloInferior = 0;
            inferior = inferior.adjacenciesOrientadas[6];

            //Hacia la derecha
            //Orientamos las caras calculada entre el nodo actual y el anterior, guardando los angulos acumulativos
            anguloInferior = orientarAdjacencies(this,inferior,anguloInferior);
            //Almacenamos el nodo derecha
            Node inferiorDerecha = inferior.adjacenciesOrientadas[4];
            //Orientamos las caras calculada entre el nodo actual y el anterior, guardando los angulos acumulativos
            anguloInferior = orientarAdjacencies(inferior,inferiorDerecha,anguloInferior);
            //Si el nodo derecha no es null, lo añadimos como seleccionable
            if (inferiorDerecha != null && inferiorDerecha.adjacenciesOrientadas[4] != null)
            {
                seleccionables.Add(inferiorDerecha.adjacenciesOrientadas[4]);
            }
            

            
            //Hacia la izquierda
            anguloInferior = 0;
            //Orientamos las caras calculada entre el nodo actual y el anterior, guardando los angulos acumulativos
            anguloInferior = orientarAdjacencies(this,inferior, anguloInferior);
            //Almacenamos el nodo izquierda
            Node inferiorIzquierda = inferior.adjacenciesOrientadas[3];
            //Orientamos las caras calculada entre el nodo actual y el anterior, guardando los angulos acumulativos
            anguloInferior = orientarAdjacencies(inferior, inferiorIzquierda, anguloInferior);
            //Si el nodo izquierda no es null, lo añadimos como seleccionable
            if (inferiorIzquierda != null && inferiorIzquierda.adjacenciesOrientadas[3] != null)
            {
                seleccionables.Add(inferiorIzquierda.adjacenciesOrientadas[3]);
            }
            
        }

        
        derecha = this;
        //Hacia la derecha
        adjacenciesOrientadas = adjacencies;
        if (derecha.adjacenciesOrientadas[4] != null)
        {
            anguloDerecha = 0;
            derecha = derecha.adjacenciesOrientadas[4];

            //Hacia arriba
            //Orientamos las caras calculada entre el nodo actual y el anterior, guardando los angulos acumulativos
            anguloDerecha = orientarAdjacencies(this, derecha, anguloDerecha);
            //Almacenamos el nodo superior
            Node derechaSuperior = derecha.adjacenciesOrientadas[1];
            //Orientamos las caras calculada entre el nodo actual y el anterior, guardando los angulos acumulativos
            anguloDerecha = orientarAdjacencies(derecha, derechaSuperior, anguloDerecha);
            //Si el nodo superior no es null, lo añadimos como seleccionable
            if (derechaSuperior != null && derechaSuperior.adjacenciesOrientadas[1] != null)
            {
                seleccionables.Add(derechaSuperior.adjacenciesOrientadas[1]);
            }


            anguloDerecha = 0;
            //Hacia la izquierda
            //Orientamos las caras calculada entre el nodo actual y el anterior, guardando los angulos acumulativos
            anguloDerecha = orientarAdjacencies(this, derecha, anguloDerecha);
            //Almacenamos el nodo inferior
            Node derechaInferior = derecha.adjacenciesOrientadas[6];
            //Orientamos las caras calculada entre el nodo actual y el anterior, guardando los angulos acumulativos
            anguloDerecha = orientarAdjacencies(derecha, derechaInferior, anguloDerecha);
            //Si el nodo inferior no es null, lo añadimos como seleccionable
            if (derechaInferior != null && derechaInferior.adjacenciesOrientadas[6] != null)
            {
                seleccionables.Add(derechaInferior.adjacenciesOrientadas[6]);
            }

        }
        
        izquierda = this;
        //Hacia la izquierda, comprueba si no es null
        adjacenciesOrientadas = adjacencies;
        if (izquierda.adjacenciesOrientadas[3] != null)
         {
             anguloIzquierda = 0;
             izquierda = izquierda.adjacenciesOrientadas[3];

             //Hacia arriba
             //Orientamos las caras calculada entre el nodo actual y el anterior, guardando los angulos acumulativos
             anguloIzquierda = orientarAdjacencies(this, izquierda, anguloIzquierda);
             //Almacenamos el nodo superior
             Node izquierdaSuperior = izquierda.adjacenciesOrientadas[1];
             //Orientamos las caras calculada entre el nodo actual y el anterior, guardando los angulos acumulativos
             anguloIzquierda = orientarAdjacencies(izquierda, izquierdaSuperior, anguloIzquierda);
             //Si el nodo superior no es null, lo añadimos como seleccionable
             if (izquierdaSuperior != null && izquierdaSuperior.adjacenciesOrientadas[1] != null)
             {
                 seleccionables.Add(izquierdaSuperior.adjacenciesOrientadas[1]);
             }

             anguloIzquierda = 0;
             //Hacia abajo
             //Orientamos las caras calculada entre el nodo actual y el anterior, guardando los angulos acumulativos
             anguloIzquierda = orientarAdjacencies(this, izquierda, anguloIzquierda);
             //Almacena el nodo inferior
             Node izquierdaInferior = izquierda.adjacenciesOrientadas[6];
             //Orientamos las caras calculada entre el nodo actual y el anterior, guardando los angulos acumulativos
             anguloIzquierda = orientarAdjacencies(izquierda, izquierdaInferior, anguloIzquierda);
             //Si el nodo inferior no es null, lo añadimos como seleccionable
             if (izquierdaInferior != null && izquierdaInferior.adjacenciesOrientadas[6] != null)
             {
                 seleccionables.Add(izquierdaInferior.adjacenciesOrientadas[6]);
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
        foreach (Node nodo in seleccionables)
        {
            nodo.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
            nodo.GetComponent<MeshRenderer>().material.color = Color.white;
        }
    }

    private void removeSeleccionableMeta()
    {
        //Lista de nodos repetidos meta a eliminar
        List<Node> eliminar = new List<Node>();
        if (seleccionables.Count > 0)
        {
            foreach (Node n in seleccionables)
            {
                if (n.isGoal || n.buttonGoal)
                {
                    eliminar.Add(n);
                }
            }
        }
        if (eliminar.Count > 0)
        {
            for(int i=0;i<eliminar.Count;i++)
            {
                seleccionables.Remove(eliminar[0]);
            }
        }
    }

    private float orientarAdjacencies(Node primero, Node adyacencia, float anguloInferior)
    {
        //Calcular ángulo
        if (adyacencia != null)
        {
            //Pruebas de Anto
            float diferenciaNormales = Vector3.SignedAngle(primero.orientation, adyacencia.orientation, Vector3.Cross(adyacencia.orientation, primero.orientation));
            Vector3 otherdirection = adyacencia.nodeForward;
            Vector3 fixedforward = adyacencia.nodeForward;
            fixedforward = Quaternion.AngleAxis(diferenciaNormales, Vector3.Cross(primero.orientation, adyacencia.orientation)) * fixedforward;

            float angulo = Vector3.SignedAngle(fixedforward, primero.nodeForward, primero.orientation);
            //Debug.Log(angulo);
            angulo = angulo + anguloInferior;
           
            anguloInferior = angulo;

            //Orienta las caras
            if (angulo < 5 && angulo > -5)
            {
                adyacencia.adjacenciesOrientadas = adyacencia.adjacencies;
                adyacencia.adjacencieNoAlcanzableOrientada = adyacencia.adjacencieNoAlcanzable;
            }
            else if (angulo > 85 && angulo < 95)
            {
                adyacencia.adjacenciesOrientadas = adyacencia.adjacencies90;
                adyacencia.adjacencieNoAlcanzableOrientada = adyacencia.adjacencieNoAlcanzable90;
            }
            else if (angulo > 175 && angulo < 185 || (angulo > -185 && angulo < -175))
            {
                adyacencia.adjacenciesOrientadas = adyacencia.adjacencies180;
                adyacencia.adjacencieNoAlcanzableOrientada = adyacencia.adjacencieNoAlcanzable180;

            }
            else if ((angulo > -95 && angulo < -85) || (angulo > 265 && angulo < 275))
            {
                adyacencia.adjacenciesOrientadas = adyacencia.adjacencies270;
                adyacencia.adjacencieNoAlcanzableOrientada = adyacencia.adjacencieNoAlcanzable270;
            }
            

        }
        return anguloInferior;
    }
    

    
}
