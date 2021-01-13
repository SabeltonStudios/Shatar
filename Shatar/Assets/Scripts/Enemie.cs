using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//clase empleada para la gestión de los enemigos
public class Enemie : MonoBehaviour
{
    #region Variables
    //Referencia al nodo actual que ocupa la pieza
    public Node node;
    //Referencia a los nodos anteriores, para cuando el jugador deshace turnos
    public Node[] previousNodes = new Node[3];
    //Tipo de pieza del enemigo
    public TipoPieza tipoPieza;
    //Tipo de pieza que desbloqueamos en el jugador 
    public TipoPieza playerChange;
    //Los enemigos nunca hacen movimiento de apertura al no ser peones
    public bool apertura = false;
    //Color para pintar nuestras posibles adyacencias
    Color colorSeleccionable = new Color(237.0f / 255.0f, 33.0f / 255.0f, 115.0f / 255.0f, 1);
    //Índice empleado para la gestión de movimiento de la pieza
    public int ID = -1;
    //Booleanos para indicar si es nuestro turno o no, así como si nos vemos afectados por vallas que bloquean nuestro movimiento
    public bool turno = false;
    public bool meAfectaVallaHorse;
    public bool meAfectaVallaCastle;
    //Listados con los nodos de movimiento, los nodos intermedios a estos y los nodos de movimiento cuando se alzan vallas que nos afectan
    public List<Node> nodesMovimiento;
    public List<Node> nodesPath;
    public List<Node> nodesVallas;
    //Booleano para permitir o no el movimiento de la pieza
    public bool move;
    //Número de nodos intermedios y referencia al GameController
    public int nodesIntermedios;
    GameController gameController;
    #endregion
    // Start is called before the first frame update
    //Al inicio indicamos a nuestro nodo que somos su pieza, cogemos el gamecontroller y habilitamos el movimiento
    void Start()
    {
        node.pieza = this.gameObject;
        gameController = FindObjectOfType<GameController>();
        move = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void MoveTo(bool undo)
    {
        //Marcamos que empezamos el turno, por lo que el jugador no se puede mover
        turno = true;
        if (!undo)//Si no nos movemos por deshacer del jugador
        {
            //Incrementamos el id, pintamos las adyacencias y desplazamos los nodos anteriores a la derecha
            ID++;
            node.DrawAdjacencies(tipoPieza, apertura, colorSeleccionable);
            //Poner a null la pieza del nodo
            shiftPreviousNodes(false);
            //Limpiamos la pieza del nodo que abandonamos
            node.pieza = null;
            //Si alguna valla está subida y me afecta, actualizo mi nodo de movimiento y deshabilito el movimiento 
            if (gameController.vallaSubidaHorse && meAfectaVallaHorse)
            {
                if (move)
                {
                    node = nodesVallas[ID % nodesVallas.Count];
                    move = false;
                }
            }
            else if (gameController.vallaSubidaCastle && meAfectaVallaCastle)
            {
                if (move)
                {
                    move = false;
                }
            }
            else//si no, lo habilito y actualizo el nodo con los normales de movimiento
            {
                if (nodesMovimiento.Count > 0)
                {
                    node = nodesMovimiento[ID % nodesMovimiento.Count];
                    move = true;
                }
            }
            //Para cada nodo de los seleccionables del nodo anterior
            foreach (Node n in previousNodes[0].seleccionables)
            {
                //Si hay una pieza que es el jugador
                if (n.pieza != null && n.pieza.tag == "Player")
                {
                    TipoPieza tipoPieza = n.pieza.GetComponent<Player>().tipoPieza;
                    //Y no está usando el peón, que pasa desapercibido
                    if (tipoPieza != TipoPieza.PEON)
                    {
                        
                        node = n;
                        //Comprobamos que ese nodo esté entre los seleccionables
                        if (!previousNodes[0].seleccionables.Contains(node))
                        {
                            node = previousNodes[0];
                        }
                    }
                }
            }
            //Si tengo nodos intermedios
            if (nodesPath.Count > 0)
            {
                //Para cada uno de ellos, si hay una pieza y es el jugador
                for (int i = 0; i < nodesIntermedios; i++)
                {
                    if (nodesPath[(ID * nodesIntermedios + i) % nodesPath.Count].pieza != null && nodesPath[(ID * nodesIntermedios + i) % nodesPath.Count].pieza.tag == "Player")
                    {
                        //Si no estoy afectado por las vallas y/o no ewstán subidas
                        if (!meAfectaVallaCastle
                                || !meAfectaVallaHorse
                                || (meAfectaVallaCastle && !gameController.vallaSubidaCastle)
                                || (meAfectaVallaHorse && !gameController.vallaSubidaHorse))
                        {
                            //Cojo como nodo en el que se encuentra el jugador, mientras esté entre los seleccionables del que abandono
                            node = nodesPath[(ID * nodesIntermedios + i) % nodesPath.Count];
                            if (!previousNodes[0].seleccionables.Contains(node))
                            {
                                node = previousNodes[0];
                            }

                        }
                    }
                }
            }
            else//Si no hay nodos intermedios como en el caso del alfil o el caballo
            {
                //Si no están subidas las vallas y no me afectan, habilito el movimiento
                if(!gameController.vallaSubidaCastle && meAfectaVallaCastle)
                {
                    move = true;
                }else if(!gameController.vallaSubidaHorse && meAfectaVallaHorse)
                {
                    move = true;
                }
            }
            //Y finalmente llamo a la corrutina de movimiento
            StartCoroutine(gameController.MoveOverSeconds(this.gameObject, node, 1, false, previousNodes[0], false));

        }
        else//Si nos movemos por deshacer del jugador, limpiamos nuestro nodo, decrementamos el índice de movimiento, 
            //hacemos la corrutina de movimiento, gaurdamos como nodo el primero del array y desplazamos el mismo hacia la izquierda
        {
            node.pieza = null;
            ID--;
            
            StartCoroutine(gameController.MoveOverSeconds(this.gameObject, previousNodes[0], 1, false, node, false));
            node = previousNodes[0];
            shiftPreviousNodes(true);
            
        }
    }
    //Método empleado para el desplazamiento en una dirección u otra del array que contiene nuestros nodos anteriores
    //Según nos movamos por deshacer del jugador o por paso de turno
    public void shiftPreviousNodes(bool left)
    {
        if (left)
        {
            for (int i = 0; i < (gameController.player.maxUndos - gameController.player.undoCont); i++)
            {
                previousNodes[i] = previousNodes[i + 1];

            }
        }
        else
        {
            for (int i = 3 - 1; i > 0; i--)
            {
                previousNodes[i] = previousNodes[i - 1];
               
            }
            previousNodes[0] = node;

        }
    }
}
