using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager
{
    private float desiredX;
    private float xRotation;

    public void Update(Camera playerCam, 
        Transform cameraRoot,
        Transform orientation, 
        float sensitivity, 
        float sensMultiplier, 
        bool isCrouching,
        float crouchCameraSpeed,
        Transform crouchPoint)
    {
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

        if (isCrouching)
        {
            playerCam.transform.position = Vector3.MoveTowards(playerCam.transform.position, crouchPoint.position, crouchCameraSpeed * Time.deltaTime);
        }
        else
        {
            playerCam.transform.position = Vector3.MoveTowards(playerCam.transform.position, cameraRoot.position, crouchCameraSpeed * Time.deltaTime);
        }
    }
}
