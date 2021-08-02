using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile", menuName = "Projectile")]
public class Projectile : ScriptableObject
{
    
    public int damage;
    public GameObject bulletPrefab;

    public float timeUnitilNextBullet;
    
}
