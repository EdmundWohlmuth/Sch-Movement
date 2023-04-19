using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHookControl : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] PlayerController PC;
    [SerializeField] LineRenderer LR;
    [SerializeField] Transform camTransform;
    [SerializeField] Transform hookTransform;

    public LayerMask grappleable;

    [Header("Grapple Stats")]
    [SerializeField] float maxDist;
    [SerializeField] float delay;
    [SerializeField] float coolDown;
    [SerializeField] float coolDownTimer;

    Vector3 grapplePoint;
    [SerializeField] bool grappling;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1;


    // Start is called before the first frame update
    void Start()
    {
        PC = GetComponent<PlayerController>();
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
            Invoke(nameof(ExitGrapple), delay);
        }
    }
    void LateUpdate()
    {
        if (grappling) LR.SetPosition(0, hookTransform.position);
    }

    void StartGrapple() // initiate the grapple
    {
        if (coolDownTimer > 0) return;

        grappling = true;

        RaycastHit hit;
        if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, maxDist, grappleable))
        {
            Debug.Log("hit");
            grapplePoint = hit.point;
            Debug.Log(hit.point);

            Invoke(nameof(Grapple), delay);
        }
        else
        {
            Debug.Log("miss");
            grapplePoint = camTransform.position + camTransform.forward * maxDist;
            Invoke(nameof(ExitGrapple), delay);        
        }

        LR.enabled = true;
        LR.SetPosition(1, grapplePoint);

    }
    void Grapple()  // do the grapple
    {

    }
    void ExitGrapple() // end the grapple
    {
        grappling = false;
        coolDownTimer = coolDown;
        LR.enabled = false;
    }
}
