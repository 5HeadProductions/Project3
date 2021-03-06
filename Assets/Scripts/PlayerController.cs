using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using MoreMountains.Feedbacks;
using UnityEngine.SceneManagement;
using System;

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

        ////////////////////////////////////////////////////////////////////////////////////////


        [SerializeField]private Turret turretTier1;
        [SerializeField]private Turret turretTier2;

        private bool _unlockedTurretTier2;

        [SerializeField] KeyCode turretSpawner; // set in the inspector 
        [SerializeField] KeyCode pauseButton;
        public PlayerCoins PlayerCoins;
        public GameObject _pauseCanvas;

        [Header("FeedBacks")]
        [SerializeField] MMFeedbacks tier1Shot;
        [SerializeField] MMFeedbacks tier2Shot;
        [SerializeField] MMFeedbacks tier3Shot;
        [SerializeField] MMFeedbacks tier4Shot;
        [SerializeField] MMFeedbacks tier1Charge;
        [SerializeField] MMFeedbacks tier2Charge;
        [SerializeField] MMFeedbacks tier3Charge;

        //boolean for the feedbacks of charging works
        bool _charge1 = true;
        bool _charge2 = true;
        bool _charge3 = true;

        public GameObject DeathCanvas;
        
        MMFeedbacks _currentShotFeedback;
        public GameObject StartButton;
        
        void Start(){
            _currentShotFeedback = tier1Shot;
            //View component for the photon network
            if(PhotonNetwork.OfflineMode == false){
                //View component for the photon network
            view = gameObject.GetComponent<PhotonView>();
            _pauseCanvas = GameObject.Find("PauseCanvas");  // dragged in the canvas instead
            _pauseCanvas.SetActive(false);
            }
            if(SceneManager.GetActiveScene().name == "HowToPlay"){
                _pauseCanvas.SetActive(false);
            }
            

            
            
            
            //Sets the bullet prefab that will be instantiated if fire is called
            SetBullet(tier1Bullet.bulletPrefab);

            //sets all the scriptable objects back to original damage
            tier1Bullet.damage = 1;
            tier2Bullet.damage = 2;
            tier3Bullet.damage = 3;
            tier4Bullet.damage = 4;
            
        }


    void Update(){
        
        if(PhotonNetwork.OfflineMode == false){
        //the if statement is so you only control one player
        if(view.IsMine){
        
        Pause();
        TempPurchase();


        //this is a check so fire cannot be called immediatly
        if(!DeathCanvas.activeInHierarchy){
        RotatePlayer();
        //this is a check so fire cannot be called immediatly
        if(_timeUntilFire <= Time.time)
        WeaponCharge();
        }
        }
        }
        else{
        
        Pause();
        TempPurchase();


    if(!DeathCanvas.activeInHierarchy){
        RotatePlayer();
        //this is a check so fire cannot be called immediatly
        if(_timeUntilFire <= Time.time)
        WeaponCharge();
        }
        }
        


        
    }

    //rotates player around earth, in Unity the sprite must have the offset that is the radius of the object you are trying to ratoate around
    void RotatePlayer()
	{
		transform.eulerAngles += new Vector3(0, 0, (-moveSpeed * Input.GetAxis("Horizontal")) * Time.deltaTime);
		rocketSprite.localEulerAngles = new Vector3(0, 0, Input.GetAxis("Horizontal") * -30);
	}

    //Instantiates a bullet prefab
    void Fire(){
        if(PhotonNetwork.OfflineMode){
            
            Quaternion newRotation = transform.rotation * _currentBullet.transform.rotation;
            GameObject bullet = Instantiate(_currentBullet,rocketSprite.position,newRotation);
            bullet.GetComponent<Rigidbody2D>().velocity = rocketSprite.position.normalized * projectileSpeed;
        }
        //so the bullet can be seen by other player
        else{
        Quaternion newRotation = transform.rotation * _currentBullet.transform.rotation;
        GameObject bullet = PhotonNetwork.Instantiate(_currentBullet.name,rocketSprite.position,newRotation);
        bullet.GetComponent<Rigidbody2D>().velocity = rocketSprite.position.normalized * projectileSpeed;
        }
        //giving the bullet velocity
        
        if(PhotonNetwork.OfflineMode)
        _currentShotFeedback?.PlayFeedbacks();
        else
        this.GetComponent<PhotonView>().RPC("CurrentShotFeedback", RpcTarget.All);

        _currentShotFeedback = tier1Shot;
        

        //sets bullet to the first one in case a charged bullet is set
        SetBullet(tier1Bullet.bulletPrefab);

    
    //sets all charges to false so all charges are off when a bullet is instantiated
        _charge1 = true;
        _charge2 = true;
        _charge3 = true;
        if(PhotonNetwork.OfflineMode)
        tier3Charge?.StopFeedbacks();
        else
        this.GetComponent<PhotonView>().RPC("StopFeedback",RpcTarget.All);
        
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
            //chargeFeedback?.PlayFeedbacks();
            DisplayCharge();
        }
        
        if(Input.GetKeyUp(KeyCode.Space)){
        if(_chargeTimer > bullet2Timer){
            SetBullet(tier2Bullet.bulletPrefab);
            _currentShotFeedback = tier2Shot;
        }
        if(_chargeTimer > bullet3Timer){
            SetBullet(tier3Bullet.bulletPrefab);
            _currentShotFeedback = tier3Shot;
        }
        if(_chargeTimer > bullet4Timer){
            SetBullet(tier4Bullet.bulletPrefab);
            _currentShotFeedback = tier4Shot;
        }

        Fire();
        _chargeTimer = 0;
        }
    }

    //called by the weapon charge function, determines what charge is being displayed by also checking the timer
    void DisplayCharge(){
        
        if(PhotonNetwork.OfflineMode){
        if(_chargeTimer > bullet2Timer && _charge1){      
            tier1Charge?.PlayFeedbacks();
            _charge1 = false;
        }
        if(_chargeTimer > bullet3Timer && _charge2){
            tier2Charge?.PlayFeedbacks();
            _charge2 = false;
        }
        if(_chargeTimer > bullet4Timer && _charge3){
            tier3Charge?.PlayFeedbacks();
            _charge3 = false;
        }
        }
        else{
            if(_chargeTimer > bullet2Timer && _charge1){      
            this.GetComponent<PhotonView>().RPC("OnlinePlayFeedbacks", RpcTarget.All, tier1Charge.GetComponent<PhotonView>().ViewID);
            _charge1 = false;
        }
        if(_chargeTimer > bullet3Timer && _charge2){
            this.GetComponent<PhotonView>().RPC("OnlinePlayFeedbacks", RpcTarget.All, tier2Charge.GetComponent<PhotonView>().ViewID);
            _charge2 = false;
        }
        if(_chargeTimer > bullet4Timer && _charge3){
            this.GetComponent<PhotonView>().RPC("OnlinePlayFeedbacks", RpcTarget.All, tier3Charge.GetComponent<PhotonView>().ViewID);
            _charge3 = false;
        }
        }
    }

    //gives the player the ability to spawn in there pause canvas and does not show this over the network
    //possible tweaks are to freexe the players controls when canvas is active
    void Pause(){
        if(Input.GetKeyDown(pauseButton) && _pauseCanvas.activeInHierarchy){
            if(PhotonNetwork.OfflineMode == true){
                ResumeTime();  //resuming time when the player presses the 'ESC' key
            }
        _pauseCanvas.SetActive(false);
        }
        else if(Input.GetKeyDown(pauseButton)){
            if(PhotonNetwork.OfflineMode == true){
                PauseTime();
            }
            _pauseCanvas.SetActive(true);
        }
    }
    void PauseTime(){
        Time.timeScale = 0;
    }
    void ResumeTime(){
        Time.timeScale = 1; 
    }
    //turret purchase*
   void TempPurchase(){

        if(Input.GetKeyDown(turretSpawner)){
            
            if(SceneManager.GetActiveScene().name == "HowToPlay"){ // used only in the how to play scene
                GameObject temp = Instantiate(turretTier1.turretPrefab, firePoint.transform.position,firePoint.transform.rotation);
                temp.GetComponent<TurretBehavior>().stackEffect?.Initialization();
                temp.GetComponent<TurretBehavior>().stackEffect?.PlayFeedbacks(); 
            }else{
                if(PhotonNetwork.OfflineMode == false){// online mode
                if(!_unlockedTurretTier2&& PlayerCoins.playerCoins >= turretTier1.buyCost){
                    PlayerCoins.SubtractCoinsFromPlayer(turretTier1.buyCost);
            GameObject temp =  PhotonNetwork.Instantiate(turretTier1.turretPrefab.name, firePoint.transform.position,firePoint.transform.rotation);
            this.GetComponent<PhotonView>().RPC("PlayTurretEffect",RpcTarget.All, temp.GetComponent<PhotonView>().ViewID);
            // temp.GetComponent<TurretBehavior>().stackEffect?.Initialization();
            //     temp.GetComponent<TurretBehavior>().stackEffect?.PlayFeedbacks();
                }
                else if(PlayerCoins.playerCoins >= turretTier2.buyCost){
                    PlayerCoins.SubtractCoinsFromPlayer(turretTier2.buyCost);
                GameObject temp = PhotonNetwork.Instantiate(turretTier2.turretPrefab.name, firePoint.transform.position,firePoint.transform.rotation);
                this.GetComponent<PhotonView>().RPC("PlayTurretEffect",RpcTarget.All, temp.GetComponent<PhotonView>().ViewID);
                // temp.GetComponent<TurretBehavior>().stackEffect?.Initialization();
                //     temp.GetComponent<TurretBehavior>().stackEffect?.PlayFeedbacks();
                }
                
            }
            else{ // for offline mode
                if(!_unlockedTurretTier2 && PlayerCoins.playerCoins >= turretTier1.buyCost){
                    PlayerCoins.SubtractCoinsFromPlayer(turretTier1.buyCost);
            GameObject temp = Instantiate(turretTier1.turretPrefab, firePoint.transform.position,firePoint.transform.rotation);
            temp.GetComponent<TurretBehavior>().stackEffect?.Initialization();
                temp.GetComponent<TurretBehavior>().stackEffect?.PlayFeedbacks();
                }
                else if(PlayerCoins.playerCoins >= turretTier2.buyCost){
                    PlayerCoins.SubtractCoinsFromPlayer(turretTier2.buyCost);
                GameObject temp = Instantiate(turretTier2.turretPrefab, firePoint.transform.position,firePoint.transform.rotation);
                temp.GetComponent<TurretBehavior>().stackEffect?.Initialization();
                    temp.GetComponent<TurretBehavior>().stackEffect?.PlayFeedbacks();
                }
            }
            }

    }

}

    public void setTier2Turret(){
        _unlockedTurretTier2 = true;
    }

    [PunRPC]
    private void StopFeedback(){
        tier3Charge?.StopFeedbacks();

    }
    [PunRPC]
    public void DestroyGameObject(int gameObjectViewID){
            PhotonView temp = PhotonView.Find(gameObjectViewID);
            if(temp != null)
            Destroy(temp.gameObject);
    }

    [PunRPC]
    private void PlayTurretEffect(int turretViewID){
        PhotonView temp = PhotonView.Find(turretViewID);
            if(temp != null){
            temp.GetComponent<TurretBehavior>().stackEffect?.Initialization();
                temp.GetComponent<TurretBehavior>().stackEffect?.PlayFeedbacks();
            }
    }

    [PunRPC]
    private void OnlinePlayFeedbacks(int feedbackViewID){
         PhotonView temp = PhotonView.Find(feedbackViewID);
            if(temp != null){
                temp.GetComponent<MMFeedbacks>()?.PlayFeedbacks();
            }
    }

    [PunRPC]
    private void CurrentShotFeedback(){
         _currentShotFeedback.PlayFeedbacks();
    }


}

    

