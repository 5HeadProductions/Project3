using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    // scriptable object of the suicide
    public EnemyStats enemyStats;
    public GameObject center; // earth
    public Rigidbody2D rb;
    private bool isMoving = false, canShoot = false;
    public GameObject FirePoint;
    private float startTime = 100.0f;
    public float fireRate = 10.0f;

    public void OnEnable(){
        Invoke("Dead", 60);
    }
    public void Dead(){
        canShoot = false;
        gameObject.SetActive(false);
    }
    public void OnDisable(){
        canShoot = false;
        CancelInvoke();
    }
    // Start is called before the first frame update
    void Start()
    {
        center = GameObject.Find("Center");
        isMoving = true;
        startTime  = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //getting the ships to look at earth
    var dir = center.transform.position - transform.position; // distance between two points in a graph
    var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;  // the angle is found by taking the oppposite over adjacent * radians to get it in degrees
    transform.rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward); // 90 is added to the angle bc 

    if(this.gameObject.activeInHierarchy && canShoot == true){
        if(Time.time > startTime){
            Fire();
            startTime = Time.time + fireRate;
            }
        }
     }

    void FixedUpdate(){
        if(!isMoving) return ;
        rb.AddForce(-transform.up * enemyStats.speed, ForceMode2D.Force);
    }

    public void Shoot(){
        canShoot = true;
        Debug.Log("Shoot was called");
        }
    public void Fire(){
        GameObject bullet = Instantiate(enemyStats.enemyBullet,FirePoint.transform.position, enemyStats.shipType.gameObject.transform.rotation);
        var dir = center.transform.position - bullet.transform.position; // distance between two points in a graph
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        dir = dir.normalized * 25;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(dir.x, dir.y);
    }

    public int SuicideDamage(){
        return enemyStats.damage;
    }
}
