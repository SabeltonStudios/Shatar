﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public Node node;
    // Start is called before the first frame update
    void Start()
    {
        node.DrawAdjacencies(TipoPieza.PEON,true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
