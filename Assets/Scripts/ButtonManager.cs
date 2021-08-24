using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
public class ButtonManager : MonoBehaviour
{
    //MainMenu
    private GameObject canvasSwitch;

    public Animator animator;
    private AudioManager _audio;
    ///////////////////
    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu"){
            canvasSwitch = GameObject.Find("Canvas Switch");//use the methods in this gameobject
        }
        _audio = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        
    }

    public void HowToPlay(){
        PhotonNetwork.OfflineMode = true;
        _audio.Play("ButtonClick");
        SceneManager.LoadScene("HowToPlay");
    }
    public void PlayButton(){
        _audio.Play("ButtonClick");
        canvasSwitch.GetComponent<CanvasSwitch>().SwitchToPlay();
        
    }

    public void BackButton(){
        _audio.Play("ButtonClick");
        canvasSwitch.GetComponent<CanvasSwitch>().SwitchToMM();
    }
    
    public void ExitButton(){
        _audio.Play("ButtonClick");
        Application.Quit();
    }

    public void LoadingScene(){
        _audio.Play("ButtonClick");
        PhotonNetwork.OfflineMode = false;
        SceneManager.LoadScene("LoadingScene");
    }

    public void LoadSinglePlayer(){ //scene loaded in Delay function
        _audio.Play("ButtonClick");
        animator.SetTrigger("Close");
        StartCoroutine(Delay());
        
    }

    private IEnumerator Delay(){
        yield return new WaitForSeconds(2);
        PhotonNetwork.OfflineMode = true;
        SceneManager.LoadScene("SinglePlayerScene");
    }
}
