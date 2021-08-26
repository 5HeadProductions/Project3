using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletBorder : MonoBehaviourPun
{
    void OnTriggerEnter2D(Collider2D other){

        if(other.CompareTag("Bullet") && PhotonNetwork.OfflineMode == false){
            other.GetComponent<PhotonView>().RPC("DeleteBullet",RpcTarget.All,other.GetComponent<PhotonView>().ViewID);
        }
        else{
            if(other.gameObject.tag == "Bullet") Destroy(other.gameObject);
        }
    }

    [PunRPC]
    private void DeleteBullet(int viewID){
        PhotonView temp = PhotonView.Find(viewID);
            if(temp != null)
            Destroy(temp.gameObject);
    }
}
