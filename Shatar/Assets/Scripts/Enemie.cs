using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemie : MonoBehaviour
{
    public Node node;
    public TipoPieza tipoPieza;
    public bool apertura = false;
    Color colorSeleccionable = new Color(1, 0.8f, 0.8f, 1);
    public int ID = 0;
    public bool turno = false;
    public List<Node> nodesMovimiento;

    // Start is called before the first frame update
    void Start()
    {
        node.pieza = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void MoveTo()
    {
        node.DrawAdjacencies(tipoPieza, apertura, colorSeleccionable);
        //Poner a null la pieza del nodo
        node.pieza = null;

        //Mover la pieza
        node = nodesMovimiento[ID%nodesMovimiento.Count];
        transform.position = node.transform.position;
        transform.up = node.orientation;
        node.pieza = this.gameObject;
        turno = false;

        //Poner cuenta atrás
    }

}
