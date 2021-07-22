using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CanvasSwitch : MonoBehaviour
{

    [SerializeField]private GameObject mmCanvas, playCanvas;
    [SerializeField]private CanvasGroup mmCanvasGroup, playCanvasGroup;
    [SerializeField]private Animator playAnimator,mmAnimator;
    public float transitionTime;
    private bool isFaded = false;
//player clicks on the play button
    public void SwitchToPlay(){ 
      // playAnimator.SetTrigger("Start");
      // mmAnimator.SetTrigger("Start");

      //FadeOut takes in the stating value, and an ending value
      // if faded is false then we want to go from 1(current alpha value) -> 0 (determined by the condition)
      // if faded is true we want to go from 0(current alpha value) -> 1(determined by the condition)
     StartCoroutine(FadeOut(mmCanvasGroup.alpha, isFaded ? 1 : 0));
     isFaded = !isFaded;  // updating if the canvas has faded or not
    }

    //players clicks on the back button from Play Canvas
    public void SwitchToMM(){
      StartCoroutine(FadeOut(mmCanvasGroup.alpha, isFaded ? 1 : 0));
      isFaded = !isFaded;
      //  playAnimator.SetBool("Done", true);
      
 
    }

    IEnumerator FadeOut(float start, float end){

          if(end == 0)playCanvas.SetActive(true); //back
          if(end == 1)mmCanvas.SetActive(true);//play
          float counter = 0f;
          while(counter < transitionTime){
              counter += Time.deltaTime;
              mmCanvasGroup.alpha = Mathf.Lerp(start, end, counter / transitionTime);
              yield return null;

          }
         //  if(end == 0)playCanvas.GetComponent<Canvas>().sortingOrder = 1; //back
         //  if(end == 0)mmCanvas.GetComponent<Canvas>().sortingOrder = 0;//back
           if(end == 0)mmCanvas.SetActive(false);//back

         //  if(end == 1)mmCanvas.GetComponent<Canvas>().sortingOrder = 1;//play
         //  if(end == 1)playCanvas.GetComponent<Canvas>().sortingOrder = 0;//play
           if(end == 1)playCanvas.SetActive(false);//play

    }

    // IEnumerator FadeIn(){
    //      mmCanvas.SetActive(true);

   // }

}
