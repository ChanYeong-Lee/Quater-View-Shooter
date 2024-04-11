using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : UIBase
{
    public Button resumeButton;
    public Button settingButton;
    public Button exitButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(() => GameSceneUIManager.Instance.CloseUI());
        settingButton.onClick.AddListener(() => GameSceneUIManager.Instance.OpenUI(UIType.Setting));
        exitButton.onClick.AddListener(Application.Quit);
    }
}
