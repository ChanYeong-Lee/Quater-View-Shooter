using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    public void Zoom(float zoomAmount)
    {
        cam.orthographicSize += zoomAmount;
    }
    public void LookAt(Vector3 pos)
    {
        float offsetX = cam.transform.position.y * 0.905f;
        float offsetY = cam.transform.position.y;
        float offsetZ = -cam.transform.position.y * 0.905f;
        Vector3 offset = new Vector3(offsetX, offsetY, offsetZ);

        transform.position = pos + offset;
    }

    public void MoveRight(float amount)
    {
        transform.Translate(Vector3.right * amount, Space.Self);
    }

    public void MoveUp(float amount)
    {
        Vector3 upward = cam.transform.forward;
        upward.y = 0.0f;
        upward.Normalize();
        upward /= Mathf.Sin(45.0f * Mathf.Deg2Rad);

        transform.Translate(upward * amount, Space.World);
    }
}
