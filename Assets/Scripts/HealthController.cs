using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public bool isAI;
    public int health;
    public int maxHelath;

    [Header("TEMP MATS")]
    public Renderer renderer;
    public Material basicMat;
    public Material damagedMat;

    public Slider healthBar;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.layer == 7) isAI = true;
        else if (gameObject.GetComponent<PlayerController>()) isAI = false;

        health = maxHelath;

        renderer = GetComponentInChildren<Renderer>();
        if (!isAI) healthBar = GameObject.Find("Health").GetComponent<Slider>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (!isAI) healthBar.value = health;

        if (health <= 0)
        {
            health = 0;
            if (isAI) 
            {
                GameManager.currentEnemies.Remove(this.gameObject.GetComponent<AIController>());
                Destroy(gameObject);
            }          
            else GameManager.gameManager.GameOver();
        }

        if (isAI) StartCoroutine(Damage());
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

    IEnumerator Damage()
    {
        renderer.material = damagedMat;
        yield return new WaitForSeconds(.2f);
        renderer.material = basicMat;
    }
}
