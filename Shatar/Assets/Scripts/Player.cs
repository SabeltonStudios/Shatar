using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public Node node;
    [SerializeField]
    public TipoPieza tipoPieza;

    [SerializeField]
    public bool apertura;

    // Start is called before the first frame update
    void Start()
    {
        node.DrawAdjacencies(tipoPieza,apertura);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
