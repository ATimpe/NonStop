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
    [SerializeField] PlayerUI playerUI;

    // TODO make a lot of these values that won't change during runtime static and set the values internally in each manager through Start() functions

    [Header("Movement")]
    [SerializeField] float baseSpeed = 50.0f;            // Player speed when the player is at normal base speed
    [SerializeField] float speedGain = 1f;               // The amount per second that speed is gained when you are bellow the base speed
    [SerializeField] float speedDecay = 0.5f;            // The amount per second that speed reduced when you are above the base speed
    [SerializeField] float midairSpeedDecay;
    [SerializeField] float speedBoostIncrement = 1f;     // The amount speedMultiplier is incremented when dashing
    [SerializeField] float strafeSpeed = 20.0f;
    [SerializeField] float speedMultiBoostCeiling;       // The speed at which you can no longer boost
    [SerializeField] float boostCooldown;                // Ine seconds the amount of time it takes to boost again
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
    private bool isDashingMidair = false;

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
    [SerializeField] float baseFOV;                      // The FOV at speed multiplier 1
    [SerializeField] float FOVMultiplier;                // FOV multiplier per per speedMultiplier increment
                                                        // ex. speedMultiplier = 1; FOV = baseFOV;
                                                        //     speedMultiplier = 2; FOV = baseFOV + (baseFOV * FOVMultiplier)

    [Header("Firing")]
    [SerializeField] float fireCooldown;                 // Time in seconds between shots
    [SerializeField] LayerMask fireMask;                 // Layers the bullet should interact with

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = gameObject.GetComponent<Rigidbody>();

        inputManager.Start(jumpBtn, fireBtn, dashBtn, crouchBtn);
        speedManager.Start(speedGain, speedDecay, midairSpeedDecay, speedBoostIncrement, speedMultiBoostCeiling, boostCooldown);
        jumpManager.Start(rb, orientation, jumpForce, crouchJumpAngle, baseSpeed);
        cameraManager.Start(playerCam, cameraRoot, orientation, crouchPoint, sensitivity, crouchCameraSpeed, baseFOV);
        fireManager.Start(fireMask);
        movementManager.Start(rb, orientation, baseSpeed, strafeSpeed);
        rotationManager.Start(rb, transform, playerFloor, groundBuffer, groundMask);
    }

    void Update()
    {
        isGrounded = GroundCheck();
        inputManager.Update(out isJumping, out isFiring, out isDashing, out isCrouching);
        speedManager.Update(isDashing, ref speedMultiplier, isGrounded, out isDashingMidair);
        jumpManager.Update(isJumping, isGrounded, isCrouching, ref speedMultiplier);
        cameraManager.Update(isCrouching, FOVMultiplier, speedMultiplier);
        fireManager.Update(isFiring, playerCam.transform.position, playerCam.transform.forward);
        movementManager.Update(speedMultiplier, isGrounded, isDashingMidair);
        rotationManager.Update(ref previousHit);
    }

    public void Death()
    {
        speedMultiplier = 0;
        rb.velocity = Vector3.zero;
        playerUI.Death();
        GameManager.Death();
        GameManager.Respawn();
        orientation.rotation = new Quaternion(0f, 0f, 0f, 0f);
        cameraManager.ResetRotation();
    }

    private bool GroundCheck()
    {
        return Physics.Raycast(rb.transform.position, -orientation.up, 1f + 0.2f, groundMask);
    }

    public float GetSpeedMultiplier()
    {
        return speedMultiplier;
    }

    public float GetSpeedMultiBoostCeiling()
    {
        return speedMultiBoostCeiling;
    }
}