using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehavior : MonoBehaviour
{
    public Transform firePoint;

    [SerializeField]Turret turret;

    private Transform _target;

    public GameObject aimArea;

    private float _timeUntilAttack;
    
    IEnumerator _currentCoroutine;

    [SerializeField]Animator animator;


    
    void Update(){
        //finds and assigns the transform of the target so we can shoot in their direction
        Collider2D temp = Physics2D.OverlapCircle(aimArea.transform.position,1);
        _target = temp.gameObject.transform;

        Debug.Log(_target.gameObject.name);
        if(Time.time > _timeUntilAttack  && _target != null){
            animator.SetBool("Firing",true);
            Debug.Log(_target.gameObject.name);
            GameObject bullet = Instantiate(turret.projectilePrefab,firePoint.transform.position, turret.projectilePrefab.transform.rotation);
            Vector2 bulletDirection = (_target.transform.position - transform.position).normalized * 10;
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletDirection.x, bulletDirection.y);
            _timeUntilAttack = Time.time + turret.fireRate;
            StartCoroutine(animatorWait());
        }
        
        
    }

    void OnDrawGizmos(){
        Gizmos.DrawWireSphere(aimArea.transform.position, turret.range);
    }

    IEnumerator animatorWait(){
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("Firing",false);

    }
}
