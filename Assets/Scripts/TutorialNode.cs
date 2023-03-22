using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialNode : MonoBehaviour
{
    [SerializeField] string tutorialText;

    void OnTriggerEnter(Collider collider)
    {
        Player p = collider.GetComponent<Player>();
        if (p != null)
        {
            PlayerUI.TutorialPopup(tutorialText);
        }
    }
}
