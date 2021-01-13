using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    public Node node;
    public Node[] previousNodes= new Node[3];
    TipoPieza[] previousPieces= new TipoPieza[3];
    bool[] actions= new bool[3];
    public GameObject[] enemiesEat = new GameObject[3];
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
    public bool move;

    public Color colorSeleccionable;
    GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        previousPieces[0] = tipoPieza;
        //undoID = 0;
        undoCont = 0;
        node.pieza = this.gameObject;
        node.DrawAdjacencies(tipoPieza,apertura, colorSeleccionable);
        apertura = false;
        //turno = true;
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

                if (Physics.Raycast(ray, out hitInfo) && !GameUIManager.menuOpened)
                {
                    MoveTo(hitInfo.point);
                }
            }
        }
    }

    public void UndoMovement()
    {
        undoCont++;

        if (undoCont <= maxUndos && numMovs>0 && turno)
        {
            numMovs--;
            if(numMovs == 0)
            {
                apertura = true;
            }
            if (!actions[0])
            {
                node.UndrawAdjacencies();
                node.pieza = null;
                
                if (enemiesEat[0] != null)
                {
                    enemiesEat[0].SetActive(true);
                    gameController.enemigos.Clear();
                    Enemie[] aux = FindObjectsOfType<Enemie>();
                    for (int i = 0; i < aux.Length; i++)
                    {
                        gameController.enemigos.Add(aux[i]);
                    }
                }

                //Añadir el nuevo nodo, mover hacia él
                node = previousNodes[0];
                //se le pasa true para despachar a la izquierda, false para la derecha, como al moverse, y si hace shift de piezas o posiciones
                shiftPreviousNodes(true, false);
                //MIRAR SI EL SHIFT ES ANTERIOR O POSTERIOR A MOVER
                StartCoroutine(gameController.MoveOverSeconds(this.gameObject, node, 1, true, previousNodes[0], true));
            }
            else
            {
                //Debug.Log(previousPieces[0]);
                gameController.cambiaPieza(previousPieces[0], true);
            }
            
            
        }
    }
    public void shiftPreviousNodes(bool left, bool pieces)
    {
        if (left)
        {
            for(int i= 0; i<(maxUndos-undoCont); i++)
            {
                actions[i] = actions[i + 1];
                enemiesEat[i] = enemiesEat[i + 1];

                if (!pieces)
                {
                    previousNodes[i] = previousNodes[i + 1];
                }
                else
                {
                    
                    previousPieces[i] = previousPieces[i + 1];
                }
            }
        }
        else
        {
            for(int i=maxUndos-1; i > 0; i--)
            {
                actions[i] = actions[i - 1];
                enemiesEat[i] = enemiesEat[i - 1];

                if (pieces)
                {
                    previousPieces[i] = previousPieces[i - 1];
                    
                }
                else
                {
                    previousNodes[i] = previousNodes[i - 1];
                }
            }
            if (pieces)
            {
                previousPieces[0] = tipoPieza;
                actions[0] = true;
            }
            else
            {
                previousNodes[0] = node;
                enemiesEat[0] = null;
                actions[0] = false;
            }
            
        }
        //Debug.Log(actions[0] + " " + actions[1] + " " + actions[2] + " ");
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

        if(nearD <= distancia && move)
        {
            numMovs++;
            //Eliminar casillas seleccionables anteriores
            node.UndrawAdjacencies();
            node.pieza = null;
            shiftPreviousNodes(false, false);
            
            //Añadir el nuevo nodo, mover hacia él
            node = aux;
            //transform.position = node.transform.position;
            StartCoroutine(gameController.MoveOverSeconds(this.gameObject, node, 1, true, previousNodes[0], false));
            if (node.GetComponent<MessageTrigger>())
            {
                node.GetComponent<MessageTrigger>().showMessages();
            }
        }
    }
}
