using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool isAI;
    public int damage;
    float destoryTimer = 5;

    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreLayerCollision(8, 8);     
    }

    // Update is called once per frame
    void Update()
    {
        if (destoryTimer <= 0)
        {
            Destroy(gameObject);
        }
        else destoryTimer -= Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 0)
        {           
            // create an effect on hit point
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == 7)
        {
            if (isAI)
            {
                Physics.IgnoreCollision(gameObject.GetComponent<CapsuleCollider>(), collision.collider);
                return;
            }
                
            // Deal damage
            collision.gameObject.GetComponent<HealthController>().TakeDamage(damage);

            //Debug.Log("hit Enemy");
            Destroy(gameObject);          
        }
        else if (collision.gameObject.layer == 6)
        {
            if (!isAI)
            {
                Physics.IgnoreCollision(gameObject.GetComponent<CapsuleCollider>(), collision.collider);
                return;
            } 
            // Deal damage
            collision.gameObject.GetComponent<HealthController>().TakeDamage(damage);

            //Debug.Log("hit Player");
            Destroy(gameObject);
        }
        
    }
}
