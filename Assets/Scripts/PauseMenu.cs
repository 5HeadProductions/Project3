using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private string _mainMenu = "MainMenu";
    private string _pauseCanvasName = "PauseCanvas";
    [SerializeField] Animator animator;

    public AudioManager _audio;

    public void Start(){
        _audio = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }
    public void ExitToMainMenu(){
        if(PhotonNetwork.OfflineMode == false){ // multiplayer
            PhotonNetwork.Disconnect();
            _audio.Play("ButtonClick");
        } 
        Time.timeScale = 1;  //resuming time when the player goes back to MAIN MENU
        _audio.Play("ButtonClick");
        animator.SetTrigger("Replay");
        StartCoroutine(FadeOut());
        StartCoroutine(Delay());
    }

    public void QuitGame(){
        _audio.Play("ButtonClick");
        PhotonNetwork.Disconnect();
        Application.Quit();
    }

    public void Back(){
        if(PhotonNetwork.OfflineMode == true){
            Time.timeScale = 1;        //resuming time when the player presses the back button
        }
        // NOT STOPPING TIME FOR MULTIPLAYER
        _audio.Play("ButtonClick");
        GameObject.Find(_pauseCanvasName).SetActive(false);
    }




    IEnumerator Delay(){
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(_mainMenu);
    }
    IEnumerator FadeOut(){ // fading out the Pause Menu/Death Canvas
        float counter = 0f;
        float transitionTime =1.5f;
        float start = 1.0f;
        float end = 0.0f;
          while(counter < transitionTime){
              counter += Time.deltaTime;
              this.gameObject.transform.parent.gameObject.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(start, end, counter / transitionTime); // lerp is cool. 
              yield return null;
           
          }

     }
    

}
