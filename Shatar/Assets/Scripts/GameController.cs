using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Enemie[] enemigos;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        enemigos = FindObjectsOfType<Enemie>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.turno)
        {
            for (int i = 0; i < enemigos.Length; i++)
            {
                if (enemigos[i].turno)
                {
                    return;
                }
            }
            player.turno = true;
            player.node.DrawAdjacencies(player.tipoPieza, player.apertura, player.colorSeleccionable);
        }
    }

    public void EnemigosTurno()
    {
        for (int i = 0; i < enemigos.Length; i++)
        {
            enemigos[i].MoveTo();
        }
        player.turno = false;
    }
}
