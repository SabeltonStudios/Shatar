using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//Clase empleada para el control del flujo de acción de cada nivel
public class GameController : MonoBehaviour
{
    #region Variables
    //Listado d eenemigos presentes en escena
    public List<Enemie> enemigos= new List<Enemie>();
    //Lista de nodos que tienen teletransporte
    [SerializeField]
    List<Node> teletransporte = new List<Node>();
    //Número de movimientos con los que hacer la correlación de estrellas al completar el nivel
    [SerializeField]
    int[] stars = new int[3];
    //Referencias al jugador, al animator de la meta y a los botones para subir vallas, así como un bool para indicar la apertura de meta
    public Player player;
    public Animator goal;
    public GameObject buttonHorse;
    public GameObject buttonCastle;
    public bool goalOpen;
    //número máximo de turnos del nivel
    [SerializeField]
    public int maxMovs;
    //Bool para indicar la victoria e índices para indicar el tipo de derrota, así como el núnero de estrellas finales
    public bool victoria = false;
    public int derrota = 0;
    public int numStars;
    //Bools que indican si las vallas móviles se encuentran o no alzadas
    public bool vallaSubidaHorse;
    public bool vallaSubidaCastle;
    //Array de texturas para actualizar el botón de valla de forma visual, así como listado de nodos con las vallas, e índice de turnos de dichas vallas
    public Texture[] castleButtonTextures;
    public Texture[] horseButtonTextures;
    public List<Node> nodesVallaHorse;
    public List<Node> nodesVallaCastle;
    public int movCastleButton = -1;
    public int movHorseButton = -1;
    //referencia al UIManager, bool de si el jugador se encuentra en movimiento
    public GameUIManager gameUIManager;
    public bool isPlayerMoving = false;
    //Bools para indicar el desbloqueo de piezas
    public bool horseUnlock;
    public bool castleUnlock;
    //Lista de los animator de las vallas, así como referencia al soundmanager, y almacenamiento del turno anterior
    public List<Animator> vallasHorse;
    public List<Animator> vallasCastle;
    [SerializeField] private SoundManager m_soundManager = null;
    public Turno turnoPrevious;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
        //Pasar el array de enemigos a una lista para un manejo más cómodo
        Enemie[] aux = FindObjectsOfType<Enemie>();
        for(int i=0; i < aux.Length; i++)
        {
            enemigos.Add(aux[i]);
        }
        //Coger referencias del jugador, controlador de sonido, de UI e inicialización del turno y de la variable de meta
        m_soundManager = FindObjectOfType<SoundManager>();
        player = FindObjectOfType<Player>();
        gameUIManager = FindObjectOfType<GameUIManager>();
        goalOpen = false;
        turnoPrevious = new Turno(player.node, player.tipoPieza, null, movHorseButton, movCastleButton, false,false);
    }

    // Update is called once per frame
    void Update()
    {
        //Si no es el turno del jugador
        if (!player.turno)
        {
            //Esperamos hasta que todos los enemigos completen su turno
            for (int i = 0; i < enemigos.Count; i++)
            {
                if (enemigos[i].turno)
                {
                    return;
                }
            }
            //Y ya podemos cederle el turno al jugador, pintar las adyacencias y, en caso de que sea el turno inicial, y seteo de la opción de apertura
            player.turno = true;
            player.node.DrawAdjacencies(player.tipoPieza, player.apertura, player.colorSeleccionable);
            if (player.numMovs == 0)
            {
                player.apertura = false;
            }
        }
    }
    //Método llamado para ceder el turno a los enemigos, pasándoles si toca deshacer o no
    //Se actualizan los botones de valla, se le quita el turno al jugador y a cada enemigo se le indica que se mueva
    public void EnemigosTurno(bool undo)
    {
        updateButtonCastle(undo);
        updateButtonHorse(undo);
        player.turno = false;
        //Debug.Log(undo);
        foreach (Enemie e in enemigos)
        {
            e.MoveTo(undo);
        }
        
    }
    //Método empleado para la comer enemigos
    //hacemos sonar un efecto, desplazamos sus nodos a la derecha, incrementamos su id y lo desactivamos, para posteriormente actualizar la lista de enemigos
    public void destruirEnemigo(GameObject pieza)
    {
        m_soundManager.Play_SoundEffect("ficha_comida1");
        //player.tipoPieza = pieza.GetComponent<Enemie>().playerChange;
        pieza.GetComponent<Enemie>().shiftPreviousNodes(false);
        pieza.GetComponent<Enemie>().ID++;
        pieza.SetActive(false);
        enemigos.Clear();
        Enemie[] aux = FindObjectsOfType<Enemie>();
        for(int i = 0; i < aux.Length; i++)
        {
            enemigos.Add(aux[i]);
        }
    }
    //Enumator empleado para mover las piezas suavemente
    //Recibe el objeto a mover, la posición final, el tiempo que tarda en moverse, el bool de que se trata del jugador o no, el nodo anterior para limpiar adyacencias en los enemigos
    //y el bool de si se mueve por avance o por deshacer del jugador
    public IEnumerator MoveOverSeconds(GameObject objectToMove, Node end, float seconds, bool playerBool, Node previo, bool undo)
    {
        //Diferentes sonidos de movimiento para el jugador y los enemigos
        if (playerBool)
        {
            m_soundManager.Play_SoundEffect("fichas1");
        }
        else
        {
            m_soundManager.Play_SoundEffect("fichas2");
        }
        //m_soundManager.Play_SoundEffect("fichaArrastrandose");
        //Se pasa de una rotación y posición inicial a la final indicada de forma gradual, en cuestión del tiempo pasado como argumento
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        Vector3 startingRot = objectToMove.transform.up;
        while (elapsedTime < seconds)
        {
            if (playerBool)
            {
                isPlayerMoving = true;
            }
            objectToMove.transform.position = Vector3.Lerp(startingPos, end.transform.position, (elapsedTime / seconds));
            objectToMove.transform.up = Vector3.Lerp(startingRot, end.orientation, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end.transform.position;
        objectToMove.transform.up = end.orientation;
        //Si el nodo que pisamos es de botón y no estamos deshaciendo, marcamos las adyacencias no alcanzavles, marcamos el booleano y actualizamos el botón
        if (end.buttonCastle && !undo)
        {
            nodesVallaCastle[0].adjacencieNoAlcanzable[4] = true;
            nodesVallaCastle[1].adjacencieNoAlcanzable[4] = true;
            nodesVallaCastle[1].adjacencieNoAlcanzable[3] = true;
            nodesVallaCastle[2].adjacencieNoAlcanzable[3] = true;
            movCastleButton = 4;
            vallaSubidaCastle = true;
            if (!playerBool)
            {
                updateButtonCastle(false);
            }
        }
        if (end.buttonHorse&& !undo)
        {
            nodesVallaHorse[0].adjacencieNoAlcanzable[4] = true;
            nodesVallaHorse[1].adjacencieNoAlcanzable[3] = true;
            movHorseButton = 5;
            vallaSubidaHorse = true;
            if (!playerBool)
            {
                updateButtonHorse(false);
            }
        }
        //Si es el jugador
        if (playerBool)
        {
            //Creamos un nuevo turno con la información actual
            Turno turnoActual = new Turno(end, player.tipoPieza, null, movHorseButton, movCastleButton, false,false);
            isPlayerMoving = false;
            //si ha excedido el número de movimientos y no se encuentra en la meta, pierde
            if (player.numMovs>= maxMovs && !end.isGoal)
            {
                DerrotaMovs();
            }
            //Si a donde va hay un enemigo, s elo come
            if (end.pieza != null && end.pieza.tag != "Player" && !undo)
            {
                turnoPrevious.enemieEat = end.pieza;
                destruirEnemigo(end.pieza);
            }
            //Si cae en una casilla de teletransporte
            if (end.teletransport && !undo)
            {
                m_soundManager.Play_SoundEffect("teleport");
                end.GetComponentInChildren<Animator>().Play("Activacion");
                //Nos movemos y orientamos al otro nodo de teletransporte
                foreach (Node n in teletransporte)
                {
                    if(n!= end)
                    {
                        player.node = n;
                        player.node.pieza = player.gameObject;
                        turnoActual.previousNode = n;
                        objectToMove.transform.position = n.transform.position;
                        objectToMove.transform.up = n.orientation;
                        break;
                    }
                }
            }
            //Si la meta no está abierta y pisamos el botón que la abre, la desbloqueamos
            if (end.buttonGoal && !goalOpen)
            {
                //Abre la meta
                goal.Play("Open");
                m_soundManager.Play_SoundEffect("goal");
                goalOpen = true;
                turnoPrevious.goalOpened = goalOpen;
            }
            //Si llegamos a la meta vencemos
            if (end.isGoal)
            {
                Victoria();
            }
            //Guardamos en el nodo destino nuestro tipo de pieza, de cara pintar adyacencias
            end.pieza = objectToMove;
            if (!undo)
            {
                if(turnoPrevious.previousNode != null)
                {
                    //Si no estamos deshaciendo y el nodo anterior no e snull, actualizamos los ID para meter el turno en la pila, y guardar el nuevo
                    turnoPrevious.vallaHorseID = movHorseButton;
                    turnoPrevious.vallaCastleID = movCastleButton;
                    player.previousTurnos.Push(turnoPrevious);
                    turnoPrevious = turnoActual;
                }
                
            }
           
            //Cedemos el turno a los enemigos
            EnemigosTurno(undo);
        }
        else{//Si somos enemigos
            //Borramos las adyacencias, comprobamos si nos comemos al jugador para indicar su derrota, y pasamos el turno
            previo.UndrawAdjacencies();
            if (end.pieza != null && end.pieza.tag == "Player")
            {
                DerrotaComido();
            }
            objectToMove.GetComponent<Enemie>().turno = false;
            end.pieza = objectToMove;
        }

        if (playerBool)
        {
            m_soundManager.Play_SoundEffect("fichas1");
        }
        else
        {
            m_soundManager.Play_SoundEffect("fichas2");
        }
    }

    public void updateButtonHorse(bool undo)
    {
        if (vallaSubidaHorse)
        {
            if (undo)
            {
                if (movHorseButton == -1)
                {
                    movHorseButton = 5;
                }
               
                //Subir las vallas
                foreach(Animator a in vallasHorse)
                {
                    m_soundManager.Play_SoundEffect("vallas1");
                    a.SetFloat("Speed", -1);
                    a.SetInteger("State", movHorseButton);
                    m_soundManager.Play_SoundEffect("vallas");
                }
                //Cambiar la textura
                buttonHorse.GetComponent<MeshRenderer>().material.SetTexture("_EmissionMap", horseButtonTextures[movHorseButton%horseButtonTextures.Length]);
                
                if (movHorseButton == 5 || movHorseButton==0)
                {
                    vallaSubidaHorse = false;
                    nodesVallaHorse[0].adjacencieNoAlcanzable[4] = false;
                    nodesVallaHorse[1].adjacencieNoAlcanzable[3] = false;
                    //movHorseButton = -1;
                }
            }
            else
            {
                if (movHorseButton > 0)
                {
                    //Bajar las vallas
                    foreach (Animator a in vallasHorse)
                    {
                        m_soundManager.Play_SoundEffect("vallas2");
                        a.SetFloat("Speed", 1);
                        a.SetInteger("State", movHorseButton);
                        m_soundManager.Play_SoundEffect("vallas");
                    }
                    movHorseButton--;
                    //Cambiar la textura
                    buttonHorse.GetComponent<MeshRenderer>().material.SetTexture("_EmissionMap", horseButtonTextures[movHorseButton]);

                    if (movHorseButton == 0)
                    {
                        vallaSubidaHorse = false;
                        nodesVallaHorse[0].adjacencieNoAlcanzable[4] = false;
                        nodesVallaHorse[1].adjacencieNoAlcanzable[3] = false;
                        movHorseButton = -1;
                    }
                }
                
            }
            
        }
    }

    public void updateButtonCastle(bool undo)
    {
        if (vallaSubidaCastle)
        {
            if (undo)
            {
                if (movCastleButton == -1)
                {
                    movCastleButton = 4;
                }
                //Subir las vallas
                foreach (Animator a in vallasCastle)
                {
                    m_soundManager.Play_SoundEffect("vallas1");
                    a.SetFloat("Speed", -1);
                    a.SetInteger("State", movCastleButton);
                    m_soundManager.Play_SoundEffect("vallas");
                }
                
                //Cambiar la textura
                buttonCastle.GetComponent<MeshRenderer>().material.SetTexture("_EmissionMap", castleButtonTextures[movCastleButton%castleButtonTextures.Length]);
                if (movCastleButton == 4 || movCastleButton == 0)
                {
                    nodesVallaCastle[0].adjacencieNoAlcanzable[4] = false;
                    nodesVallaCastle[1].adjacencieNoAlcanzable[4] = false;
                    nodesVallaCastle[1].adjacencieNoAlcanzable[3] = false;
                    nodesVallaCastle[2].adjacencieNoAlcanzable[3] = false;
                    vallaSubidaCastle = false;
                    movCastleButton = -1;
                }
            }
            else
            {
                if (movCastleButton > 0)
                {
                    //Bajar las vallas
                    foreach (Animator a in vallasCastle)
                    {
                        m_soundManager.Play_SoundEffect("vallas2");
                        a.SetFloat("Speed", 11);
                        a.SetInteger("State", movCastleButton);
                        m_soundManager.Play_SoundEffect("vallas");
                    }
                    movCastleButton--;
                    //Cambiar la textura
                    buttonCastle.GetComponent<MeshRenderer>().material.SetTexture("_EmissionMap", castleButtonTextures[movCastleButton]);

                    if (movCastleButton == 0)
                    {
                        nodesVallaCastle[0].adjacencieNoAlcanzable[4] = false;
                        nodesVallaCastle[1].adjacencieNoAlcanzable[4] = false;
                        nodesVallaCastle[1].adjacencieNoAlcanzable[3] = false;
                        nodesVallaCastle[2].adjacencieNoAlcanzable[3] = false;
                        vallaSubidaCastle = false;
                        movCastleButton = -1;
                    }
                }
                
            }
        }
    }
    //Métodos para la gestión de distintos tipos de derrota y victoria gestionados por el UIManager
    public void DerrotaMovs()
    {
        derrota = 1;
    }
    public void DerrotaComido()
    {
        m_soundManager.Play_SoundEffect("ficha_comida2");
        derrota = 2;
    }
    public void Victoria()
    {
        //Según nuestros movimientos así el número de estrellas
        victoria = true;
        if (player.numMovs <= stars[0])
        {
            numStars = 3;
        }else if (player.numMovs <= stars[1])
        {
            numStars = 2;
        }else if (player.numMovs <= stars[2])
        {
            numStars = 1;
        }
        else
        {
            numStars = 0;
        }
    }
    // Método invocado por el jugador para realizar el cambio de pieza, 
    public void cambiaPieza(TipoPieza tipoPieza, bool undo)
    {
       //si es el turno del jugador
        if (player.turno)
        {
            m_soundManager.Play_SoundEffect("fichas3");
            
            player.tipoPieza = tipoPieza;
            //activamos el tipo de ficha que se pase por argumento
            player.transform.GetChild(0).gameObject.SetActive(false);
            player.transform.GetChild(1).gameObject.SetActive(false);
            player.transform.GetChild(2).gameObject.SetActive(false);
            switch (player.tipoPieza)
            {
                case TipoPieza.PEON:
                    player.transform.GetChild(0).gameObject.SetActive(true);
                    break;
                case TipoPieza.CABALLO:
                    player.transform.GetChild(1).gameObject.SetActive(true);
                    break;
                case TipoPieza.TORRE:
                    player.transform.GetChild(2).gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
            //Limpiamos las adyacencias
            if (player.node != null) { 
            player.node.UndrawAdjacencies();
                }
            if (!undo)
            {//Si no estamos cambiando por deshacer, incrementamos el número de turnos y actualizamos el turno actual
                player.numMovs++;
                Turno turnoActual = new Turno(player.node, player.tipoPieza,null, movHorseButton, movCastleButton, false,false);
                if (turnoPrevious.previousNode != null)
                {
                    turnoPrevious.vallaHorseID = movHorseButton;
                    turnoPrevious.vallaCastleID = movCastleButton;
                    turnoPrevious.cambioPieza = true;
                    player.previousTurnos.Push(turnoPrevious);
                    turnoPrevious = turnoActual;
                }
            }
            else//Si es por deshacer, sólo actualizamos el UI
            {
                gameUIManager.changePieceTo(player.tipoPieza, undo);
            }
            //Finalmente cedemos el turno a los enemigos
            EnemigosTurno(undo);
        }
    }
}
