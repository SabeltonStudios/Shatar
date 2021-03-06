﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Clase empleada para mostrar los mensajes del nivel de tutorial
public class TutorialMessages : MonoBehaviour
{
    //referencias al jugador, indice del mensaje actual, gifs de peón y caballo, referencia al gamecontroller y uimanager, 
    //objeto para marcar casilla azul y botones de giro de cámara
    public Player player;
    public int messageShownRightNow;
    public GameObject pawnGif;
    public GameObject knightGif;
    public GameController gameController;
    public GameUIManager gameUIManager;
    public GameObject blueSquare1;
    public GameObject cameraControllerButtons;
    //panel en que se muestran los mensajes y panel transparente
    public GameObject messagesPanel;
    public GameObject transparentPanel;
    //UIText de los mensajes e instancia de la clase
    public static TutorialMessages instance;
    public Text text;
    //Objeto y UIText para el mensaje inicial
    public GameObject messagesPanelInicial;
    public Text textInicial;
    //Cadenas de mensajes en castellano e inglés
    private string messagesString = "¡Bienvenido a Shatar! Vamos a aprender los controles básicos\n" + //0
        "El peón es tu pieza inicial, que solo puede avanzar en una dirección\n" +//1
        "Toca la casilla a la que quieres ir para mover tu ficha\n" +//2
        "Las casillas verdes son las opciones que tienes para moverte\n" +//3
        "¿Ves esa torre que tienes al lado? Pues ya sabes lo que tienes que hacer...\n" +//4
        "¿Ves esa torre que tienes al lado? Pues ya sabes lo que tienes que hacer...\n" +//5
        "¡Bien! Las casillas en rosa son el alcance de los enemigos\n" +//6
        "¡Vaya! Parece que no puedes saltar la valla\n" +//7
        "No pasa nada. ¡Te presentamos al caballo, experto en salto de vallas!\n" +//8
        "Pulsa el botón redondo azul de arriba y elige el caballo\n" + //9
        "¿Ves que te puedes comer una torre? Pues no siempre es buena idea comértela\n" +//10
        "Si entras en el rango de movimiento de un enemigo, te comerá\n" +//11
        "Ve a la casilla azul\n" +//12
        "Genial, ahora, para avanzar sin que te vean las torres, cambia a peón\n" +//13
        "El peón es tan pequeño que pasa desapercibido. ¡Es la única pieza con este poder!\n" +//14
        "Siendo peón puedes comerte la torre de arriba sin que te vea la de abajo\n" + //15
        "¡Genial! Ahora nos acercamos a la parte oculta del cubo. ¡No te preocupes!\n" +//16
        "Puedes girar el tablero con los botones de abajo\n" +//17
        "¡Mira! Lo que tienes justo delante es la casilla meta\n" +//18
        "El objetivo de cada nivel es llegar ahí, pero parece que está cerrada\n" +//19
        "Sigue avanzando y ahora pensamos algo\n" +//20
        "Perfecto, debes saber que si la meta está cerrada, hay siempre un botón que la abre\n" +//21
        "En este caso el botón está en la cara de abajo, ve a pulsarlo para terminar el nivel\n" +//22
        "Te recuerdo que puedes cambiar al caballo cuando quieras\n" + //23
        "Si te fijas en el botón verás que tiene una forma concreta\n" +//24
        "Cada tipo de pieza tiene una forma diferente en su base\n" + //25
        "En este caso, solo el peón, de base redonda, puede activar el botón\n" +//26
        "¡Muy bien! La puerta se ha abierto, ya puedes ir a la casilla meta\n" +//27
        "Ten cuenta que solo el peón cabe por ella (y así ocurrirá en todos los niveles)\n" +//28
        "Pulsa en este cartel para ocultarlo";//29

    private string messagesStringEnglish = "¡Welcome to Shatar! Let's learn the basic controls\n" + //0
        "The pawn is your starting piece, which can only move in one direction\n" +//1
        "Touch the square you want to go to move your piece\n" +//2
        "The green squares are the options you have to move in your turn\n" +//3
        "Do you see that rook right next to you? Well, you know what to do...\n" +//4
        "Do you see that rook right next to you? Well, you know what to do...\n" +//5
        "Great! Pink squares show the enemies’ movement range\n" +//6
        "It seems like you can’t jump the fence. Dammit!\n" +//7
        "Don’t worry. This is the knight, an expert in hurdling!\n" +//8
        "Press the blue round button in the upper part and choose the knight\n" + //9
        "Can you see you could eat that rook? Well, not always is a good idea to do so\n" +//10
        "If you enter an enemy’s movement range, they will eat you. Go to the blue square\n" +//11
        "Go to the blue square\n" +//12
        "Great! Now, to go on without being seen, switch back to pawn\n" +//13
        "The pawn is so small that will go unnoticed. It’s the only piece with this power!\n" +//14
        "If you’re a pawn you can eat the upper rook without being noticed by the lower one\n" + //15
        "Good job! Now we get close to the hidden part of the cube. Don’t worry!\n" +//16
        "Use the lower buttons to turn the board\n" +//17
        "What you have right in front of you is the goal square\n" +//18
        "The aim of every level is to get there, but it seems to be closed\n" +//19
        "Okay, keep moving a bit and we’ll think of something\n" +//20
        "Perfect, you should know that if the goal square is closed, there is always a button to open it\n" +//21
        "In this case, that button is on the lower face of the board. Go press it to finish the level\n" +//22
        "Friendly reminder: you can switch to being a knight at any moment\n" + //23
        "If you look closely at the button, you’ll see that it has a specific shape\n" +//24
        "Every chessman has a different shape in its base\n" + //25
        "In this case, only the pawn, having a round base can activate the button\n" +//26
        "Well done! The goal is open, you can now go and finish the level\n" +//27
        "Just have in mind that only the pawn is small enough to fit through it (and so will occur in any level)\n" +//28
        "Press here to hide this message";//29
                                          //array de mensajes
    private string[] messages;

    void Start()
    {
        //Se inicializa el índice a -1, se desactivan los botones de cámara, se muestra el mensaje inicial y se cogen las referencias 
        messageShownRightNow = -1;
        cameraControllerButtons.SetActive(false);
        StartCoroutine(Fade(messagesPanelInicial, 1f, 0.7f));
        gameUIManager = FindObjectOfType<GameUIManager>();
        instance = this;
        //Se hace el split de mensajes para almacenarlos en el array
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
        //Se muestra el mensaje inicial e inicia la corrutina que muestra el siguiente
        ShowMessages(0);
        StartCoroutine(WaitAndShowMessage1());
    }

    private void Update()
    {
        //Aquellos objetos que hayan llegado a alpha cero, se desactivan
        if (pawnGif.activeSelf && pawnGif.GetComponent<CanvasGroup>().alpha == 0)
        {
            pawnGif.SetActive(false);
        }
        if (knightGif.activeSelf && knightGif.GetComponent<CanvasGroup>().alpha == 0)
        {
            knightGif.SetActive(false);
        }
        if (transparentPanel.activeSelf && transparentPanel.GetComponent<CanvasGroup>().alpha == 0)
        {
            transparentPanel.SetActive(false);
        }
        if (transparentPanel.activeSelf && messagesPanelInicial.GetComponent<CanvasGroup>().alpha == 0)
        {
            messagesPanelInicial.SetActive(false);
        }
    }
    //método empleado para mostrar una secuencia de mensajes con paso de tiempo
    public void ShowMessages(int[] indexes)
    {
        StartCoroutine(WaitAndShowNextMessage(indexes));
    }
    //método empleado para mostrar cada uno de los mensajes
    public void ShowMessages(int index)
    {
        string textContent = "";
        messageShownRightNow = index;
        textContent = messages[index];
        if (index == 0)
        {
            textInicial.text = textContent;
        }
        else
        {
            text.text = textContent;
        }
        //Según el mensaje, se muestran y desbloquean distintas cosas, como gifs de figuras o giros de cámara
        if (messageShownRightNow == 8)
        {
            FadeInTransparentPanel();
            knightGif.SetActive(true);
            StartCoroutine(Fade(knightGif, 1f, 0.5f));
            UnlockKnight();
        }
        if (messageShownRightNow == 12)
        {
            blueSquare1.SetActive(true);
        }
        if (messageShownRightNow > 12)
        {
            blueSquare1.SetActive(false);
        }
        if (messageShownRightNow == 17)
        {
            cameraControllerButtons.SetActive(true);
        }
    }
    //método empleado para ocultar el panel de mensajes
    public void HideMessage()
    {
        if (messageShownRightNow == 29)
        {
            gameObject.SetActive(false);
        }
    }
    //enumerator para mostrar los mensaje con paso de tiempo
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
    //método para mostrar el segundo mensaje y transicionar desde inicial
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
    //Enumerator para el paso de mensajes entre el tercer y cuarto mensaje
    IEnumerator WaitMessages2And3()
    {
        StartCoroutine(Fade(pawnGif, 0f, 0.5f));
        FadeOutTransparentPanel();
        ShowMessages(2);
        yield return new WaitForSeconds(7);
        if (messageShownRightNow != 4) ShowMessages(3);
    }
    //Método para mostrar el tercere y cuarto mensaje
    public void ShowMessages2And3()
    {
        StartCoroutine(WaitMessages2And3());
    }
    //método para habilitar el movimiento del jugador
    public void LetPlayerMove()
    {
        player.move = true;
    }
    //Método para desbloquear la pieza caballo
    public void UnlockKnight()
    {
        gameController.horseUnlock = true;
        gameUIManager.UpdateChangePieceButtonsEnabled();
    }
    //Métodos para realizar el fade de entrada y salida de diferentes elementos
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
    //método para pasar al siguiente mensaje de forma manual
   /* public void ShowNextMessage()
    {
        ShowMessages(messageShownRightNow + 1 < messages.Length ? messageShownRightNow + 1 : messageShownRightNow);
    }*/
    //método para pasar al anterior mensaje de forma manual
    public void ShowPreviousMessage()
    {
        ShowMessages(messageShownRightNow - 1 > -1 ? messageShownRightNow - 1 : messageShownRightNow);
    }
    public void skipTutorial()
    {
        gameObject.GetComponent<CanvasGroup>().alpha = 0;
        UnlockKnight();
        LetPlayerMove();
        pawnGif.GetComponent<CanvasGroup>().alpha = 0;
        knightGif.GetComponent<CanvasGroup>().alpha = 0;
        cameraControllerButtons.SetActive(true);

    }
}
