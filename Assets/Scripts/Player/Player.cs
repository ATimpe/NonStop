// https://www.youtube.com/watch?v=C70QxpI9F5Y
// https://github.com/DaniDevy/FPS_Movement_Rigidbody

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private InputManager inputManager = new InputManager();
    private RotationManager rotationManager = new RotationManager();
    private MovementManager movementManager = new MovementManager();
    private FireManager fireManager = new FireManager();
    private CameraManager cameraManager = new CameraManager();
    private JumpManager jumpManager = new JumpManager();
    private SpeedManager speedManager = new SpeedManager();

    private Rigidbody rb;

    // Core
    [SerializeField] Transform orientation;

    [Header("Movement")]
    [SerializeField] float baseSpeed = 50.0f;            // Player speed when the player is at normal base speed
    [SerializeField] float speedGain = 1f;               // The amount per second that speed is gained when you are bellow the base speed
    [SerializeField] float speedDecay = 0.5f;            // The amount per second that speed reduced when you are above the base speed
    [SerializeField] float speedBoostIncrement = 1f;     // The amount speedMultiplier is incremented when dashing
    [SerializeField] float strafeSpeed = 20.0f;
    // TODO: maybe add a seperate multiplier for strafe speed
    float speedMultiplier = 0f;                          // The multiplier used for player speed

    [Header("Input")]
    [SerializeField] string jumpBtn;
    [SerializeField] string fireBtn;
    [SerializeField] string dashBtn;
    [SerializeField] string crouchBtn;
    private bool isJumping = false;
    private bool isFiring = false;
    private bool isDashing = false;
    private bool isCrouching = false;
    
    [Header("Jumping")]
    [SerializeField] float jumpForce = 30.0f;
    [SerializeField] float crouchJumpAngle;
    private bool isGrounded = false;

    [Header("Crouching")]
    [SerializeField] float crouchCameraSpeed;
    [SerializeField] Transform crouchPoint;

    [Header("Rotation Control")]
    [SerializeField] Transform playerFloor;
    [SerializeField] float groundBuffer = 3f;
    [SerializeField] LayerMask groundMask;
    private RaycastHit previousHit;     

    [Header("Camera and Mouse Movement")]
    [SerializeField] Camera playerCam;
    [SerializeField] Transform cameraRoot;
    [SerializeField] float sensitivity = 15.0f;
    [SerializeField] float sensMultiplier = 1f;

    [Header("Firing")]
    [SerializeField] float fireCooldown;                 // Time in seconds between shots
    [SerializeField] LayerMask fireMask;                 // Layers the bullet should interact with

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = gameObject.GetComponent<Rigidbody>();
        Time.timeScale = 1f;
    }

    void Update()
    {
        inputManager.Update(jumpBtn, out isJumping, fireBtn, out isFiring, dashBtn, out isDashing, crouchBtn, out isCrouching);
        speedManager.Update(isDashing, ref speedMultiplier, speedGain, speedDecay, speedBoostIncrement);
        //Debug.Log(speedMultiplier);
        isGrounded = GroundCheck();
        jumpManager.Update(isJumping, isGrounded, rb, orientation, jumpForce, isCrouching, crouchJumpAngle);
        cameraManager.Update(playerCam, cameraRoot, orientation, sensitivity, sensMultiplier, isCrouching, crouchCameraSpeed, crouchPoint);
        fireManager.Update(isFiring, playerCam.transform.position, playerCam.transform.forward, fireMask);
        //rotationManager.Update(transform);
        rotationManager.Update(rb, transform, playerFloor, groundBuffer, orientation, ref previousHit, groundMask);
    }

    private void FixedUpdate()
    {
        movementManager.FixedUpdate(orientation, rb, baseSpeed, speedMultiplier, strafeSpeed, IsolateUpVelocity(rb.velocity, orientation.rotation));
    }

    // Used for jumping while rotated. It gets the upwards velocity relative to the current rotation of the player.
    private Vector3 IsolateUpVelocity(Vector3 velocity, Quaternion rotation)
    {
        // Rotates the current velocity vector to be what the velocity would be if the player was facing (0, 0, 1) direction
        Vector3 VelocityAtForward = Quaternion.Inverse(rotation) * velocity;
        float UpVelocity = VelocityAtForward.y;
        return rotation * new Vector3(0f, UpVelocity, 0f);
    }

    private bool GroundCheck()
    {
        return Physics.Raycast(rb.transform.position, -orientation.up, 1f + 0.2f);
    }
}