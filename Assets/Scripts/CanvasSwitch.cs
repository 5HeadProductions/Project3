using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CanvasSwitch : MonoBehaviour
{

    [SerializeField]private GameObject mmCanvas, playCanvas;
    [SerializeField]private CanvasGroup mmCanvasGroup, playCanvasGroup;
    [SerializeField]private Animator playAnimator,mmAnimator;

//player clicks on the play button
    public void SwitchToPlay(){ 
       playAnimator.SetTrigger("Start");
       mmAnimator.SetTrigger("Start");
     StartCoroutine(Fade());
       
    }

    //players clicks on the back button from Play Canvas
    public void SwitchToMM(){
        StopAllCoroutines();
      StartCoroutine(FadeIn());
        playAnimator.SetBool("Done", true);
      
 
    }

    IEnumerator Fade(){
         yield return new WaitForSeconds(1.5f);
          mmCanvas.SetActive(false);
       mmCanvasGroup.interactable = false;
    }

     IEnumerator FadeIn(){
          mmCanvas.SetActive(true);
         yield return new WaitForSeconds(1.5f);
          mmAnimator.SetBool("Done", true);
        mmCanvasGroup.interactable = false;
    }

}
