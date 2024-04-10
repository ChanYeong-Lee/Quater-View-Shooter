using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool leftClick;
    public bool rightClick;

    public bool stop;
    public bool attack;
    public bool dash;
    public bool move;
    public bool escape;

    private void Update()
    {
        InputLeftClick();
        InputRightClick();

        InputStop();
        InputAttack();
        InputDash();
        InputMove();

        InputEscape();
    }

    private void InputLeftClick()
    {
        leftClick = Input.GetMouseButton(0);
    }

    private void InputRightClick()
    {
        rightClick = Input.GetMouseButton(1);
    }

    private void InputStop()
    {
        stop = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.H);
    }

    private void InputAttack()
    {
        attack = Input.GetKeyDown(KeyCode.A);
    }

    private void InputDash()
    {
        dash = Input.GetKeyDown(KeyCode.LeftShift);
    }
    private void InputMove()
    {
        move = Input.GetKeyDown(KeyCode.M);
    }

    private void InputEscape()
    {
        escape = Input.GetKeyDown(KeyCode.Escape);
    }

}
