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
        attack,
        missingGun
    }
    state currentState;

    bool sightCheck;
    bool attackCheck;
    bool lineOfSight;
    bool obstructedSight;
    [SerializeField] float attackRange;
    [SerializeField] float sightRange;
    
    [SerializeField] LayerMask playerMask;
    [SerializeField] LayerMask obstrutionMask;
    Vector3 startPos;

    // Refrences
    GameObject player;
    [SerializeField] NavMeshAgent agent;
    WeaponController WC;
    [SerializeField] GameObject gunPos;

    private void OnEnable()
    {
        GameManager.currentEnemies.Add(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player"); // prefreably the only time and place I use this
        WC = GetComponent<WeaponController>();

        startPos = transform.position;
        currentState = state.idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (WC.weapon == WeaponController.Weapons.Melee) currentState = state.missingGun;

        sightCheck = Physics.CheckSphere(transform.position, sightRange, playerMask);
        lineOfSight = Physics.Raycast(transform.position, player.transform.position - transform.position, sightRange, playerMask);
        obstructedSight = Physics.Raycast(transform.position, player.transform.position - transform.position, sightRange, obstrutionMask);

        //-STATE-MACHINE-------
        switch (currentState)
        {
            case state.idle:

                if (lineOfSight && sightCheck && !obstructedSight)
                {
                    currentState = state.chase;
                }
                              
                break;

            case state.chase:

                gunPos.transform.rotation = new Quaternion(0,0,0,0);

                if (!sightCheck && !lineOfSight)
                {
                    agent.SetDestination(startPos);
                    currentState = state.idle;
                }
                else if (sightCheck && lineOfSight && !obstructedSight)
                {
                    agent.SetDestination(player.transform.position); // go to player                   

                    attackCheck = Physics.CheckSphere(transform.position, attackRange, playerMask);

                    if (attackCheck && lineOfSight && !obstructedSight)
                    {
                        currentState = state.attack;
                    }
                }

                break;

            case state.attack:

                //Debug.Log("pew");
                // fight
                gunPos.transform.LookAt(player.transform.position);

                WC.Fire();

                agent.SetDestination(player.transform.position);               

                if (!attackCheck && !lineOfSight || obstructedSight)
                {
                    currentState = state.chase;
                }

                break;

            case state.missingGun:


                break;

            default:

                break;
        }
    }

    void OnStartedState(state state)
    {
        switch (state)
        {
            case state.idle:
                break;

            case state.chase:
                break;

            case state.attack:
                break;

            case state.missingGun:
                break;

            default:
                break;
        }
    }
    void OnEndedState(state state)
    {
        switch (state)
        {
            case state.idle:
                break;

            case state.chase:
                break;

            case state.attack:
                break;

            case state.missingGun:
                break;

            default:
                break;
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, attackRange);

        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, sightRange);

        /*if (lineOfSight && !obstructedSight)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, player.transform.position);
        }
        else if (obstructedSight)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, player.transform.position - transform.position);
        }
        else if (!sightCheck)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawLine(transform.position, player.transform.position);
        }*/

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(agent.destination, 0.35f);
    }

}
