// https://www.youtube.com/watch?v=C70QxpI9F5Y
// https://github.com/DaniDevy/FPS_Movement_Rigidbody

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementTest : MonoBehaviour
{
    private float yaw = 0.0f, pitch = 0.0f;
    private Rigidbody rb;

    [SerializeField] Transform orientation;
    [SerializeField] Camera playerCam;

    [SerializeField] float walkSpeed = 30.0f, sensitivity = 15.0f, jumpForce = 30.0f;
    [SerializeField] float GroundBuffer = 3f;

    public float sensMultiplier = 1f;
    private float xRotation;

    RotationManager RM = new RotationManager();

    // Firing
    private FireManager _FireManager = new FireManager();
    public string fireBtn;
    public LayerMask fireMask;

    void Start()

    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Physics.Raycast(rb.transform.position, -transform.up, 1.2f))
        {
            rb.velocity += orientation.rotation * Vector3.up * jumpForce;
        }
        Look();
        _FireManager.Update(fireBtn, playerCam.transform.position, playerCam.transform.forward, fireMask);
    }

    private void FixedUpdate()
    {
        RM.Update(rb, transform, GroundBuffer, orientation);
        Movement();
    }

    private float desiredX;
    private void Look() {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;
        
        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }

    void Movement()
    {
        // TODO: Fix diagonal movement going faster
        Vector2 axis = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * walkSpeed;
        //Vector3 forward = new Vector3(-Camera.main.transform.right.z, 0.0f, Camera.main.transform.right.x);
        Vector3 wishDirection = (orientation.forward * axis.x + orientation.right * axis.y + IsolateUpVelocity(rb.velocity, orientation.eulerAngles));
        rb.velocity = wishDirection;
    }

    // Used for jumping while rotated. It gets the upwards velocity relative to the current rotation of the player.
    private Vector3 IsolateUpVelocity(Vector3 velocity, Vector3 eulerAngles)
    {
        // Rotates the current velocity vector to be what the velocity would be if the player was facing (0, 0, 1) direction
        Vector3 VelocityAtForward = Quaternion.Inverse(orientation.rotation) * velocity;
        float UpVelocity = VelocityAtForward.y;
        return orientation.rotation * new Vector3(0f, UpVelocity, 0f);
    }
}