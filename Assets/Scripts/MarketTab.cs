using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketTab : MonoBehaviour
{
    public Animator animator;
    
    private bool _tabOpen = false;

    public void OpenTab(){
        
        if(!_tabOpen){
        animator.SetBool("Activated",true);
        _tabOpen = true;
        }
        else{
            animator.SetBool("Activated",false);
            _tabOpen = false;
        }
    }
}
