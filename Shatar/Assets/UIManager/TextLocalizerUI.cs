using System;
using UnityEngine;
using UnityEngine.UI;

public class TextLocalizerUI : MonoBehaviour
{
    Text textField;

    public string key;

    private MenuUIManager m_UIManager;

    private void Awake()
    {
        m_UIManager = GameObject.Find("UIManager").GetComponent<MenuUIManager>();
    }

    private void Start()
    {
        textField = GetComponent<Text>();
        string value = Localization.GetLocalizedValue(key);
        textField.text = value;

        m_UIManager.OnVariableChangeEvent += VariableChangeHandler;
    }

    private void VariableChangeHandler()
    {
        string value = Localization.GetLocalizedValue(key);
        textField.text = value;
    }
}
