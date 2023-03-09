using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager
{
    public void FixedUpdate(Transform orientation, Rigidbody rb, float baseSpeed, float speedMultiplier, float strafeSpeed, Vector3 upVelocity)
    {
        // TODO: Fix diagonal movement going faster
        // May add back Horizontal movement. Horizontal movement is set up but is set to zero at the moment.
        Vector2 axis = new Vector2(speedMultiplier, 0f);
        Vector3 direction = (orientation.forward * axis.x * baseSpeed + orientation.right * axis.y * strafeSpeed + upVelocity);
        rb.velocity = direction;
    }
}
