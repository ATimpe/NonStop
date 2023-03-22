using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finishline : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        Player player = collider.GetComponent<Player>();
        Debug.Log("Finish!");

        if (player != null)
        {
            Debug.Log("Finish!");
            GameManager.Finish();
        }
    }
}
