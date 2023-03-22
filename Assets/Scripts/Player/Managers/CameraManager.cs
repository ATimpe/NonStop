using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager
{
    private float desiredX;
    private float xRotation;
    private Camera playerCam;
    private Transform cameraRoot;
    private Transform orientation;
    private Transform crouchPoint;
    private float sensitivity;
    private float crouchCameraSpeed;
    private float baseFOV;

    public void Start(Camera _playerCam, Transform _cameraRoot, Transform _orientation, Transform _crouchPoint, float _sensitivity, float _crouchCameraSpeed, float _baseFOV)
    {
        playerCam = _playerCam;
        cameraRoot = _cameraRoot;
        orientation = _orientation;
        crouchPoint = _crouchPoint;
        sensitivity = _sensitivity;
        crouchCameraSpeed = _crouchCameraSpeed;
        baseFOV = _baseFOV;
    }

    public void Update(bool isCrouching, float FOVMultiplier, float speedMultiplier)
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime;

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

        playerCam.fieldOfView = baseFOV + (speedMultiplier - 1f) * FOVMultiplier * baseFOV;
    }
}
