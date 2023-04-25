using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        Physics.IgnoreLayerCollision(8, 8);  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 0)
        {
            // create an effect on hit point
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == 7 || collision.gameObject.layer == 6)
        {
            // Deal damage
            Destroy(gameObject);
        }
        
    }
}
