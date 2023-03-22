using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager
{
    private Rigidbody rb;
    private Transform transform; 
    private Transform playerFloor;
    private float groundBuffer; 
    private LayerMask groundMask;

    public void Start(Rigidbody _rb, Transform _transform, Transform _playerFloor, float _groundBuffer, LayerMask _groundMask)
    {
        rb = _rb;
        transform = _transform;
        playerFloor = _playerFloor;
        groundBuffer = _groundBuffer;
        groundMask = _groundMask;
    }

    public void Update(ref RaycastHit previousHit)
    {
        // https://gamedev.stackexchange.com/questions/151659/rotating-according-to-ground-normal-on-unity-3d
		RaycastHit hit;
		if (Physics.Raycast(transform.position, -transform.up, out hit, groundBuffer, groundMask)) {
            Quaternion slopeRotation = Quaternion.FromToRotation(transform.up, hit.normal);
            //transform.rotation = Quaternion.Slerp(transform.rotation, slopeRotation * transform.rotation, 10 * Time.deltaTime);
            rb.MoveRotation(slopeRotation * transform.rotation);
            Physics.gravity = -transform.up * Physics.gravity.magnitude;
		}

        else
        {
            //Vector3 previousForward = transform.forward;
            //Vector3[] isolatedVelocities = IsolateVelocity.RelativeVelocity(rb.velocity, transform.rotation);
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, transform.eulerAngles.y, 0f), 100f * Time.deltaTime));
            //rb.velocity = Quaternion.FromToRotation(previousForward, transform.forward) * (isolatedVelocities[0] + isolatedVelocities[2]) + isolatedVelocities[1];
            Physics.gravity = Vector3.down * Physics.gravity.magnitude;
        }
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
