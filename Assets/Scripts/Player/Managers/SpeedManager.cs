using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager
{
    private float boostCooldownTimer = 0;
    private float speedGain;
    private float speedDecay;
    private float midairSpeedDecay;
    private float speedBoostIncrement;
    private float speedMultiBoostCeiling;
    private float boostCooldown;

    public void Start(float _speedGain, float _speedDecay, float _midairSpeedDecay, float _speedBoostIncrement, float _speedMultiBoostCeiling, float _boostCooldown)
    {
        speedGain = _speedGain;
        speedDecay = _speedDecay;
        midairSpeedDecay = _midairSpeedDecay;
        speedBoostIncrement = _speedBoostIncrement;
        speedMultiBoostCeiling = _speedMultiBoostCeiling;
        boostCooldown = _boostCooldown;
    }

    public void Update(bool isDashing, ref float speedMultiplier, bool isGrounded, out bool isDashingMidair)
    {
        // Resets midair dash fron frame
        isDashingMidair = false;

        if (isDashing && speedMultiplier <= speedMultiBoostCeiling && boostCooldownTimer <= 0)
        {
            speedMultiplier += speedBoostIncrement;
            boostCooldownTimer = boostCooldown;
            if (!isGrounded)
            {
                isDashingMidair = true;
            }
        }

        if (speedMultiplier < 1f)
        {
            speedMultiplier += speedGain * Time.deltaTime;
            if (speedMultiplier > 1f)
            {
                speedMultiplier = 1f;
            }
        }
        else if (speedMultiplier > 1f)
        {
            // Depending on if you're grounded or not, the speed decay is different
            speedMultiplier -= isGrounded ? speedDecay * Time.deltaTime : midairSpeedDecay * Time.deltaTime;
            if (speedMultiplier < 1f)
            {
                speedMultiplier = 1f;
            }
        }

        if (boostCooldownTimer > 0)
        {
            boostCooldownTimer -= Time.deltaTime;
        }
    }
}
