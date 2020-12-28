using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public Node node;

    [SerializeField]
    public TipoPieza tipoPieza;

    [SerializeField]
    public bool apertura;
    public float distancia = 10;
    public bool turno;

    public Color colorSeleccionable = new Color(0.75f, 1, 0, 1);
    GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        node.pieza = this.gameObject;
        node.DrawAdjacencies(tipoPieza,apertura, colorSeleccionable);
        apertura = false;
        turno = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && turno)
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo))
            {
                MoveTo(hitInfo.point);
            }
        }

        //Mirar por eventos
        if (turno)
        {
           
            
        }
    }

    private void MoveTo(Vector3 point)
    {
        float nearD = float.MaxValue;

        Node aux = null;
        foreach (Node n in node.seleccionables)
        {
            if (Vector3.Distance(point, n.transform.position) < nearD)
            {
                nearD = Vector3.Distance(point, n.transform.position);
                aux = n;
            }
        }

        if(nearD <= distancia)
        {
            //Eliminar casillas seleccionables anteriores
            node.UndrawAdjacencies();
            node.pieza = null;

            //Añadir el nuevo nodo, mover hacia él
            node = aux;
            transform.position = node.transform.position;
            transform.up = node.orientation;
            node.pieza = this.gameObject;
            gameController.EnemigosTurno();
        }
    }

}
