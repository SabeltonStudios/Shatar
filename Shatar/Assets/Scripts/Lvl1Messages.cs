using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lvl1Messages : MonoBehaviour
{
    //public enum Language { ESP, ENG }
    //public Language language;
    public int messageShownRightNow;
    private bool messagesAlreadyShown;

    public GameObject messagesPanel;

    public Text text;

    private string messagesString = "Esa casilla tan rara que ves ahí es una casilla de teletransporte\n" +//0
        "Como puedes ver hay dos, cuando caigas en una, aparecerás en la otra\n" +//1
        "Pulsa en este cartel para ocultarlo";//2

    private string messagesStringEnglish = "That square right there that looks so weird is a teleporting square\n" +//0
        "as you can see there's two of them, when you step on one, you'll appear on the other one\n" +//1
        "Press here to hide this message";//2

    private string[] messages;

    void Start()
    {
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

    public void HideMessage()
    {
        if (messageShownRightNow == 2)
        {
            StartCoroutine(Fade(messagesPanel, 0, 1.0f));
        }
    }

    public void ShowMessages(int[] indexes)
    {
        StartCoroutine(WaitAndShowNextMessage(indexes));
    }

    public void ShowMessages(int index)
    {
        messageShownRightNow = index;
        string textContent = messages[index];
        text.text = textContent;
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
        ShowMessages(messageShownRightNow + 1 < messages.Length ? messageShownRightNow + 1 : messageShownRightNow);
    }

    public void ShowPreviousMessage()
    {
        ShowMessages(messageShownRightNow - 1 > -1 ? messageShownRightNow - 1 : messageShownRightNow);
    }
}
