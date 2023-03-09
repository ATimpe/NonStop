using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager
{
    public void Update(bool isDashing, ref float speedMultiplier, float speedGain, float speedDecay, float speedBoostIncrement)
    {
        if (isDashing)
        {
            speedMultiplier += speedBoostIncrement;
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
            speedMultiplier -= speedDecay * Time.deltaTime;
            if (speedMultiplier < 1f)
            {
                speedMultiplier = 1f;
            }
        }
    }
}
