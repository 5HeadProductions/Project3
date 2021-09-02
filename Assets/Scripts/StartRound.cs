using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using Photon.Pun;
public class StartRound : MonoBehaviour
{

  [SerializeField]Animator animator;
  [SerializeField] MMFeedbacks clickSound;

  public void StartR(){
    if(PhotonNetwork.OfflineMode){
    clickSound?.PlayFeedbacks();
    animator.SetTrigger("FadeOut");
    EnemySpawner.Instance.BeginRound();
    }
    else{
      this.GetComponent<PhotonView>().RPC("OnlineStartR",RpcTarget.MasterClient);
      this.GetComponent<PhotonView>().RPC("BeginRound", RpcTarget.MasterClient);
    }
  }

  [PunRPC]
  public void OnlineStartR(){
    clickSound?.PlayFeedbacks();
    animator.SetTrigger("FadeOut");
  }
  [PunRPC]
  public void BeginRound(){
    EnemySpawner.Instance.BeginRound();
  }

}
