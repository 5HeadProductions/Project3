using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class MarketTab : MonoBehaviour
{
    public Animator animator;
    

    //Get all the 4 projectiles so the damage can be upgraded in game
    public Projectile projectileTier1;
    public Projectile projectileTier2;
    public Projectile projectileTier3;
    public Projectile projectileTier4;
    
    private bool _tabOpen = false;

    public PlayerController PlayerController;

    private PlayerCoins _playerCoins;

    

    [Header("Adjustable Values")]
    [Multiline(4)]
    public string Descripion;
    public TextMeshProUGUI cost;

    public TextMeshProUGUI cooldownCost;
    public TextMeshProUGUI turretUpgradeCost;
    public int damageIncrease;
    public int initialUpgradeBulletCost;
    public int upgradeBulletCostIncrease;
    public int maxUpgradeBulletAmount;
    //For the upgrade of charge time decrese
    [Multiline(4)]
    public string Descripion2;
    public float chargeTimeDecrease;
    public int initialCooldownUpgradeCost;
    public int cooldownUpgradeCostIncrease;
    public int maxCooldownUpgradeAmount;


    void Start(){
        _playerCoins = GameObject.Find("GameManager").GetComponent<PlayerCoins>();
        // Debug.Log("worked fine");
        // cost.text = initialUpgradeBulletCost.ToString();
        // cooldownCost.text = initialUpgradeBulletCost.ToString();
        // turretUpgradeCost.text = "100";
    }

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
        if(maxUpgradeBulletAmount > 0 && _playerCoins.playerCoins > initialUpgradeBulletCost){
        if(PhotonNetwork.OfflineMode){
            _playerCoins.SubtractCoinsFromPlayer(initialUpgradeBulletCost);
        initialUpgradeBulletCost += upgradeBulletCostIncrease;
        projectileTier1.damage += damageIncrease;
        projectileTier2.damage += damageIncrease;
        projectileTier3.damage += damageIncrease;
        projectileTier4.damage += damageIncrease;
        maxUpgradeBulletAmount--;
        
        cost.text = initialUpgradeBulletCost.ToString();
        }
        else 
        this.GetComponent<PhotonView>().RPC("UpdateText", RpcTarget.AllBuffered, cost.gameObject.GetComponent<PhotonView>().ViewID);
        }
        if(maxUpgradeBulletAmount == 0){
            if(PhotonNetwork.OfflineMode)
            cost.text = "MAX";
            else
            this.GetComponent<PhotonView>().RPC("MaxText", RpcTarget.AllBuffered, cost.gameObject.GetComponent<PhotonView>().ViewID);
        }
        //else display, cannot upgrade anymore
    }

    //changes cooldown fields in scriptable objects
    public void UpgradeBulletCooldown(){
        if(maxCooldownUpgradeAmount > 0 && _playerCoins.playerCoins > initialCooldownUpgradeCost){
            if(PhotonNetwork.OfflineMode){
            _playerCoins.SubtractCoinsFromPlayer(initialCooldownUpgradeCost);
            initialCooldownUpgradeCost += cooldownUpgradeCostIncrease;
        projectileTier1.timeUnitilNextBullet -= chargeTimeDecrease;
        projectileTier2.timeUnitilNextBullet -= chargeTimeDecrease;
        projectileTier3.timeUnitilNextBullet -= chargeTimeDecrease;
        cooldownCost.text = initialCooldownUpgradeCost.ToString();
        maxCooldownUpgradeAmount--;
            }
            else{
                this.GetComponent<PhotonView>().RPC("UpdateCooldown", RpcTarget.AllBuffered, cooldownCost.gameObject.GetComponent<PhotonView>().ViewID);
            }
        }
        if(maxCooldownUpgradeAmount == 0){
            if(PhotonNetwork.OfflineMode)
            cooldownCost.text = "MAX";
            else
            this.GetComponent<PhotonView>().RPC("MaxText", RpcTarget.AllBuffered, cooldownCost.gameObject.GetComponent<PhotonView>().ViewID);
        }
    }

    //Player holds a boolean saying whether or not they can place a tier 2 turret, this function updates it so it can
    public void UnlockTurretTier2(){
        if(_playerCoins.playerCoins > 100){
            if(PhotonNetwork.OfflineMode){
            _playerCoins.SubtractCoinsFromPlayer(100);
        PlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        PlayerController.setTier2Turret();
        turretUpgradeCost.text = "MAX";
            }
            else
            this.GetComponent<PhotonView>().RPC("TurretUnlockOnline", RpcTarget.AllBuffered, turretUpgradeCost.gameObject.GetComponent<PhotonView>().ViewID);
        }
    }


    [PunRPC]
    void TurretUnlockOnline(int viewID){
        _playerCoins.SubtractCoinsFromPlayer(100);
        PlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        PlayerController.setTier2Turret();
        turretUpgradeCost.text = "MAX";

    }

    [PunRPC]
    private void UpdateText(int ViewID){
        _playerCoins.SubtractCoinsFromPlayer(initialUpgradeBulletCost);
        initialUpgradeBulletCost += upgradeBulletCostIncrease;
        projectileTier1.damage += damageIncrease;
        projectileTier2.damage += damageIncrease;
        projectileTier3.damage += damageIncrease;
        projectileTier4.damage += damageIncrease;
        maxUpgradeBulletAmount--;
        
        PhotonView textBox = PhotonView.Find(ViewID);
        if(textBox != null){
            textBox.GetComponent<TextMeshProUGUI>().text = initialUpgradeBulletCost.ToString();

            
        }
    }

    [PunRPC]
    void MaxText(int viewID){
        PhotonView textBox = PhotonView.Find(viewID);
        if(textBox != null){
            textBox.GetComponent<TextMeshProUGUI>().text = "MAX";
    }
    }


    [PunRPC]
    private void UpdateCooldown(int ViewID){
        _playerCoins.SubtractCoinsFromPlayer(initialCooldownUpgradeCost);
            initialCooldownUpgradeCost += cooldownUpgradeCostIncrease;
        projectileTier1.timeUnitilNextBullet -= chargeTimeDecrease;
        projectileTier2.timeUnitilNextBullet -= chargeTimeDecrease;
        projectileTier3.timeUnitilNextBullet -= chargeTimeDecrease;
        cooldownCost.text = initialCooldownUpgradeCost.ToString();
        maxCooldownUpgradeAmount--;
        
        PhotonView textBox = PhotonView.Find(ViewID);
        if(textBox != null){
            textBox.GetComponent<TextMeshProUGUI>().text = initialCooldownUpgradeCost.ToString();

            
        }
    }
}
