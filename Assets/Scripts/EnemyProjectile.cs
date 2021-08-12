using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public EnemyStats enemyStats;
       // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.CompareTag("Turret")){
           col.gameObject.GetComponent<TurretBehavior>().turretHealth -= enemyStats.damage;
            if( col.gameObject.GetComponent<TurretBehavior>().turretHealth < 1){
                //turret got shot by enemy bullet, then destroy turret
                Destroy(col.gameObject); // single player
            }
            Destroy(this.gameObject); // single player
        }
    }
    public int DoDamage(){
        return enemyStats.damage;
        
    }


}
