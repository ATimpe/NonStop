using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] public Transform spawn;

    void OnTriggerEnter(Collider collider)
    {
        Player player = collider.GetComponent<Player>();

        if (player != null)
        {
            GameManager.Checkpoint(this);
            // TODO: Give checkpoint its own text thing
            PlayerUI.TutorialPopup("CHECKPOINT");
            gameObject.GetComponent<Collider>().enabled = false;
        }
    }
}
