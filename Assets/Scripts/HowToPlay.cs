using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlay : MonoBehaviour
{

    // this script is used just to control the "how to play" text animation
    [SerializeField]Animator animator;
    void Start()
    {   
        StartCoroutine(GrowText());
        
    }
    public IEnumerator GrowText(){
        yield return new WaitForSeconds(30);
        animator.SetTrigger("Play");

    }
}
