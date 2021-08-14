using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class StartRound : MonoBehaviour
{

  [SerializeField]Animator animator;
  [SerializeField] MMFeedbacks clickSound;

  public void StartR(){
    clickSound?.PlayFeedbacks();
    animator.SetTrigger("FadeOut");
    EnemySpawner.Instance.BeginRound();
  }
}
