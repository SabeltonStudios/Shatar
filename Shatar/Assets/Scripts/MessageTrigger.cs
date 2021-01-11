using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageTrigger : MonoBehaviour
{
    // Message to show when player is in this node
    public int[] message;
    private bool alreadyShown;

    public void showMessages()
    {
        if (!alreadyShown)
        {
            TutorialMessages.instance.ShowMessages(message);
            alreadyShown = true;
        }
    }
}
