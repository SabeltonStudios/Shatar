using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUIManager : MonoBehaviour
{
    #region Variables
    [Header("Main Menu")]
    [SerializeField] private GameObject mainMenu = null;
    [SerializeField] private Button b_play = null;
    [SerializeField] private Button b_settings = null;
    [SerializeField] private GameObject i_settings = null;
    [SerializeField] private Button b_twitter = null;
    [SerializeField] private Button b_instagram = null;
    [SerializeField] private Button b_youtube = null;
    [SerializeField] private Button b_tiktok = null;

    [Header("LevelsMap")]
    [SerializeField] private GameObject levelsMap = null;
    [SerializeField] private Button b_store = null;
    [SerializeField] private GameObject levelPanel = null;
    [SerializeField] private GameObject transparentPanel = null;
    [SerializeField] private Text t_levelNumberTitle = null;
    [SerializeField] private Text t_currentGems = null;
    [SerializeField] private Button b_playLevel = null;
    [SerializeField] private Button b_backToLevelMap = null;
    [SerializeField] private Button b_backToMenu_levelsMap = null;

    [Header("Settings")]
    [SerializeField] private GameObject settings = null;
    [SerializeField] private GameObject t_currentLanguage = null;
    [SerializeField] private Button b_language_left = null;
    [SerializeField] private Button b_language_right = null;
    [SerializeField] private Button b_muteSoundEffects = null;
    [SerializeField] private GameObject i_mutedSoundEffects = null;
    [SerializeField] private GameObject i_unmutedSoundEffects = null;
    [SerializeField] private Button b_muteMusic = null;
    [SerializeField] private GameObject i_mutedMusic = null;
    [SerializeField] private GameObject i_unmutedMusic = null;
    [SerializeField] private Button b_backToMenu_settings = null;

    [Header("Store")]
    [SerializeField] private GameObject store = null;
    [SerializeField] private Button b_purchase_5gems = null;
    [SerializeField] private Button b_purchase_10gems = null;
    [SerializeField] private Button b_purchase_15gems = null;
    [SerializeField] private Button b_backToMenu_store = null;
    [SerializeField] private GameObject trasparentPanel_store = null;
    [SerializeField] private GameObject confirmPurchasePanel = null;
    [SerializeField] private GameObject confirm_5gems = null;
    [SerializeField] private GameObject confirm_10gems = null;
    [SerializeField] private GameObject confirm_15gems = null;
    [SerializeField] private Button b_cancel = null;
    [SerializeField] private Button b_confirm = null;
    [SerializeField] private GameObject thankYouForPurchase = null;
    [SerializeField] private Button b_back_store = null;

    [Header("Other")]
    [SerializeField] private GameObject disablePanel = null;

    [Header("LevelsButtons")]
    [SerializeField] private Button b_level0 = null;
    [SerializeField] private Button b_level1 = null;
    [SerializeField] private Button b_level2 = null;

    private float fadeOutTime = 1f;
    private bool isFadeOutFinished = false;
    private bool isFadeInFinished = false;

    private bool changeStateDisablePanel = false;

    private Localization.Language currentLanguage;

    private static int currentGems;
    public static bool soundEffectsMuted = false;
    public static bool musicMuted = false;

    private bool isInLevelsMap = false;
    private bool isInStore = false;

    [Header("Scripts")]
    [SerializeField] private SoundManager m_soundManager = null;

    #endregion

    #region LoadScene Methods
    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator LoadSceneAfterWait(string sceneName, float time)
    {
        yield return new WaitForSeconds(time);

        SceneManager.LoadScene(sceneName);
    }

    IEnumerator LoadMenuAfterWait(GameObject lastMenu, float time)
    {
        yield return new WaitForSeconds(time);

        goToLevelsMap(lastMenu);
    }
    #endregion  

    void Start()
    {
        AddListeners();
        AddSocialMediaButtonListeners();
        AddLevelButtonListeners();
        goToMainMenu(null);

        m_soundManager.Mute_Music("song_menu", musicMuted);
        m_soundManager.Play_Music("song_menu");
    }

    private void Update()
    {
        if (changeStateDisablePanel)
        {
            if (isFadeInFinished && isFadeOutFinished)
            {
                StopAllCoroutines();
                disablePanel.SetActive(false);
                changeStateDisablePanel = false;
            }
        }

        if (isInLevelsMap)
        {
            if (levelPanel.gameObject.activeSelf && levelPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("exitPanel"))
            {
                if (isInLevelsMap && levelPanel.GetComponent<CanvasGroup>().alpha <= 0)
                {
                    levelPanel.SetActive(false);
                }
            }
        }
    }

    #region MainMenu Methods
    private void goToMainMenu(GameObject lastMenu)
    {
        DeactivateActivateMenu(lastMenu, mainMenu);
    }
    
    private void OpenSocialMedia(string socialMediaUrl)
    {
        Application.OpenURL(socialMediaUrl);
    }

    private void AddSocialMediaButtonListeners()
    {
        b_twitter.onClick.AddListener(() => { PlaySoundEffect("click_button");  OpenSocialMedia("https://twitter.com/SabeltonStudios"); });
        b_instagram.onClick.AddListener(() => { PlaySoundEffect("click_button"); OpenSocialMedia("https://www.instagram.com/sabeltonstudios"); });
        b_youtube.onClick.AddListener(() => { PlaySoundEffect("click_button"); OpenSocialMedia("https://www.youtube.com/channel/UCaw0EJIphiofJF5lcD1SEJg"); });
        b_tiktok.onClick.AddListener(() => { PlaySoundEffect("click_button"); OpenSocialMedia("https://vm.tiktok.com/ZSnu8T8B/"); });
    }
    #endregion  

    #region LevelMap Methods
    private void goToLevelsMap(GameObject lastMenu)
    {
        DeactivateActivateMenu(lastMenu, levelsMap);

        isInLevelsMap = true;

        if (currentGems < 100)
        {
            t_currentGems.GetComponent<Text>().text = currentGems.ToString("00");
        }
        else
        {
            t_currentGems.GetComponent<Text>().text = currentGems.ToString();
        }

        b_backToMenu_levelsMap.gameObject.SetActive(true);
    }

    private void OpenLevelPanel(int levelNumber)
    {
        t_levelNumberTitle.GetComponent<Text>().text = Localization.GetLocalizedValue("t_level") + " " + levelNumber;
        ActivateLevelPanel(true);
        switch (levelNumber)
        {
            case 0:
                b_playLevel.onClick.AddListener(() => { PlaySoundEffect("click_button"); LoadScene("Level0"); });
                break;
            case 1:
                //b_playLevel.onClick.AddListener(() => { PlaySoundEffect("click_button"); LoadScene("Level1"); });
                break;
            case 2:
                //b_playLevel.onClick.AddListener(() => { PlaySoundEffect("click_button"); LoadScene("Level2"); });
                break;
        }
    }

    private void ActivateLevelPanel(bool state)
    {
        if (state)
        {
            levelPanel.SetActive(state);
        }
        transparentPanel.SetActive(state);
        levelPanel.GetComponent<Animator>().SetBool("isActive", state);
    }

    private void AddLevelButtonListeners()
    {
        b_level0.onClick.AddListener(() => { PlaySoundEffect("click_button"); OpenLevelPanel(0); });
        b_level1.onClick.AddListener(() => { PlaySoundEffect("click_button"); OpenLevelPanel(1); });
        b_level2.onClick.AddListener(() => { PlaySoundEffect("click_button"); OpenLevelPanel(2); });
    }
    #endregion 

    #region Settings Methods
    private void goToSettings(GameObject lastMenu)
    {
        DeactivateActivateMenu(lastMenu, settings);
        b_backToMenu_settings.gameObject.SetActive(true);
    }

    private void updateSoundEffectsState()
    {
        if (soundEffectsMuted)
        {
            soundEffectsMuted = false;
            i_mutedSoundEffects.SetActive(false);
            i_unmutedSoundEffects.SetActive(true);
        }
        else
        {
            soundEffectsMuted = true;
            i_mutedSoundEffects.SetActive(true);
            i_unmutedSoundEffects.SetActive(false);
        }
    }

    private void updateMusicState()
    {
        if (musicMuted)
        {
            musicMuted = false;
            i_mutedMusic.SetActive(false);
            i_unmutedMusic.SetActive(true);
        }
        else
        {
            musicMuted = true;
            i_mutedMusic.SetActive(true);
            i_unmutedMusic.SetActive(false);
        }
        m_soundManager.Mute_Music("song_menu", musicMuted);
    }

    private void UpdateLanguage()
    {
        if (Localization.language == Localization.Language.English)
        {
            t_currentLanguage.GetComponent<Text>().text = "Español";
            Localization.SetLanguage(Localization.Language.Spanish);
        }
        else
        {
            t_currentLanguage.GetComponent<Text>().text = "English";
            Localization.SetLanguage(Localization.Language.English);
        }
        LanguageValue = Localization.language;
    }

    private Localization.Language LanguageValue
    {
        get { return currentLanguage; }
        set
        {
            if (currentLanguage == value) return;
            currentLanguage = value;
            if (OnVariableChangeEvent != null)
                OnVariableChangeEvent();
        }
    }
    public delegate void OnVariableChangeDelegate();
    public event OnVariableChangeDelegate OnVariableChangeEvent;
    #endregion 

    #region Store Methods
    private void goToStore(GameObject lastMenu)
    {
        isInStore = true;

        DeactivateActivateMenu(lastMenu, store);
        b_backToMenu_store.gameObject.SetActive(true);
    }

    private void OpenPurchasePanel(string purchaseType)
    {
        trasparentPanel_store.SetActive(true);
        confirmPurchasePanel.SetActive(true);
        confirm_5gems.SetActive(false);
        confirm_10gems.SetActive(false);
        confirm_15gems.SetActive(false);
        int gemsPurchased = 0;
        switch (purchaseType)
        {
            case "5_Gems":
                confirm_5gems.SetActive(true);
                gemsPurchased = 5;
                break;
            case "10_Gems":
                confirm_10gems.SetActive(true);
                gemsPurchased = 10;
                break;
            case "15_Gems":
                confirm_15gems.SetActive(true);
                gemsPurchased = 15;
                break;
        }
        b_confirm.onClick.AddListener(() => { PlaySoundEffect("click_button"); confirmPurchasePanel.SetActive(false); OpenThankYouPanel(gemsPurchased); });
    }

    private void OpenThankYouPanel(int gemsPurchased)
    {
        currentGems += gemsPurchased;
        thankYouForPurchase.SetActive(true);
    }
    #endregion 

    private void DeactivateActivateMenu(GameObject lastMenu, GameObject newMenu)
    {
        disablePanel.SetActive(true);
        changeStateDisablePanel = true;
        if (lastMenu != null)
        {
            StartCoroutine(FadeOutRoutine(lastMenu));
        }
        else
        {
            isFadeOutFinished = true;
        }
        StartCoroutine(FadeInRoutine(newMenu));
    }

    private IEnumerator FadeOutRoutine(GameObject menu)
    {
        float startAlpha;
        isFadeOutFinished = false;
        startAlpha = menu.GetComponent<CanvasGroup>().alpha;
        for (float t = 0.1f; t < fadeOutTime; t += Time.deltaTime)
        {
            float porcentaje = t / fadeOutTime;
            menu.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(startAlpha, 0f, porcentaje);
            yield return null;
        }
        menu.GetComponent<CanvasGroup>().alpha = 0f;
        menu.SetActive(false);
        isFadeOutFinished = true;
    }

    private IEnumerator FadeInRoutine(GameObject menu)
    {
        float startAlpha;
        isFadeInFinished = false;
        menu.SetActive(true);
        startAlpha = menu.GetComponent<CanvasGroup>().alpha;
        for (float t = 0.1f; t < fadeOutTime; t += Time.deltaTime)
        {
            float porcentaje = t / fadeOutTime;
            menu.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(startAlpha, 1f, porcentaje);
            yield return null;
        }
        menu.GetComponent<CanvasGroup>().alpha = 1f;
        isFadeInFinished = true;
    }

    private void AddListeners()
    {
        //Main Menu Buttons
        b_play.onClick.AddListener(() => { PlaySoundEffect("click_button"); StartCoroutine(LoadMenuAfterWait(mainMenu, 0.23f)); } );
        b_settings.onClick.AddListener(() => { PlaySoundEffect("click_button"); i_settings.GetComponent<Animator>().SetBool("onClick", true); goToSettings(mainMenu); });

        //Settings Buttons
        b_muteSoundEffects.onClick.AddListener(() => { PlaySoundEffect("click_button"); updateSoundEffectsState(); });
        b_muteMusic.onClick.AddListener(() => { PlaySoundEffect("click_button"); updateMusicState(); });
        b_backToMenu_settings.onClick.AddListener(() => { PlaySoundEffect("click_button"); goToMainMenu(settings); });
        b_language_left.onClick.AddListener(() => { PlaySoundEffect("click_button"); UpdateLanguage(); });
        b_language_right.onClick.AddListener(() => { PlaySoundEffect("click_button"); UpdateLanguage(); });

        //Level Map Buttons
        b_store.onClick.AddListener(() => { PlaySoundEffect("click_button"); goToStore(levelsMap); isInLevelsMap = false; });
        b_backToMenu_levelsMap.onClick.AddListener(() => { PlaySoundEffect("click_button"); goToMainMenu(levelsMap); isInLevelsMap = false; });
        b_backToLevelMap.onClick.AddListener(() => { PlaySoundEffect("click_button"); ActivateLevelPanel(false); });

        //Store Buttons
        b_purchase_5gems.onClick.AddListener(() => { PlaySoundEffect("click_button"); OpenPurchasePanel("5_Gems"); });
        b_purchase_10gems.onClick.AddListener(() => { PlaySoundEffect("click_button"); OpenPurchasePanel("10_Gems"); });
        b_purchase_15gems.onClick.AddListener(() => { PlaySoundEffect("click_button"); OpenPurchasePanel("15_Gems"); });
        b_cancel.onClick.AddListener(() => 
        {
            PlaySoundEffect("click_button");
            b_confirm.onClick.RemoveAllListeners();
            confirmPurchasePanel.SetActive(false);
            trasparentPanel_store.SetActive(false);
        });
        b_back_store.onClick.AddListener(() =>
        {
            PlaySoundEffect("click_button");
            b_confirm.onClick.RemoveAllListeners();
            thankYouForPurchase.SetActive(false);
            trasparentPanel_store.SetActive(false);
        });
        b_backToMenu_store.onClick.AddListener(() => { PlaySoundEffect("click_button"); goToLevelsMap(store); isInStore = false; });
    }

    private void PlaySoundEffect(string name)
    {
        if (!soundEffectsMuted)
        {
            m_soundManager.Play_SoundEffect(name);
        }
    }
}
