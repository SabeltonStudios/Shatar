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
    public int ID = 0;
    public bool turno = false;
    public List<Node> nodesMovimiento;
    public List<Node> nodesPath;
    GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        node.pieza = this.gameObject;
        gameController = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void MoveTo(bool undo)
    {
        //Debug.Log("Enemigo pintando adyacencias");
        turno = true;
        if (!undo)
        {
            node.DrawAdjacencies(tipoPieza, apertura, colorSeleccionable);
            //Poner a null la pieza del nodo
            previousNode = node;
            node.pieza = null;
            if(nodesPath[ID % nodesPath.Count].pieza!=null && nodesPath[ID % nodesPath.Count].pieza.tag == "Player")
            {
                node = nodesPath[ID % nodesPath.Count];
            }
            else
            {
                node = nodesMovimiento[ID % nodesMovimiento.Count];
            }
            
            StartCoroutine(gameController.MoveOverSeconds(this.gameObject, node, 1, false, previousNode, false));
            ID++;
        }
        else
        {
            node.pieza = null;
            ID--;
            node = previousNode;
            StartCoroutine(gameController.MoveOverSeconds(this.gameObject, node, 1, false, previousNode, false));
        }
    }

}
