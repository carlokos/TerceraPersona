using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    private bool canMove = true;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float groundDrag;
    
    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    private bool readyToJump = true;

    [Header("Dodge")]
    [SerializeField] private float DodgeCD;
    [SerializeField] private float DodgeForce = 1.5f;
    [SerializeField] private float DodgeDuration = 1.59f;
    private float DodgeICD;
    private bool isDodging = false;
    private bool canDodge = true; 

    [Header("Keybinds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode drawWeaponKey = KeyCode.E;
    [SerializeField] private KeyCode dodgeKey = KeyCode.Mouse1;

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
    public bool IsDodging { get => isDodging; set => isDodging = value; }
    public bool CanMove { get => canMove; set => canMove = value; }
    public bool CanDodge { get => canDodge; set => canDodge = value; }

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
        DodgeICD = DodgeCD;
    }

    // Update is called once per frame
    void Update()
    {
        //Ground Check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        DodgeICD -= Time.deltaTime;

        if (grounded)
        {
            rb.drag = groundDrag;
            canDodge = true;
        }
        else
        {
            rb.drag = 0;
            canDodge = false;
        }
        if(canMove) Inputs();
        SpeedControl();
        StateHandler();
    }

    private void FixedUpdate()
    { 
        if(canMove) MovePlayer();

        if (isDodging && (horizontalInput != 0 || VerticalInput != 0))
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * DodgeForce, ForceMode.VelocityChange);
        }
    }

    private void StateHandler()
    {
        // Mode Sprinting
        if(grounded && !combat)
        {
            state = MovementState.exploring;
            anim.SetBool("Jump", false);
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
            anim.SetBool("Jump", true);
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

        //Use or not use the bow
        if (Input.GetKeyDown(drawWeaponKey) && !combat){
            combat = true;
            equipBow();
            cameraManager.SwitchCamera(ThirdPersonCamera.CameraStyle.Combat);
        }

        else if(Input.GetKeyDown(drawWeaponKey) && combat){ 
            combat = false;
            anim.SetBool("Combat", false);
            disarmBow();
            cameraManager.SwitchCamera(ThirdPersonCamera.CameraStyle.Basic);
        }

        //Triggers the shot animation
        if(Input.GetKey(KeyCode.Mouse0) && combat && bowCD.CanShoot)
        {
            anim.SetTrigger("Shot");
        }

        if (Input.GetKeyDown(dodgeKey) && canDodge && DodgeICD <= 0 && (horizontalInput != 0 || VerticalInput != 0))
        {
            DodgeICD = DodgeCD;
            StartCoroutine(roll());
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

    //Limits the speed of the player to feel it more smoothly
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
        //reset Y-velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void finishRoll()
    {
        canDodge = true;
    }

    private IEnumerator roll()
    {
        canMove = false;
        isDodging = true;
        anim.SetTrigger("Roll");
        yield return new WaitForSeconds(DodgeDuration);
        isDodging = false;
        canMove = true;
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
