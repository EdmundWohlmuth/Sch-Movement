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
    [SerializeField] state currentState;

    bool sightCheck;
    bool attackCheck;
    bool lineOfSight;
    bool obstructedSight;

    [SerializeField] bool lookingForGun;

    [SerializeField] float attackRange;
    [SerializeField] float sightRange;

    [SerializeField] LayerMask playerMask;
    [SerializeField] LayerMask obstrutionMask;
    Vector3 startPos;

    // Refrences
    GameObject player;
    public GameObject viewPos;
    [SerializeField] GameObject destination;
    public NavMeshAgent agent;
    WeaponController WC;
    [SerializeField] GameObject gunPos;
    [SerializeField] GameObject pos;
    public Animator animator;

    private void OnEnable()
    {
        GameManager.currentEnemies.Add(this);
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
        animator.SetFloat("XMovement", agent.velocity.x);
        animator.SetFloat("ZMovement", agent.velocity.z);

        if (WC.weapon == WeaponController.Weapons.Melee && lookingForGun == false && destination == null)
        {
            lookingForGun = true;
            currentState = state.missingGun;
        }

        sightCheck = Physics.CheckSphere(viewPos.transform.position, sightRange, playerMask);
        lineOfSight = Physics.Raycast(viewPos.transform.position, player.transform.position - viewPos.transform.position, sightRange, playerMask);
        obstructedSight = Physics.Raycast(viewPos.transform.position, player.transform.position - viewPos.transform.position, sightRange, obstrutionMask);

        

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

                gunPos.transform.rotation = new Quaternion(0, 0, 0, 0);

                if (!sightCheck && !lineOfSight || !sightCheck && lineOfSight)
                {
                    Debug.DrawRay(viewPos.transform.position, player.transform.position - viewPos.transform.position, Color.gray, obstrutionMask);
                    agent.SetDestination(startPos);
                    currentState = state.idle;
                }
                /*else if (lineOfSight && obstructedSight)
                {
                    //currentState = state.idle;
                    agent.SetDestination(startPos);
                    Debug.DrawRay(viewPos.transform.position, player.transform.position - viewPos.transform.position, Color.gray, obstrutionMask);
                }*/
                else if (lineOfSight) // && !o
                {
                    Debug.DrawRay(viewPos.transform.position, player.transform.position - viewPos.transform.position, Color.blue, playerMask);

                    agent.SetDestination(player.transform.position); // go to player                   

                    attackCheck = Physics.CheckSphere(viewPos.transform.position, attackRange, playerMask);

                    if (attackCheck && lineOfSight && !obstructedSight)
                    {
                        currentState = state.attack;
                    }
                }
                else if (!lineOfSight && obstructedSight)
                {
                    currentState = state.idle;
                    Debug.DrawRay(viewPos.transform.position, player.transform.position - viewPos.transform.position, Color.black);
                }
                else
                {
                    Debug.Log("Line of Sight: " + lineOfSight);
                    Debug.Log("Obstructed: " + obstructedSight);
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
                Debug.DrawRay(viewPos.transform.position, player.transform.position - viewPos.transform.position, Color.red);

                break;

            case state.missingGun:
                
                if (lookingForGun) FindGun();
                else if (destination != null) PickpGun();

                if (WC.weapon != WeaponController.Weapons.Melee) currentState = state.chase;

                break;

            default:

                break;
        }

        if (currentState != state.missingGun)
        {
            agent.stoppingDistance = 10f;
        }
        else agent.stoppingDistance = 0.1f;
    }

    void PickpGun()
    {
        agent.SetDestination(destination.transform.position);
       // Debug.Log(Vector3.Distance(transform.position, destination.transform.position));

        if (Vector3.Distance(transform.position, destination.transform.position) < 1f)
        {
            //Debug.Log(gameObject.name + " picked up gun");
            destination.GetComponent<DroppedWeapon>().PickUp(WC);
            destination = null;
        }
    }

    void FindGun()
    {        
        NavMeshPath path = new NavMeshPath();

        if (GameManager.droppedWeapons.Count > 0)
        {
            for (int i = 0; i < GameManager.droppedWeapons.Count; i++)
            {
                if (agent.CalculatePath(GameManager.droppedWeapons[i].transform.position, path))
                {
                    if (destination != null && destination != GameManager.droppedWeapons[i].gameObject)
                    {
                        if (Vector3.Distance(transform.position, destination.transform.position) > 
                            Vector3.Distance(transform.position, GameManager.droppedWeapons[i].transform.position))
                        {
                            if (destination.GetComponent<DroppedWeapon>().taken != true)
                            {
                                destination = GameManager.droppedWeapons[i].gameObject;
                                destination.GetComponent<DroppedWeapon>().taken = true;
                            }
                                                     
                        }
                    } 
                    else if (destination == null) destination = GameManager.droppedWeapons[i].gameObject;
                }
            }
            if (destination != null)
            {
                agent.SetDestination(destination.transform.position);
                lookingForGun = false;
            }
            else lookingForGun = true;
        }

        void OnDrawGizmos()
        {
            //Gizmos.color = Color.red;
            //Gizmos.DrawWireSphere(transform.position, attackRange);

            //Gizmos.color = Color.yellow;
            //Gizmos.DrawWireSphere(transform.position, sightRange);

            /*        if (lineOfSight && !obstructedSight)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawLine(pos.transform.position, player.transform.position);
                    }
                    else if (obstructedSight)
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawRay(pos.transform.position, player.transform.position);
                    }
                    else if (!sightCheck)
                    {
                        Gizmos.color = Color.gray;
                        Gizmos.DrawLine(pos.transform.position, player.transform.position);
                    }*/

            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(agent.destination, 0.35f);
        }

    }
}
