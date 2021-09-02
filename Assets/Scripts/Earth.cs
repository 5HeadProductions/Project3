using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
using Photon.Pun;

public class Earth : MonoBehaviour
{
    [SerializeField] MMProgressBar healthBar;
    public MMFeedbacks earthDamaged; //earth turn read when it is hit

    [SerializeField]MMFeedbacks OnEarthHit;
    public int earthHealth;
    public GameObject deathCanvas;
    
    // Update is called once per frame
    void Update()
    {
        
    }

// inner collider, suicide ships hit and enemy bullets
   public void OnCollisionEnter2D(Collision2D col){ 
        if(col.transform.tag == "Suicide"){
            if(PhotonNetwork.OfflineMode){
            EnemySpawner.Instance.UpdateEnemyTracker();
            col.gameObject.SetActive(false);
            // queue explosion/camera shake 
            earthHealth = earthHealth - col.gameObject.GetComponent<BasicEnemy>().SuicideDamage();
            }
            else{
                this.GetComponent<PhotonView>().RPC("SuicideDamage", RpcTarget.AllBuffered, col.gameObject.GetComponent<PhotonView>().ViewID);
            }

            }
        if(col.transform.tag == "EnemyBullet"){ // enemy bullet hits earth
            //earth take damage
           // queue explosion/camera shake
        if(PhotonNetwork.OfflineMode){
           earthHealth -= col.gameObject.GetComponent<EnemyProjectile>().DoDamage();
           OnEarthHit?.PlayFeedbacks();
            Destroy(col.gameObject);
            healthBar.Minus10Percent();
        }
        else{
            this.GetComponent<PhotonView>().RPC("EarthUpdate", RpcTarget.All, col.gameObject.GetComponent<PhotonView>().ViewID);
        }


        }
        OnEarthHit?.PlayFeedbacks(); // plays sound
        earthDamaged?.PlayFeedbacks();//plays red earth
        if(earthHealth <= 0){
          //earth got destroyed
          if(PhotonNetwork.OfflineMode)
            deathCanvas.SetActive(true);
            else
            this.GetComponent<PhotonView>().RPC("DeathCanvas", RpcTarget.AllBuffered);
        }
    }

// outer collider
    public void OnTriggerEnter2D(Collider2D col){ 
        if(col.transform.tag == "Enemy" || col.tag == "Boss"){
            col.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            //call shoot
            if(PhotonNetwork.OfflineMode)
            col.gameObject.GetComponent<BasicEnemy>().CanShoot();
            else
            this.GetComponent<PhotonView>().RPC("OnlineCanShoot", RpcTarget.MasterClient, col.GetComponent<PhotonView>().ViewID);
        }
    }

    [PunRPC]
    void OnlineCanShoot(int viewID){
        PhotonView col = PhotonView.Find(viewID);
        col.gameObject.GetComponent<BasicEnemy>().CanShoot();
    }
    [PunRPC]
    void DeathCanvas(){
        deathCanvas.SetActive(true);
    }

    [PunRPC]
    void EarthUpdate(int viewID){
        PhotonView col = PhotonView.Find(viewID);
        earthHealth -= col.gameObject.GetComponent<EnemyProjectile>().DoDamage();
           OnEarthHit?.PlayFeedbacks();
            Destroy(col.gameObject);
            healthBar.Minus10Percent();
    }

    [PunRPC]
    void SuicideDamage( int viewID){
        PhotonView col = PhotonView.Find(viewID);
        OnEarthHit?.PlayFeedbacks();
        col.gameObject.SetActive(false);
        healthBar.Minus10Percent();
            // queue explosion/camera shake 
            earthHealth = earthHealth - col.gameObject.GetComponent<BasicEnemy>().SuicideDamage();
    }

    
}
