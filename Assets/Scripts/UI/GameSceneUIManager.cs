using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIType
{
    Menu,
    Setting,
    Respawn,
    Victory,
    GameOver,
}

public class GameSceneUIManager : MonoBehaviour
{
    public static GameSceneUIManager Instance { get; private set; }

    public SettingUI setting;
    public MenuUI menu;

    private Dictionary<UIType, UIBase> uiDictionary;
    private UIBase currentUI;

    private void Awake()
    {
        Instance = this;
        
        uiDictionary = new Dictionary<UIType, UIBase>()
        {
            { UIType.Menu, menu },
            { UIType.Setting, setting },
        };
    }

    public void OpenUI(UIType type)
    {
        if (currentUI == null)
        {
            CursorManager.Instance.ChangeCursor(CursorState.None);
            GameManager.Instance.playerController.enabled = false;
        }

        if (currentUI != null)
        {
            currentUI.Close();
            print($"Close {currentUI.name} ");
        }

        currentUI = uiDictionary[type];
        currentUI.Open();

        print($"Open {currentUI.name} ");
    }

    public void CloseUI()
    {
        CursorManager.Instance.ChangeCursor(CursorState.Normal);
        GameManager.Instance.playerController.enabled = true;
        
        if (currentUI != null)
        {
            currentUI.Close();
            print($"CloseUI, Close {currentUI.name}");
        }

        currentUI = null;
    }
}
