using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    // CHARACTER CONTROLLER BY: "DAVE / GAME DEVLOPEMENT" https://www.youtube.com/@davegamedevelopment
    // WITH EDITS BY EDMUND WOHLMUTH

    [Header("Movement")]
    [SerializeField] float speed;
    [SerializeField] float speedModifier;
    [SerializeField] float maxSpeed;
    [SerializeField] float drag;
    float horizontalInput;
    float verticalInput;

    [Header("Jumping")]
    [SerializeField] float jumpForce;
    [SerializeField] float jumpCoolDown;
    [SerializeField] float airMulriplier;
    [SerializeField] bool canJump;
    [SerializeField] float wallJumpHorizontalForce;
    [SerializeField] float wallJumpVerticalForce;
    KeyCode jumpKey = KeyCode.Space;

    [Header("Wall Running")]
    [SerializeField] float wallRunSpeed;
    [SerializeField] float wallRunForce;
    [SerializeField] float maxWallRunTime;
    float wallRunTimer;
    [SerializeField] float wallCheckDist;
    [SerializeField] float minJumpHeight;
    RaycastHit leftWallHit;
    RaycastHit rightWallHit;
    bool leftWall;
    bool rightWall;
    [SerializeField] bool wallRunning;
    [SerializeField] bool exitingWall;
    float exitWallTime;
    [SerializeField] float exitWallTimer;

    [Header("GroundCheck")]
    [SerializeField] float height;
    public LayerMask ground;
    [SerializeField] bool isGrounded;
    Vector3 moveDirection;

    [Header("Gun Control")]
    KeyCode shootKey = KeyCode.Mouse0;
    bool shooting;

    [Header("Refrences")]
    public Rigidbody rb;
    public GameObject cam;
    public GameObject camHolder;
    public GrapplingHookControl GHC;
    public Transform orientation;
    public WeaponController WC;
    public GameObject speedlinesHolder;
    SimpleCameraController cameraScript;
    public static PlayerController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GHC = GetComponent<GrapplingHookControl>();
        rb.freezeRotation = true;
        cameraScript = cam.GetComponent<SimpleCameraController>();
        speedlinesHolder.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
        ClampSpeed();
        CheckGrounded();
        if (!isGrounded) CheckForWall();
        if (exitingWall) ExitWallRun();
    }
    private void FixedUpdate()
    {
        if (wallRunning) WallRunningMovement();
        else MovePlayer();

        //effects
        if (rb.velocity.magnitude > 20f) 
        {
            speedlinesHolder.SetActive(true);
            cameraScript.FOVEffect(75, .25f);
        } 
        else if (rb.velocity.magnitude > 15f)
        {
            speedlinesHolder.SetActive(false);
            cameraScript.FOVEffect(75, .25f);
        }
        else
        {
            speedlinesHolder.SetActive(false);
            cameraScript.FOVEffect(60, .25f);
        }           
    }

    void PlayerInput()
    {
        if (WC.weaponData != null && WC.weaponData.isAutoFire) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        // if (Input.GetAxisRaw("Horizontal") == 1) Debug.Log("horizontal");
        // if (Input.GetAxisRaw("Vertical") == 1) Debug.Log("vertical");
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && canJump && isGrounded) // might alter this to make Bhopping more difficult
        {
            canJump = false;
            Jump();
            StartCoroutine(WaitforJump(jumpCoolDown)); // Invoke - runs meathods after x seconds           
        }
        if (shooting)
        {
            WC.Fire();
        }
    }

    //-MOVEMENT-------
    void MovePlayer()
    {
        if (GHC.grappling) return;
        moveDirection = cam.transform.forward * verticalInput + cam.transform.right * horizontalInput;

        if (isGrounded) rb.AddForce(new Vector3(moveDirection.normalized.x, 0f, moveDirection.normalized.z) * speed * speedModifier, ForceMode.Force);
        else rb.AddForce(new Vector3(moveDirection.normalized.x, 0f, moveDirection.normalized.z) * speed * airMulriplier, ForceMode.Force);
    }
    void ClampSpeed()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > speed && isGrounded)
        {
            /*Vector3 clampVel = flatVel.normalized * speed;
            rb.velocity = new Vector3(clampVel.x, rb.velocity.y, clampVel.z);*/

            rb.velocity = Vector3.MoveTowards(rb.velocity, Vector3.zero, 1.5f * Time.fixedDeltaTime);
        }
    }

    //-WALLRUNNING------
    void StartWallRun()
    {
        //Debug.Log("Wall Running!");
        wallRunning = true;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); // might want to disable this
        if (leftWall) cameraScript.TiltEffect(-5f, 0.25f);
        else if (rightWall) cameraScript.TiltEffect(5f, 0.25f);
    }
    void WallRunningMovement()
    {
        rb.useGravity = false;

        Vector3 wallNormal = rightWall ? rightWallHit.normal : leftWallHit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude) wallForward = -wallForward;

        rb.AddForce(wallForward * speed, ForceMode.Force);
        // stick to wall
        if (!(leftWall && horizontalInput < 0) && !(rightWall && horizontalInput > 0)) rb.AddForce(-wallNormal * wallRunForce, ForceMode.Force);
    }
    void StopWallRun()
    {
        //Debug.Log("Stop Wall Running");
        wallRunning = false;
        rb.useGravity = true;
        cameraScript.TiltEffect(0, 0.25f);
    }

    void ExitWallRun()
    {
        if (wallRunning) StopWallRun();

        if (exitWallTime > 0)
        {
            exitWallTime -= Time.deltaTime;
        }
        else if (exitWallTime <= 0)
        {
            exitingWall = false;
        }       
    }

    public void Recoil()
    {
        if (WC.weaponData == null) return;
        if (!isGrounded) rb.AddForce(-cam.transform.forward * WC.weaponData.knockback, ForceMode.Impulse);
    }

    //-JUMP-CONTROLS----
    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    void ResetJump()
    {
        canJump = true;
    }

    //-WALL-JUMPING-----
    void WallJump()
    {
        // enter exiting wall state
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = rightWall ? rightWallHit.normal : leftWallHit.normal;

        Vector3 forceToApply = transform.up * wallJumpVerticalForce + wallNormal * wallJumpHorizontalForce;

        // reset y velocity and add force
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }

    //-GROUNED-CHECK----
    void CheckGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, height * 0.5f + 0.2f, ground);
        if (isGrounded) rb.drag = drag;
        else rb.drag = 0.5f;
    }
    //-WALL-CHECK-------
    void CheckForWall()
    {
        rightWall = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDist);
        leftWall = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDist);

        if (rightWall || leftWall) 
        {
            if (Input.GetKeyDown(jumpKey))
            {
                //Debug.Log("Wall Jump!");
                StopWallRun();
                WallJump();
            }           
        }
        
        if (rightWall || leftWall && !isGrounded && verticalInput > 0 && !exitingWall)
        {
            if (!wallRunning) StartWallRun();
            
        }
        else
        {
            if (wallRunning) StopWallRun();
        }
    }

    IEnumerator WaitforJump(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ResetJump();
    }

    //-EXTRA-SFX-----------------
    public void CameraShake()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(gameObject.transform.position, new Vector3(1, 2, 1));
    }
}