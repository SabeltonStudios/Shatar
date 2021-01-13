using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMessages : MonoBehaviour
{
    //public enum Language { ESP, ENG }
    //public Language language;
    public Player player;
    public int messageShownRightNow;
    public GameObject pawnGif;
    public GameObject knightGif;
    public GameController gameController;
    public GameUIManager gameUIManager;
    public GameObject blueSquare1;
    public GameObject cameraControllerButtons;

    public GameObject messagesPanel;
    public GameObject transparentPanel;

    public static TutorialMessages instance;
    public Text text;

    public GameObject messagesPanelInicial;
    public Text textInicial;

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
        "Sigue avanzando y ahora pensamos algo\n"+//20
        "Perfecto, debes saber que si la meta está cerrada, hay siempre un botón que la abre\n" +//21
        "En este caso el botón está en la cara de abajo, ve a pulsarlo para terminar el nivel\n" +//22
        "Te recuerdo que puedes cambiar al caballo cuando quieras\n" + //23
        "Si te fijas en el botón verás que tiene una forma concreta\n" +//24
        "Cada tipo de pieza tiene una forma diferente en su base\n" + //25
        "En este caso, solo el peón, de base redonda, puede activar el botón\n"+//26
        "¡Muy bien! La puerta se ha abierto, ya puedes ir a la casilla meta\n" +//27
        "Ten cuenta que solo el peón cabe por ella (y así ocurrirá en todos los niveles)\n";//28

    private string messagesStringEnglish = "¡Welcome to Shatar! Let's learn the basic controls\n" + //0
        "The pawn is your starting piece, which can only move in one direction\n" +//1
        "Touch the square you want to go to move your piece or pawn\n" +//2
        "Green sqaures are the options you have to move\n" +//3
        "Do you see that rook that you have by your side? You know what you have to do...\n" +//4
        "Do you see that rook that you have by your side? You know what you have to do...\n" +//5
        "¡Good! Squares in pink are the range of the enemies\n" +//6
        "Well! It seems you can't jump over the fence\n" +//7
        "It is okay. We introduce you the knight, expert in jumping over fences!\n" +//8
        "Press the blue circle button above and choose the knight\n" + //9
        "Do you see you can eat a rook? Well, that is not always a good idea\n" +//10
        "If you enter in an enemy's range, it will eat you\n" +//11
        "Go to the blue square\n" +//12
        "Great, now, to progress without the rooks seeing you, change back to the pawn\n" +//13
        "The pawn is so little that goes unnoticed. It is the only piece with this power!\n" +//14
        "Being a pawn you can now eat the rook above without being seeing by the one below\n" + //15
        "¡Awesome! Now we reach the hidden side of the cube. Don't worry!\n" +//16
        "You can rotate the board with these buttons below\n" +//17
        "¡Look! What you have just ahead is the goal square\n" +//18
        "The goal of each level is to get there, but it seems it's locked\n" +//19
        "Keep moving and we will think of something\n" +//20
        "Perfect, you must know that if the goal square is closed and inaccesible, there is always a button that opens it\n" +//21
        "In this case the button is in the face down, go and press it to finish the level\n" +//22
        "I remind you that you can change to the knight anytime you want\n" + //23
        "If you look at the button you will see that it has a specific shape\n" +//24
        "Each type of piece has a different shape at its base\n" + //25
        "In this case, only the round-based pawn can activate the button\n" +//26
        "Great! The door has been opened, you can now go to the goal square\n" +//27
        "Keep in mind that only the pawn fits through it (and this will happen in all the levels)\n";//28

    private string[] messages;
    
    void Start()
    {
        messageShownRightNow = -1;
        cameraControllerButtons.SetActive(false);
        StartCoroutine(Fade(messagesPanelInicial, 1f, 0.7f));
        gameUIManager = FindObjectOfType<GameUIManager>();
        instance = this;
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
        ShowMessages(0);
        StartCoroutine(WaitAndShowMessage1());
    }

    private void Update()
    {
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

    public void ShowMessages(int[] indexes)
    {
        StartCoroutine(WaitAndShowNextMessage(indexes));
    }

    public void ShowMessages(int index)
    {
        string textContent = "";
        messageShownRightNow = index;
        textContent = messages[index];
        /*
        switch (language)
        {
            case Language.ENG:
                textContent = messages[index];
                break;
            case Language.ESP:
                textContent = messages[index];
                break;
        }
        */
        if (index == 0)
        {
            textInicial.text = textContent;
        }
        else
        {
            text.text = textContent;
        }

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
        yield return new WaitForSeconds(7);
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

    public void ShowNextMessage()
    {
        ShowMessages(messageShownRightNow+1);
    }

    public void ShowPreviousMessage()
    {
        ShowMessages(messageShownRightNow - 1);
    }
}
