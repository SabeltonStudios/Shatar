using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    //Camera Controller Arrows
    [SerializeField] private GameObject arrowUp = null;
    [SerializeField] private GameObject arrowDown = null;

    //Menu Pausa
    [SerializeField] private Button b_pausa = null;
    [SerializeField] private GameObject menuPausa = null;
    [SerializeField] private Button b_reanudar = null;
    [SerializeField] private Button b_volverMenu_pausa = null;
    [SerializeField] private GameObject transparentPanelMenu = null;
    [SerializeField] private GameObject panelConfirmacionSalir = null;
    [SerializeField] private Button b_si = null;
    [SerializeField] private Button b_no = null;

    //Undo Move
    [SerializeField] private Text t_movesLeft = null;
    [SerializeField] private Text t_undoCont = null;
    [SerializeField] private Button b_UndoMove = null;

    //Undo Menu
    [SerializeField] private GameObject menuUndo = null;
    [SerializeField] private Text t_movesLeft_menuUndo = null;
    [SerializeField] private Button b_siDeshacer = null;
    [SerializeField] private Button b_noDeshacer = null;

    //Change Piece Menu
    [SerializeField] private GameObject changePieceMenu = null;
    [SerializeField] private GameObject i_peon = null;
    [SerializeField] private GameObject i_caballo = null;
    [SerializeField] private GameObject i_torre = null;
    [SerializeField] private Button changePiece = null;
    [SerializeField] private Button changeToPeon = null;
    [SerializeField] private Button changeToCaballo = null;
    [SerializeField] private Button changeToTorre = null;
    private bool isClickingOnChangePieceButton = false;

    //Victoria Menu
    [SerializeField] private GameObject victoriaMenu = null;
    [SerializeField] private GameObject transparentPanel = null;
    [SerializeField] private List<GameObject> estrellas = new List<GameObject>();
    [SerializeField] private Text t_numMov = null;
    [SerializeField] private Text t_mejorNumMov = null;
    [SerializeField] private Button b_volverMenu_victoria = null;
    [SerializeField] private Button b_siguienteNivel = null;

    //Derrota Menu
    [SerializeField] private GameObject derrotaMenu = null;
    [SerializeField] private GameObject t_sinMov = null;
    [SerializeField] private GameObject t_alcanzEnemigo = null;
    [SerializeField] private Button b_volverMenu_derrota = null;
    [SerializeField] private Button b_reintentarNivel = null;

    //Scripts
    [SerializeField] private SoundManager m_soundManager = null;
    [SerializeField] private CameraController m_cameraController = null;
    [SerializeField] private GameController m_gameController = null;
    [SerializeField] private Player m_player = null;

    private int movesLeft;

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void Start()
    {
        movesLeft = m_gameController.maxMovs;
        t_movesLeft.text = movesLeft.ToString();

        //Undo Buttons
        b_UndoMove.onClick.AddListener(() => { AbrirMenuUndoMov(true); });
        b_noDeshacer.onClick.AddListener(() => { AbrirMenuUndoMov(false); });
        b_siDeshacer.onClick.AddListener(() => 
        {
            m_player.UndoMovement();
            t_undoCont.text = "" + (m_player.maxUndos - m_player.undoCont);
            AbrirMenuUndoMov(false);
            t_movesLeft_menuUndo.text = t_undoCont.text + " movimientos"; ////////////////TRADUCIR 
            ///////////////////////////RESTAR GEMAS
        });

        //Change Piece Buttons
        changePiece.onClick.AddListener(() => {
            showChangePieceMenu();
        });
        changeToPeon.onClick.AddListener(() => {
            changePieceTo(TipoPieza.PEON);
            showChangePieceMenu();
        });
        changeToCaballo.onClick.AddListener(() => {
            changePieceTo(TipoPieza.CABALLO);
            showChangePieceMenu();
        });
        changeToTorre.onClick.AddListener(() => {
            changePieceTo(TipoPieza.TORRE);
            showChangePieceMenu();
        });

        //Menu Pausa Buttons
        b_pausa.onClick.AddListener(() => AbrirMenuPausa(true));
        b_reanudar.onClick.AddListener(() => AbrirMenuPausa(false));
        b_volverMenu_pausa.onClick.AddListener(() => AbrirMenuConfirmacion(true));

        //Panel Confirmacion Buttons
        b_si.onClick.AddListener(() => LoadScene("Menus"));
        b_no.onClick.AddListener(() => AbrirMenuConfirmacion(false));

        m_soundManager.Mute_Music("song_menu", PlayerData.MusicMuted);
        m_soundManager.Play_Music("song_menu");
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
        }

        if (m_gameController.victoria)
        {
            AbrirPanelVictoria();
        }

        if (m_gameController.derrota > 0)
        {
            AbrirPanelDerrota(m_gameController.derrota);
        }

        /*
        if (Input.GetMouseButtonDown(0) && changePieceMenu.activeSelf) //salir del menú de elegir pieza haciendo click en cualquier parte de la pantalla
        {
            changePieceMenu.SetActive(false);
        }
        */

        if (movesLeft == m_gameController.maxMovs || m_player.undoCont > m_player.maxUndos)
        {
            b_UndoMove.enabled = false;
            b_UndoMove.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
        }
        else
        {
            if (movesLeft == m_gameController.maxMovs - 1)
            {
                b_UndoMove.enabled = true;
                b_UndoMove.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
            }
        }
    }

    private void AbrirMenuPausa(bool state)
    {
        transparentPanel.SetActive(state);
        menuPausa.SetActive(state);
    }

    private void AbrirMenuConfirmacion(bool state)
    {
        transparentPanel.SetActive(!state);
        transparentPanelMenu.SetActive(state);
        panelConfirmacionSalir.SetActive(state);
    }

    private void AbrirMenuUndoMov(bool state)
    {
        transparentPanel.SetActive(state);
        menuUndo.SetActive(state);
    }

    private void AbrirPanelVictoria()
    {
        transparentPanel.SetActive(true);
        victoriaMenu.SetActive(true);
        ActualizarNumEstrellas();
        t_numMov.text = m_player.numMovs.ToString() + "movimientos"; ///////////////////actualizar "movimientos" para que se traduzca
        //////////////////////////////////////////////////////////////ActualizarPlayerData(); //Actualiza mejores movimientos y estrellas
        b_volverMenu_victoria.onClick.AddListener(() => LoadScene("Menus")); /////////////Hacer que aparezca en levelMapMenu
        b_siguienteNivel.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    private void AbrirPanelDerrota(int tipoDerrota)
    {
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
        b_volverMenu_victoria.onClick.AddListener(() => LoadScene("Menus")); /////////////Hacer que aparezca en levelMapMenu
        b_reintentarNivel.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
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

    private void changePieceTo(TipoPieza tipoPieza)
    {
        i_peon.SetActive(false);
        i_caballo.SetActive(false);
        i_torre.SetActive(false);
        changeToPeon.enabled = true;
        changeToCaballo.enabled = true;
        changeToTorre.enabled = true;
        changeToPeon.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
        changeToCaballo.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
        changeToTorre.GetComponent<Image>().color = new Color(255, 255, 255, 1f);

        switch (tipoPieza)
        {
            case TipoPieza.PEON:
                m_gameController.cambiaPieza(TipoPieza.PEON);
                i_peon.SetActive(true);
                changeToPeon.enabled = false;
                changeToPeon.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
                break;
            case TipoPieza.CABALLO:
                m_gameController.cambiaPieza(TipoPieza.CABALLO);
                i_caballo.SetActive(true);
                changeToCaballo.enabled = false;
                changeToCaballo.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
                break;
            case TipoPieza.TORRE:
                m_gameController.cambiaPieza(TipoPieza.TORRE);
                i_torre.SetActive(true);
                changeToTorre.enabled = false;
                changeToTorre.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
                break;
        }
    }
}
