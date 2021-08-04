using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerController : MonoBehaviour
{

    	public float moveSpeed;
        [SerializeField]Transform firePoint;
        [SerializeField] float projectileSpeed;

        private float _timeUntilFire = 0;

        //Shooting Cooldown
        [SerializeField] float timeBetweenProjectiles;
        
        public Transform rocketSprite;

        private float _chargeTimer = 0;

        //////////////////////Projectile Info/////////////////////////////////////////////////////
        public Projectile tier1Bullet;
        public Projectile tier2Bullet;
        public Projectile tier3Bullet;
        public Projectile tier4Bullet;
        private GameObject _currentBullet;

        public GameObject charge1;
        public GameObject charge2;
        public GameObject charge3;
        ////////////////////////////////////////////////////////////////////////////////////////

        [SerializeField]private Turret turretTier1;
        [SerializeField]private Turret turretTier2;

        private bool _unlockedTurretTier2;
        void Start(){
            //View component for the photon network

            //Sets the bullet prefab that will be instantiated if fire is called
            SetBullet(tier1Bullet.bulletPrefab);
            
            //sets all charges to false so they cannot be seen until called upon
            charge1.SetActive(false);
            charge2.SetActive(false);
            charge3.SetActive(false);

            //sets all the scriptable objects back to original damage
            tier1Bullet.damage = 1;
            tier2Bullet.damage = 2;
            tier3Bullet.damage = 3;
            tier4Bullet.damage = 4; 

            tier1Bullet.timeUnitilNextBullet = 1;
            tier2Bullet.timeUnitilNextBullet = 2;
            tier3Bullet.timeUnitilNextBullet = 3;
            
        }


    void Update(){
        //the if statement is so you only control one player

        RotatePlayer();

        TempPurchase();

        //this is a check so fire cannot be called immediatly
        if(_timeUntilFire <= Time.time)WeaponCharge();


        
    }

    //rotates player around earth, in Unity the sprite must have the offset that is the radius of the object you are trying to ratoate around
    void RotatePlayer ()
	{
		transform.eulerAngles += new Vector3(0, 0, (-moveSpeed * Input.GetAxis("Horizontal")) * Time.deltaTime);
		rocketSprite.localEulerAngles = new Vector3(0, 0, Input.GetAxis("Horizontal") * -30);
	}

    //Instantiates a bullet prefab
    void Fire(){
        //so the bullet can be seen by other player
        GameObject bullet = Instantiate(_currentBullet,rocketSprite.position,transform.rotation);
        //giving the bullet velocity
        bullet.GetComponent<Rigidbody2D>().velocity = rocketSprite.position.normalized * projectileSpeed;

        //sets bullet to the first one in case a charged bullet is set
        SetBullet(tier1Bullet.bulletPrefab);

        
    
    //sets all charges to false so all charges are off when a bullet is instantiated
        charge1.SetActive(false);
        charge2.SetActive(false);
        charge3.SetActive(false);
        _timeUntilFire = Time.time + timeBetweenProjectiles;
    }

    
    //helper funtion for the WeaponCharge method
    void SetBullet(GameObject curretBullet){
        _currentBullet = curretBullet;
    }

    //called in update weapon charge is increasing a timer while the space key is held down, the timer is what determines what is current
    //bullet that will be called by the fire function
    void WeaponCharge(){
        
        if(Input.GetKey(KeyCode.Space)){
            _chargeTimer += Time.deltaTime;
            DisplayCharge();
        }
        
        if(Input.GetKeyUp(KeyCode.Space)){
        if(_chargeTimer > tier1Bullet.timeUnitilNextBullet)
            SetBullet(tier2Bullet.bulletPrefab);
        if(_chargeTimer > tier2Bullet.timeUnitilNextBullet)
            SetBullet(tier3Bullet.bulletPrefab);
        if(_chargeTimer > tier3Bullet.timeUnitilNextBullet)
            SetBullet(tier4Bullet.bulletPrefab);

        Fire();
        _chargeTimer = 0;
        }
    }

    //called by the weapon charge function, determines what charge is being displayed by also checking the timer
    void DisplayCharge(){
        
        
        if((_chargeTimer > tier1Bullet.timeUnitilNextBullet)){
            charge1.SetActive(true);
        }
        if(_chargeTimer > tier2Bullet.timeUnitilNextBullet){
            charge1.SetActive(false);
            charge2.SetActive(true);
        }
        if(_chargeTimer > tier3Bullet.timeUnitilNextBullet){
            charge2.SetActive(false);
            charge3.SetActive(true);
        }
    }

    void TempPurchase(){

        if(Input.GetKeyDown(KeyCode.T)){
            if(!_unlockedTurretTier2)
            Instantiate(turretTier1.turretPrefab, firePoint.transform.position,firePoint.transform.rotation);
            else
            Instantiate(turretTier2.turretPrefab, firePoint.transform.position,firePoint.transform.rotation);
            
        }
    }

    public void setTier2Turret(){
        _unlockedTurretTier2 = true;
    }

}
