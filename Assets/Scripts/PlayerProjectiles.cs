using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectiles : MonoBehaviour
{
    //Plan for this script is that it takes a scriptable object that will have the info of damage
    public Projectile projectile;

    void Start(){
        transform.Rotate(0,0,90);
    }
    
    // void OnCollisionEnter(Collider2D other){

    // }
   
}
