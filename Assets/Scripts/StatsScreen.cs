using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StatsScreen : MonoBehaviour
{
    [SerializeField] string clearTimeLabel;
    [SerializeField] TextMeshProUGUI clearTimeTXT;
    [SerializeField] string deathsLabel;
    [SerializeField] TextMeshProUGUI deathsTXT;
    private float clearTime;
    private int deaths;

    void Start()
    {
        StatsHolder sh = FindObjectsOfType<StatsHolder>()[0];
        clearTime = sh.GetClearTime();
        deaths = sh.GetDeaths();
    }

    void Update()
    {
        clearTimeTXT.text = clearTimeLabel + clearTime;
        deathsTXT.text = deathsLabel + deaths;
    }

    public void Restart()
    {
        SceneManager.LoadScene("TestLevel1");
    }

}
