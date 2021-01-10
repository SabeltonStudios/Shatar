﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    List<Enemie> enemigos= new List<Enemie>();
    [SerializeField]
    List<Node> teletransporte = new List<Node>();
    [SerializeField]
    int[] stars = new int[3];
    Player player;
    public Animator goal;
    public GameObject buttonHorse;
    public GameObject buttonCastle;
    public bool goalOpen;
    [SerializeField]
    public int maxMovs;
    public bool victoria = false;
    public int derrota = 0;
    public int numStars;
    public bool vallaSubidaHorse;
    public bool vallaSubidaCastle;
    public Texture[] castleButtonTextures;
    public Texture[] horseButtonTextures;
    int movCastleButton = -1;
    int movHorseButton = -1;

    public bool isPlayerMoving = false;

    [SerializeField] private SoundManager m_soundManager = null;

    // Start is called before the first frame update
    void Start()
    {
        //Pasar el array de enemigos a una lista para un manejo más cómodo
        Enemie[] aux = FindObjectsOfType<Enemie>();
        for(int i=0; i < aux.Length; i++)
        {
            enemigos.Add(aux[i]);
        }
        player = FindObjectOfType<Player>();
        goalOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.turno)
        {
            for (int i = 0; i < enemigos.Count; i++)
            {
                if (enemigos[i].turno)
                {
                    return;
                }
            }

            player.turno = true;
            player.node.DrawAdjacencies(player.tipoPieza, player.apertura, player.colorSeleccionable);
            if (player.numMovs == 0)
            {
                player.apertura = false;
            }
        }
        /*
        if (Input.GetKeyDown(KeyCode.C))
        {
            if(player.tipoPieza == TipoPieza.PEON)
            {
                cambiaPieza(TipoPieza.CABALLO);
            }else if(player.tipoPieza == TipoPieza.CABALLO)
            {
                cambiaPieza(TipoPieza.TORRE);
            }
            else if (player.tipoPieza == TipoPieza.TORRE)
            {
                cambiaPieza(TipoPieza.PEON);
            }
           
        }
        */
        if (Input.GetKeyDown(KeyCode.M)) {
            goal.Play("Open");
        }
    }

    public void EnemigosTurno(bool undo)
    {
        
        player.turno = false;
        foreach(Enemie e in enemigos)
        {
            e.MoveTo(undo);
        }
    }
    public void destruirEnemigo(GameObject pieza)
    {
        //player.tipoPieza = pieza.GetComponent<Enemie>().playerChange;
        enemigos.Remove(pieza.GetComponent<Enemie>());
        Destroy(pieza);
    }
    //Enumator empleado para mover las piezas suavemente
    //Recibe el objeto a mover, la posición final, el tiempo que tarda en moverse, el bool de que se trata del juegador o no, el nodo anterior para limpiar adyacencias en los enemigos
    //y el bool de si se mueve por avance o por deshacer del jugador
    public IEnumerator MoveOverSeconds(GameObject objectToMove, Node end, float seconds, bool playerBool, Node previo, bool undo)
    {
        if (playerBool)
        {
            m_soundManager.Play_SoundEffect("fichas1");
        }
        else
        {
            m_soundManager.Play_SoundEffect("fichas2");
        }

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
        if (end.buttonCastle)
        {
            movCastleButton = 4;
            vallaSubidaCastle = true;
            if (!playerBool)
            {
                updateButtonCastle();
            }
        }
        if (end.buttonHorse)
        {
            movHorseButton = 5;
            vallaSubidaHorse = true;
            if (!playerBool)
            {
                updateButtonHorse();
            }
        }
        
        if (playerBool)
        {
            isPlayerMoving = false;

            //player.numMovs++;
            updateButtonCastle();
            updateButtonHorse();
            if (player.numMovs>= maxMovs)
            {
                DerrotaMovs();
            }
            if (end.pieza != null &&!undo)
            {
                destruirEnemigo(end.pieza);
            }
            //Si cae en una casilla de teletransporte
            if (end.teletransport)
            {
                foreach(Node n in teletransporte)
                {
                    if(n!= end)
                    {
                        player.node = n;
                        objectToMove.transform.position = n.transform.position;
                        objectToMove.transform.up = n.orientation;
                        break;
                    }
                }
            }
            if (end.buttonGoal && !goalOpen)
            {
                //Abre la meta
                goal.Play("Open");
                goalOpen = true;
            }
            
            if (end.isGoal)
            {
                Victoria();
            }
            end.pieza = objectToMove;
            
            //Hace falta ocupar la pieza antes de indicarles que se muevan para que puedan comprobar si estoy en su camino
            EnemigosTurno(undo);
        }
        else{
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

    private void updateButtonHorse()
    {
        if (movHorseButton > 0 && vallaSubidaHorse)
        {
            movHorseButton--;
            //Cambiar la textura
            buttonHorse.GetComponent<MeshRenderer>().material.SetTexture("_EmissionMap", horseButtonTextures[movHorseButton]);
            //Bajar las vallas
            if (movHorseButton == 0)
            {
                vallaSubidaHorse = false;
            }
        }
    }

    private void updateButtonCastle()
    {
        if (movCastleButton > 0 && vallaSubidaCastle)
        {
            movCastleButton--;
            //Cambiar la textura
            buttonCastle.GetComponent<MeshRenderer>().material.SetTexture("_EmissionMap", castleButtonTextures[movCastleButton]);
            //Bajar las vallas
            if (movCastleButton == 0)
            {
                vallaSubidaCastle = false;
            }
        }
    }
    public void DerrotaMovs()
    {
        derrota = 1;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void DerrotaComido()
    {
        derrota = 2;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Victoria()
    {
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
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void cambiaPieza(TipoPieza tipoPieza, bool undo)
    {
        if (player.turno)
        {
            m_soundManager.Play_SoundEffect("fichas3");

            //if (undo) { Debug.Log("cambio a la anterior"); } else { Debug.Log("cambio a la siguiente"); }
            player.shiftPreviousNodes(undo, true);
            player.tipoPieza = tipoPieza;
            
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
            if (player.node != null) { 
            player.node.UndrawAdjacencies();
                }
            if (!undo)
            {
                player.numMovs++;
            }
            EnemigosTurno(undo);
        }
    }
}
