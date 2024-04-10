using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInput : MonoBehaviour
{
    public bool space;
    public Vector2 arrow;
    public float scroll;
    private void Update()
    {
        InputSpace();
        InputArrow();
        InputScroll();
    }

    private void InputSpace()
    {
        space = Input.GetKey(KeyCode.Space);
    }

    private void InputArrow()
    {
        bool left = Input.GetKey(KeyCode.LeftArrow);
        bool right = Input.GetKey(KeyCode.RightArrow);
        bool up = Input.GetKey(KeyCode.UpArrow);
        bool down = Input.GetKey(KeyCode.DownArrow);

        if (left && right)
        {
            arrow.x = 0.0f;
        }
        else if (left)
        {
            arrow.x = -1.0f;
        }
        else if (right)
        {
            arrow.x = 1.0f;
        }
        else
        {
            arrow.x = 0.0f;
        }
    }

    private void InputScroll()
    {
        scroll = Input.mouseScrollDelta.y;
    }
}
