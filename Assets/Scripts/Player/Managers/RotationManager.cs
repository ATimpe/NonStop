using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager
{
    public void Update(Rigidbody rb, Transform transform, float GroundBuffer, Transform Orientation)
    {
        // https://gamedev.stackexchange.com/questions/151659/rotating-according-to-ground-normal-on-unity-3d
		RaycastHit hit;
		if (Physics.Raycast(transform.position, -transform.up, out hit, GroundBuffer)) {
            Vector3 StartingForward = Orientation.forward;

			Quaternion slopeRotation = Quaternion.FromToRotation(transform.up, hit.normal);
            transform.rotation = Quaternion.Slerp(transform.rotation, slopeRotation * transform.rotation, 10 * Time.deltaTime);

            Vector3 EndingForward = Orientation.forward;
            Vector3 DirectionChange = EndingForward - StartingForward;
            //Debug.Log("dir change: " + DirectionChange);

            // Rotates the velocity with player to avoid building up too much force in different directions causing the player to fall off the edge
            rb.velocity += DirectionChange * rb.velocity.magnitude;
            //CapsuleCollider PlayerCollider = GetComponent<CapsuleCollider>();
            //transform.position = hit.point - (-transform.up * PlayerCollider.bounds.extents.y - PlayerCollider.center);
		}

        Physics.gravity = -transform.up * Physics.gravity.magnitude;
        //Debug.Log(Physics.gravity);
    }
}
