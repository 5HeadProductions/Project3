using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Script is meant to handle player movement, firing and anything else related to the players actions

    	public float moveSpeed;
        public GameObject bulletPrefab;

        public Transform rocketSprite;

    void Update(){
        RotatePlayer();

        if(Input.GetKeyDown(KeyCode.Space)){
            ChargeShot();
        }
    }

    //rotates player around earth, in Unity the sprite must have the offset that is the radius of the object you are trying to ratoate around
    void RotatePlayer ()
	{
		transform.eulerAngles += new Vector3(0, 0, (-moveSpeed * Input.GetAxis("Horizontal")) * Time.deltaTime);
		rocketSprite.localEulerAngles = new Vector3(0, 0, Input.GetAxis("Horizontal") * -30);
	}

    void ChargeShot(){
      Instantiate(bulletPrefab,rocketSprite.position,Quaternion.identity);

      
    }
}
