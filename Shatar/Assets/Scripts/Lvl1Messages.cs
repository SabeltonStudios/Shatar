using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Clase empleada para mostrar los mensajes guía en el nivel 1
public class Lvl1Messages : MonoBehaviour
{
    //Indice con el mensaje actual, booleno de si el mensaje ya ha sido mostrado, panel en que se muestran y contenido de texto UI del mismo
    public int messageShownRightNow;
    private bool messagesAlreadyShown;

    public GameObject messagesPanel;

    public Text text;
    //Declaración de mensajes en castellano e inglés
    private string messagesString = "Esa casilla tan rara que ves ahí es una casilla de teletransporte\n" +//0
        "Como puedes ver hay dos, cuando caigas en una, aparecerás en la otra\n" +//1
        "Pulsa en este cartel para ocultarlo";//2

    private string messagesStringEnglish = "That square right there that looks so weird is a teleporting square\n" +//0
        "as you can see there's two of them, when you step on one, you'll appear on the other one\n" +//1
        "Press here to hide this message";//2
    //Array de mensajes
    private string[] messages;

    void Start()
    {
        //Se inicializa el ID a -1, generamos el array de mensajes y empleamos uno u otro según el idioma
        messageShownRightNow = -1;
        string[] stringSeparators = new string[] { "\n" };
        switch (Localization.GetLanguage())
        {
            case Localization.Language.English:
                messages = messagesStringEnglish.Split(stringSeparators, System.StringSplitOptions.None);
                break;
            case Localization.Language.Spanish:
                messages = messagesString.Split(stringSeparators, System.StringSplitOptions.None);
                break;
        }
    }

    private void Update()
    {

    }
    //método para mostrar el mensaje relativo al teletransporte
    public void ExplainTeleportingSquares()
    {
        if (!messagesAlreadyShown)
        {
            messagesAlreadyShown = true;
            StartCoroutine(Fade(messagesPanel, 1, 1.0f));
            int[] teleportingSquareExplanation = { 0, 1, 2 };
            ShowMessages(teleportingSquareExplanation);
        }
    }
    //método empelado para ocultar el panel de mensajes tras un fade del mismo, si nos encontramos en el último mensaje
    public void HideMessage()
    {
        if (messageShownRightNow == 2)
        {
            StartCoroutine(Fade(messagesPanel, 0, 1.0f));
        }
    }

    //Método para mostrar el paso de mensajes tras un tiempo
    public void ShowMessages(int[] indexes)
    {
        StartCoroutine(WaitAndShowNextMessage(indexes));
    }
    //Método para mostrar cada mensaje
    public void ShowMessages(int index)
    {
        messageShownRightNow = index;
        string textContent = messages[index];
        text.text = textContent;
    }
    //Enumerator para mostrar los mensajes tras un paso de tiempo
    IEnumerator WaitAndShowNextMessage(int[] mes)
    {
        for (int i = 0; i < mes.Length; i++)
        {
            if (mes[i] > messageShownRightNow)
            {
                ShowMessages(mes[i]);
                yield return new WaitForSeconds(7);
            }
        }
    }
    //Enumerator para desvanecer el objeto pasado y desvisualizarlo
    private IEnumerator Fade(GameObject gameObject, float amount, float time)
    {
        float startAlpha;
        startAlpha = gameObject.GetComponent<CanvasGroup>().alpha;
        for (float t = 0.1f; t < time; t += Time.deltaTime)
        {
            float porcentaje = t / time;
            gameObject.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(startAlpha, amount, porcentaje);
            yield return null;
        }
        gameObject.GetComponent<CanvasGroup>().alpha = amount;
        if (gameObject.GetComponent<CanvasGroup>().alpha == 0)
        {
            gameObject.SetActive(false);
        }
    }
    //Método para pasar al siguiente mensaje de forma manual
    public void ShowNextMessage()
    {
        ShowMessages(messageShownRightNow + 1 < messages.Length ? messageShownRightNow + 1 : messageShownRightNow);
    }
    //Método para pasar al anterior mensaje de forma manual
    public void ShowPreviousMessage()
    {
        ShowMessages(messageShownRightNow - 1 > -1 ? messageShownRightNow - 1 : messageShownRightNow);
    }
}
