using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMessages : MonoBehaviour
{
    public enum Language { ESP, ENG }
    public Language language;
    public Player player;
    public int messageShownRightNow;
    public GameObject pawnGif;
    public GameObject knightGif;
    public GameController gameController;
    public GameUIManager gameUIManager;

    public GameObject messagesPanel;
    public GameObject transparentPanel;

    public static TutorialMessages instance;
    public Text text;

    public GameObject messagesPanelInicial;
    public Text textInicial;

    private string messagesString = "¡Bienvenido a Shatar! Vamos a aprender los controles básicos\n" +
        "El peón es tu pieza inicial\n" +
        "Toca la casilla a la que quieres ir para mover tu ficha\n" +
        "Las casillas verdes son las opciones que tienes para moverte.\n" +
        "¿Ves esa torre de ahí? Pues ya sabes lo que tienes que hacer...\n" +
        "¡Vaya! Pues no lo sabías...\n" +
        "¡Bien! Sigamos avanzando.\n" +
        "Parece que no puedes saltar la valla, ¡vaya!\n" +
        "No pasa nada. ¡Te presentamos al caballo, experto en salto de vallas!\n" +
        "¡Mira! ¡Tienes otra torre en bandeja!\n" +
        "¡Cuidado! Si entras en el rango de movimiento de un enemigo, te comerá. No pasa nada, puedes darle a este botón para deshacer un movimiento.\n" +
        "Será mejor dejar a las torres en paz, quitaremos una para ponértelo más fácil, pero tenemos que seguir avanzando. Ve a la casilla azul.\n" +
        "Genial, ahora, para avanzar pasando entre las torres, cambia a peón, ya que el peón es tan pequeño que pasará desapercibido. ¡Es la única pieza con este poder!\n" +
        "Ahora nos acercamos a la parte oculta del cubo. ¡No te preocupes! Prueba girar el tablero hacia la izquierda con este botón.\n¡Bien hecho! Ahora puedes seguir avanzando.\n" +
        "¡Mira! ¡Eso de ahí es la casilla meta! El objetivo de cada nivel es llegar allí, pero parece que está cerrada. Debe de haber algún botón por ahí que la abra.\n" +
        "¿Por cierto, has probado a girar hacia arriba y abajo?\n¡Ahí está! Fíjate en la forma que tiene el botón. Tendrás que llegar a él con la pieza que encaje.\n" +
        "¡Muy bien! La puerta se ha abierto, ya puedes ir a la casilla meta, pero ten cuenta que solo el peón, que es el más pequeño, cabe por ella ";

    private string[] messages;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Fade(messagesPanelInicial, 1f, 0.7f));
        gameUIManager = FindObjectOfType<GameUIManager>();
        instance = this;
        string[] stringSeparators = new string[] { "\n" };
        messages = messagesString.Split(stringSeparators, System.StringSplitOptions.None);
        ShowMessages(0);
        StartCoroutine(WaitAndShowMessage1());
    }

    private void Update()
    {
        if (pawnGif.GetComponent<CanvasGroup>().alpha == 0)
        {
            pawnGif.SetActive(false);
        }
        if (knightGif.GetComponent<CanvasGroup>().alpha == 0)
        {
            knightGif.SetActive(false);
        }
        if (transparentPanel.GetComponent<CanvasGroup>().alpha == 0)
        {
            transparentPanel.SetActive(false);
        }
        if (messagesPanelInicial.GetComponent<CanvasGroup>().alpha == 0)
        {
            messagesPanelInicial.SetActive(false);
        }
    }

    public void ShowMessages(int[] indexes)
    {
        StartCoroutine(WaitAndShowNextMessage(indexes));
    }

    public void ShowMessages(int index)
    {
        string textContent = "";
        messageShownRightNow = index;
        switch (language)
        {
            case Language.ENG:
                textContent = messages[index];
                break;
            case Language.ESP:
                textContent = messages[index];
                break;
        }
        if (index == 0)
        {
            textInicial.text = textContent;
        }
        else
        {
            text.text = textContent;
        }

        if (index == 8)
        {
            FadeInTransparentPanel();
            knightGif.SetActive(true);
            StartCoroutine(Fade(knightGif, 1f, 0.5f));
            UnlockKnight();
        }
    }

    IEnumerator WaitAndShowNextMessage(int[] mes)
    {
        for (int i = 0; i < mes.Length; i++)
        {
            ShowMessages(mes[i]);
            yield return new WaitForSeconds(4);
        }
    }

    IEnumerator WaitAndShowMessage1()
    {
        yield return new WaitForSeconds(3);
        ShowMessages(1);
        StartCoroutine(Fade(messagesPanelInicial, 0f, 1f));
        StartCoroutine(Fade(messagesPanel, 1f, 1f));
        yield return new WaitForSeconds(1);
        FadeInTransparentPanel();
        pawnGif.SetActive(true);
        StartCoroutine(Fade(pawnGif, 1f, 0.5f));
    }

    IEnumerator WaitMessages2And3()
    {
        StartCoroutine(Fade(pawnGif, 0f, 0.5f));
        FadeOutTransparentPanel();
        ShowMessages(2);
        yield return new WaitForSeconds(4);
        if (messageShownRightNow != 4) ShowMessages(3);
    }

    public void ShowMessages2And3()
    {
        StartCoroutine(WaitMessages2And3());
    }

    public void LetPlayerMove()
    {
        player.move = true;
    }

    public void UnlockKnight()
    {
        gameController.horseUnlock = true;
        gameUIManager.UpdateChangePieceButtonsEnabled();
    }

    public void FadeInTransparentPanel()
    {
        transparentPanel.SetActive(true);
        StartCoroutine(Fade(transparentPanel, 1f, 0.5f));
    }

    public void FadeOutTransparentPanel()
    {
        StartCoroutine(Fade(transparentPanel, 0f, 0.5f));
    }

    public void FadeOutHorse()
    {
        StartCoroutine(Fade(knightGif, 0f, 0.5f));
    }

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
    }
}
