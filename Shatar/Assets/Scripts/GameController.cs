using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    List<Enemie> enemigos= new List<Enemie>();
    Player player;

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
        player.tipoPieza = pieza.GetComponent<Enemie>().playerChange;
        enemigos.Remove(pieza.GetComponent<Enemie>());
        Destroy(pieza);
        GameObject child;
        player.transform.GetChild(0).gameObject.SetActive(false);
        player.transform.GetChild(1).gameObject.SetActive(false);
        switch (player.tipoPieza)
        {
            case TipoPieza.PEON:
                player.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case TipoPieza.CABALLO:
                player.transform.GetChild(1).gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
    //Enumator empleado para mover las piezas suavemente
    public IEnumerator MoveOverSeconds(GameObject objectToMove, Node end, float seconds, bool player, Node previo)
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
        if (player){
            if (end.pieza != null)
            {
                destruirEnemigo(end.pieza);
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
}
