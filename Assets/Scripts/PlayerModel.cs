using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerModel : MonoBehaviour
{
    public PlayerMove move { get; private set; }
    public NavMeshAgent agent { get; private set; }
    public Rigidbody rigid { get; private set; }

    private void Awake()
    {
        move = GetComponent<PlayerMove>();
        agent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
    }
}
