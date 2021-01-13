using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    [Header("Camera Controller Arrows")]
    [SerializeField] private GameObject arrowUp = null;
    [SerializeField] private GameObject arrowDown = null;

    [Header("Menu Pausa")]
    [SerializeField] private Button b_pausa = null;
    [SerializeField] private GameObject menuPausa = null;
    [SerializeField] private Button b_reanudar = null;
    [SerializeField] private Button b_volverMenu_pausa = null;
    [SerializeField] private GameObject transparentPanelMenu = null;
    [SerializeField] private GameObject panelConfirmacionSalir = null;
    [SerializeField] private Button b_si = null;
    [SerializeField] private Button b_no = null;

    [Header("Undo Moves")]
    [SerializeField] private Text t_movesLeft = null;
    [SerializeField] private Text t_undoCont = null;
    [SerializeField] private Text t_gemsNum = null;
    [SerializeField] private Button b_UndoMove = null;

    [Header("Undo Menu")]
    [SerializeField] private GameObject menuUndo = null;
    [SerializeField] private Text t_movesLeft_menuUndo = null;
    [SerializeField] private Button b_siDeshacer = null;
    [SerializeField] private Button b_noDeshacer = null;

    [Header("Change Piece Menu")]
    [SerializeField] private GameObject changePieceMenu = null;
    [SerializeField] private GameObject menuChangeBg = null;
    [SerializeField] private GameObject i_peon = null;
    [SerializeField] private GameObject i_caballo = null;
    [SerializeField] private GameObject i_torre = null;
    [SerializeField] private Button changePiece = null;
    [SerializeField] private Button changeToPeon = null;
    [SerializeField] private Button changeToCaballo = null;
    [SerializeField] private Button changeToTorre = null;

    [Header("Victoria Menu")]
    [SerializeField] private GameObject victoriaMenu = null;
    [SerializeField] private GameObject transparentPanel = null;
    [SerializeField] private List<GameObject> estrellas = new List<GameObject>();
    [SerializeField] private Text t_numMov = null;
    [SerializeField] private Text t_mejorNumMov = null;
    [SerializeField] private Button b_volverMenu_victoria = null;
    [SerializeField] private Button b_siguienteNivel = null;
    [SerializeField] private GameObject estrellasNecesarias = null;

    [Header("Derrota Menu")]
    [SerializeField] private GameObject derrotaMenu = null;
    [SerializeField] private GameObject t_sinMov = null;
    [SerializeField] private GameObject t_alcanzEnemigo = null;
    [SerializeField] private Button b_volverMenu_derrota = null;
    [SerializeField] private Button b_reintentarNivel = null;

    [Header("Scripts")]
    [SerializeField] private SoundManager m_soundManager = null;
    [SerializeField] private CameraController m_cameraController = null;
    [SerializeField] private GameController m_gameController = null;
    [SerializeField] private Player m_player = null;

    [Header("Other")]
    [SerializeField] private GameObject sceneFadePanel = null;
    [SerializeField] private GameObject disableChangePieceButton = null;
    [SerializeField] private GameObject disableChangePieceButton2 = null;

    private string levelName;

    private bool gamePaused = false;

    private int numEstrellasNecesarias;
    private bool puedePasarDeNivel = true;
    
    private float changePieceMenuBgStartPosRight;

    private int movesLeft;
    private int undoCont;

    public static bool menuOpened = false;

    private float fadeOutTime = 1f;

    private bool abrirPanelVictoria = true;
    private bool abrirPanelDerrota = true;

    private bool fadeAnimationOn = false;
    private bool bgMoveAnimationOn = false;

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator LoadSceneAfterWait(string sceneName, float time)
    {
        yield return new WaitForSeconds(time);

        SceneManager.LoadScene(sceneName);
    }

    IEnumerator LoadSceneAfterWait(int sceneIndex, float time)
    {
        yield return new WaitForSeconds(time);

        SceneManager.LoadScene(sceneIndex);
    }

    void Start()
    {
        menuOpened = false;

        PlayerData.backFromLevel = true;
        undoCont = 3;

        StartCoroutine(FadeOutRoutine(sceneFadePanel));
        movesLeft = m_gameController.maxMovs;
        t_movesLeft.text = movesLeft.ToString();

        //Undo Buttons
        b_UndoMove.onClick.AddListener(() => { m_soundManager.Play_SoundEffect("click_button"); AbrirMenuUndoMov(true); menuOpened = true; });
        b_noDeshacer.onClick.AddListener(() => { m_soundManager.Play_SoundEffect("click_button"); AbrirMenuUndoMov(false); menuOpened = false; });
        b_siDeshacer.onClick.AddListener(() => 
        {
            m_soundManager.Play_SoundEffect("click_button");
            m_player.UndoMovement();
            undoCont = (m_player.maxUndos - m_player.undoCont);
            t_undoCont.text = undoCont.ToString();
            AbrirMenuUndoMov(false);
            t_movesLeft_menuUndo.text = t_undoCont.text + " " + Localization.GetLocalizedValue("t_moves");
            PlayerData.Gems--;
            menuOpened = false;
        });

        changeToPeon.enabled = false;
        changeToCaballo.enabled = m_gameController.horseUnlock;
        changeToTorre.enabled = m_gameController.castleUnlock;

        changeToPeon.GetComponent<CanvasGroup>().alpha = 0f;
        changeToCaballo.GetComponent<CanvasGroup>().alpha = 0f;
        changeToTorre.GetComponent<CanvasGroup>().alpha = 0f;

        levelName = SceneManager.GetActiveScene().name;
        if (levelName == "Level2") //En este nivel se muestra la torre en el menú de cambiar pieza
        {
            changePieceMenuBgStartPosRight = 0f;
            changeToTorre.GetComponent<CanvasGroup>().alpha = 0f;
        }
        else
        {
            changePieceMenuBgStartPosRight = -113.77f;
            changeToTorre.gameObject.SetActive(false);
        }

        //Change Piece Buttons
        changePiece.onClick.AddListener(() => {
            m_soundManager.Play_SoundEffect("click_button");
            showChangePieceMenu();
        });
        changeToPeon.onClick.AddListener(() => {
            m_soundManager.Play_SoundEffect("click_button");
            changePieceTo(TipoPieza.PEON,false);
            showChangePieceMenu();
        });
        changeToCaballo.onClick.AddListener(() => {
            m_soundManager.Play_SoundEffect("click_button");
            if (TutorialMessages.instance)
            {
                int[] messages = { 10, 11, 12 };
                TutorialMessages.instance.ShowMessages(messages);
            }
            changePieceTo(TipoPieza.CABALLO,false);
            showChangePieceMenu();
        });
        changeToTorre.onClick.AddListener(() => {
            m_soundManager.Play_SoundEffect("click_button");
            changePieceTo(TipoPieza.TORRE,false);
            showChangePieceMenu();
        });

        //Menu Pausa Buttons
        b_pausa.onClick.AddListener(() => { m_soundManager.Play_SoundEffect("click_button"); AbrirMenuPausa(true); gamePaused = true; menuOpened = true; });
        b_reanudar.onClick.AddListener(() => { m_soundManager.Play_SoundEffect("click_button"); AbrirMenuPausa(false); gamePaused = false; menuOpened = false; });
        b_volverMenu_pausa.onClick.AddListener(() => { m_soundManager.Play_SoundEffect("click_button"); AbrirMenuConfirmacion(true); });

        //Panel Confirmacion Buttons
        b_si.onClick.AddListener(() => {
            gamePaused = false;
            m_soundManager.Play_SoundEffect("click_button");
            Time.timeScale = 1.0f;
            StartCoroutine(FadeInRoutine(sceneFadePanel));
            StartCoroutine(m_soundManager.SoundFadeOut("song_menu", 1.2f));
            StartCoroutine(LoadSceneAfterWait("Menus", 1.2f));
        });
        b_no.onClick.AddListener(() => { m_soundManager.Play_SoundEffect("click_button"); AbrirMenuConfirmacion(false); });

        m_soundManager.Mute_Music("song_menu", PlayerData.MusicMuted);
        m_soundManager.Play_Music("song_menu");

        //Victoria
        b_volverMenu_victoria.onClick.AddListener(() => {
            m_soundManager.Play_SoundEffect("click_button");
            StartCoroutine(FadeInRoutine(sceneFadePanel));
            //StartCoroutine(m_soundManager.SoundFadeOut("song_menu", 1.2f));
            StartCoroutine(LoadSceneAfterWait("Menus", 1.2f));
        });
        b_siguienteNivel.onClick.AddListener(() => {
            m_soundManager.Play_SoundEffect("click_button");
            StartCoroutine(FadeInRoutine(sceneFadePanel));
            //StartCoroutine(m_soundManager.SoundFadeOut("song_menu", 1.2f));
            if (puedePasarDeNivel)
            {
                StartCoroutine(LoadSceneAfterWait(SceneManager.GetActiveScene().buildIndex + 1, 1.2f));
            }
        });

        //Derrota
        b_volverMenu_derrota.onClick.AddListener(() => {
            m_soundManager.Play_SoundEffect("click_button");
            StartCoroutine(FadeOutRoutine(sceneFadePanel));
            //StartCoroutine(m_soundManager.SoundFadeOut("song_menu", 1.2f));
            StartCoroutine(LoadSceneAfterWait("Menus", 1.2f));
        });
        b_reintentarNivel.onClick.AddListener(() => {
            m_soundManager.Play_SoundEffect("click_button");
            StartCoroutine(FadeOutRoutine(sceneFadePanel));
            //StartCoroutine(m_soundManager.SoundFadeOut("song_menu", 1.2f));
            StartCoroutine(LoadSceneAfterWait(SceneManager.GetActiveScene().name, 1.2f));
        });

        UpdateChangePieceButtonsEnabled();
    }

    private void Update()
    {
        UpdateArrowUp();

        if (menuUndo.gameObject.activeSelf && menuUndo.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("popUpStore_exit"))
        {
            if (menuUndo.GetComponent<CanvasGroup>().alpha <= 0)
            {
                menuUndo.SetActive(false);
            }
        }
        if (!fadeAnimationOn && !bgMoveAnimationOn && AnimatorAnimationsHasFinished()) //Para prevenir que spawnee el botón del menú y funcionen mal las animaciones
        {
            disableChangePieceButton.SetActive(false);
            disableChangePieceButton2.SetActive(false);
        }
        else
        {
            disableChangePieceButton.SetActive(true);
            if (changeToPeon.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("peonOutside") ||
            changeToCaballo.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("caballoOutside"))
            {
                disableChangePieceButton2.SetActive(false);
            }
            else
            {
                disableChangePieceButton2.SetActive(true);
            }
        }

        if (!m_player.turno) //Después de cada turno se actualiza el num de movimientos
        {
            movesLeft = m_gameController.maxMovs - m_player.numMovs;
            t_movesLeft.text = movesLeft.ToString();

            UnableDisableChangePiece(false); //botones deshabilitados cuando no es el turno del jugador
            UnableDisableUndoButton(false);
        }
        else
        {
            if (!m_gameController.isPlayerMoving)
            {
                movesLeft = m_gameController.maxMovs - m_player.numMovs;
                t_movesLeft.text = movesLeft.ToString();
                if (!m_player.node.buttonGoal && !m_player.node.buttonCastle && !m_player.node.buttonHorse)
                {
                    UnableDisableChangePiece(true);
                }
                //if (movesLeft == m_gameController.maxMovs || m_player.undoCont > m_player.maxUndos)
                if (movesLeft == m_gameController.maxMovs || undoCont <= 0)
                {;
                    UnableDisableUndoButton(false);
                }
                else
                {
                    UnableDisableUndoButton(true);
                }
            }
            else
            {
                UnableDisableChangePiece(false); //botones deshabilitados cuando no es el turno del jugador
                UnableDisableUndoButton(false);
                if (changeToPeon.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("peonInside") ||
                changeToCaballo.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("caballoInside"))
                {
                    changePieceMenuAnimationExit();
                }
            }
        }

        if (m_gameController.victoria && abrirPanelVictoria)
        {
            abrirPanelVictoria = false;
            AbrirPanelVictoria();
        }

        if (m_gameController.derrota > 0 && abrirPanelDerrota)
        {
            abrirPanelDerrota = false;
            AbrirPanelDerrota(m_gameController.derrota);
        }

        if (gamePaused && menuPausa.activeSelf && menuPausa.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("pausaInside"))
        {
            Time.timeScale = 0f;
        }
        if (!gamePaused && menuPausa.activeSelf && menuPausa.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("pausaOutside"))
        {
            menuPausa.SetActive(false);
        }

        if (transparentPanel.activeSelf && transparentPanel.GetComponent<CanvasGroup>().alpha == 0)
        {
            transparentPanel.SetActive(false);
        }
    }

    private void AbrirMenuPausa(bool state)
    {
        if (state)
        {
            transparentPanel.SetActive(true);
            StartCoroutine(Fade(transparentPanel, 1f, 0.5f));
            menuPausa.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            StartCoroutine(Fade(transparentPanel, 0f, 0.5f));
        }
        menuPausa.GetComponent<Animator>().SetBool("isActive", state);
    }

    private void AbrirMenuConfirmacion(bool state)
    {
        transparentPanel.SetActive(!state);
        transparentPanelMenu.SetActive(state);
        panelConfirmacionSalir.SetActive(state);
    }

    private void AbrirMenuUndoMov(bool state)
    {
        t_gemsNum.text = PlayerData.Gems.ToString();
        transparentPanel.SetActive(state);
        transparentPanel.GetComponent<CanvasGroup>().alpha = 1f;
        if (state)
            menuUndo.SetActive(state);
        if (PlayerData.Gems <= 0)
        {
            b_siDeshacer.enabled = false;
            b_siDeshacer.GetComponent<CanvasGroup>().alpha = 0.5f;
        }
        menuUndo.GetComponent<Animator>().SetBool("isActive", state);
    }

    private void AbrirPanelVictoria()
    {
        if (AvanzaAlSiguienteNivel())
        {
            PlayerData.NivelActual++;
        }
        transparentPanel.SetActive(true);
        StartCoroutine(Fade(transparentPanel, 1f, 0.5f));
        StartCoroutine(m_soundManager.SoundFadeOut("song_menu", 1.2f));
        m_soundManager.Play_SoundEffect("victoria");
        menuOpened = true;
        victoriaMenu.SetActive(true);
        victoriaMenu.GetComponent<Animator>().SetBool("isActive", true);
        if (PlayerData.playingLevel == 0) //Si está en el tutorial, por completarlo se consiguen directamente 3 estrellas
            m_gameController.numStars = 3;
        ActualizarNumEstrellas();
        t_numMov.text = m_player.numMovs.ToString() + " " + Localization.GetLocalizedValue("t_moves");
        ActualizarPlayerData(); //Actualiza si se ha logrado mejor puntuacion (movimientos y estrellas)
        PlayerData.Stars = PlayerData.Level0Estrellas + PlayerData.Level1Estrellas + PlayerData.Level2Estrellas;
        switch (levelName) //Mirar si tiene estrellas suficientes para pasar al siguiente nivel
        {
            case "Tutorial":
                numEstrellasNecesarias = 3; //Pero siempre puede pasar de nivel así que esto no es necesario
                puedePasarDeNivel = true;
                break;
            case "Level1":
            case "Level2":
                numEstrellasNecesarias = 5;
                if (PlayerData.Stars < 5)
                    puedePasarDeNivel = false;
                else
                    puedePasarDeNivel = true;
                break;
        }
        if (!puedePasarDeNivel)
        {
            b_siguienteNivel.enabled = false;
            b_siguienteNivel.GetComponent<CanvasGroup>().alpha = 0.5f;
            estrellasNecesarias.GetComponent<Text>().text = numEstrellasNecesarias.ToString();
            estrellasNecesarias.SetActive(true);
        }
    }

    private int UpdateBestScore(int mejorPuntuacion)
    {
        if (m_player.numMovs < mejorPuntuacion)
        {
            mejorPuntuacion = m_player.numMovs;
        }
        else
        {
            if (mejorPuntuacion == 0)
            {
                mejorPuntuacion = m_player.numMovs;
            }
        }
        t_mejorNumMov.text = Localization.GetLocalizedValue("t_bestMov") + " " + mejorPuntuacion + " " + Localization.GetLocalizedValue("t_moves");
        return mejorPuntuacion;
    }

    private void ActualizarPlayerData()
    {
        switch (PlayerData.playingLevel)
        {
            case 0:
                PlayerData.Level0MejorPuntuacion = UpdateBestScore(PlayerData.Level0MejorPuntuacion);
                if (m_gameController.numStars > PlayerData.Level0Estrellas)
                {
                    PlayerData.Level0Estrellas = m_gameController.numStars;
                }
                break;
            case 1:
                PlayerData.Level1MejorPuntuacion = UpdateBestScore(PlayerData.Level1MejorPuntuacion);
                if (m_gameController.numStars > PlayerData.Level1Estrellas)
                {
                    PlayerData.Level1Estrellas = m_gameController.numStars;
                }
                break;
            case 2:
                PlayerData.Level2MejorPuntuacion = UpdateBestScore(PlayerData.Level2MejorPuntuacion);
                if (m_gameController.numStars > PlayerData.Level2Estrellas)
                {
                    PlayerData.Level2Estrellas = m_gameController.numStars;
                }
                break;
        }
    }

    private void AbrirPanelDerrota(int tipoDerrota)
    {
        transparentPanel.SetActive(true);
        StartCoroutine(Fade(transparentPanel, 1f, 0.5f));
        StartCoroutine(m_soundManager.SoundFadeOut("song_menu", 1.2f));
        m_soundManager.Play_SoundEffect("derrota");
        menuOpened = true;
        derrotaMenu.SetActive(true);
        derrotaMenu.GetComponent<Animator>().SetBool("isActive", true);

        t_sinMov.SetActive(false);
        t_alcanzEnemigo.SetActive(false);

        switch (tipoDerrota)
        {
            case 1: //Sin movimientos
                t_sinMov.SetActive(true);
                break;
            case 2: //Alcanzado por el enemigo
                t_alcanzEnemigo.SetActive(true);
                break;
        }
    }

    private void ActualizarNumEstrellas()
    {
        for (int i = 0; i < m_gameController.numStars; i++)
        {
            estrellas[i].SetActive(true);
        }
    }

    private void UpdateArrowUp()
    {
        if (m_cameraController.checkIfCanTurnUp())
        {
            arrowUp.SetActive(true);
            arrowDown.SetActive(false);
        }
        else
        {
            if (m_cameraController.enabledMov)
            {
                arrowUp.SetActive(false);
                arrowDown.SetActive(true);
            }
        }
    }

    private IEnumerator BgMoveAnimation(float endPos, float time)
    {
        bgMoveAnimationOn = true;
        Vector2 startPosition;
        RectTransform rt = menuChangeBg.GetComponent<RectTransform>();
        startPosition = rt.offsetMax;
        for (float t = 0.1f; t < time; t += Time.deltaTime)
        {
            float porcentaje = t / time;
            rt.offsetMax = Vector2.Lerp(rt.offsetMax, new Vector2(endPos, rt.offsetMax.y), porcentaje);
            yield return null;
        }
        bgMoveAnimationOn = false;
    }

    private void changePieceMenuAnimationEnter()
    {
        StartCoroutine(BgMoveAnimation(changePieceMenuBgStartPosRight, 1f));
        changeToPeon.GetComponent<Animator>().SetBool("isActive", true);
        StartCoroutine(Fade(changeToPeon.gameObject, 1f, 0.2f));
        changeToCaballo.GetComponent<Animator>().SetBool("isActive", true);
        StartCoroutine(Fade(changeToCaballo.gameObject, 1f, 0.2f));
        changeToTorre.GetComponent<Animator>().SetBool("isActive", true);
        StartCoroutine(Fade(changeToTorre.gameObject, 1f, 0.3f));
    }

    private void changePieceMenuAnimationExit()
    {
        StartCoroutine(BgMoveAnimation(-330, 1f));
        changeToTorre.GetComponent<Animator>().SetBool("isActive", false);
        StartCoroutine(Fade(changeToTorre.gameObject, 0f, 0.1f));
        changeToCaballo.GetComponent<Animator>().SetBool("isActive", false);
        StartCoroutine(Fade(changeToCaballo.gameObject, 0f, 0.2f));
        changeToPeon.GetComponent<Animator>().SetBool("isActive", false);
        StartCoroutine(Fade(changeToPeon.gameObject, 0f, 0.2f));
    }

    private void showChangePieceMenu()
    {
        changePieceMenu.SetActive(true);
        if (changeToPeon.GetComponent<Animator>().GetBool("isActive"))
        {
            //EXIT
            changePieceMenuAnimationExit();
        }
        else
        {
            //ENTER  
            changePieceMenuAnimationEnter();
        }
    }

    public void changePieceTo(TipoPieza tipoPieza, bool undo)
    {
        i_peon.SetActive(false);
        i_caballo.SetActive(false);
        i_torre.SetActive(false);
        switch (tipoPieza)
        {
            case TipoPieza.PEON:
                i_peon.SetActive(true);
                break;
            case TipoPieza.CABALLO:
                i_caballo.SetActive(true);
                break;
            case TipoPieza.TORRE:
                i_torre.SetActive(true);
                break;
        }
        if (!undo)
        {
            m_gameController.cambiaPieza(tipoPieza, false);
        }
        UpdateChangePieceButtonsEnabled();
    }

    public void UpdateChangePieceButtonsEnabled()
    {
        changeToPeon.enabled = true;
        changeToCaballo.enabled = m_gameController.horseUnlock;
        changeToTorre.enabled = m_gameController.castleUnlock;
        switch (m_player.tipoPieza)
        {
            case TipoPieza.PEON:
                changeToPeon.enabled = false;
                break;
            case TipoPieza.CABALLO:
                changeToCaballo.enabled = false;
                break;
            case TipoPieza.TORRE:
                changeToTorre.enabled = false;
                break;
        }
        if (changeToPeon.enabled)
            changeToPeon.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
        else
            changeToPeon.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
        if (changeToCaballo.enabled)
            changeToCaballo.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
        else
            changeToCaballo.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
        if (changeToTorre.enabled)
            changeToTorre.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
        else
            changeToTorre.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
    }

    private void UnableDisableUndoButton(bool state)
    {
        b_UndoMove.enabled = state;
        if (state)
            b_UndoMove.GetComponent<CanvasGroup>().alpha = 1f;
        else
            b_UndoMove.GetComponent<CanvasGroup>().alpha = 0.5f;
    }

    private void UnableDisableChangePiece(bool state)
    {
        changePiece.enabled = state;
        if (state)
            changePiece.GetComponent<CanvasGroup>().alpha = 1f;
        else
            changePiece.GetComponent<CanvasGroup>().alpha = 0.5f;
    }

    private IEnumerator FadeOutRoutine(GameObject menu)
    {
        float startAlpha;
        startAlpha = menu.GetComponent<CanvasGroup>().alpha;
        for (float t = 0.1f; t < fadeOutTime; t += Time.deltaTime)
        {
            float porcentaje = t / fadeOutTime;
            menu.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(startAlpha, 0f, porcentaje);
            yield return null;
        }
        menu.GetComponent<CanvasGroup>().alpha = 0f;
        menu.SetActive(false);
    }

    private IEnumerator FadeInRoutine(GameObject menu)
    {
        float startAlpha;
        menu.SetActive(true);
        startAlpha = menu.GetComponent<CanvasGroup>().alpha;
        for (float t = 0.1f; t < fadeOutTime; t += Time.deltaTime)
        {
            float porcentaje = t / fadeOutTime;
            menu.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(startAlpha, 1f, porcentaje);
            yield return null;
        }
        menu.GetComponent<CanvasGroup>().alpha = 1f;
    }

    private IEnumerator Fade(GameObject gameObject, float amount, float time)
    {
        fadeAnimationOn = true;
        float startAlpha;
        startAlpha = gameObject.GetComponent<CanvasGroup>().alpha;
        for (float t = 0.1f; t < time; t += Time.deltaTime)
        {
            float porcentaje = t / time;
            gameObject.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(startAlpha, amount, porcentaje);
            yield return null;
        }
        gameObject.GetComponent<CanvasGroup>().alpha = amount;
        fadeAnimationOn = false;
    }

    private bool AnimatorAnimationsHasFinished()
    {
        if (changeToPeon.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("peonExit") || changeToPeon.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("peonEnter") || changeToPeon.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("peonOutside"))
        {
            return false;
        }
        if (changeToCaballo.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("caballoExit") || changeToCaballo.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("caballoEnter") || changeToCaballo.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("caballoOutside"))
        {
            return false;
        }
        if (changeToTorre.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("torreExit") || changeToTorre.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("torreEnter") || changeToTorre.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("torreOutside"))
        {
            return false;
        }
        return true;
    }

    private bool AvanzaAlSiguienteNivel()
    {
        switch (PlayerData.playingLevel)
        {
            case 0:
                if (PlayerData.Level0MejorPuntuacion <= 0)
                    return true;
                else
                    return false;
            case 1:
                if (PlayerData.Level1MejorPuntuacion <= 0)
                    return true;
                else
                    return false;
            case 2:
                if (PlayerData.Level2MejorPuntuacion <= 0)
                    return true;
                else
                    return false;
                break;
        }
        return false;
    }
}
