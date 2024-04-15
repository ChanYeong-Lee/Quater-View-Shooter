using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PanelType
{
    Login,
    Menu,
    Lobby,
    Room,
}

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance { get; private set; }

    public LoginPanel login;
    public MenuPanel menu;
    public LobbyPanel lobby;
    public RoomPanel room;

    private Dictionary<PanelType, UIBase> panelDictionary;
    private UIBase currentPanel;

    private void Awake()
    {
        Instance = this;

        panelDictionary = new Dictionary<PanelType, UIBase>
        {
            { PanelType.Login, login },
            { PanelType.Menu, menu },
            { PanelType.Lobby, lobby },
            { PanelType.Room, room },
        };

        foreach (var panel in panelDictionary.Values)
        {
            panel.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        OpenPanel(PanelType.Login);    
    }

    public void OpenPanel(PanelType type)
    {
        Debug.Log($"{type} 패널을 엽니다.");

        if (currentPanel != null)
        {
            currentPanel.Close();
        }   

        currentPanel = panelDictionary[type];
        currentPanel.Open();
    }
}