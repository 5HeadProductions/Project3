using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{

    public GameObject center;
    public Rigidbody2D rb;
    public float moveSpeed;
    private bool isMoving = false;
    // Start is called before the first frame update
    void Start()
    {
        center = GameObject.Find("Center");
        isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
    var dir = center.transform.position - transform.position; // distance between two points in a graph
    var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;  // the angle is found by taking the oppposite over adjacent * radians to get it in degrees
    transform.rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward); // 90 is added to the angle bc 
    }

    void FixedUpdate(){
        if(!isMoving) return ;

        rb.AddForce(-transform.up * moveSpeed, ForceMode2D.Force);
    }

    ///collison when they hit turret or earth
}
