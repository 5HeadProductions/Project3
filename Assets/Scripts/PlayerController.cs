using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Script is meant to handle player movement, firing and anything else related to the players actions

    	public float moveSpeed;
        public GameObject bulletPrefab;
        public GameObject chargedBulletPrefab;

        public Transform rocketSprite;

        public float chargeTimer = 0;

    void Update(){
        RotatePlayer();

        if(Input.GetKey(KeyCode.Space)){
            chargeTimer += Time.deltaTime;
        }
        if(Input.GetKeyUp(KeyCode.Space) && chargeTimer > 2){
            ChargeFire();
            chargeTimer = 0;
        }
        else if(Input.GetKeyUp(KeyCode.Space) && chargeTimer < 2){
            
            Fire();
            chargeTimer = 0;
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
      GameObject bullet = Instantiate(bulletPrefab,rocketSprite.position,transform.rotation);

      bullet.GetComponent<Rigidbody2D>().velocity = rocketSprite.position.normalized * 100;
    }

    void ChargeFire(){
        GameObject bullet = Instantiate(chargedBulletPrefab,rocketSprite.position,transform.rotation);

      bullet.GetComponent<Rigidbody2D>().velocity = rocketSprite.position.normalized * 100;
    }
}
