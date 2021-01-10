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

    [Header("Chance Piece Menu")]
    [SerializeField] private GameObject changePieceMenu = null;
    [SerializeField] private GameObject i_peon = null;
    [SerializeField] private GameObject i_caballo = null;
    [SerializeField] private GameObject i_torre = null;
    [SerializeField] private Button changePiece = null;
    [SerializeField] private Button changeToPeon = null;
    [SerializeField] private Button changeToCaballo = null;
    [SerializeField] private Button changeToTorre = null;
    private bool isClickingOnChangePieceButton = false;

    [Header("Victoria Menu")]
    [SerializeField] private GameObject victoriaMenu = null;
    [SerializeField] private GameObject transparentPanel = null;
    [SerializeField] private List<GameObject> estrellas = new List<GameObject>();
    [SerializeField] private Text t_numMov = null;
    [SerializeField] private Text t_mejorNumMov = null;
    [SerializeField] private Button b_volverMenu_victoria = null;
    [SerializeField] private Button b_siguienteNivel = null;

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

    private int movesLeft;
    private int undoCont;

    public static bool menuOpened = false;

    private float fadeOutTime = 1f;
    private bool isFadeOutFinished = false;
    private bool isFadeInFinished = false;

    private bool abrirPanelVictoria = true;
    private bool abrirPanelDerrota = true;

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
        PlayerData.Gems = 30;
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
            //t_undoCont.text = "" + (m_player.maxUndos - m_player.undoCont);
            t_undoCont.text = undoCont.ToString();
            AbrirMenuUndoMov(false);
            t_movesLeft_menuUndo.text = t_undoCont.text + " movimientos"; ////////////////TRADUCIR 
            PlayerData.Gems--;
            menuOpened = false;
        });

        //Change Piece Buttons
        changePiece.onClick.AddListener(() => {
            m_soundManager.Play_SoundEffect("click_button");
            showChangePieceMenu();
        });
        changeToPeon.onClick.AddListener(() => {
            m_soundManager.Play_SoundEffect("click_button");
            changePieceTo(TipoPieza.PEON);
            showChangePieceMenu();
        });
        changeToCaballo.onClick.AddListener(() => {
            m_soundManager.Play_SoundEffect("click_button");
            changePieceTo(TipoPieza.CABALLO);
            showChangePieceMenu();
        });
        changeToTorre.onClick.AddListener(() => {
            m_soundManager.Play_SoundEffect("click_button");
            changePieceTo(TipoPieza.TORRE);
            showChangePieceMenu();
        });

        //Menu Pausa Buttons
        b_pausa.onClick.AddListener(() => { m_soundManager.Play_SoundEffect("click_button"); AbrirMenuPausa(true); menuOpened = true; });
        b_reanudar.onClick.AddListener(() => { m_soundManager.Play_SoundEffect("click_button"); AbrirMenuPausa(false); menuOpened = false; });
        b_volverMenu_pausa.onClick.AddListener(() => { m_soundManager.Play_SoundEffect("click_button"); AbrirMenuConfirmacion(true); });

        //Panel Confirmacion Buttons
        b_si.onClick.AddListener(() => {
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
            PlayerData.NivelActual++;
            StartCoroutine(FadeInRoutine(sceneFadePanel));
            StartCoroutine(m_soundManager.SoundFadeOut("song_menu", 1.2f));
            StartCoroutine(LoadSceneAfterWait("Menus", 1.2f));
        });
        b_siguienteNivel.onClick.AddListener(() => {
            m_soundManager.Play_SoundEffect("click_button");
            PlayerData.NivelActual++;
            StartCoroutine(FadeInRoutine(sceneFadePanel));
            StartCoroutine(m_soundManager.SoundFadeOut("song_menu", 1.2f));
            StartCoroutine(LoadSceneAfterWait(SceneManager.GetActiveScene().buildIndex + 1, 1.2f));
        });

        //Derrota
        b_volverMenu_derrota.onClick.AddListener(() => {
            m_soundManager.Play_SoundEffect("click_button");
            StartCoroutine(FadeOutRoutine(sceneFadePanel));
            StartCoroutine(m_soundManager.SoundFadeOut("song_menu", 1.2f));
            StartCoroutine(LoadSceneAfterWait("Menus", 1.2f));
        });
        b_reintentarNivel.onClick.AddListener(() => {
            m_soundManager.Play_SoundEffect("click_button");
            StartCoroutine(FadeOutRoutine(sceneFadePanel));
            StartCoroutine(m_soundManager.SoundFadeOut("song_menu", 1.2f));
            StartCoroutine(LoadSceneAfterWait(SceneManager.GetActiveScene().name, 1.2f));
        });
    }

    private void Update()
    {
        UpdateArrowUp();
        UpdateArrowDown();

        if (!m_player.turno) //Después de cada turno se actualiza el num de movimientos
        {
            movesLeft = m_gameController.maxMovs - m_player.numMovs;
            //Debug.Log("movesLeft [" + movesLeft + "] = " + "m_gameController.maxMovs [" + m_gameController.maxMovs + "] - m_player.numMovs [ " + m_player.numMovs + " ]");
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

                UnableDisableChangePiece(true);
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
    }

    private void AbrirMenuPausa(bool state)
    {
        transparentPanel.SetActive(state);
        menuPausa.SetActive(state);
        if (state)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
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
        menuUndo.SetActive(state);
        if (PlayerData.Gems <= 0)
        {
            b_siDeshacer.enabled = false;
            b_siDeshacer.GetComponent<CanvasGroup>().alpha = 0.5f;
        }
    }

    private void AbrirPanelVictoria()
    {
        m_soundManager.Play_SoundEffect("victoria");
        menuOpened = true;
        transparentPanel.SetActive(true);
        victoriaMenu.SetActive(true);
        if (PlayerData.playingLevel == 0) //Si está en el tutorial, por completarlo se consiguen directamente 3 estrellas
            m_gameController.numStars = 3;
        ActualizarNumEstrellas();
        t_numMov.text = m_player.numMovs.ToString() + "movimientos"; ///////////////////actualizar "movimientos" para que se traduzca
        ActualizarPlayerData(); //Actualiza si se ha logrado mejor puntuacion (movimientos y estrellas)
    }

    private void UpdateBestScore(int mejorPuntuacion, int mejorCantidadEstrellas)
    {
        t_mejorNumMov.text = "Mejor : " + mejorPuntuacion + " movimientos"; ///////////////////actualizar "Mejor" y "movimientos" para que se traduzca
        if (m_player.numMovs > mejorPuntuacion)
        {
            mejorPuntuacion = m_player.numMovs;
        }
        if (m_gameController.numStars > mejorCantidadEstrellas)
        {
            mejorCantidadEstrellas = m_gameController.numStars;
        }
    }

    private void ActualizarPlayerData()
    {
        switch (PlayerData.playingLevel)
        {
            case 0:
                UpdateBestScore(PlayerData.Level0MejorPuntuacion, PlayerData.Level0Estrellas);
                break;
            case 1:
                UpdateBestScore(PlayerData.Level1MejorPuntuacion, PlayerData.Level1Estrellas);
                break;
            case 2:
                UpdateBestScore(PlayerData.Level2MejorPuntuacion, PlayerData.Level2Estrellas);
                break;
        }
    }

    private void AbrirPanelDerrota(int tipoDerrota)
    {
        m_soundManager.Play_SoundEffect("derrota");
        menuOpened = true;
        transparentPanel.SetActive(true);
        derrotaMenu.SetActive(true);

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
            arrowUp.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
        }
        else
        {
            if (m_cameraController.enabledMov)
            {
                arrowUp.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
            }
        }
    }

    private void UpdateArrowDown()
    {
        if (m_cameraController.checkIfCanTurnDown())
        {
            arrowDown.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
        }
        else
        {
            if (m_cameraController.enabledMov)
            {
                arrowDown.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
            }
        }
    }

    private void showChangePieceMenu()
    {
        if (changePieceMenu.activeSelf)
        {
            changePieceMenu.SetActive(false);
        }
        else
        {
            changePieceMenu.SetActive(true);
        }
    }

    public void changePieceTo(TipoPieza tipoPieza)
    {
        i_peon.SetActive(false);
        i_caballo.SetActive(false);
        i_torre.SetActive(false);
        changeToPeon.enabled = true;
        changeToCaballo.enabled = m_gameController.horseUnlock;
        changeToTorre.enabled = m_gameController.castleUnlock;
        changeToPeon.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
        changeToCaballo.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
        changeToTorre.GetComponent<Image>().color = new Color(255, 255, 255, 1f);

        switch (tipoPieza)
        {
            case TipoPieza.PEON:
                m_gameController.cambiaPieza(TipoPieza.PEON,false);
                i_peon.SetActive(true);
                changeToPeon.enabled = false;
                changeToPeon.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
                break;
            case TipoPieza.CABALLO:
                m_gameController.cambiaPieza(TipoPieza.CABALLO,false);
                i_caballo.SetActive(true);
                changeToCaballo.enabled = false;
                changeToCaballo.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
                break;
            case TipoPieza.TORRE:
                m_gameController.cambiaPieza(TipoPieza.TORRE,false);
                i_torre.SetActive(true);
                changeToTorre.enabled = false;
                changeToTorre.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
                break;
        }
    }

    private void UnableDisableUndoButton(bool state)
    {
        b_UndoMove.enabled = state;
        if (state)
            b_UndoMove.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
        else
            b_UndoMove.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
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
    

    public delegate void OnVariableChangeDelegate();
    public event OnVariableChangeDelegate OnVariableChangeEvent;
}
