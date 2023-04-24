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
    int ammo;       // how many bullets gun has
    int damage;     // damage gun deals
    int knockback;  // how much the gun pushes player in air

    [Header("Weapon stats")]
    int DoubleBarrelAmmo = 2;
    int DoubleBarrelDmg = 10;
    int DoubleBarrelKnockback = 10;

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
}
