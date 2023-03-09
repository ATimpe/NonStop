using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager
{
    public void Update(Rigidbody rb, Transform transform, Transform playerFloor, float GroundBuffer, Transform Orientation, ref RaycastHit previousHit, LayerMask groundMask)
    {
        // https://gamedev.stackexchange.com/questions/151659/rotating-according-to-ground-normal-on-unity-3d
		RaycastHit hit;
		if (Physics.Raycast(transform.position, -transform.up, out hit, GroundBuffer, groundMask)) {
            Quaternion slopeRotation = Quaternion.FromToRotation(transform.up, hit.normal);
            //transform.rotation = Quaternion.Slerp(transform.rotation, slopeRotation * transform.rotation, 10 * Time.deltaTime);
            rb.MoveRotation(slopeRotation * transform.rotation);
		}

        Physics.gravity = -transform.up * Physics.gravity.magnitude;
    }
}

// Obsolete Code
    /*
    public RaycastHit PredictNext(RaycastHit currentHit, RaycastHit previousHit, LayerMask groundMask)
    {
        // Based on the direction and difference in the normals, this predicts where the player will be
        // on the next frame
        Vector3 direction = currentHit.point - previousHit.point;
        Vector3 rotationDifference = currentHit.normal - previousHit.normal;
        Vector3 nextDirection = direction + (rotationDifference * direction.magnitude);
        Vector3 nextPosition = currentHit.point + nextDirection;
        Vector3 nextNormal = currentHit.normal + rotationDifference;

        // Casts a ray down onto the predicted point to get the predicted raycast of the next frame
        RaycastHit hitUp;
        Physics.Raycast(nextPosition + nextNormal * 3f, -nextNormal, out hitUp, 3f, groundMask);

        return hitUp;
    }
    */

    /*if (targetRotation != null)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        Physics.gravity = -transform.up * Physics.gravity.magnitude;
    }*/

    //Vector3 StartingForward = Orientation.forward;

    //Quaternion slopeRotation = Quaternion.FromToRotation(transform.up, hit.normal);
    //transform.rotation = Quaternion.Slerp(transform.rotation, slopeRotation * transform.rotation, 10 * Time.deltaTime);

    // If the previous frame had a hit, a prediction of where the player will be next frame and what rotation they will have will
    // be used to rotate the player so that the player doesn't clip into the ground
    //if (hitOnPrevFrame)
    //{
    //    RaycastHit nextHit = PredictNext(hit, previousHit, groundMask);
    //    Quaternion slopeRotation = Quaternion.FromToRotation(transform.up, nextHit.normal);
    //    transform.rotation = Quaternion.Slerp(transform.rotation, slopeRotation * transform.rotation, 10 * Time.deltaTime);
    //}
    //else
    //{

    //transform.position = hit.point + transform.position - playerFloor.position;


    //}

    //Vector3 EndingForward = Orientation.forward;
    //Vector3 DirectionChange = EndingForward - StartingForward;
    //Debug.Log("dir change: " + DirectionChange);

    // Rotates the velocity with player to avoid building up too much force in different directions causing the player to fall off the edge
    //rb.velocity += DirectionChange * rb.velocity.magnitude;
    //CapsuleCollider PlayerCollider = GetComponent<CapsuleCollider>();
    //transform.position = hit.point - (-transform.up * PlayerCollider.bounds.extents.y - PlayerCollider.center);

    // Records the hit from the previous FixedUpdate() which will be used to predict where the player will be on the next frame
    //previousHit = hit;
    //hitOnPrevFrame = true;
