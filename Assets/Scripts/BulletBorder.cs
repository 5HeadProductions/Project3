using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletBorder : MonoBehaviourPun
{
    void OnTriggerEnter2D(Collider2D other){

        if(other.CompareTag("Bullet") && PhotonNetwork.OfflineMode == false){
            other.GetComponent<PhotonView>().RPC("DestroyGameObject",RpcTarget.All,other.GetComponent<PhotonView>().ViewID);
        }
        else{
            Destroy(other.gameObject);
        }
    }
}
