using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemie : MonoBehaviour
{
    public Node node;
    Node previousNode;
    public TipoPieza tipoPieza;
    public TipoPieza playerChange;
    public bool apertura = false;
    Color colorSeleccionable = new Color(237.0f/255.0f, 33.0f/255.0f, 115.0f/255.0f, 1);
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

        if(this.gameController.name=="Enemy0")
            Debug.Log(ID % nodesMovimiento.Count);
        //Debug.Log("Enemigo pintando adyacencias");
        turno = true;
        if (!undo)
        {
            ID++;
            node.DrawAdjacencies(tipoPieza, apertura, colorSeleccionable);
            //Poner a null la pieza del nodo
            previousNode = node;
            node.pieza = null;
            if (gameController.vallaSubidaHorse && meAfectaVallaHorse)
            {
                if (move)
                {
                    node = nodesVallas[ID % nodesVallas.Count];
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
            
            foreach (Node n in previousNode.seleccionables)
            {
                if (n.pieza != null && n.pieza.tag == "Player")
                {
                    TipoPieza tipoPieza = n.pieza.GetComponent<Player>().tipoPieza;
                    if (tipoPieza != TipoPieza.PEON)
                    {
                        node = n;
                    }
                }
            }
            if (nodesPath.Count > 0)
            {
                for(int i = 0; i < nodesIntermedios; i++)
                {
                    if (nodesPath[(ID * nodesIntermedios + i) % nodesPath.Count].pieza != null && nodesPath[(ID*nodesIntermedios+i) % nodesPath.Count].pieza.tag == "Player")
                    {
                        node = nodesPath[(ID * nodesIntermedios + i) % nodesPath.Count];
                    }
                }
            }
            StartCoroutine(gameController.MoveOverSeconds(this.gameObject, node, 1, false, previousNode, false));

        }
        else
        {
            node.pieza = null;
            ID--;
            //node = nodesMovimiento[Mathf.Abs(ID) % nodesMovimiento.Count];
            node = previousNode;
            StartCoroutine(gameController.MoveOverSeconds(this.gameObject, node, 1, false, previousNode, false));
        }
    }

}
