using UnityEngine;

public class MoveCameraTest : MonoBehaviour {

    public Transform player;

    void Update() {
        transform.position = player.transform.position;
    }
}