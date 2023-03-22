using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsolateVelocity : MonoBehaviour
{
    // Returns a list of 3 vectors with the velocity along the current object's rotation along the x, y and z axis
    // Used mostly for isolating velocities so they can be rotated more easily
    // 0 - x
    // 1 - y
    // 2 - z
    public static Vector3[] RelativeVelocity(Vector3 velocity, Quaternion rotation)
    {
        Vector3[] vectorList = new Vector3[3];
        // Rotates the current velocity vector to be what the velocity would be if the player was facing (0, 0, 1) direction
        Vector3 VelocityAtForward = Quaternion.Inverse(rotation) * velocity;
        float XVelocity = VelocityAtForward.x;
        float YVelocity = VelocityAtForward.y;
        float ZVelocity = VelocityAtForward.z;
        vectorList[0] = rotation * new Vector3(XVelocity, 0f, 0f);
        vectorList[1] = rotation * new Vector3(0f, YVelocity, 0f);
        vectorList[2] = rotation * new Vector3(0f, 0f, ZVelocity);
        return vectorList;
    }
}
