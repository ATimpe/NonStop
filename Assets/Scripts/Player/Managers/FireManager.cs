using System.Collections;
using UnityEngine;

public class FireManager
{
    public void Update(bool isFiring, Vector3 firePosition, Vector3 fireDirection, LayerMask lm)
    {
        if (isFiring)
        {
            // TODO: Trigger hud animation
            RaycastHit hit;
            // Maybe change the max distance to a defined value to avoid targets far away from being triggered
            if (Physics.Raycast(firePosition, fireDirection, out hit, Mathf.Infinity, lm))
            {
                //Debug.Log(hit.collider.gameObject);

                BulletTrigger bt = hit.collider.gameObject.GetComponent<BulletTrigger>();

                // If the object hit has the bullet trigger script, it will trigger whatever event it has attached
                if (bt != null)
                {
                    bt.Trigger();
                }
            }
        }
    }
}
