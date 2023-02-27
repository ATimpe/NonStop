using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Transform OpenTransform;
    [SerializeField] Transform CloseTransform;
    [SerializeField] float MoveSpeed;
    [SerializeField] bool StartClosed;

    private Vector3 TargetPosition;

    void Start()
    {
        if (StartClosed)
        {
            transform.position = CloseTransform.position;
        }
        else
        {
            transform.position = OpenTransform.position;
        }

        TargetPosition = transform.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, TargetPosition, MoveSpeed * Time.deltaTime);
    }

    public void Open()
    {
        TargetPosition = OpenTransform.position;
    }

    public void Close()
    {
        TargetPosition = CloseTransform.position;
    }
}
