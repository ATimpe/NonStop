using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Slider speedSlider;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] Image deathOverlay;
    [SerializeField] Animator deathOverlayAnimator;
    [SerializeField] Animator tutorialPopupAnimator;
    [SerializeField] TextMeshProUGUI tutorialPopupText;

    void Start()
    {
        speedSlider.minValue = 1f;
        speedSlider.maxValue = player.GetSpeedMultiBoostCeiling();
    }

    void Update()
    {
        speedSlider.value = player.GetSpeedMultiplier();
        speedText.text = player.GetSpeedMultiplier().ToString();
    }

    public void Death()
    {
        deathOverlayAnimator.Play("Death");
    }
    
    public static void TutorialPopup(string tutorialText)
    {
        PlayerUI playerUI = FindObjectsOfType<PlayerUI>()[0];
        playerUI.tutorialPopupAnimator.Play("Popup");
        playerUI.tutorialPopupText.text = tutorialText;
    }
}
