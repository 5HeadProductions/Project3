using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectiles : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb2d;

    [SerializeField] float speed = 20f;

    public GameObject player;
    
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
      //  _rb2d.velocity = player.transform.position.normalized * speed;
    }

   
}
