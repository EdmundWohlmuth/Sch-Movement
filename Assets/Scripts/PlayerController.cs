using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    KeyCode jumpKey = KeyCode.Space;

    [Header("GroundCheck")]
    [SerializeField] float height;
    public LayerMask ground;
    [SerializeField] bool isGrounded;


    Vector3 moveDirection;

    [Header("Refrences")]
    Rigidbody rb;
    public GameObject cam;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
        ClampSpeed();
        CheckGrounded();

        // Debug.Log(transform.position);
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }

    void PlayerInput()
    {
        // if (Input.GetAxisRaw("Horizontal") == 1) Debug.Log("horizontal");
        // if (Input.GetAxisRaw("Vertical") == 1) Debug.Log("vertical");
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && canJump && isGrounded)
        {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCoolDown); // Invoke - runs meathods after x seconds
        }
    }

    //-MOVEMENT-------
    void MovePlayer()
    {
        moveDirection = cam.transform.forward * verticalInput + cam.transform.right * horizontalInput;

        if (isGrounded) rb.AddForce(new Vector3(moveDirection.normalized.x, 0f, moveDirection.normalized.z) * speed * speedModifier, ForceMode.Force);
        else rb.AddForce(new Vector3(moveDirection.normalized.x, 0f, moveDirection.normalized.z) * speed * airMulriplier, ForceMode.Force);
    }
    void ClampSpeed()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > speed && isGrounded)
        {
            Vector3 clampVel = flatVel.normalized * speed;
            rb.velocity = new Vector3(clampVel.x, rb.velocity.y, clampVel.z);
        }
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

    //-GROUNED-CHECK----
    void CheckGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, height * 0.5f + 0.2f, ground);
        if (isGrounded) rb.drag = drag;
        else rb.drag = 0.5f;
    }
}