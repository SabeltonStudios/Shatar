using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    public Node node;
    Node[] previousNodes= new Node[3];
    //int undoID;
    public int undoCont;
    public int maxUndos=3;
    public int numMovs = 0;
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
        
        //undoID = 0;
        undoCont = 0;
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
                    numMovs++;
                    MoveTo(hitInfo.point);
                }
            }

            //Mirar por eventos
            
        }
        /*if (Input.GetKeyDown(KeyCode.Z))
        {
            UndoMovement();
        }*/
    }
    public void UndoMovement()
    {
        undoCont++;
        Debug.Log(previousNodes[0]);
        if (undoCont <= maxUndos && previousNodes[0]!=null && turno)
        {
            numMovs--;

            node.UndrawAdjacencies();
            node.pieza = null;

            //Añadir el nuevo nodo, mover hacia él
            node = previousNodes[0];
            //se le pasa true para despachar a la izquierda, false para la derecha, como al moverse
            shiftPreviousNodes(true);
            //MIRAR SI EL SHIFT ES ANTERIOR O POSTERIOR A MOVER
            StartCoroutine(gameController.MoveOverSeconds(this.gameObject, node, 1, true, previousNodes[0],true));
        }
    }
    private void shiftPreviousNodes(bool left)
    {
        if (left)
        {
            for(int i= 0; i<(maxUndos-undoCont); i++)
            {
                previousNodes[i] = previousNodes[i + 1];
            }
        }
        else
        {
            for(int i=maxUndos-1; i > 0; i--)
            {
                previousNodes[i] = previousNodes[i - 1];
            }
            previousNodes[0] = node;
            
        }
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
            
            //Eliminar casillas seleccionables anteriores
            node.UndrawAdjacencies();
            node.pieza = null;
            shiftPreviousNodes(false);
            
            

            //Añadir el nuevo nodo, mover hacia él
            node = aux;
            //transform.position = node.transform.position;
            StartCoroutine(gameController.MoveOverSeconds(this.gameObject, node, 1, true, previousNodes[0], false));
        }
        
    }
}
