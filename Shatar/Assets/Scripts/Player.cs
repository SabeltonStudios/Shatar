using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public Node node;
    Node previousNode;
    int numMovs = 0;
    [SerializeField]
    public TipoPieza tipoPieza;

    [SerializeField]
    public bool apertura;
    public float distancia;
    public bool turno;

    public Color colorSeleccionable = new Color(0, 1, 0, 1);
    GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        previousNode = node;
        node.pieza = this.gameObject;
        node.DrawAdjacencies(tipoPieza,apertura, colorSeleccionable);
        apertura = false;
        turno = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (turno)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hitInfo;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hitInfo))
                {
                    MoveTo(hitInfo.point);
                }
            }

            //Mirar por eventos
            if (Input.GetKeyDown(KeyCode.Z))
            {
                UndoMovement();
            }
        }
    }
    private void UndoMovement()
    {
        Debug.Log("Deshacer movimiento");
        numMovs++;
        node.UndrawAdjacencies();
        node.pieza = null;

        //Añadir el nuevo nodo, mover hacia él
        node = previousNode;
        StartCoroutine(gameController.MoveOverSeconds(this.gameObject, node, 1, true, previousNode));
    }
    private void MoveTo(Vector3 point)
    {
        float nearD = float.MaxValue;
        Vector3 startPosition = transform.position;
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
            numMovs++;
            Debug.Log("Número de movimientos: "+numMovs);
            //Eliminar casillas seleccionables anteriores
            node.UndrawAdjacencies();
            node.pieza = null;
            previousNode = node;

            //Añadir el nuevo nodo, mover hacia él
            node = aux;
            //transform.position = node.transform.position;
            StartCoroutine(gameController.MoveOverSeconds(this.gameObject, node, 1, true, previousNode));
        }
    }
    
}
