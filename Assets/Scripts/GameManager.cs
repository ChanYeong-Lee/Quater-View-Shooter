using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PlayerController playerController;
    public PlayerModel playerModel;

    public int kill;
    public int death;

    
    private void Awake()
    {
        Instance = this;
    }
}
