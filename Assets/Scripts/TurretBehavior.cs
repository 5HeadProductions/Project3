using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehavior : MonoBehaviour
{
    public Transform firePoint;

    [SerializeField]Turret turret;

    private Transform _target;

    public GameObject aimArea;
    public GameObject projectilePrefab;

    private float _timeUntilAttack;


    void Update(){
        Collider2D temp = Physics2D.OverlapCircle(aimArea.transform.position,1);
        _target = temp.gameObject.transform;

        //make into coroutine
        if(Time.time > _timeUntilAttack){
            GameObject bullet = Instantiate(projectilePrefab,firePoint.transform.position, projectilePrefab.transform.rotation);
            Vector2 bulletDirection = (_target.transform.position - transform.position).normalized * 10;
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletDirection.x, bulletDirection.y);
            _timeUntilAttack = Time.time + turret.fireRate;
        }
    }

    void OnDrawGizmos(){
        Gizmos.DrawWireSphere(aimArea.transform.position, turret.range);
    }
}
