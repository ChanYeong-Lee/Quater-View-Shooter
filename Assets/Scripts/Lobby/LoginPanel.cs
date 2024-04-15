using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : UIBase
{
    public TMP_InputField nickNameInput;
    public Button loginButton;

    private void Awake()
    {
        loginButton.onClick.AddListener(OnLoginButtonClick);
    }

    private void OnEnable()
    {
        SetUIInteractable(true);
    }

    public void SetUIInteractable(bool interactable)
    {
        loginButton.interactable = interactable;
    }

    private void OnLoginButtonClick()
    {
        SetUIInteractable(false);

        PhotonNetwork.NickName = nickNameInput.text;

        PhotonNetwork.ConnectUsingSettings();
    }
}
