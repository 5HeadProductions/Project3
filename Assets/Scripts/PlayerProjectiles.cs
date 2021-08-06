using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using Photon.Pun;

public class PlayerProjectiles : MonoBehaviour
{
    //Plan for this script is that it takes a scriptable object that will have the info of damage
    public Projectile projectile;
    
    public MMFeedbacks OnInstantiation;
   

    void Start(){
        transform.Rotate(0,0,90);
        OnInstantiation.Initialization(this.gameObject);
        
        OnInstantiation?.PlayFeedbacks();
    }
    
    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Suicide"){
            EnemySpawner.Instance.UpdateEnemyTracker();
            other.gameObject.SetActive(false);
            Destroy(gameObject);

        }
     }
   
   [PunRPC]
    public void DestroyGameObject(int gameObjectViewID){
            PhotonView temp = PhotonView.Find(gameObjectViewID);
            if(temp != null)
            Destroy(temp.gameObject);
    }
}
