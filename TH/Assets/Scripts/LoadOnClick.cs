using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadOnClick : MonoBehaviour {
    GameObject panelInitBet, panelGame;
    GameObject btnCheck, btnCall, btnRaise, btnFold, btnSurrender;

    public void Start() {
        panelInitBet = GameObject.FindGameObjectWithTag("PanelInitBet");
        
        btnCheck = GameObject.Find("btnCheck");
        btnCall = GameObject.Find("btnCall");
        btnRaise = GameObject.Find("btnRaise");
        btnFold = GameObject.Find("btnFold");
        btnSurrender = GameObject.Find("btnSurrender");

        if (btnCheck != null) {
            btnCheck.GetComponent<Button>().interactable = false;
            //btnCheck.GetComponentInChildren<Button>().colors.disabledColor = Color.gray;
        }
        if (btnCall != null) btnCall.GetComponent<Button>().interactable = false;
        if (btnFold != null) btnFold.GetComponent<Button>().interactable = false;
        if (btnSurrender != null) btnSurrender.GetComponent<Button>().interactable = false;

        panelGame = GameObject.Find("PanelGame");
        panelGame.SetActive(false);
    }

    public void LoadScene(int level) {
        Application.LoadLevel(level);
    }

    public void btnStartGame() {
        GameObject btnRepeatBet = GameObject.Find("btnRepeatBet");
        if (btnRepeatBet != null) btnRepeatBet.SetActive(false);
        GameObject lblPanelBet = GameObject.Find("lblPanelBet");
        if (lblPanelBet != null) lblPanelBet.SetActive(false);

        //panelInitBet.hideFlags = HideFlags.HideAndDontSave;
        panelInitBet.SetActive(false);
        panelGame.SetActive(true);
        
        //Debug.Log("btn start game" + panelInitBet.name);
    }

    public void btnMinBetClick() {
        //Debug.Log("btn min bet");
    }

    public void btnRaiseClick() {
        panelInitBet.SetActive(true);
        panelGame.SetActive(false);
    }

}
