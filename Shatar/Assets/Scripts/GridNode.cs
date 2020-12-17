using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode: MonoBehaviour
{
    public int id;
    public Vector3 pos;

    //Rferencias a casillas adyacentes
    [SerializeField]
    public GameObject forward;
    [SerializeField]
    public GameObject forwardLeft;
    [SerializeField]
    public GameObject forwardRight;
    [SerializeField]
    public GameObject backward;
    [SerializeField]
    public GameObject backwardLeft;
    [SerializeField]
    public GameObject backwardRight;
    [SerializeField]
    public GameObject left;
    [SerializeField]
    public GameObject right;

    public GridNode(Vector3 p)
    {
        pos.x = p.x;
        pos.y = p.y;
        pos.z = p.z;
    }

    public void setPosition(Vector3 p)
    {
        pos.x = p.x;
        pos.y = p.y;
        pos.z = p.z;
    }
}
