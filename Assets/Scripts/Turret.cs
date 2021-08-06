using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Turret", menuName = "Turret")]

public class Turret : ScriptableObject
{
    public int turretTier;
    public int damage;
    public int health;
    public int range;
    public GameObject projectilePrefab;
    public GameObject turretPrefab;
    public int buyCost;
    public int sellCost;

    public int fireRate;

    public int bulletSpeed;

    

}
