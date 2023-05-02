using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    public WeaponData weaponData;
    public int ammo;  // how many bullets gun has left


    [Header("Refs")]
    [SerializeField] Camera mainCam;
    [SerializeField] Camera gunCam;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletOrign;
    [SerializeField] Transform gunPos;
    [SerializeField] PlayerController PC;
    [SerializeField] AudioSource source;
    [SerializeField] AudioManager AM;

    [Header("Weapon handling")]
    [SerializeField] bool isAI;
    [SerializeField] bool canShoot;
    [SerializeField] bool AINeedsToReload;
    [SerializeField] float fireRateTimer;
    [SerializeField] float reloadTimer;

    // Start is called before the first frame update
    void Start()
    {
        if (!isAI) PC = GetComponent<PlayerController>();
        source = GetComponent<AudioSource>();
        SwitchWeapon();       
    }

    // Update is called once per frame
    void Update()
    {
        if (weaponData != null) Graphics.DrawMesh(weaponData.currentMesh, Matrix4x4.TRS(gunPos.position, gunPos.rotation, new Vector3(.5f, .5f, .5f)), weaponData.gunMat, 3, null, 0);

        if (!canShoot) ResetShot();
        if (isAI && AINeedsToReload) AIAmmoTrack();
    }

    public void SwitchWeapon()
    {
        // pulls the weapon type from

        for (int i = 0; i < GameManager.gameManager.weaponType.Count; i++)
        {
            if (GameManager.gameManager.weaponType[i].weaponType == weapon)     // weaponData is the script
            {                                                                   // weaponType is the List from weaponData
                weaponData = GameManager.gameManager.weaponType[i];             // weapon is the enum
                ammo = GameManager.gameManager.weaponType[i].maxAmmo;
                break;
            }
        }
    }

    public void SetMelee()
    {
        weaponData = null;
        weapon = Weapons.Melee;
        //currentMesh = null; // switch to melee mesh
        //SwitchWeapon();
    }

    public void Fire()
    {
        if (weapon == Weapons.Melee) return;
        if (!canShoot || isAI && AINeedsToReload && AINeedsToReload) return;
        if (weaponData == null) return;

        //Debug.Log("BANG!");

        //Debug.Log("BANG!");

        Ray ray;

        if (!isAI) ray = gunCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // player uses camera
        else ray = new Ray(gunPos.transform.position, gunPos.transform.forward); // enemy uses gun pos

        RaycastHit hit;
        Vector3 targetPos;

        if (Physics.Raycast(ray, out hit)) targetPos = hit.point;
        else targetPos = ray.GetPoint(50);

        Vector3 bulletDirectrion = targetPos - bulletOrign.position;

        for (int i = 0; i < weaponData.projectilesPerShot; i++)
        {
            float spreadX = Random.Range(-weaponData.bulletSpread, weaponData.bulletSpread);
            float spreadY = Random.Range(-weaponData.bulletSpread, weaponData.bulletSpread);

            Vector3 directionWithSpread = bulletDirectrion + new Vector3(spreadX, spreadY, 0f);
            bulletDirectrion = directionWithSpread;

            GameObject currentBullet = Instantiate(bullet, bulletOrign.position, Quaternion.identity);
            currentBullet.transform.right = bulletDirectrion.normalized;
            currentBullet.GetComponent<Bullet>().damage = weaponData.damage;
            currentBullet.GetComponent<Rigidbody>().AddForce(bulletDirectrion.normalized * weaponData.projectileSpeed, ForceMode.Impulse);
        }

        if (weaponData != null) AudioManager.audioManager.PlaySound(source, weaponData.sound, false);
        if (!isAI)
        {
            ammo--;
          
            if (ammo <= 0)
            {
                // toss weapon

                SetMelee();
            }
            PC.Recoil();           
            if (weaponData != null) mainCam.DOShakePosition(0.25f, weaponData.camShake, 10, 30);           
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
            if (weaponData != null) ammo = weaponData.maxAmmo;
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
        if (weaponData == null) return;
        if (fireRateTimer >= weaponData.fireRate)
        {
            fireRateTimer = weaponData.fireRate;
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
