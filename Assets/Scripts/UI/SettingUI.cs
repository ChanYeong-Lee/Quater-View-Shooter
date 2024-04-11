using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : UIBase
{
    [Header("기본")]
    public Button closeButton;

    [Header("마우스 민감도")]
    public Slider sensitivitySlider;
    public TMP_InputField sensitivityInput;

    private void Awake()
    {
        closeButton.onClick.AddListener(() => GameSceneUIManager.Instance.CloseUI());

        sensitivitySlider.onValueChanged.AddListener((value) => sensitivityInput.SetTextWithoutNotify(value.ToString("F2")));
        sensitivityInput.onValueChanged.AddListener((value) => sensitivitySlider.SetValueWithoutNotify(int.Parse(value)));
    }

}