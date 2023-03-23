using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] int startingCheckpoint = -1;           // Purely for testing purposes, should always be set to -1 for starting spawn
    [SerializeField] Transform spawn;
    [SerializeField] Checkpoint[] checkpoints;
    [SerializeField] Player player;
    [SerializeField] float YDeathFloor;
    [SerializeField] StatsHolder statsHolder;
    [SerializeField] string finishScene;
    private int currentCheckpoint = -1;
    private float currentTime = 0f;
    private int deaths = 0;

    void Start()
    {
        currentCheckpoint = startingCheckpoint;
        Respawn();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("TestLevel1");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("TitleScene");
            Cursor.lockState = CursorLockMode.None;
            Destroy(FindObjectsOfType<StatsHolder>()[0].gameObject);
        }

        if (player.transform.position.y <= YDeathFloor)
        {
            player.Death();
        }

        currentTime += Time.deltaTime;
    }

    public static void Death()
    {
        GameManager gm = FindObjectsOfType<GameManager>()[0];
        gm.deaths++;
    }

    public static void Respawn()
    {
        GameManager gm = FindObjectsOfType<GameManager>()[0];
        if (gm.currentCheckpoint == -1)
        {
            gm.player.transform.position = gm.spawn.position;
            gm.player.transform.rotation = gm.spawn.rotation;
        }
        else
        {
            gm.player.transform.position = gm.checkpoints[gm.currentCheckpoint].spawn.position;
            gm.player.transform.rotation = gm.checkpoints[gm.currentCheckpoint].spawn.rotation;
        }
    }

    // When the player reaches a checkpoint, it
    public static void Checkpoint(Checkpoint c)
    {
        GameManager gm = FindObjectsOfType<GameManager>()[0];
        for (int i = 0; i < gm.checkpoints.Length; i++)
        {
            if (c == gm.checkpoints[i])
            {
                gm.currentCheckpoint = i;
                return;
            }
        }
    }

    public static void Finish()
    {
        GameManager gm = FindObjectsOfType<GameManager>()[0];
        gm.statsHolder.SetStats(gm.currentTime, gm.deaths);
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(gm.finishScene);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawRay(new Vector3(0f, YDeathFloor, 0f), Vector3.forward * 900f);
    }
}
