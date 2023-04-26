using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public bool isAI;
    public int health;
    public int maxHelath;

    PlayerController PC; 

    // Start is called before the first frame update
    void Start()
    {
        PC = GetComponent<PlayerController>();

        if (gameObject.layer == 7) isAI = true;
        else isAI = false;

        health = maxHelath;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;

        }
    }

    public void Heal(int healing)
    {
        health += healing;
        if (health > maxHelath)
        {
            health = maxHelath;
        }
    }
}
