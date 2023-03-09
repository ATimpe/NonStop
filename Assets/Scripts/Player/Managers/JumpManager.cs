using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpManager
{
    // Jumping gets its own manager since key presses don't work in FixedUpdate()
    public void Update(bool isJumping,
        bool isGroudned,
        Rigidbody rb, 
        Transform orientation, 
        float jumpForce,
        bool isCrouching,
        float crouchJumpAngle)
    {
        if (isJumping && isGroudned && !isCrouching)
        {
            rb.velocity += orientation.rotation * Vector3.up * jumpForce;
        }

        else if (isJumping && isGroudned && isCrouching)
        {
            rb.velocity += Quaternion.AngleAxis(-crouchJumpAngle, orientation.right) * orientation.forward * jumpForce;
        }
    }
}
