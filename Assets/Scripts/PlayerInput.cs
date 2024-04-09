using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool leftClick;
    public bool rightClick;

    private void Update()
    {
        InputLeftClick();
        InputRightClick();
    }

    private void InputLeftClick()
    {
        leftClick = Input.GetMouseButton(0);
    }

    private void InputRightClick()
    {
        rightClick = Input.GetMouseButton(1);
    }
}
