using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
    //[SerializeField] private ScrollRect scrollView = null;
    //[SerializeField] private List<GameObject> niveles = new List<GameObject>();
    [SerializeField] private List<GameObject> selectedLevel_stars = new List<GameObject>();
    [SerializeField] private Button b_store = null;
    [SerializeField] private GameObject i_store = null;
    [SerializeField] private GameObject levelPanel = null;
    [SerializeField] private List<GameObject> levelsEstrellasNecesarias = new List<GameObject>();
    [SerializeField] private GameObject t_noTienesEstrellasNecesarias;
    [SerializeField] private Text t_levelMejorMov = null;
    [SerializeField] private GameObject transparentPanel = null;
    [SerializeField] private Text t_levelNumberTitle = null;
    [SerializeField] private Text t_currentGems = null;
    [SerializeField] private Text t_currentStars = null;
    [SerializeField] private Button b_playLevel = null;
    [SerializeField] private Button b_backToLevelMap = null;
    [SerializeField] private Button b_backToMenu_levelsMap = null;

    //Coming soon
    [SerializeField] private GameObject levelPanelComingSoon = null;
    [SerializeField] private Button b_twitterComingSoon = null;
    [SerializeField] private Button b_instagramComingSoon = null;
    [SerializeField] private Button b_youtubeComingSoon = null;
    [SerializeField] private Button b_tiktokComingSoon = null;
    [SerializeField] private Button b_backToLevelMapComingSoon = null;

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
    [SerializeField] private GameObject sceneFadePanel = null;

    [Header("LevelsButtons")]
    [SerializeField] private List<Button> b_levels = new List<Button>();
    [SerializeField] private List<GameObject> level0_stars = new List<GameObject>();
    [SerializeField] private List<GameObject> level1_stars = new List<GameObject>();
    [SerializeField] private List<GameObject> level2_stars = new List<GameObject>();
    [SerializeField] private Sprite starComplete = null;
    [SerializeField] private Sprite starIncomplete = null;
    [SerializeField] private Sprite lockedLevelSprite = null;
    [SerializeField] private Sprite unlockedLevelSprite = null;

    private float fadeOutTime = 1f;
    private bool isFadeOutFinished = false;
    private bool isFadeInFinished = false;

    private bool changeStateDisablePanel = false;

    private Localization.Language currentLanguage;

    //private static int currentGems = 0;
    public static bool soundEffectsMuted = false;
    public static bool musicMuted = false;

    private bool isInLevelsMap = false;
    private bool isInStore = false;

    private float initialPosY;
    private float initialPosZ;
    private float initialDifference;

    private int numLevels = 3;

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
        StartCoroutine(FadeOutRoutine(sceneFadePanel));

        AddListeners();
        AddSocialMediaButtonListeners(b_twitter, b_instagram, b_youtube, b_tiktok);
        AddLevelButtonListeners();

        /*
        PlayerData.NivelActual = 0;
        PlayerData.Gems = 0;
        PlayerData.Stars = 0;
        PlayerData.Level0Estrellas = 0;
        PlayerData.Level1Estrellas = 0;
        PlayerData.Level2Estrellas = 0;
        PlayerData.Level0MejorPuntuacion = 0;
        PlayerData.Level1MejorPuntuacion = 0;
        PlayerData.Level2MejorPuntuacion = 0;
        */

        UpdateAvailableLevelsSprites();
        UpdateCurrentStars();

        if (!PlayerData.backFromLevel)
        {
            goToMainMenu(null);
        }
        else
        {
            PlayerData.backFromLevel = false;
            //scrollView.verticalNormalizedPosition = (niveles[0].GetComponent<RectTransform>().anchoredPosition.y) / 217;
            goToLevelsMap(null);
        }

        //REVISAR ESTO
        ///VVVV Creo que esto no hace falta
        if (!PlayerPrefs.HasKey("musicMuted"))
        {
            MuteMusic(false);
        }
        if (!PlayerPrefs.HasKey("soundEffectsMuted"))
        {
            MuteSoundEffects(false);
        }
        MuteMusic(PlayerData.MusicMuted);
        MuteSoundEffects(PlayerData.SoundEffectsMuted);
        
        //Ver en qué idioma estaba configurado el juego
        if (PlayerPrefs.GetInt("language", 0) == 0)
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

        m_soundManager.Mute_Music("song_menu", PlayerData.MusicMuted);
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
            if (levelPanelComingSoon.gameObject.activeSelf && levelPanelComingSoon.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("exitPanel"))
            {
                if (isInLevelsMap && levelPanelComingSoon.GetComponent<CanvasGroup>().alpha <= 0)
                {
                    levelPanelComingSoon.SetActive(false);
                }
            }
            if (transparentPanel.gameObject.activeSelf && transparentPanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("transparentPanel_exit"))
            {
                if (transparentPanel.GetComponent<CanvasGroup>().alpha <= 0)
                {
                    transparentPanel.SetActive(false);
                }
            }
        }

        if (isInStore)
        {
            if (confirmPurchasePanel.gameObject.activeSelf && confirmPurchasePanel.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("popUpStore_exit"))
            {
                if (confirmPurchasePanel.GetComponent<CanvasGroup>().alpha <= 0)
                {
                    confirmPurchasePanel.SetActive(false);
                }
            }
            else
            {
                if (thankYouForPurchase.gameObject.activeSelf && thankYouForPurchase.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("popUpStore_exit"))
                {
                    if (thankYouForPurchase.GetComponent<CanvasGroup>().alpha <= 0)
                    {
                        thankYouForPurchase.SetActive(false);
                    }
                }
            }
            if (trasparentPanel_store.gameObject.activeSelf && trasparentPanel_store.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("transparentPanel_exit"))
            {
                if (trasparentPanel_store.GetComponent<CanvasGroup>().alpha <= 0)
                {
                    trasparentPanel_store.SetActive(false);
                }
            }
        }
    }

    #region MainMenu Methods
    private void goToMainMenu(GameObject lastMenu)
    {
        DeactivateActivateMenu(lastMenu, mainMenu);
    }

    private void AddSocialMediaButtonListeners(Button twitter, Button instagram, Button youtube, Button tiktok)
    {
        twitter.onClick.AddListener(() => { PlaySoundEffect("click_button"); Application.ExternalEval("window.open('https://twitter.com/SabeltonStudios','Sabelton Twitter')"); });
        instagram.onClick.AddListener(() => { PlaySoundEffect("click_button"); Application.ExternalEval("window.open('https://www.instagram.com/sabeltonstudios','Sabelton Instagram')"); });
        youtube.onClick.AddListener(() => { PlaySoundEffect("click_button"); Application.ExternalEval("window.open('https://www.youtube.com/channel/UCaw0EJIphiofJF5lcD1SEJg','Sabelton Youtube')"); });
        tiktok.onClick.AddListener(() => { PlaySoundEffect("click_button"); Application.ExternalEval("window.open('https://vm.tiktok.com/ZSnu8T8B/','Sabelton TikTok')"); });
    }
    #endregion  

    #region LevelMap Methods
    private void goToLevelsMap(GameObject lastMenu)
    {
        DeactivateActivateMenu(lastMenu, levelsMap);

        isInLevelsMap = true;

        t_currentGems.text = PlayerData.Gems.ToString("00");
        
        b_backToMenu_levelsMap.gameObject.SetActive(true);
    }

    private void OpenLevelPanel(int levelNumber)
    {
        ActivateLevelPanel(true);
        t_levelNumberTitle.GetComponent<Text>().text = Localization.GetLocalizedValue("t_level") + " " + levelNumber;

        t_noTienesEstrellasNecesarias.SetActive(false);
        b_playLevel.enabled = true;
        b_playLevel.GetComponent<CanvasGroup>().alpha = 1f;
        b_playLevel.onClick.RemoveAllListeners();

        switch (levelNumber)
        {
            case 0:
                PlayerData.playingLevel = 0;
                UpdatePopUpLevelStars(PlayerData.Level0Estrellas, selectedLevel_stars);
                t_levelMejorMov.text = Localization.GetLocalizedValue("t_bestMov") + " " + PlayerData.Level0MejorPuntuacion.ToString() + " " + Localization.GetLocalizedValue("t_moves");
                b_playLevel.onClick.AddListener(() => {
                    PlaySoundEffect("click_button");
                    StartCoroutine(m_soundManager.SoundFadeOut("song_menu", 1.2f));
                    StartCoroutine(FadeInRoutine(sceneFadePanel));
                    StartCoroutine(LoadSceneAfterWait("Tutorial", 1.2f));
                });
                break;
            case 1:
                PlayerData.playingLevel = 1;
                UpdatePopUpLevelStars(PlayerData.Level1Estrellas, selectedLevel_stars);
                t_levelMejorMov.text = Localization.GetLocalizedValue("t_bestMov") + " " + PlayerData.Level1MejorPuntuacion.ToString() + " " + Localization.GetLocalizedValue("t_moves");
                if (PlayerData.Stars < 3) //Si jugador no tiene estrellas suficientes, deshabilitar botón y poner texto
                {
                    t_noTienesEstrellasNecesarias.SetActive(true);
                    b_playLevel.enabled = false;
                    b_playLevel.GetComponent<CanvasGroup>().alpha = 0.5f;
                }
                b_playLevel.onClick.AddListener(() => {
                    PlaySoundEffect("click_button");
                    StartCoroutine(m_soundManager.SoundFadeOut("song_menu", 1.2f));
                    StartCoroutine(FadeInRoutine(sceneFadePanel));
                    StartCoroutine(LoadSceneAfterWait("Level1", 1.2f));
                });
                break;
            case 2:
                PlayerData.playingLevel = 2;
                UpdatePopUpLevelStars(PlayerData.Level2Estrellas, selectedLevel_stars);
                t_levelMejorMov.text = Localization.GetLocalizedValue("t_bestMov") + " " + PlayerData.Level2MejorPuntuacion.ToString() + " " + Localization.GetLocalizedValue("t_moves");
                if (PlayerData.Stars < 5)
                {
                    t_noTienesEstrellasNecesarias.SetActive(true);
                    b_playLevel.enabled = false;
                    b_playLevel.GetComponent<CanvasGroup>().alpha = 0.5f;
                }
                b_playLevel.onClick.AddListener(() => {
                    PlaySoundEffect("click_button");
                    StartCoroutine(m_soundManager.SoundFadeOut("song_menu", 1.2f));
                    StartCoroutine(FadeInRoutine(sceneFadePanel));
                    StartCoroutine(LoadSceneAfterWait("Level2", 1.2f));
                });
                break;
        }
    }

    private void ActivateLevelPanel(bool state)
    {
        if (state)
        {
            b_store.enabled = false;
            i_store.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
            levelPanel.SetActive(state);
        }
        else
        {
            b_store.enabled = true;
            i_store.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
        }
        //transparentPanel.SetActive(state);
        if (!transparentPanel.activeSelf)
        {
            transparentPanel.SetActive(state);
        }
        transparentPanel.GetComponent<Animator>().SetBool("isActive", state);
        levelPanel.GetComponent<Animator>().SetBool("isActive", state);
    }

    private void ActivatePanel(GameObject panel, GameObject transparentPanel_Menu, bool state)
    {
        if (state)
        {
            panel.SetActive(state);
        }
        if (!transparentPanel_Menu.activeSelf)
        {
            transparentPanel_Menu.SetActive(state);
        }
        transparentPanel_Menu.GetComponent<Animator>().SetBool("isActive", state);
        panel.GetComponent<Animator>().SetBool("isActive", state);
    }

    private void AddLevelButtonListeners()
    {
        b_levels[0].onClick.AddListener(() => { PlaySoundEffect("click_button"); OpenLevelPanel(0); });
        b_levels[1].onClick.AddListener(() => { PlaySoundEffect("click_button"); OpenLevelPanel(1); });
        b_levels[2].onClick.AddListener(() => { PlaySoundEffect("click_button"); OpenLevelPanel(2); });
        b_levels[3].onClick.AddListener(() => { PlaySoundEffect("click_button"); ActivatePanel(levelPanelComingSoon, transparentPanel, true);});
    }

    private void UpdateAvailableLevelsSprites()
    {
        for (int i = 0; i <= PlayerData.NivelActual; i++)
        {
            b_levels[i].GetComponent<Image>().sprite = unlockedLevelSprite;
            b_levels[i].enabled = true;
            if (i > 0) { levelsEstrellasNecesarias[i].SetActive(true); }
            UpdateAllLevelStars(i);
        }
        for (int i = (PlayerData.NivelActual + 1); i < b_levels.Count; i++)
        {
            b_levels[i].GetComponent<Image>().sprite = lockedLevelSprite;
            b_levels[i].enabled = false;
        }

        if (PlayerData.NivelActual < numLevels && haCompletadoUltimoNivelDisponible()) //permitir comprar con estrellas siguiente nivel
        {
            b_levels[PlayerData.NivelActual + 1].enabled = true;
            levelsEstrellasNecesarias[PlayerData.NivelActual].SetActive(true);
        }
    }

    private void UpdateCurrentStars()
    {
        int currentStars = PlayerData.Level0Estrellas + PlayerData.Level1Estrellas + PlayerData.Level2Estrellas;
        PlayerData.Stars = currentStars;
        t_currentStars.text = PlayerData.Stars.ToString("00");
    }

    private bool haCompletadoUltimoNivelDisponible()
    {
        switch (PlayerData.NivelActual)
        {
            case 0:
                if (PlayerData.Level0MejorPuntuacion > 0)
                    return true;
                else
                    return false;
            case 1:
                if (PlayerData.Level1MejorPuntuacion > 0)
                    return true;
                else
                    return false;
            case 2:
                if (PlayerData.Level2MejorPuntuacion > 0)
                    return true;
                else
                    return false;
                break;
        }
        return false;
    }

    private void UpdateAllLevelStars(int numLevel)
    {
        switch (numLevel)
        {
            case 0:
                UpdateLevelStars(PlayerData.Level0Estrellas, level0_stars, PlayerData.Level0MejorPuntuacion);
                break;
            case 1:
                UpdateLevelStars(PlayerData.Level1Estrellas, level1_stars, PlayerData.Level1MejorPuntuacion);
                break;
            case 2:
                UpdateLevelStars(PlayerData.Level2Estrellas, level2_stars, PlayerData.Level2MejorPuntuacion);
                break;
        }
    }

    private void UpdateLevelStars(int numStars, List<GameObject> level_stars, int numMov)
    {
        if (numMov > 0) //Se actualizan las estrellas una vez haya completado el nivel
        {
            for (int i = 0; i < numStars; i++)
            {
                level_stars[i].SetActive(true);
            }
            for (int i = numStars; i < 3; i++)
            {
                level_stars[i].SetActive(true);
                level_stars[i].GetComponent<Image>().sprite = starIncomplete;
            }
        }
    }

    private void UpdatePopUpLevelStars(int numStars, List<GameObject> level_stars)
    {
        for (int i = 0; i < numStars; i++)
        {
            level_stars[i].SetActive(true);
            level_stars[i].GetComponent<Image>().sprite = starComplete;
        }
        for (int i = numStars; i < 3; i++)
        {
            level_stars[i].SetActive(false);
            //level_stars[i].GetComponent<Image>().sprite = starIncomplete;
        }
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
        if (PlayerData.SoundEffectsMuted)
        {
            PlayerData.SoundEffectsMuted = false;
            i_mutedSoundEffects.SetActive(false);
            i_unmutedSoundEffects.SetActive(true);
        }
        else
        {
            PlayerData.SoundEffectsMuted = true;
            i_mutedSoundEffects.SetActive(true);
            i_unmutedSoundEffects.SetActive(false);
        }
    }

    private void updateMusicState()
    {
        if (PlayerData.MusicMuted)
        {
            PlayerData.MusicMuted = false;
            i_mutedMusic.SetActive(false);
            i_unmutedMusic.SetActive(true);
        }
        else
        {
            PlayerData.MusicMuted = true;
            i_mutedMusic.SetActive(true);
            i_unmutedMusic.SetActive(false);
        }
        m_soundManager.Mute_Music("song_menu", PlayerData.MusicMuted);
    }

    private void MuteMusic(bool state)
    {
        PlayerData.MusicMuted = state;
        i_mutedMusic.SetActive(state);
        i_unmutedMusic.SetActive(!state);
    }

    private void MuteSoundEffects(bool state)
    {
        PlayerData.SoundEffectsMuted = state;
        i_mutedSoundEffects.SetActive(state);
        i_unmutedSoundEffects.SetActive(!state);
    }

    private void UpdateLanguage()
    {
        if (Localization.language == Localization.Language.English)
        {
            t_currentLanguage.GetComponent<Text>().text = "Español";
            Localization.SetLanguage(Localization.Language.Spanish);
            PlayerPrefs.SetInt("language", 0);
        }
        else
        {
            t_currentLanguage.GetComponent<Text>().text = "English";
            Localization.SetLanguage(Localization.Language.English);
            PlayerPrefs.SetInt("language", 1);
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
        //trasparentPanel_store.SetActive(true);
        //confirmPurchasePanel.SetActive(true);
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
        ActivatePanel(confirmPurchasePanel, trasparentPanel_store, true);
        if (PlayerData.Gems >= 999)
        {
            b_confirm.enabled = false;
        }
        else
        {
            b_confirm.enabled = true;
            b_confirm.onClick.AddListener(() => {
                PlaySoundEffect("click_button");
                confirmPurchasePanel.SetActive(false);
                PlaySoundEffect("comprar");
                OpenThankYouPanel(gemsPurchased);
            });
        }
    }

    private void OpenThankYouPanel(int gemsPurchased)
    {
        PlayerData.Gems += gemsPurchased;
        //PlayerData.Gems = currentGems;
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

        //Coming Soon
        AddSocialMediaButtonListeners(b_twitterComingSoon, b_instagramComingSoon, b_youtubeComingSoon, b_tiktokComingSoon);
        b_backToLevelMapComingSoon.onClick.AddListener(() => { PlaySoundEffect("click_button"); ActivatePanel(levelPanelComingSoon, transparentPanel, false); });

        //Store Buttons
        b_purchase_5gems.onClick.AddListener(() => { PlaySoundEffect("click_button"); OpenPurchasePanel("5_Gems"); });
        b_purchase_10gems.onClick.AddListener(() => { PlaySoundEffect("click_button"); OpenPurchasePanel("10_Gems"); });
        b_purchase_15gems.onClick.AddListener(() => { PlaySoundEffect("click_button"); OpenPurchasePanel("15_Gems"); });
        b_cancel.onClick.AddListener(() => 
        {
            PlaySoundEffect("click_button");
            b_confirm.onClick.RemoveAllListeners();
            //confirmPurchasePanel.SetActive(false);
            ActivatePanel(confirmPurchasePanel, trasparentPanel_store, false);
            //trasparentPanel_store.SetActive(false);
        });
        b_back_store.onClick.AddListener(() =>
        {
            PlaySoundEffect("click_button");
            b_confirm.onClick.RemoveAllListeners();
            //thankYouForPurchase.SetActive(false);
            //trasparentPanel_store.SetActive(false);
            ActivatePanel(thankYouForPurchase, trasparentPanel_store, false);
        });
        b_backToMenu_store.onClick.AddListener(() => { PlaySoundEffect("click_button"); goToLevelsMap(store); isInStore = false; });
    }

    private void PlaySoundEffect(string name)
    {
        if (!PlayerData.SoundEffectsMuted)
        {
            m_soundManager.Play_SoundEffect(name);
        }
    }
}
