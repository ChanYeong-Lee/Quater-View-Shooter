using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform mainTarget;
    public Camera cam;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            float offsetX = cam.transform.position.y * 0.905f;
            float offsetY = cam.transform.position.y;
            float offsetZ = -cam.transform.position.y * 0.905f;
            Vector3 offset = new Vector3(offsetX, offsetY, offsetZ);
            cam.transform.position = mainTarget.transform.position + offset; 
        }
    }

    public void Zoom(float zoomAmount)
    {
        cam.orthographicSize += zoomAmount;
    }

    public void MoveRight(float amount)
    {
        cam.transform.Translate(Vector3.right * amount, Space.Self);
    }

    public void MoveUp(float amount)
    {
        Vector3 upward = cam.transform.forward;
        upward.y = 0.0f;
        upward.Normalize();
        upward /= Mathf.Sin(45.0f * Mathf.Deg2Rad);

        cam.transform.Translate(upward * amount, Space.World);
    }
}
