using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsHolder : MonoBehaviour
{
    private float clearTime;
    private int deaths;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetStats(float _clearTime, int _deaths)
    {
        clearTime = _clearTime;
        deaths = _deaths;
    }

    public float GetClearTime()
    {
        return clearTime;
    }

    public int GetDeaths()
    {
        return deaths;
    }
}
