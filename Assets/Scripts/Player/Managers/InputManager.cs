using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    // Allocates all inputs to this manager so that active player input can be easily switched on and off like during pausing
    public void Update(string jumpBtn, out bool isJumping, string fireBtn, out bool isFiring, string dashBtn, out bool isDashing, string crouchBtn, out bool isCrouching)
    {
        isJumping = Input.GetButtonDown(jumpBtn);
        isFiring = Input.GetButtonDown(fireBtn);
        isDashing = Input.GetButtonDown(dashBtn);
        isCrouching = Input.GetButton(crouchBtn);
    }
}
