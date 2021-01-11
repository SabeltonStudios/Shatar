using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMessages : MonoBehaviour
{
    public enum Language { ESP, ENG}
    public Language language;
    public Player player;
    public int messageShownRightNow;
    public GameObject pawnGif;
    public GameObject knightGif;
    public GameController gameController;

    public static TutorialMessages instance;
    public Text text;

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
        instance = this;
        string[] stringSeparators = new string[] { "\n" };
        messages = messagesString.Split(stringSeparators, System.StringSplitOptions.None);
        ShowMessages(0);
        StartCoroutine(WaitAndShowMessage1());
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
        text.text = textContent;

        if (index == 8)
        {
            knightGif.SetActive(true);
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
        yield return new WaitForSeconds(4);
        ShowMessages(1);
        pawnGif.SetActive(true);
    }

    IEnumerator WaitMessages2And3()
    {
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
    }
}
