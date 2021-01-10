using System;
using UnityEngine;
using UnityEngine.UI;

public class TextLocalizerGameUI : MonoBehaviour
{
    Text textField;

    public string key;

    private void Start()
    {
        textField = GetComponent<Text>();
        string value = Localization.GetLocalizedValue(key);
        textField.text = value;
    }
}
