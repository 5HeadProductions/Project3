using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemyStats : ScriptableObject
{   

    public GameObject shipType;
    public GameObject enemyBullet;
    public int health;
    public int damage;
    public float speed;
    public float fireRate;

    public int coinsDroppedOnDeath;
    
}
