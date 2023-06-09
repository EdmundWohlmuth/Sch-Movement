using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHookControl : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] PlayerController PC;
    [SerializeField] WeaponController WC;
    [SerializeField] LineRenderer LR;
    [SerializeField] Transform camTransform;
    [SerializeField] Transform hookTransform;
    [SerializeField] Camera cam;
    Rigidbody rb;

    public LayerMask grappleable;

    [Header("Grapple Stats")]
    [SerializeField] float maxDist;
    [SerializeField] float delay;
    [SerializeField] float coolDown;
    [SerializeField] float coolDownTimer;
    [SerializeField] float overshootYAxis;
    [SerializeField] float maximumVelocity;
    Vector3 velocityToSet;

    Vector3 grapplePoint;
    public bool grappling;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1;


    // Start is called before the first frame update
    void Start()
    {
        PC = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (coolDownTimer > 0)
        {
            coolDownTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(grappleKey))
        {
            StartGrapple();
        }
        if (Input.GetKeyDown(KeyCode.Space) && grappling)
        {
            Invoke(nameof(ExitGrapple), delay); // COROUTINES
        }
    }
    void LateUpdate()
    {
        if (grappling) LR.SetPosition(0, hookTransform.position);
    }

    void StartGrapple() // initiate the grapple
    {
        if (coolDownTimer > 0) return;
        //PC.freeze = true;

        grappling = true;

        RaycastHit hit;
        if (Physics.SphereCast(camTransform.position, .5f, camTransform.forward, out hit, maxDist, grappleable))
        {
            //Debug.Log("hit");
            grapplePoint = hit.point;
            //Debug.Log(hit.point);

            AudioManager.audioManager.PlaySound(PC.source, AudioManager.audioManager.grappleStart[Random.Range(0, AudioManager.audioManager.grappleStart.Count)], false);

            if (hit.transform.gameObject.layer == 7) // IF HIT ENEMY, TAKE GUN IF AVAILABLE
            {
                //Debug.Log("enemy");
                if (hit.transform.gameObject.GetComponent<WeaponController>().weapon != WeaponController.Weapons.Melee)
                {
                    WC.weapon = hit.transform.gameObject.GetComponent<WeaponController>().weapon;
                    hit.transform.gameObject.GetComponent<WeaponController>().SetMelee();
                    WC.SwitchWeapon();
                }
                else
                {
                    // pull enemy?
                }
                Invoke(nameof(ExitGrapple), delay); // if pulling enemy is a feature, move this line up to the if
            }
            else if (hit.transform.gameObject.layer == 9) // OPENABLE DOOR
            {
                // Debug.Log("door hit");
                // Debug.Log(hit.transform.gameObject.name);
                GameManager.gameManager.arenaManager.SpawnArena(hit.transform.gameObject.GetComponent<Door>().direction);
                Invoke(nameof(Grapple), delay);
            }
            else Invoke(nameof(Grapple), delay);
        }
        else
        {
            //Debug.Log("miss");
            grapplePoint = camTransform.position + camTransform.forward * maxDist;
            Invoke(nameof(ExitGrapple), delay);        
        }

        LR.enabled = true;
        LR.SetPosition(1, grapplePoint);

    }
    void Grapple()  // do the grapple
    {
        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        JumpToPosition(grapplePoint, highestPointOnArc);

        Invoke(nameof(ExitGrapple), 1f);
    }
    void ExitGrapple() // end the grapple
    {
        grappling = false;
        coolDownTimer = coolDown;
        LR.enabled = false;
    }

    //-GRAPPLE-MATHS-------
    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        grappling = true;

        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        SetVelocity();

        Invoke(nameof(ResetRestrictions), 3f);
    }
    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }
    private void SetVelocity()
    {
        //enableMovementOnNextTouch = true;
        if (rb.velocity.magnitude > 4) velocityToSet *= 1.75f;
        rb.velocity += velocityToSet;
        if (rb.velocity.magnitude > maximumVelocity)
            rb.velocity = rb.velocity.normalized * maximumVelocity;

        //cam.DoFov(grappleFov);
    }
    public void ResetRestrictions()
    {
        grappling = false;
        //cam.DoFov(85f);
    }
}
