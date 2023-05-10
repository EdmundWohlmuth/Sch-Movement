using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedWeapon : MonoBehaviour
{
    Rigidbody rb;
    public int weaponNum;
    public bool taken;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        GameManager.droppedWeapons.Add(gameObject);
        rb.AddForce(Vector3.right * 2f, ForceMode.Impulse);      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUp(WeaponController wc)
    {
       // Debug.Log("PICKUP");
        wc.GetComponent<WeaponController>().weapon = (WeaponController.Weapons)weaponNum;
        wc.SwitchWeapon();
        GameManager.droppedWeapons.Remove(gameObject);
        Destroy(gameObject);
    }
}
