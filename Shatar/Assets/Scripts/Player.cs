using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Método empleado para el manejo del jugador
public class Player : MonoBehaviour
{
    //referencia al nodo que ocupamos y pila con los turnos anteriores para deshacer
    [SerializeField]
    public Node node;
    public Stack<Turno> previousTurnos = new Stack<Turno>();

    //int undoID;
    //Enteros para contabilizar el número de deshacer y turnos, y referencia a nuestro tipo de pieza
    public int undoCont;
    public int maxUndos = 3;
    public int numMovs = 0;
    [SerializeField]
    public TipoPieza tipoPieza;
    //Bool para indicar si es movimiento de apertura
    [SerializeField]
    public bool apertura;
    //Float empleado como distancia mínima para considerar que el jugador ha clicado en una casilla
    public float distancia;
    //booleanos para indicar si es nuestro turno y si podemos movernos
    public bool turno;
    public bool move;
    //Color emnpleado para pintar nuestras casillas seleccionables y referencia al gamecontroller
    public Color colorSeleccionable;
    GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        //Seteo del nodo de que somos su pieza y pintado de adyacencias
        gameController = FindObjectOfType<GameController>();
        //undoID = 0;
        undoCont = 0;
        node.pieza = this.gameObject;
        node.DrawAdjacencies(tipoPieza, apertura, colorSeleccionable);
        apertura = false;
        //turno = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Si es nuestro turno
        if (turno)
        {
            //y se hace clic en pantalla
            if (Input.GetMouseButtonDown(0))
            {
                //Se hace raycast para ver dnd hemos clicado, y se llama al move to con esa posición
                RaycastHit hitInfo;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hitInfo) && !GameUIManager.menuOpened)
                {
                    MoveTo(hitInfo.point);
                }
            }
        }
    }
    //Método empleado para deshacer movimientos
    public void UndoMovement()
    {
        //Se incrementa el contador, ya que tenemos un límite de deshacer, y se comprueba que no supere dicho límite, sea nuestro turno y haya algo que deshacer
        undoCont++;

        if (undoCont <= maxUndos && numMovs > 0 && turno)
        {
            //reducimos el número de turnos, y si es cero es que hemos vuelto al inicio y habilitamos el movimiento de apertira
            numMovs--;
            if (numMovs == 0)
            {
                apertura = true;
            }
            //Hacemos pop de la pila de turnos, y se la pasamos al gamecontroller
            Turno previousTurno = previousTurnos.Pop();
            gameController.turnoPrevious = previousTurno;
            if (!previousTurno.cambioPieza)
            //Si en el turno anterior no cambiamos de pieza es que nos movimos
            {
                //Limpiamos nodo y adyacencias
                node.UndrawAdjacencies();
                node.pieza = null;
                //Si me había comido un enemigo también lo deshago, provocando su aparición y actualizando la lista de enemigos
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
                //Si he deshecho tras desbloquear la meta, esta se vuelve a bloquear
                if (previousTurno.goalOpened)
                {
                    gameController.goalOpen = false;
                    gameController.goal.Play("Close");
                }

                //Se llama a la corrutina del gamecontroller para movernos
                StartCoroutine(gameController.MoveOverSeconds(this.gameObject, previousTurno.previousNode, 1, true, node, true));
                //Y actualizamos la información del nodo que ocupamos
                node = previousTurno.previousNode;
                node.pieza = this.gameObject;
            }
            else
            {//si es un deshacer cambio de pieza, llamamos al método pertinente en el gamecontroller
                gameController.cambiaPieza(previousTurno.previousPieza, true);
            }
            //Actualizamos los botones de vallas
            gameController.movCastleButton = previousTurno.vallaCastleID;
            gameController.movHorseButton = previousTurno.vallaHorseID;
            gameController.updateButtonCastle(true);
            gameController.updateButtonHorse(true);

        }
    }
    //Método empleado para comprobar si el punto clicado es una casilla válida
    private void MoveTo(Vector3 point)
    {
        //Para ello se calcula la distancia de clic a cada uno de los nodos seleccionables, y se guarda el más cercano
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
        //Aparte de ser el más cercano, también debe estar a una distancia mínima para considerar que se ha clicado sobre él, y poder movernos
        if (nearD <= distancia && move)
        {
            numMovs++;
            //Eliminar casillas seleccionables anteriores y limpiado de pieza
            node.UndrawAdjacencies();
            node.pieza = null;
            //Se llama a la corrutina del gamecontroller para movernos
            StartCoroutine(gameController.MoveOverSeconds(this.gameObject, aux, 1, true, node, false));
            //Añadimos el nuevo nodo y si es un trigger de mensaje, hacemos lo propio
            node = aux;
            if (node.GetComponent<MessageTrigger>())
            {
                node.GetComponent<MessageTrigger>().showMessages();
            }
        }
    }
}
