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
        if(PhotonNetwork.OfflineMode == false) PhotonNetwork.Disconnect();
        Time.timeScale = 1;  //resuming time when the player goes back to MAIN MENU
        SceneManager.LoadScene(_mainMenu);
    }

    public void QuitGame(){
        PhotonNetwork.Disconnect();
        Application.Quit();
    }

    public void Back(){
        if(PhotonNetwork.OfflineMode == true){
            Time.timeScale = 1;        //resuming time when the player presses the back button
        }
        GameObject.Find(_pauseCanvasName).SetActive(false);
    }
    

}
