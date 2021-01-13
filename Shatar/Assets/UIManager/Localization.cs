using System.Collections.Generic;
using UnityEngine;

public class Localization : MonoBehaviour
{
    public enum Language
    {
        Spanish,
        English
    }

    public static Language language = Language.Spanish;

    private static Dictionary<string, string> localizedES;
    private static Dictionary<string, string> localizedEN;

    public static bool isInit;

    public static void Init()
    {
        CSVLoader csvLoader = new CSVLoader();
        csvLoader.LoadCSV();

        localizedES = csvLoader.GetDictionaryValues("es");
        localizedEN = csvLoader.GetDictionaryValues("en");

        isInit = true;
    }

    public static string GetLocalizedValue(string key)
    {
        if (!isInit) { Init(); }

        string value = key;

        switch (language)
        {
            case Language.Spanish:
                localizedES.TryGetValue(key, out value);
                break;
            case Language.English:
                localizedEN.TryGetValue(key, out value);
                break;
        }

        return value;
    }

    public static void SetLanguage(Language newLanguage)
    {
        language = newLanguage;
    }

    public static Language GetLanguage()
    {
        return language;
    }
}
