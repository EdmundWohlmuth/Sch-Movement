using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedWeapon : MonoBehaviour
{
    Rigidbody rb;
    int weaponNum;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        GameManager.droppedWeapons.Add(gameObject);
        rb.AddForce(Vector3.forward * 2f, ForceMode.Impulse);      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUp(WeaponController wc)
    {
        wc.GetComponent<WeaponController>().weapon = (WeaponController.Weapons)weaponNum;
    }
}
