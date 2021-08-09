using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketTab : MonoBehaviour
{
    public Animator animator;
    public int damageIncrease;
    public float chargeTimeDecrease;

    //Get all the 4 projectiles so the damage can be upgraded in game
    public Projectile projectileTier1;
    public Projectile projectileTier2;
    public Projectile projectileTier3;
    public Projectile projectileTier4;
    
    private bool _tabOpen = false;

    public PlayerController PlayerController;


    //function checks a boolean to se if tab is open, if not it plays an animation
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

    //changes all the damage fields in the scriptable objects, could be changed to change player stats instead 
    public void UpgradeBullet(){
        projectileTier1.damage += damageIncrease;
        projectileTier2.damage += damageIncrease;
        projectileTier3.damage += damageIncrease;
        projectileTier4.damage += damageIncrease;
    }

    //changes cooldown fields in scriptable objects
    public void UpgradeBulletCooldown(){
        projectileTier1.timeUnitilNextBullet -= chargeTimeDecrease;
        projectileTier2.timeUnitilNextBullet -= chargeTimeDecrease;
        projectileTier3.timeUnitilNextBullet -= chargeTimeDecrease;
    }

    //Player holds a boolean saying whether or not they can place a tier 2 turret, this function updates it so it can
    public void UnlockTurretTier2(){
        PlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        PlayerController.setTier2Turret();
    }
}
