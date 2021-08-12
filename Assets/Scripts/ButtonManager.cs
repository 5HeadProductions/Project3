using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ButtonManager : MonoBehaviour
{
    //MainMenu
    private GameObject canvasSwitch;

    public Animator animator;
    ///////////////////
    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu"){
            canvasSwitch = GameObject.Find("Canvas Switch");//use the methods in this gameobject
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HowToPlay(){
        Debug.Log("CHANGING SCENE TO HOW TO PLAY");
    }
    public void PlayButton(){
        canvasSwitch.GetComponent<CanvasSwitch>().SwitchToPlay();
        
    }

    public void BackButton(){
        canvasSwitch.GetComponent<CanvasSwitch>().SwitchToMM();
    }
    
    public void ExitButton(){
        Debug.Log("APPLICATION QUITING");
        Application.Quit();
    }

    public void LoadingScene(){
        SceneManager.LoadScene("LoadingScene");
    }

    public void LoadSinglePlayer(){
        animator.SetTrigger("Close");
        StartCoroutine(Delay());
        
    }

    private IEnumerator Delay(){
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("SinglePlayerScene");
    }
}
