using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public bool isAI;
    public int health;
    public int maxHelath;

    public Slider healthBar;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.layer == 7) isAI = true;
        else if (gameObject.GetComponent<PlayerController>()) isAI = false;

        health = maxHelath;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (!isAI) healthBar.value = health;

        if (health <= 0)
        {
            health = 0;
            if (isAI) Destroy(gameObject);
            else GameManager.gameManager.GameOver();
        }
    }

    public void Heal(int healing)
    {
        health += healing;
        if (!isAI) healthBar.value = health;

        if (health > maxHelath)
        {
            health = maxHelath;
        }
    }

    public void ResetHealth()
    {
        health = maxHelath;
    }
}
