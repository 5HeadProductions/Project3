using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TurretBehavior : MonoBehaviourPun
{
    public Transform firePoint;
    private Transform _playerFirePoint;

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
    public PlayerController PlayerController;

    void Start(){
        // _currentTurret = turretTier1;
        startTime = Time.time;
        _playerFirePoint = GameObject.Find("PlayerFirePoint").transform;
        PlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    
    void Update(){
        //finds and assigns the transform of the target so we can shoot in their direction
        Collider2D temp = Physics2D.OverlapCircle(aimArea.transform.position,1);
        if(temp != null)_target = temp.gameObject.transform;

        
        if(Time.time > _timeUntilAttack  && _target != null && !_target.CompareTag("Turret")){
            animator.SetBool("Firing",true);
          
            GameObject bullet = Instantiate(_currentTurret.projectilePrefab,firePoint.transform.position, firePoint.transform.rotation);
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
        //If statement ensures this code is only excecuted once by checking their start times
        //Debug.Log(this.gameObject.name + " " + other.gameObject.name);
        //Debug.Log("this.startTime" + this.startTime);
        if(this.startTime > other.gameObject.GetComponent<TurretBehavior>().startTime && other.gameObject.CompareTag("Turret")){
            //for the turrets of tier 1 combining
           if(_currentTurret.turretTier == 1 && other.gameObject.GetComponent<TurretBehavior>()._currentTurret.turretTier == 1){
               //Destroy both turrets
               Debug.Log(this.gameObject.name);
               Debug.Log(other.gameObject.name);
               
               //PhotonNetwork.Destroy(this.gameObject);
               //PhotonNetwork.Destroy(other.gameObject);
               PlayerController.GetComponent<PhotonView>().RPC("DestroyGameObject",RpcTarget.MasterClient,this.GetComponent<PhotonView>().ViewID);
               
            //Instantiate the tier 2 turret
               GameObject temp = PhotonNetwork.Instantiate(turretTier2.turretPrefab.name, _playerFirePoint.position,
                _playerFirePoint.rotation);
           }
           //for turrets of tier 2 combining
           else if(_currentTurret.turretTier == 2){
               //Getting rid of the tier 1 turrets that spawn on top
               if(other.gameObject.GetComponent<TurretBehavior>()._currentTurret.turretTier == 1 && other.gameObject.GetComponent<TurretBehavior>()._currentTurret.turretTier == 1){
                   Debug.Log("Can't combine different level turrets");
                   PhotonNetwork.Destroy(other.gameObject);
                   return;
               }
               //Destroy both turrets
               PhotonNetwork.Destroy(this.gameObject);
               PhotonNetwork.Destroy(other.gameObject);
            //Instantiate the tier 2 turret
               GameObject temp = PhotonNetwork.Instantiate(turretTier3.turretPrefab.name, _playerFirePoint.position,
                _playerFirePoint.rotation);
           }
        
        }
    }
}
