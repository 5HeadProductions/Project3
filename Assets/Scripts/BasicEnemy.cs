using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class BasicEnemy : MonoBehaviour
{
    // scriptable object of the suicide
    public EnemyStats enemyStats;
    public GameObject center; // earth
    public Rigidbody2D rb;
    private bool isMoving = false, canShoot = false, isDashing = false;
    public GameObject FirePoint;
    private float startTime = 100.0f;
    public int enemyHealth; // needs to be public for the player projectile script to get the enemies health
    public MMFeedbacks onHitFeedback, onSpawnFeedback;

    public void OnEnable(){ // killing the enemy after it has been alive for a number of time
        Invoke("Dead", 120);
    }
    public void Dead(){
        GameObject.Find("FeedbackManager").GetComponent<FeedbackManager>().ShipExplosion();
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
        center = GameObject.Find("EnemySpawnerCenter");
        enemyHealth = enemyStats.health; // assigning the scriptable objects value of each ships health here
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
            startTime = Time.time + enemyStats.fireRate;
            }
        }
     }

    void FixedUpdate(){
        if(!isMoving) return ;
        rb.AddForce(-transform.up * enemyStats.speed, ForceMode2D.Force);
        if(this.gameObject.CompareTag("Boss")){
            BossMovement(); // giving additional movement to the bosses
        }
    }

    
    /*
    Giving the bosses more mobility allowing them to orbit around the earth
    When there is an even amount of bosses then they orbit in the opposite direction
    */
    private void BossMovement(){
        GameObject[] activeBosses = GameObject.FindGameObjectsWithTag("Boss"); // keeps track of how many enemies are currently active
        GameObject earth = GameObject.Find("BossCenter");   //GO in the scene that will rotate the its children, that being the boss ships
        GameObject earthTwo = GameObject.Find("BossCenterTwo");// GO similar to the one above but rotates in the opposite direction
        int i = 0;
        while(i < activeBosses.Length){
            if(activeBosses[i].activeInHierarchy){
                    //boss ships rotate
                if(i % 2 == 0){
                    activeBosses[i].transform.SetParent(earthTwo.transform);
                    rb.AddForce(-transform.up * enemyStats.speed, ForceMode2D.Impulse);
                    earthTwo.transform.eulerAngles += new Vector3(0,0,(-5f * Time.deltaTime));
                    }else{
                        activeBosses[i].transform.SetParent(earth.transform);
                        rb.AddForce(-transform.up * enemyStats.speed, ForceMode2D.Impulse);
                        earth.transform.eulerAngles += new Vector3(0,0,(5f * Time.deltaTime));
                    }
                }
                i++;
            }
    }

    /*  
    Can shoot is called from the Earth script which enables the ships to start shooting.
    To create a delay and not let the ships shoot instantly when they collided, the 
    coroutine was made to make that delay.
    CanShoot itself didn't become the coroutine because Earth couldn't start the coroutine.
    */
    public void CanShoot(){
       StartCoroutine(Delay()); 
        }
    public IEnumerator Delay(){
        yield return new WaitForSeconds(.5f);
           canShoot = true;
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
