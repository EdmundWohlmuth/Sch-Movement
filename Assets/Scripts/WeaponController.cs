using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public enum Weapons
    {
        DBShotgun, // Double Barrel
        RShotgun,  // Repeater
        AutoPistol, // 3 round burst
        Revolver,  
        SMG,
        AssaultRifle,
        RPG,
        Melee // default
    }
    public Weapons weapon;

    [Header("Current Weapon Stats")]
    int maxAmmo;                // how many bullets gun has total
    int ammo;                   // how many bullets gun has left
    int damage;                 // damage gun deals
    int knockback;              // how much the gun pushes player in air
    int projectilesPerShot;     // how many bullets per shot (shotguns)
    int projectileSpeed;        // how quick the bullets move
    int fireRate;               // how quickly does the gun shoot
    int bulletSpread;           // how much the bullet deviates
    bool isAutoFire;            // can hold down to shoot

    [Header("DBShotgun stats")]
    int doubleBarrelAmmo = 2;
    int doubleBarrelDmg = 10;
    int doubleBarrelKnockback = 10;
    int doubleBarrelBulletAmmount = 12;

    [Header("RShotgun stats")]
    int repeaterAmmo = 6;
    int repeaterDmg = 10;
    int repeaterKnockback = 10;

    [Header("AutoPistol stats")]

    [Header("Revolver stats")]

    [Header("SMG stats")]

    [Header("AssaultRifle stats")]

    [Header("RPG stats")]

    [Header("Refs")]
    [SerializeField] Camera mainCam;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletOrign;


    [SerializeField] bool isAI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchWeapon()
    {
        // Load relevent mesh when equiping new weapon
        // unload old mesh
        // just the mesh like is done in the village game with the selection icon

        switch (weapon)
        {
            case Weapons.DBShotgun:

                maxAmmo = doubleBarrelAmmo;
                ammo = doubleBarrelAmmo;
                damage = doubleBarrelDmg;
                knockback = doubleBarrelKnockback;

                break;

            case Weapons.RShotgun:

                break;

            case Weapons.AutoPistol:
                break;

            case Weapons.Revolver:
                break;

            case Weapons.SMG:
                break;

            case Weapons.AssaultRifle:
                break;

            case Weapons.RPG:
                break;

            case Weapons.Melee:
                if (isAI)
                {
                    // find new weapon
                }
                else
                {
                    // run player input
                }
                break;

            default:
                break;
        }
    }

    public void SetMelee()
    {
        weapon = Weapons.Melee;
    }

    public void Fire()
    {
        Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
        RaycastHit hit;
        Vector3 targetPos;

        if (Physics.Raycast(ray, out hit)) targetPos = hit.point;
        else targetPos = ray.GetPoint(50);

        Vector3 bulletDirectrion = targetPos - bulletOrign.position;

        float spreadX = Random.Range(-bulletSpread, bulletSpread);
        float spreadY = Random.Range(-bulletSpread, bulletSpread);

        Vector3 directionWithSpread = bulletDirectrion + new Vector3(spreadX, spreadY, 0f);
        directionWithSpread = bulletDirectrion;

        for (int i = 0; i < projectilesPerShot; i++)
        {
            GameObject currentBullet = Instantiate(bullet, bulletOrign.position, Quaternion.identity);
            currentBullet.transform.forward = bulletDirectrion.normalized;
            currentBullet.GetComponent<Rigidbody>().AddForce(bulletDirectrion.normalized * projectileSpeed, ForceMode.Impulse);
        }

        if (!isAI)
        {
            ammo--;

            if (ammo <= 0)
            {
                // toss weapon
                SetMelee();
            }
        }

        if (isAutoFire)
        {
            // keep firing
        }
        else
        {

        }

    }

    void ResetShot()
    {

    }

    public void Melee()
    {

    }
}
