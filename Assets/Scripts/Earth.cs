using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Earth : MonoBehaviour
{

    public TextMeshProUGUI health_Txt;
    public int earthHealth;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(earthHealth < 0){
          //earth got destroyed
        }
    }

   public void OnCollisionEnter2D(Collision2D col){ // inner collider
        if(col.transform.tag == "Suicide"){
            EnemySpawner.Instance.UpdateEnemyTracker();
            col.gameObject.SetActive(false);
            // queue explosion/camera shake 
            earthHealth = earthHealth - col.gameObject.GetComponent<BasicEnemy>().SuicideDamage();;

            }
        if(col.transform.tag == "EnemyBullet"){
            //earth take damage
           // queue explosion/camera shake

           earthHealth = earthHealth -  col.gameObject.GetComponent<EnemyProjectile>().DoDamage();;
            Destroy(col.gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D col){ // outer collider
        if(col.transform.tag == "Enemy"){
            col.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            //call shoot
            col.gameObject.GetComponent<BasicEnemy>().Shoot();
        }
    }

    
}
