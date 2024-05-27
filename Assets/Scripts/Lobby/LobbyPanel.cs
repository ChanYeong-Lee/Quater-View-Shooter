using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPanel : UIBase
{
    public RectTransform roomContent;
    public RoomElement roomPrefab;

    public Button exitButton;
    public Button refreshButton;
    private List<RoomElement> rooms;

    private void Awake()
    {
        rooms = new List<RoomElement>();

        exitButton.onClick.AddListener(OnExitButtonClick);
        refreshButton.onClick.AddListener(OnRefreshButtonClick);
    }
    
    private void OnEnable()
    {
        UpdateRoomList();
    }

    public void UpdateRoomList()
    {
        if (gameObject.activeSelf == false)
        {
            return;
        }
        
        print("Open Lobby Panel, Update Room List");

        foreach (RoomElement room in rooms)
        {
            Destroy(room.gameObject);
        }
        rooms.Clear();

        foreach (RoomInfo room in PhotonManager.Instance.roomList)
        {
            RoomElement roomInstance = Instantiate(roomPrefab, roomContent);
            roomInstance.SetRoom(room);
            rooms.Add(roomInstance);
        }
    }

    private void OnExitButtonClick()
    {
        PhotonNetwork.LeaveLobby();
    }

    private void OnRefreshButtonClick()
    {
        UpdateRoomList();
    }
}
