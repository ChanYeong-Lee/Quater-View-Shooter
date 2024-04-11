using Unity.VisualScripting;
using UnityEngine;

public enum Command
{
    None,
    Attack,
    Move,
    Dash,
}

public class PlayerController : MonoBehaviour
{
    public PlayerModel model;
    public Camera cam;

    private PlayerInput input;

    public LayerMask groundLayer;
    public LayerMask characterLayer;

    private Command currentCommand;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = cam.ScreenToWorldPoint(mousePosition);

        Ray ray = new Ray(worldPosition, cam.transform.forward);

        RaycastHit characterHit;
        RaycastHit groundHit;

        if (currentCommand == Command.None)
        {
            if (Physics.Raycast(ray, out characterHit, 100.0f, characterLayer))
            {
                CursorManager.Instance.ChangeCursor(CursorState.CheckEnemy);
            }
            else
            {
                CursorManager.Instance.ChangeCursor(CursorState.Normal);
            }
        }

        if (input.attack)
        {
            if (model.attack.canAttack && model.attack.inputAttack == false)
            {
                ChangeCommand(Command.Attack);
            }
        }
        if (input.move)
        {
            ChangeCommand(Command.Move);
        }
        if (input.dash)
        {
            if (model.move.canDash)
            {
                ChangeCommand(Command.Dash);
            }
        }
        if (input.stop)
        {
            model.move.Stop();
        }

        if (input.leftClick)
        {
            if (Physics.Raycast(ray, out groundHit, 100.0f, groundLayer))
            {
                Vector3 target = new Vector3(groundHit.point.x, 0.0f, groundHit.point.z);
                Vector3 direction = target - model.transform.position;
                switch (currentCommand)
                {
                    case Command.None:
                        break;
                    case Command.Attack:
                        model.attack.TryShot();
                        model.move.Rotate(direction, () => model.attack.Shot(direction), () => model.attack.CancelShot());
                        break; 
                    case Command.Move:
                        model.move.Move(groundHit.point);
                        break;
                    case Command.Dash:
                        model.move.Dash(direction.normalized);
                        break;
                }

                if (currentCommand != Command.None)
                {
                    ChangeCommand(Command.None);
                }
            }
            else
            {
                if (currentCommand != Command.None)
                {
                    currentCommand = Command.None;
                }
            }
        }

        if (input.rightClick)
        {
            if (currentCommand != Command.None)
            {
                ChangeCommand(Command.None);
            }

            if (Physics.Raycast(ray, out groundHit, 100.0f, groundLayer))
            {
                model.move.Move(groundHit.point);
            }
            else
            {
                model.move.ResetAction();
            }
        }

        if (input.escape)
        {
            if (currentCommand != Command.None)
            {
                ChangeCommand(Command.None);
            }
            else
            {
                GameSceneUIManager.Instance.OpenUI(UIType.Menu);
            }
        }
    }

    private void ChangeCommand(Command command)
    {
        print($"Change Command {command}");
        switch (command)
        {
            case Command.None:
                CursorManager.Instance.ChangeCursor(CursorState.Normal);
                break;
            case Command.Attack:
                CursorManager.Instance.ChangeCursor(CursorState.Attack);
                break;
            case Command.Move:
                CursorManager.Instance.ChangeCursor(CursorState.Move);
                break;
            case Command.Dash:
                CursorManager.Instance.ChangeCursor(CursorState.Dash);
                break;
        }

        currentCommand = command;
    }
}