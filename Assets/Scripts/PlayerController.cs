using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    //Script is meant to handle player movement, firing and anything else related to the players actions

        PhotonView view;
    	public float moveSpeed;
        

        public Transform rocketSprite;

        private float _chargeTimer = 0;

        
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

        void Start(){
            view = gameObject.GetComponent<PhotonView>();
            SetBullet(tier1Bullet.bulletPrefab);
            charge1.SetActive(false);
            charge2.SetActive(false);
            charge3.SetActive(false);
        }


    void Update(){
        if(view.IsMine){
        RotatePlayer();

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
      GameObject bullet = PhotonNetwork.Instantiate(_currentBullet.name,rocketSprite.position,transform.rotation);

      bullet.GetComponent<Rigidbody2D>().velocity = rocketSprite.position.normalized * 100;

    SetBullet(tier1Bullet.bulletPrefab);
    charge1.SetActive(false);
    charge2.SetActive(false);
    charge3.SetActive(false);
    }

    

    void SetBullet(GameObject curretBullet){
        _currentBullet = curretBullet;
    }

    void WeaponCharge(){
        if(Input.GetKey(KeyCode.Space)){
            _chargeTimer += Time.deltaTime;
            DisplayCharge();
        }
        //Tier 1
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

    void DisplayCharge(){
        if(_chargeTimer > bullet2Timer)
            charge1.SetActive(true);
        if(_chargeTimer > bullet3Timer){
            charge1.SetActive(false);
            charge2.SetActive(true);
        }
        if(_chargeTimer > bullet4Timer){
            charge2.SetActive(false);
            charge3.SetActive(true);
        }
    }



}
