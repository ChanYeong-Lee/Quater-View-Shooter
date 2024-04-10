using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public PlayerModel model;

    public Vector3 direction;

    private void Update()
    {
        model.move.Move(model.transform.position + direction);
    }
}
