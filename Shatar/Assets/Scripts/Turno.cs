using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turno : MonoBehaviour
{
    public Node previousNode;
    public TipoPieza previousPieza;
    public GameObject enemieEat;
    public int vallaHorseID;
    public int vallaCastleID;
    public bool cambioPieza;

    public Turno(Node p,TipoPieza tp,GameObject et, int vh, int vc, bool cambioP)
    {
        previousNode = p;
        previousPieza = tp;
        enemieEat = et;
        vallaHorseID = vh;
        vallaCastleID = vc;
        cambioPieza = cambioP;
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
