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
    public MMFeedbacks OnCollision;
    public PlayerCoins PlayerCoins;
    private Color changeColor;
    public MMFeedbacks shipDeathFeedback;
 

   

    void Start(){
        PlayerCoins = GameObject.Find("GameManager").GetComponent<PlayerCoins>();
        transform.Rotate(0,0,90);
        OnInstantiation.Initialization(this.gameObject);
        OnInstantiation?.PlayFeedbacks();
    }
    
    // player shoots at the ship 
    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Suicide" || other.gameObject.tag == "Boss"){
            OnCollision?.PlayFeedbacks();
            PlayerCoins.AddCoinsToPlayer(other.gameObject.GetComponent<BasicEnemy>().enemyStats.coinsDroppedOnDeath);
            
            other.gameObject.GetComponent<BasicEnemy>().enemyHealth -= projectile.damage;
            other.gameObject.GetComponent<BasicEnemy>().onHitFeedback?.PlayFeedbacks(); // taking damage animaiton
            if(other.gameObject.GetComponent<BasicEnemy>().enemyHealth < 1){
                shipDeathFeedback?.PlayFeedbacks();
                if(other.gameObject.tag == "Enemy")  other.gameObject.GetComponent<BasicEnemy>().enemyHealth = 3; // ressting the ships health
                if(other.gameObject.tag == "Suicide")  other.gameObject.GetComponent<BasicEnemy>().enemyHealth = 1;
            EnemySpawner.Instance.UpdateEnemyTracker();
            if(PhotonNetwork.OfflineMode)
            other.gameObject.SetActive(false); // "killing" the enemy
            else
            this.GetComponent<PhotonView>().RPC("DisableEnemyShip", RpcTarget.AllBuffered,other.gameObject.GetComponent<PhotonView>().ViewID);
            GameObject.Find("FeedbackManager").GetComponent<FeedbackManager>().ShipExplosion(new Vector3(other.transform.position.x,other.transform.position.y, 0));
            }
            if(PhotonNetwork.OfflineMode){
            this.GetComponent<SpriteRenderer>().enabled = false;
            this.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject, 1f); // destorying the bullet
            }
            else{
                this.GetComponent<PhotonView>().RPC("DisableProjectile", RpcTarget.AllBuffered);
            }

        }
     }
   
   [PunRPC]
    private void DisableEnemyShip(int targetViewID){
        PhotonView targetPhotonView = PhotonView.Find(targetViewID);
            if(targetPhotonView != null)
            targetPhotonView.gameObject.SetActive(false);
    }

    [PunRPC]
    private void DisableProjectile(){
        // PhotonView targetPhotonView = PhotonView.Find(targetViewID);
        //     if(targetPhotonView != null){
            this.GetComponent<SpriteRenderer>().enabled = false;
            this.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject, 1f); // destorying the bullet
    //}
    }




}
