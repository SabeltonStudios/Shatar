using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    public Node node;
    public Stack<Turno> previousTurnos = new Stack<Turno>();

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
            Turno previousTurno = previousTurnos.Pop();
            gameController.turnoPrevious = previousTurno;
            if (!previousTurno.cambioPieza)
            {
                node.UndrawAdjacencies();
                node.pieza = null;
                
                if (previousTurno.enemieEat != null)
                {
                    previousTurno.enemieEat.SetActive(true);
                    gameController.enemigos.Clear();
                    Enemie[] aux = FindObjectsOfType<Enemie>();
                    for (int i = 0; i < aux.Length; i++)
                    {
                        gameController.enemigos.Add(aux[i]);
                    }
                }
                if (previousTurno.goalOpened)
                {
                    gameController.goalOpen = false;
                    gameController.goal.Play("Close");
                }
                
                
                //MIRAR SI EL SHIFT ES ANTERIOR O POSTERIOR A MOVER
                StartCoroutine(gameController.MoveOverSeconds(this.gameObject, previousTurno.previousNode, 1, true, node, true));
                node = previousTurno.previousNode;
                node.pieza = this.gameObject;
            }
            else
            {
                //Debug.Log(previousPieces[0]);
                gameController.cambiaPieza(previousTurno.previousPieza, true);
            }
            gameController.movCastleButton = previousTurno.vallaCastleID;
            gameController.movHorseButton = previousTurno.vallaHorseID;
            gameController.updateButtonCastle(true);
            gameController.updateButtonHorse(true);
            
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

        if(nearD <= distancia && move)
        {
            numMovs++;
            //Eliminar casillas seleccionables anteriores
            node.UndrawAdjacencies();
            node.pieza = null;
            
            StartCoroutine(gameController.MoveOverSeconds(this.gameObject, aux, 1, true, node, false));
            //Añadir el nuevo nodo, mover hacia él
            node = aux;
            if (node.GetComponent<MessageTrigger>())
            {
                node.GetComponent<MessageTrigger>().showMessages();
            }
        }
    }
}
