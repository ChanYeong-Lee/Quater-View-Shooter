using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CameraInput input;

    public Transform mainTarget;
    public CameraMove cam;

    public float padding;
    public float sensitivity;

    private void Update()
    {
        if (input.space)
        {
            cam.LookAt(mainTarget.transform.position);
        }

        Move();
    }

    private void Move()
    {
        float minX = padding;
        float maxX = Screen.width - padding;

        float minY = padding;
        float maxY = Screen.height - padding;

        float moveAmount = sensitivity * Time.deltaTime;

        if (Input.mousePosition.x < minX)
        {
            cam.MoveRight(-moveAmount);
        }

        if (Input.mousePosition.x > maxX)
        {
            cam.MoveRight(moveAmount);
        }

        if (Input.mousePosition.y < minY)
        {
            cam.MoveUp(-moveAmount);
        }

        if (Input.mousePosition.y > maxY)
        {
            cam.MoveUp(moveAmount);
        }
    }
}
