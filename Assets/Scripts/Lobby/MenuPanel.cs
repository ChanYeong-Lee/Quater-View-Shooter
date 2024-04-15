using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : UIBase
{
    public RectTransform mainPanel;
    public RectTransform createRoomPanel;

    [Header("메인")]
    public TMP_Text nickNameText;
    public Button createRoomButton;
    public Button findRoomButton;
    public Button logoutButton;


    [Header("방 만들기")]
    public TMP_InputField roomNameInput;
    public Slider playerNumberSlider;
    public TMP_InputField playerNumberInput;
    public Button cancelButton;
    public Button createButton;

    private void Awake()
    {
        createRoomButton.onClick.AddListener(OnCreateRoomButtonClick);
        findRoomButton.onClick.AddListener(OnFindRoomButtonClick);
        logoutButton.onClick.AddListener(OnLogoutButtonClick);

        cancelButton.onClick.AddListener(OnCancelButtonClick);
        createButton.onClick.AddListener(OnCreateButtonClick);

        playerNumberInput.onValueChanged.AddListener(value => playerNumberSlider.SetValueWithoutNotify(Mathf.Clamp(int.Parse(value), 1, 8)));
        playerNumberSlider.onValueChanged.AddListener(value => playerNumberInput.SetTextWithoutNotify(value.ToString()));
    }

    private void OnEnable()
    {
        createRoomPanel.gameObject.SetActive(false);
        mainPanel.gameObject.SetActive(true);

        nickNameText.text = $"안녕하세요, {PhotonNetwork.NickName} 님";
    }

    private void OnCreateRoomButtonClick()
    { 
        mainPanel.gameObject.SetActive(false);
        createRoomPanel.gameObject.SetActive(true);

        roomNameInput.text = "";
        playerNumberSlider.SetValueWithoutNotify(1);
        playerNumberInput.SetTextWithoutNotify("1");
    }

    private void OnLogoutButtonClick()
    {
        PhotonNetwork.Disconnect();
    }

    private void OnFindRoomButtonClick()
    {
        PhotonNetwork.JoinLobby();
    }

    private void OnCancelButtonClick()
    {
        createRoomPanel.gameObject.SetActive(false);
        mainPanel.gameObject.SetActive(true);
    }

    private void OnCreateButtonClick()
    {
        string roomName = roomNameInput.text;
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = (int)playerNumberSlider.value;
        PhotonNetwork.CreateRoom(roomName, options);
    }
}
