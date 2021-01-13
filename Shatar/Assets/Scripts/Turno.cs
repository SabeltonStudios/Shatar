using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Clase empleada para almacenar la información relativa a cada turno
public class Turno : MonoBehaviour
{
    //Se almacenan el nodo anterior, el tipo de pieza, el enemigo comido, los ID de los botones de vallas y la apertura de meta
    public Node previousNode;
    public TipoPieza previousPieza;
    public GameObject enemieEat;
    public int vallaHorseID;
    public int vallaCastleID;
    public bool cambioPieza;
    public bool goalOpened;

    public Turno(Node p, TipoPieza tp, GameObject et, int vh, int vc, bool cambioP, bool goalO)
    {
        previousNode = p;
        previousPieza = tp;
        enemieEat = et;
        vallaHorseID = vh;
        vallaCastleID = vc;
        cambioPieza = cambioP;
        goalO = goalOpened;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
