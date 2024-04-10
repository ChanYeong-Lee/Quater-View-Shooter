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

        if (Physics.Raycast(ray, out characterHit, 100.0f, characterLayer))
        {
            CursorManager.Instance.ChangeCursor(CursorState.Attack);
        }
        else
        {
            CursorManager.Instance.ChangeCursor(CursorState.None);
        }


        if (input.rightClick)
        {
            if (currentCommand != Command.None)
            {
                currentCommand = Command.None;
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
                currentCommand = Command.None;
            }
            else
            {

            }
        }
    }
}
