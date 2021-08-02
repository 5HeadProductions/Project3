using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketTab : MonoBehaviour
{
    public Animator animator;
    public int damageIncrease;
    public float chargeTimeDecrease;

    public Projectile projectileTier1;
    public Projectile projectileTier2;
    public Projectile projectileTier3;
    public Projectile projectileTier4;
    
    private bool _tabOpen = false;

    public PlayerController PlayerController;


    public void OpenTab(){
        
        if(!_tabOpen){
        animator.SetBool("Activated",true);
        _tabOpen = true;
        }
        else{
            animator.SetBool("Activated",false);
            _tabOpen = false;
        }
    }

    public void UpgradeBullet(){
        projectileTier1.damage += damageIncrease;
        projectileTier2.damage += damageIncrease;
        projectileTier3.damage += damageIncrease;
        projectileTier4.damage += damageIncrease;
    }

    public void UpgradeBulletCooldown(){
        projectileTier1.timeUnitilNextBullet -= chargeTimeDecrease;
        projectileTier2.timeUnitilNextBullet -= chargeTimeDecrease;
        projectileTier3.timeUnitilNextBullet -= chargeTimeDecrease;
    }

    public void UnlockTurretTier2(){
        Debug.Log(GameObject.FindGameObjectWithTag("Player").name);
        PlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        PlayerController.setTier2Turret();
    }
}
