using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpManager
{
    private Rigidbody rb;
    private Transform orientation;
    private float jumpForce;
    private float crouchJumpAngle;
    private float baseSpeed;

    public void Start(Rigidbody _rb, Transform _orientation, float _jumpForce, float _crouchJumpAngle, float _baseSpeed)
    {
        rb = _rb;
        orientation = _orientation;
        jumpForce = _jumpForce;
        crouchJumpAngle = _crouchJumpAngle;
        baseSpeed = _baseSpeed;
    }

    // Jumping gets its own manager since key presses don't work in FixedUpdate()
    public void Update(bool isJumping, bool isGroudned, bool isCrouching, ref float speedMultiplier)
    {
        if (isJumping && isGroudned && !isCrouching)
        {
            Debug.Log("Jump");
            rb.velocity += orientation.rotation * Vector3.up * jumpForce;
        }

        else if (isJumping && isGroudned && isCrouching)
        {
            // Since the game calculates the speed based on speed multiplier it will increase the speed multiplier based on the angle and jump force
            Vector3 jumpVector = Quaternion.AngleAxis(-crouchJumpAngle, orientation.right) * orientation.forward * jumpForce;
            // The jump vector if the player had no rotation
            Vector3 jumpVectorRelative = Quaternion.Inverse(orientation.rotation) * jumpVector;
            speedMultiplier += jumpVectorRelative.z / baseSpeed;
            rb.velocity += jumpVector;
        }
    }
}
