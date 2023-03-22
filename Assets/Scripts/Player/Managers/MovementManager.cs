using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager
{
    private Vector3 previousFrameForward;
    private Rigidbody rb;
    private Transform orientation;
    private float baseSpeed;
    private float strafeSpeed;

    public void Start(Rigidbody _rb, Transform _orientation, float _baseSpeed, float _strafeSpeed)
    {
        rb = _rb;
        orientation = _orientation;
        baseSpeed = _baseSpeed;
        strafeSpeed = _strafeSpeed;
        previousFrameForward = orientation.rotation * Vector3.forward;
    }

    public void Update(float speedMultiplier, bool isGrounded, bool isDashingMidair)
    {
        // TODO: Fix diagonal movement going faster
        // May add back Horizontal movement. Horizontal movement is set up but is set to zero at the moment.

        Vector3[] isolatedVelocities = IsolateVelocity.RelativeVelocity(rb.velocity, orientation.rotation);

        // Grounded movement
        if (isGrounded)
        {
            Vector2 axis = new Vector2(speedMultiplier, 0f);
            rb.velocity = orientation.forward * axis.x * baseSpeed + orientation.right * axis.y * strafeSpeed + isolatedVelocities[1];
        }

        // Air movement
        else
        {
            Vector2 axis = new Vector2(speedMultiplier, 0f);
            //rb.velocity = orientation.forward * axis.x * baseSpeed + orientation.right * axis.y * strafeSpeed + new Vector3(0f, rb.velocity.y, 0f);
            
            rb.velocity = Quaternion.FromToRotation(previousFrameForward, orientation.rotation * Vector3.forward) * rb.velocity;
            // Thinking about maybe making mid air dash go in the direction of the camera
            if (isDashingMidair)
            {
                Vector3 direction = orientation.forward * axis.x * baseSpeed + orientation.right * axis.y * strafeSpeed;
                Vector3 velocityRelative = Quaternion.Inverse(orientation.rotation) * rb.velocity;
                Vector3 velocityYRelative = orientation.rotation * new Vector3(0f, velocityRelative.y, 0f);
                rb.velocity = velocityYRelative + direction;
            }
        }

        previousFrameForward = orientation.rotation * Vector3.forward;
    }
}
