using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingTexture : MonoBehaviour
{
    [SerializeField] float scrollSpeed;
    private MeshRenderer mr;

    void Start()
    {
        mr = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        mr.material.mainTextureOffset = new Vector2(Time.realtimeSinceStartup * scrollSpeed, Time.realtimeSinceStartup * scrollSpeed);
    }
}
