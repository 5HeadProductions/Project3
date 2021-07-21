using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CanvasSwitch : MonoBehaviour
{

    [SerializeField]private Animator playAnimator;
    [SerializeField]private Animator mmAnimator;
 //   public CanvasGroup mmCanvas, playCanvas;

    public void SwitchToPlay(){
        playAnimator.SetTrigger("Start");
        mmAnimator.SetTrigger("Start");

    //     animator.SetBool("Done", true);
   //     mmCanvas.alpha = 0;
   //     playCanvas.alpha = 1;
    }
    public void SwitchToMM(){
//animator.SetBool("Done", true);
   //     playCanvas.alpha = 0;
   //     mmCanvas.alpha = 1;
    }



}
