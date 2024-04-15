using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomElement : MonoBehaviour
{
    public TMP_Text roomNameText;
    public RoomInfo room;
    public Button enterButton;

    private void Awake()
    {
        enterButton.onClick.AddListener(OnEnterButtonClick);
    }

    public void SetRoom(RoomInfo room)
    {
        this.room = room;
        roomNameText.text = room.Name;
    }

    private void OnEnterButtonClick()
    {
        PhotonNetwork.JoinRoom(room.Name);
    }
}
