using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Clase empleada para indicar al gestor de mensajes que muestre el mensaje indicado 
public class MessageTrigger : MonoBehaviour
{
    //Mensajes a mostrar cuando el jugador está en un nodo concreto, y booleano de si ya se ha mostrado el mensaje
    public int[] message;
    private bool alreadyShown;

    //método empleado para evitar mostrar el mensaje multiples veces
    public void showMessages()
    {
        if (!alreadyShown)
        {
            TutorialMessages.instance.ShowMessages(message);
            alreadyShown = true;
        }
    }
}
