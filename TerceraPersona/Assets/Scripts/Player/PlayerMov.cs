using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float groundDrag;
    
    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    private bool readyToJump = true;

    [Header("Keybinds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode drawWeaponKey = KeyCode.E;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;
    private bool grounded;

    [Header("Animation")]
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject standarBow;
    [SerializeField] private GameObject usableBow;
    [SerializeField] private BowBehaviour bowCD;
    private bool combat = false;

    [SerializeField] private Transform orientation;

    private float horizontalInput;
    private float VerticalInput;

    private ThirdPersonCamera cameraManager;

    private Vector3 moveDirection;
    private Rigidbody rb;

    private MovementState state;

    public bool Combat { get => combat; set => combat = value; }

    private enum MovementState
    {
        exploring,
        combat,
        air
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        cameraManager = FindObjectOfType<ThirdPersonCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        //Ground Check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
        Inputs();
        SpeedControl();
        StateHandler();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void StateHandler()
    {
        // Mode Sprinting
        if(grounded && !combat)
        {
            state = MovementState.exploring;
            anim.SetBool("Combat", false);
            moveSpeed = sprintSpeed;
        }

        //Mode Combat
        else if (combat)
        {
            state = MovementState.combat;
            anim.SetBool("Combat", true);
        }

        // Mode Air
        else
        {
            state = MovementState.air;
        }
    }

    private void Inputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");

        anim.SetFloat("SpeedX", horizontalInput, 0.1f, Time.deltaTime);
        anim.SetFloat("SpeedY", VerticalInput, 0.1f, Time.deltaTime);
        //when to jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKeyDown(drawWeaponKey) && !combat){
            combat = true;
            equipBow();
            cameraManager.SwitchCamera(ThirdPersonCamera.CameraStyle.Combat);
        }
        else if(Input.GetKeyDown(drawWeaponKey) && combat){ 
            combat = false;
            disarmBow();
            cameraManager.SwitchCamera(ThirdPersonCamera.CameraStyle.Basic);
        }

        if(Input.GetKey(KeyCode.Mouse0) && combat && bowCD.CanShoot)
        {
            anim.SetTrigger("Shot");
        }
    }

    private void MovePlayer()
    {
        //calculate movement direction
        moveDirection = orientation.forward * VerticalInput + orientation.right * horizontalInput;
        
        //on ground
        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        //on air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedSpeed = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedSpeed.x, rb.velocity.y, limitedSpeed.z);
        }
    }

    private void Jump()
    {
        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void equipBow()
    {
        standarBow.SetActive(false);
        usableBow.SetActive(true);
    }

    private void disarmBow()
    {
        usableBow.SetActive(false);
        standarBow.SetActive(true);
    }
}