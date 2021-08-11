using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRound : MonoBehaviour
{

  [SerializeField]Animator animator;

  public void StartR(){
    animator.SetTrigger("FadeOut");
    EnemySpawner.Instance.BeginRound();
  }
}
