using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using MoreMountains.Feedbacks;

public class TurretBehavior : MonoBehaviourPun
{
    public MMFeedbacks stackEffect;
    public MMFeedbacks shootEffect;

    public MMFeedbacks destroyedEffect;
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

    public LayerMask enemyLayer;

    private Collider2D _targetCollider;

    public int turretHealth;

    void Start(){
        
        // _currentTurret = turretTier1;
        turretHealth = _currentTurret.health;
        startTime = Time.time;
        _playerFirePoint = GameObject.Find("PlayerFirePoint").transform;
        PlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    
    void Update(){
        //finds and assigns the transform of the target so we can shoot in their direction
        _targetCollider = Physics2D.OverlapCircle(aimArea.transform.position,_currentTurret.range,enemyLayer);
        
        if(_targetCollider != null && (_targetCollider.CompareTag("Enemy") || _targetCollider.CompareTag("Suicide"))){   
            _target = _targetCollider.gameObject.transform;
            
            }
            if(_target != null && _target.gameObject.activeInHierarchy == false){
               _targetCollider = null;
                _target = null;
                
        }
        
        if(Time.time > _timeUntilAttack && _target != null && _targetCollider != null && !_target.CompareTag("Turret") ){
            animator.SetBool("Firing",true);
            
          
            GameObject bullet = Instantiate(_currentTurret.projectilePrefab,firePoint.transform.position, firePoint.transform.rotation);
            Vector2 bulletDirection = (_target.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(bulletDirection.y,bulletDirection.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.AngleAxis(angle,Vector3.forward);
            bullet.transform.Rotate(0,0,-90);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletDirection.x, bulletDirection.y) * _currentTurret.bulletSpeed;
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
        if(other.gameObject.CompareTag("Turret")  &&  this.startTime > other.gameObject.GetComponent<TurretBehavior>().startTime){
            //for the turrets of tier 1 combining
           if(_currentTurret.turretTier == 1 && other.gameObject.GetComponent<TurretBehavior>()._currentTurret.turretTier == 1){
               //Destroy both turrets
               if(PhotonNetwork.OfflineMode == false){
               PlayerController.GetComponent<PhotonView>().RPC("DestroyGameObject",RpcTarget.All,this.GetComponent<PhotonView>().ViewID);
                PlayerController.GetComponent<PhotonView>().RPC("DestroyGameObject",RpcTarget.All,other.gameObject.GetComponent<PhotonView>().ViewID);
            //Instantiate the tier 2 turret
               GameObject temp = PhotonNetwork.Instantiate(turretTier2.turretPrefab.name, _playerFirePoint.position,
                _playerFirePoint.rotation);
                temp.GetComponent<TurretBehavior>().stackEffect?.Initialization();
                temp.GetComponent<TurretBehavior>().stackEffect?.PlayFeedbacks();
           }
           else{
           Destroy(this.gameObject);
           Destroy(other.gameObject);
            //Instantiate the tier 2 turret
               GameObject temp = Instantiate(turretTier2.turretPrefab, _playerFirePoint.position,
                _playerFirePoint.rotation);
                temp.GetComponent<TurretBehavior>().stackEffect?.Initialization();
                temp.GetComponent<TurretBehavior>().stackEffect?.PlayFeedbacks();
           }
           }
           //for turrets of tier 2 combining
           else if(_currentTurret.turretTier == 1 && (other.gameObject.GetComponent<TurretBehavior>()._currentTurret.turretTier == 2 ||
           other.gameObject.GetComponent<TurretBehavior>()._currentTurret.turretTier == 3)){
               if(PhotonNetwork.OfflineMode == false)
                   PlayerController.GetComponent<PhotonView>().RPC("DestroyGameObject",RpcTarget.All,this.gameObject.GetComponent<PhotonView>().ViewID);
                   else
                   Destroy(this.gameObject);
           }
               if(_currentTurret.turretTier == 2 && other.gameObject.GetComponent<TurretBehavior>()._currentTurret.turretTier == 2){
               //Destroy both turrets
               if(PhotonNetwork.OfflineMode == false){
               PlayerController.GetComponent<PhotonView>().RPC("DestroyGameObject",RpcTarget.All,this.GetComponent<PhotonView>().ViewID);
                PlayerController.GetComponent<PhotonView>().RPC("DestroyGameObject",RpcTarget.All,other.gameObject.GetComponent<PhotonView>().ViewID);
            //Instantiate the tier 2 turret
               GameObject temp = PhotonNetwork.Instantiate(turretTier3.turretPrefab.name, _playerFirePoint.position,
                _playerFirePoint.rotation);
                temp.GetComponent<TurretBehavior>().stackEffect?.Initialization();
                temp.GetComponent<TurretBehavior>().stackEffect?.PlayFeedbacks();
               }
               else{
                   Destroy(this.gameObject);
                   Destroy(other.gameObject);
                   GameObject temp = Instantiate(turretTier3.turretPrefab, _playerFirePoint.position,
                _playerFirePoint.rotation);
                temp.GetComponent<TurretBehavior>().stackEffect?.Initialization();
                temp.GetComponent<TurretBehavior>().stackEffect?.PlayFeedbacks();
               }
           }
            else if(_currentTurret.turretTier == 2 && (other.gameObject.GetComponent<TurretBehavior>()._currentTurret.turretTier == 1 ||
           other.gameObject.GetComponent<TurretBehavior>()._currentTurret.turretTier == 3)){
               if(PhotonNetwork.OfflineMode == false)
                   PlayerController.GetComponent<PhotonView>().RPC("DestroyGameObject",RpcTarget.All,this.gameObject.GetComponent<PhotonView>().ViewID);
                   else
                   Destroy(this.gameObject);
           }
        
        }
    }
}

