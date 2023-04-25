using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    enum state
    {
        idle,
        chase,
        attack
    }
    state currentState;

    [SerializeField] bool sightCheck;
    [SerializeField] bool attackCheck;
    [SerializeField] bool lineOfSight;
    [SerializeField] float sightRange;
    [SerializeField] float attackRange;

    // Refrences
    GameObject player;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");

        currentState = state.idle;
    }

    // Update is called once per frame
    void Update()
    {            
        //-STATE-MACHINE-------
        switch (currentState)
        {
            case state.idle:

                sightCheck = Physics.CheckSphere(transform.position, sightRange, 6);
                lineOfSight = Physics.Raycast(transform.position, transform.forward, sightRange, 6);

                break;

            case state.chase:

                if (!sightCheck && !lineOfSight)
                {
                    currentState = state.idle;
                }
                else if (sightCheck && lineOfSight)
                {
                    attackCheck = Physics.CheckSphere(transform.position, attackRange, 6);
                    if (attackCheck)
                    {
                        currentState = state.attack;
                    }
                }

                break;

            case state.attack:

                // fight

                break;

            default:
                break;
        }
    }


}
