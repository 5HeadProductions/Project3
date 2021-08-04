using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun
{
    //Script is meant to handle player movement, firing and anything else related to the players actions

        PhotonView view;
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
        public float bullet2Timer;
        public Projectile tier2Bullet;
        public float bullet3Timer;
        public Projectile tier3Bullet;
        public float bullet4Timer;
        public Projectile tier4Bullet;
        private GameObject _currentBullet;

        public GameObject charge1;
        public GameObject charge2;
        public GameObject charge3;
        ////////////////////////////////////////////////////////////////////////////////////////

        private GameObject _pauseCanvas;

        [SerializeField]private Turret turretTier1;
        [SerializeField]private Turret turretTier2;

        private bool _unlockedTurretTier2;

        [SerializeField] KeyCode turretSpawner;
        [SerializeField] KeyCode pauseButton;
        void Start(){
            
            _pauseCanvas = GameObject.Find("PauseCanvas");
            _pauseCanvas.SetActive(false);
            //View component for the photon network
            view = gameObject.GetComponent<PhotonView>();
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
            
        }


    void Update(){
        //the if statement is so you only control one player
        if(view.IsMine){
        RotatePlayer();
        Pause();
        TempPurchase();


        //this is a check so fire cannot be called immediatly
        if(_timeUntilFire <= Time.time)
        WeaponCharge();
        }

        
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
        GameObject bullet = PhotonNetwork.Instantiate(_currentBullet.name,rocketSprite.position,transform.rotation);
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
        if(_chargeTimer > bullet2Timer)
            SetBullet(tier2Bullet.bulletPrefab);
        if(_chargeTimer > bullet3Timer)
            SetBullet(tier3Bullet.bulletPrefab);
        if(_chargeTimer > bullet4Timer)
            SetBullet(tier4Bullet.bulletPrefab);

        Fire();
        _chargeTimer = 0;
        }
    }

    //called by the weapon charge function, determines what charge is being displayed by also checking the timer
    void DisplayCharge(){
        
        
        if((_chargeTimer > bullet2Timer)){      
            charge1.SetActive(true);
        }
        if(_chargeTimer > bullet3Timer){
            charge1.SetActive(false);
            charge2.SetActive(true);
        }
        if(_chargeTimer > bullet4Timer){
            charge2.SetActive(false);
            charge3.SetActive(true);
        }
    }

    //gives the player the ability to spawn in there pause canvas and does not show this over the network
    //possible tweaks are to freexe the players controls when canvas is active
    void Pause(){
        if(_pauseCanvas.activeInHierarchy && Input.GetKeyDown(pauseButton)){
        _pauseCanvas.SetActive(false);
        }
        else if(Input.GetKeyDown(pauseButton)){
            _pauseCanvas.SetActive(true);
        }
        

    }
    //turret purchase*
   void TempPurchase(){

        if(Input.GetKeyDown(turretSpawner)){
            if(!_unlockedTurretTier2){
            PhotonNetwork.Instantiate(turretTier1.turretPrefab.name, firePoint.transform.position,firePoint.transform.rotation);
            }
            else
            PhotonNetwork.Instantiate(turretTier2.turretPrefab.name, firePoint.transform.position,firePoint.transform.rotation);
            
        }
    }

    public void setTier2Turret(){
        _unlockedTurretTier2 = true;
    }

    [PunRPC]
    public void DestroyGameObject(int gameObjectViewID){
            PhotonView temp = PhotonView.Find(gameObjectViewID);
            if(temp != null)
            Destroy(temp.gameObject);
    }
}

    

