using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
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
            
        }


    void Update(){
        //the if statement is so you only control one player
        if(view.IsMine){
        RotatePlayer();
        Pause();


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

    void Pause(){
        if(_pauseCanvas.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape)){
        _pauseCanvas.SetActive(false);
        }
        else if(Input.GetKeyDown(KeyCode.Escape)){
            _pauseCanvas.SetActive(true);
        }
        

    }



}
