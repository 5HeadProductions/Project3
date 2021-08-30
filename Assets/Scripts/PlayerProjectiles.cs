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
        // if(PhotonNetwork.OfflineMode){
        //transform.Rotate(0,0,90);
        OnInstantiation.Initialization(this.gameObject);
        OnInstantiation?.PlayFeedbacks();
        // }
        // else {
    
        // this.GetComponent<PhotonView>().RPC("BulletStart",RpcTarget.MasterClient);
        // }
    }
    
    // player shoots at the ship 
    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "EnemyBullet" || other.gameObject.tag == "BossEnemyBullet"){
            if(PhotonNetwork.OfflineMode){
            OnCollision?.PlayFeedbacks(); // normal function call
            this.GetComponent<SpriteRenderer>().enabled = false;
            this.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(gameObject, 2f); // destorying the bullet
            Destroy(other.gameObject);
            }
            else
            this.GetComponent<PhotonView>().RPC("BulletCollision", RpcTarget.All, other.gameObject.GetComponent<PhotonView>().ViewID); //RPC call
        }
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Suicide" || other.gameObject.tag == "Boss"){
            if(PhotonNetwork.OfflineMode){
            OnCollision?.PlayFeedbacks(); // normal function call
            
            }
            else
            this.GetComponent<PhotonView>().RPC("PlayFeedback", RpcTarget.All); //RPC call

            other.gameObject.GetComponent<BasicEnemy>().enemyHealth -= projectile.damage;
            if(PhotonNetwork.OfflineMode)
            other.gameObject.GetComponent<BasicEnemy>().onHitFeedback?.PlayFeedbacks(); // taking damage animaiton
            else{
                this.GetComponent<PhotonView>().RPC("TakingDamageFeedback", RpcTarget.All, other.gameObject.GetComponent<PhotonView>().ViewID);
            }
            if(other.gameObject.GetComponent<BasicEnemy>().enemyHealth < 1){
                GameObject.Find("FeedbackManager").GetComponent<FeedbackManager>().ShipExplosion(new Vector3(other.transform.position.x,other.transform.position.y, 0));
                if(PhotonNetwork.OfflineMode){
                PlayerCoins.AddCoinsToPlayer(other.gameObject.GetComponent<BasicEnemy>().enemyStats.coinsDroppedOnDeath);
                shipDeathFeedback?.PlayFeedbacks();
                }
                else{
                this.GetComponent<PhotonView>().RPC("UpdatePlayerCoins", RpcTarget.AllBuffered, other.gameObject.GetComponent<BasicEnemy>().enemyStats.coinsDroppedOnDeath);
                this.GetComponent<PhotonView>().RPC("DeathFeedback", RpcTarget.All);
                }
                if(other.gameObject.tag == "Enemy")  other.gameObject.GetComponent<BasicEnemy>().enemyHealth = 3; // ressting the ships health
                if(other.gameObject.tag == "Suicide")  other.gameObject.GetComponent<BasicEnemy>().enemyHealth = 1;
            EnemySpawner.Instance.UpdateEnemyTracker();
            if(PhotonNetwork.OfflineMode)
            other.gameObject.SetActive(false); // "killing" the enemy
            else
            this.GetComponent<PhotonView>().RPC("DisableEnemyShip", RpcTarget.AllBuffered,other.gameObject.GetComponent<PhotonView>().ViewID);
            
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
        GameObject.Find("FeedbackManager").GetComponent<FeedbackManager>().ShipExplosion(new Vector3(targetPhotonView.gameObject.transform.position.x,targetPhotonView.gameObject.transform.position.y, 0));
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

    [PunRPC]

    private void UpdatePlayerCoins(int coinsAdded){
        PlayerCoins.AddCoinsToPlayer(coinsAdded);
    }

    [PunRPC]

    private void TakingDamageFeedback(int viewID){
        PhotonView temp = PhotonView.Find(viewID);
            if(temp != null)
        temp.gameObject.GetComponent<BasicEnemy>().onHitFeedback?.PlayFeedbacks();
    }

    //activate 
    [PunRPC]
    private void PlayFeedback(){
        
        OnCollision?.PlayFeedbacks();
       
    }

    [PunRPC]
    private void BulletStart(){
        transform.Rotate(0,0,90);
        OnInstantiation.Initialization(this.gameObject);
        OnInstantiation?.PlayFeedbacks();
    }
    
    [PunRPC]
    private void DeleteBullet(int viewID){
        PhotonView temp = PhotonView.Find(viewID);
            if(temp != null)
            Destroy(temp.gameObject);
    }

    [PunRPC]
    private void DeathFeedback(){
         shipDeathFeedback.PlayFeedbacks();
    }
    [PunRPC]
    private void BulletCollision(int enemyBulletViewID){
        PhotonView bullet = PhotonView.Find(this.gameObject.GetComponent<PhotonView>().ViewID);
        PhotonView enemyBullet = PhotonView.Find(enemyBulletViewID);
        Debug.Log(bullet.gameObject.name);
        
        OnCollision?.PlayFeedbacks(); // normal function call
            bullet.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            bullet.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(bullet.gameObject, 1f); // destorying the bullet
            
            Destroy(enemyBullet.gameObject);
    }
}
