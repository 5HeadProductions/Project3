using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TurretBehavior : MonoBehaviour
{
    public Transform firePoint;

    [SerializeField]Turret turretTier1;
    [SerializeField]Turret turretTier2;
    [SerializeField]Turret turretTier3;

    public Turret _currentTurret;


    private Transform _target;

    public GameObject aimArea;

    private float _timeUntilAttack;
    
    IEnumerator _currentCoroutine;

    [SerializeField]Animator animator;

    public float startTime;

    void Start(){
        _currentTurret = turretTier1;
        startTime = Time.time;
    }
    
    void Update(){
        //finds and assigns the transform of the target so we can shoot in their direction
        Collider2D temp = Physics2D.OverlapCircle(aimArea.transform.position,1);
        if(temp != null)_target = temp.gameObject.transform;

        
        if(Time.time > _timeUntilAttack  && _target != null){
            animator.SetBool("Firing",true);
          
            GameObject bullet = Instantiate(_currentTurret.projectilePrefab,firePoint.transform.position, _currentTurret.projectilePrefab.transform.rotation);
            Vector2 bulletDirection = (_target.transform.position - transform.position).normalized * 10;
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletDirection.x, bulletDirection.y);
            _timeUntilAttack = Time.time + _currentTurret.fireRate;
            StartCoroutine(animatorWait());
        }
        
        
    }

    void OnDrawGizmos(){
        Gizmos.DrawWireSphere(aimArea.transform.position, _currentTurret.range);
    }

    IEnumerator animatorWait(){
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("Firing",false);

    }

    void OnCollisionEnter2D(Collision2D other){
        if(this.startTime > other.gameObject.GetComponent<TurretBehavior>().startTime){
        if(other.gameObject != null){
        if(this.gameObject.CompareTag("NewTurret")){
           if(_currentTurret.turretTier == 1){
               this.tag = "PlacedTurret";
               PhotonNetwork.Destroy(this.gameObject);
               Debug.Log("placed turret destroyed");
               PhotonNetwork.Destroy(other.gameObject);
               Debug.Log("new turret destroyed turret destroyed");


               //other.gameObject.GetComponent<TurretBehavior>()._currentTurret = turretTier2;

               GameObject temp = PhotonNetwork.Instantiate(turretTier2.turretPrefab.name, firePoint.position,
                _currentTurret.turretPrefab.transform.rotation);

                temp.tag = "PlacedTurret";
           }
        }
        }
        }
    }
}
