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
    public PlayerCoins PlayerCoins;
    private Color changeColor;
 

   

    void Start(){
        PlayerCoins = GameObject.Find("GameManager").GetComponent<PlayerCoins>();
        transform.Rotate(0,0,90);
        OnInstantiation.Initialization(this.gameObject);
       
        
        OnInstantiation?.PlayFeedbacks();
    }
    
    // player shoots at the ship 
    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "Suicide" || other.gameObject.tag == "Boss"){
            PlayerCoins.AddCoinsToPlayer(other.gameObject.GetComponent<BasicEnemy>().enemyStats.coinsDroppedOnDeath);
            
            other.gameObject.GetComponent<BasicEnemy>().enemyHealth -= projectile.damage;
            other.gameObject.GetComponent<BasicEnemy>().onHitFeedback?.PlayFeedbacks(); // taking damage animaiton
            if( other.gameObject.GetComponent<BasicEnemy>().enemyHealth < 1){
                if(other.gameObject.tag == "Enemy")  other.gameObject.GetComponent<BasicEnemy>().enemyHealth = 3;
                if(other.gameObject.tag == "Suicide")  other.gameObject.GetComponent<BasicEnemy>().enemyHealth = 1;
            EnemySpawner.Instance.UpdateEnemyTracker();
            other.gameObject.SetActive(false); // "killing" the enemy
            }
            Destroy(gameObject); // destorying the bullet

        }
     }
   
   [PunRPC]
    public void DestroyGameObject(int gameObjectViewID){
            PhotonView temp = PhotonView.Find(gameObjectViewID);
            if(temp != null)
            Destroy(temp.gameObject);
    }
}
