using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    private string jumpBtn;
    private string fireBtn;
    private string dashBtn;
    private string crouchBtn;

    public void Start(string _jumpBtn, string _fireBtn, string _dashBtn, string _crouchBtn)
    {
        jumpBtn = _jumpBtn;
        fireBtn = _fireBtn;
        dashBtn = _dashBtn;
        crouchBtn = _crouchBtn;
    }

    // Allocates all inputs to this manager so that active player input can be easily switched on and off like during pausing
    public void Update(out bool isJumping, out bool isFiring, out bool isDashing, out bool isCrouching)
    {
        isJumping = Input.GetButtonDown(jumpBtn);
        isFiring = Input.GetButtonDown(fireBtn);
        isDashing = Input.GetButtonDown(dashBtn);
        isCrouching = Input.GetButton(crouchBtn);
    }
}
