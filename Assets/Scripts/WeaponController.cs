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
    [Header("Current Weapon Stats")]
    public Weapons weapon;

    public Material gunMat;
    [SerializeField] Mesh currentMesh;
    [SerializeField] int maxAmmo;                // how many bullets gun has total
    [SerializeField] int ammo;                   // how many bullets gun has left
    [SerializeField] int damage;                 // damage gun deals
    public int knockback;                        // how much the gun pushes player in air
    [SerializeField] int projectilesPerShot;     // how many bullets per shot (shotguns)
    [SerializeField] float projectileSpeed;      // how quick the bullets move
    [SerializeField] float fireRate;             // how quickly does the gun shoot
    [SerializeField] float bulletSpread;         // how much the bullet deviates
    public bool isAutoFire;                      // can hold down to shoot

    [SerializeField] float fireRateTimer;
    [SerializeField] float reloadTimer;

    [Header("DBShotgun stats")]
    public Mesh DBShotgunMesh;
    int doubleBarrelAmmo = 2;
    int doubleBarrelDmg = 3;
    int doubleBarrelKnockback = 20;
    int doubleBarrelBulletAmmount = 12;
    float doubleBarrelProjectileSpeed = 50;        
    float doubleBarrelFireRate = 0.25f;
    float doubleBarrelBulletSpread = 0.5f;           

    [Header("RShotgun stats")]
    int repeaterAmmo = 5;
    int repeaterDmg = 2;
    int repeaterKnockback = 10;
    int repeaterBulletAmmount = 10;
    float repeaterProjectileSpeed = 50;
    float repeaterFireRate = 0.1f;
    float repeaterBulletSpread = 0.3f;

    [Header("AutoPistol stats")]

    [Header("Revolver stats")]

    public Mesh RevolverMesh;
    int revolverAmmo = 6;
    int revolverDmg = 10;
    int revolverKnockback = 6;
    int revolverBulletAmmount = 1;
    float revolverProjectileSpeed = 50;
    float revolverFireRate = 0.5f;
    float revolverBulletSpread = 0f;

    [Header("SMG stats")]

    [Header("AssaultRifle stats")]

    [Header("RPG stats")]

    [Header("Refs")]
    [SerializeField] Camera mainCam;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletOrign;
    [SerializeField] Transform gunPos;


    [SerializeField] bool isAI;
    [SerializeField] bool canShoot;
    [SerializeField] bool AINeedsToReload;

    // Start is called before the first frame update
    void Start()
    {
        SwitchWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        Graphics.DrawMesh(currentMesh, Matrix4x4.TRS(gunPos.position, gunPos.rotation, new Vector3(.5f, .5f, .5f)), gunMat, 3, null, 0);

        if (!canShoot) ResetShot();
        if (isAI && AINeedsToReload) AIAmmoTrack();
    }

    public void SwitchWeapon()
    {
        // Load relevent mesh when equiping new weapon
        // unload old mesh
        // just the mesh like is done in the village game with the selection icon

        switch (weapon)
        {
            case Weapons.DBShotgun:

                //Debug.Log("SHOT-GUN");
                maxAmmo = doubleBarrelAmmo;
                ammo = doubleBarrelAmmo;
                damage = doubleBarrelDmg;
                knockback = doubleBarrelKnockback;
                projectilesPerShot = doubleBarrelBulletAmmount;
                projectileSpeed = doubleBarrelProjectileSpeed;        
                fireRate = doubleBarrelFireRate;              
                bulletSpread = doubleBarrelBulletSpread;           
                isAutoFire = false;
                currentMesh = DBShotgunMesh;
                canShoot = true;

                break;

            case Weapons.RShotgun:

                maxAmmo = repeaterAmmo;
                ammo = repeaterAmmo;
                damage = repeaterDmg;
                knockback = repeaterKnockback;
                projectilesPerShot = repeaterBulletAmmount;
                projectileSpeed = repeaterProjectileSpeed;
                fireRate = repeaterFireRate;
                bulletSpread = repeaterBulletSpread;
                isAutoFire = false;
                canShoot = true;

                break;

            case Weapons.AutoPistol:
                break;

            case Weapons.Revolver:

                maxAmmo = revolverAmmo;
                ammo = revolverAmmo;
                damage = revolverDmg;
                knockback = revolverKnockback;
                projectilesPerShot = revolverBulletAmmount;
                projectileSpeed = revolverProjectileSpeed;
                fireRate = revolverFireRate;
                bulletSpread = revolverBulletSpread;
                isAutoFire = false;
                currentMesh = RevolverMesh;
                canShoot = true;

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
        currentMesh = null; // switch to melee mesh
    }

    public void Fire()
    {
        if (weapon == Weapons.Melee) return;
        if (!canShoot || isAI && AINeedsToReload && AINeedsToReload) return;

        //Debug.Log("BANG!");

        Ray ray;

        if (!isAI) ray = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // player uses camera
        else ray = new Ray(gunPos.transform.position, gunPos.transform.forward); // enemy uses gun pos

        RaycastHit hit;
        Vector3 targetPos;

        if (Physics.Raycast(ray, out hit)) targetPos = hit.point;
        else targetPos = ray.GetPoint(50);

        Vector3 bulletDirectrion = targetPos - bulletOrign.position;

        for (int i = 0; i < projectilesPerShot; i++)
        {
            float spreadX = Random.Range(-bulletSpread, bulletSpread);
            float spreadY = Random.Range(-bulletSpread, bulletSpread);

            Vector3 directionWithSpread = bulletDirectrion + new Vector3(spreadX, spreadY, 0f);
            bulletDirectrion = directionWithSpread;

            GameObject currentBullet = Instantiate(bullet, bulletOrign.position, Quaternion.identity);
            currentBullet.transform.right = bulletDirectrion.normalized;
            currentBullet.GetComponent<Bullet>().damage = damage;
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
        else
        {
            ammo--;
            if (ammo <= 0)
            {
                reloadTimer = 0;
                AINeedsToReload = true;
            }               
        }
        fireRateTimer = 0;
        canShoot = false;
    }

    void AIAmmoTrack()
    {
        if (reloadTimer >= 2) // hardcoded "reload" timer for ai
        {
            reloadTimer = 2;
            AINeedsToReload = false;
            ammo = maxAmmo;
        }
        else
        {
            reloadTimer += Time.deltaTime;
            AINeedsToReload = true;
            canShoot = false;
        }
    }

    void ResetShot()
    {
        if (fireRateTimer >= fireRate)
        {
            fireRateTimer = fireRate;
            canShoot = true;
        }
        else
        {
            fireRateTimer += Time.deltaTime;
            canShoot = false;
        }
    }

    public void Melee()
    {

    }
}
