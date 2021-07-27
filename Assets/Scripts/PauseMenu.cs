using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private string _mainMenu = "MainMenu";
    private string _pauseCanvasName = "PauseCanvas";
    public void ExitToMainMenu(){
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(_mainMenu);
    }

    public void QuitGame(){
        PhotonNetwork.Disconnect();
        Application.Quit();
    }

    public void Back(){
        GameObject.Find(_pauseCanvasName).SetActive(false);
    }
    

}
