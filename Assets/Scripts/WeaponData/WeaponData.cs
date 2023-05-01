using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
    public WeaponController.Weapons weaponType;
    public Material gunMat;
    public Mesh currentMesh;
    //public GameObject projectile;
    public int maxAmmo;                             // how many bullets gun has total
    public int damage;                              // damage gun deals
    public int knockback;                           // how much the gun pushes player in air
    public int projectilesPerShot;                  // how many bullets per shot (shotguns)
    public float projectileSpeed;                   // how quick the bullets move
    public float fireRate;                          // how quickly does the gun shoot
    public float bulletSpread;                      // how much the bullet deviates
    public float camShake;
    public bool isAutoFire;                         // can hold down to shoot
}
