using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemie : MonoBehaviour
{
    public Node node;
    public Node[] previousNodes = new Node[3];
    public TipoPieza tipoPieza;
    public TipoPieza playerChange;
    public bool apertura = false;
    Color colorSeleccionable = new Color(237.0f / 255.0f, 33.0f / 255.0f, 115.0f / 255.0f, 1);
    public int ID = -1;
    public bool turno = false;
    public bool meAfectaVallaHorse;
    public bool meAfectaVallaCastle;
    public List<Node> nodesMovimiento;
    public List<Node> nodesPath;
    public List<Node> nodesVallas;
    public bool move;
    public int nodesIntermedios;
    GameController gameController;

    // Start is called before the first frame update
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
        if (this.gameController.name == "Enemy0")
            Debug.Log(ID % nodesMovimiento.Count);
        //Debug.Log("Enemigo pintando adyacencias");
        turno = true;
        if (!undo)
        {
            ID++;
            node.DrawAdjacencies(tipoPieza, apertura, colorSeleccionable);
            //Poner a null la pieza del nodo
            shiftPreviousNodes(false);

            node.pieza = null;
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
            else
            {
                if (nodesMovimiento.Count > 0)
                {
                    node = nodesMovimiento[ID % nodesMovimiento.Count];
                    move = true;
                }
            }
            foreach (Node n in previousNodes[0].seleccionables)
            {
                if (n.pieza != null && n.pieza.tag == "Player")
                {
                    TipoPieza tipoPieza = n.pieza.GetComponent<Player>().tipoPieza;
                    if (tipoPieza != TipoPieza.PEON)
                    {
                        node = n;
                        if (!previousNodes[0].seleccionables.Contains(node))
                        {
                            node = previousNodes[0];
                        }
                    }
                }
            }
            if (nodesPath.Count > 0)
            {
                for (int i = 0; i < nodesIntermedios; i++)
                {
                    if (nodesPath[(ID * nodesIntermedios + i) % nodesPath.Count].pieza != null && nodesPath[(ID * nodesIntermedios + i) % nodesPath.Count].pieza.tag == "Player")
                    {
                        if (!meAfectaVallaCastle
                                || !meAfectaVallaHorse
                                || (meAfectaVallaCastle && !gameController.vallaSubidaCastle)
                                || (meAfectaVallaHorse && !gameController.vallaSubidaHorse))
                        {
                            node = nodesPath[(ID * nodesIntermedios + i) % nodesPath.Count];
                            if (!previousNodes[0].seleccionables.Contains(node))
                            {
                                node = previousNodes[0];
                            }

                        }
                    }
                }
            }
            else
            {
                if(!gameController.vallaSubidaCastle && meAfectaVallaCastle)
                {
                    move = true;
                }else if(!gameController.vallaSubidaHorse && meAfectaVallaHorse)
                {
                    move = true;
                }
            }
            
            StartCoroutine(gameController.MoveOverSeconds(this.gameObject, node, 1, false, previousNodes[0], false));

        }
        else
        {
            node.pieza = null;
            ID--;
            //node = nodesMovimiento[Mathf.Abs(ID) % nodesMovimiento.Count];
            
            StartCoroutine(gameController.MoveOverSeconds(this.gameObject, previousNodes[0], 1, false, node, false));
            node = previousNodes[0];
            shiftPreviousNodes(true);
            
        }
    }

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
        //Debug.Log(actions[0] + " " + actions[1] + " " + actions[2] + " ");
    }
}
