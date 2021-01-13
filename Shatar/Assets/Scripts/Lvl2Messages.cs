using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Clase empleada para mostrar los mensajes guía en el nivel 2
public class Lvl2Messages : MonoBehaviour
{
    //Indice con el mensaje actual, gif explicativo de la torre, panel en que se muestran y contenido de texto UI del mismo

    public int messageShownRightNow;
    public GameObject rookGif;

    public GameObject messagesPanel;

    public Text text;
    //Cadena de mensajes en castellano e inglés
    private string messagesString = "¡En este nivel puedes utilizar una nueva pieza!¡La torre!\n" +//0
        "En este nivel hay botones especiales. Pero su efecto no dura eternamente\n" +//1
        "Una vez que dejes de pulsar el botón, el número de luces que tenga encendidas...\n" +//2
        "indicará los turnos que quedan para que deje de tener efecto\n" +//3
        "Pulsa en este cartel para ocultarlo";//4

    private string messagesStringEnglish = "In this level you'll get to use a new piece! The rook!\n" +//0
        "In this level there are special buttons. But their effect doesn't last forever\n" +//1
        "Once you release a button, the number of lights turned on it'll have...\n" +//2
        "will indicate you the turns left for it to stop causing any effect\n" +//3
        "Press here to hide this message";//4
    //Array de mensajes
    private string[] messages;

    void Start()
    {//Se inicializa el ID a -1, generamos el array de mensajes y empleamos uno u otro según el idioma
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
        //Mostramos el gif y hacemos aparecer el panel para mostrar los mensajes
        FadeInRookGif();
        StartCoroutine(Fade(messagesPanel, 1, 1.0f));
        ShowMessages(0);
    }

    private void Update()
    {

    }
    //Método para mostrar la secuencia de mensajes con paso de tiempo
    public void ExplainButtons()
    {
        int[] buttonsMessages = { 1, 2, 3, 4 };
        ShowMessages(buttonsMessages);
    }
    //método para ocultar el panel 
    public void HideMessage()
    {
        if (messageShownRightNow == 4)
        {
            StartCoroutine(Fade(messagesPanel, 0, 1.0f));
        }
    }
    //Método para mostrar esos mensajes con paso de tiempo
    public void ShowMessages(int[] indexes)
    {
        StartCoroutine(WaitAndShowNextMessage(indexes));
    }
    //Método para mostrar cada uno de los mensajes
    public void ShowMessages(int index)
    {
        messageShownRightNow = index;
        string textContent = messages[index];
        text.text = textContent;
    }
    //Enumerator para mostrar los mensajes con paso de tiempo
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
    //Enumerator para desvanecer el objeto pasado
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
    //método para realizar el fade de entrada del gif
    public void FadeInRookGif()
    {
        StartCoroutine(Fade(rookGif, 1, 1.0f));
    }
    //método para realizar el fade de salida del gif
    public void FadeOutRookGif()
    {
        StartCoroutine(Fade(rookGif, 0, 1.0f));
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
