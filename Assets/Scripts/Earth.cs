using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;

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
            EnemySpawner.Instance.UpdateEnemyTracker();
            col.gameObject.SetActive(false);
            // queue explosion/camera shake 
            earthHealth = earthHealth - col.gameObject.GetComponent<BasicEnemy>().SuicideDamage();;

            }
        if(col.transform.tag == "EnemyBullet"){ // enemy bullet hits earth
            //earth take damage
           // queue explosion/camera shake

           earthHealth -= col.gameObject.GetComponent<EnemyProjectile>().DoDamage();
           OnEarthHit?.PlayFeedbacks();
            Destroy(col.gameObject);
            healthBar.Minus10Percent();


        }
        OnEarthHit?.PlayFeedbacks(); // plays sound
        earthDamaged?.PlayFeedbacks();//plays red earth
        if(earthHealth <= 0){
          //earth got destroyed
            deathCanvas.SetActive(true);
        }
    }

// outer collider
    public void OnTriggerEnter2D(Collider2D col){ 
        if(col.transform.tag == "Enemy" || col.tag == "Boss"){
            col.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            //call shoot
            col.gameObject.GetComponent<BasicEnemy>().CanShoot();
        }
    }

    
}
