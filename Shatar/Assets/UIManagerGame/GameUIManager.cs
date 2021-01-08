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

    //Undo Move
    [SerializeField] private Text t_movesLeft = null;
    [SerializeField] private Button b_UndoMove = null;

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

    //Scripts
    [SerializeField] private CameraController m_cameraController = null;
    [SerializeField] private GameController m_gameController = null;
    [SerializeField] private Player m_player = null;

    private int movesLeft;

    void Start()
    {
        movesLeft = m_gameController.maxMovs;
        t_movesLeft.text = movesLeft.ToString();
        b_UndoMove.onClick.AddListener(() => { m_player.UndoMovement(); });

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
    }

    private void Update()
    {
        UpdateArrowUp();
        UpdateArrowDown();

        if (!m_player.turno) //Después de cada turno se actualiza el num de movimientos
        {
            movesLeft = m_gameController.maxMovs - m_player.numMovs;
            Debug.Log("movesLeft [" + movesLeft + "] = " + "m_gameController.maxMovs [" + m_gameController.maxMovs + "] - m_player.numMovs [ " + m_player.numMovs + " ]");
            t_movesLeft.text = movesLeft.ToString();
        }

        /*
        if (Input.GetMouseButtonDown(0) && changePieceMenu.activeSelf)
        {
            changePieceMenu.SetActive(false);
        }
        */

        if (movesLeft == m_gameController.maxMovs)
        {
            b_UndoMove.enabled = false;
        }
        else
        {
            if (movesLeft == m_gameController.maxMovs - 1)
            {
                b_UndoMove.enabled = true;
            }
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
