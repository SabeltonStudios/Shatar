﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    List<Enemie> enemigos= new List<Enemie>();
    [SerializeField]
    List<Node> teletransporte = new List<Node>();
    Player player;
    [SerializeField]
    int maxMovs;
    int numStars;

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
        }
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
    }

    public void EnemigosTurno()
    {
        player.turno = false;
        foreach(Enemie e in enemigos)
        {
            e.MoveTo();
        }
    }
    public void destruirEnemigo(GameObject pieza)
    {
        //player.tipoPieza = pieza.GetComponent<Enemie>().playerChange;
        enemigos.Remove(pieza.GetComponent<Enemie>());
        Destroy(pieza);
    }
    //Enumator empleado para mover las piezas suavemente
    public IEnumerator MoveOverSeconds(GameObject objectToMove, Node end, float seconds, bool playerBool, Node previo)
    {
        
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        Vector3 startingRot = objectToMove.transform.up;
        while (elapsedTime < seconds)
        {
            objectToMove.transform.position = Vector3.Lerp(startingPos, end.transform.position, (elapsedTime / seconds));
            objectToMove.transform.up = Vector3.Lerp(startingRot, end.orientation, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        objectToMove.transform.position = end.transform.position;
        objectToMove.transform.up = end.orientation;
        if (playerBool)
        {
            player.numMovs++;
            if(player.numMovs>= maxMovs)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            if (end.pieza != null)
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
            if (end.isGoal)
            {
                Victoria();
            }
            EnemigosTurno();
        }
        else{
            previo.UndrawAdjacencies();
            if (end.pieza != null && end.pieza.tag == "Player")
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            objectToMove.GetComponent<Enemie>().turno = false;
        }
        end.pieza = objectToMove;

    }
    private void Victoria()
    {
        if (player.numMovs <= 10)
        {
            numStars = 3;
        }else if (player.numMovs < 15)
        {
            numStars = 2;
        }else if (player.numMovs < 20)
        {
            numStars = 1;
        }
        else
        {
            numStars = 0;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void cambiaPieza(TipoPieza tipoPieza)
    {
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
        player.node.UndrawAdjacencies();
        player.node.DrawAdjacencies(player.tipoPieza, player.apertura, player.colorSeleccionable);
    }
}
