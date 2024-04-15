using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance { get; private set; }

    private ClientState currentState;
    public List<RoomInfo> roomList;

    private void Awake()
    {
        Instance = this; 

        roomList = new List<RoomInfo>();
    }

    private void Update()
    {
        if (PhotonNetwork.NetworkClientState != currentState)
        {
            currentState = PhotonNetwork.NetworkClientState;
            Debug.Log($"네트워크 상태 변경 : {currentState}");
        }
    }

    public override void OnConnected()
    {
        Debug.Log("연결되었습니다.");
        PanelManager.Instance.OpenPanel(PanelType.Menu);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"연결을 해제합니다. 사유 : {cause.ToString()}");
        PanelManager.Instance.OpenPanel(PanelType.Login);   
    }
    public override void OnJoinedRoom()
    {
        PanelManager.Instance.OpenPanel(PanelType.Room);
    }

    public override void OnLeftRoom()
    {
        PanelManager.Instance.OpenPanel(PanelType.Menu);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log($"방을 생성합니다. 방 이름 {PhotonNetwork.CurrentRoom.Name}");
    }

    public override void OnJoinedLobby()
    {
        PanelManager.Instance.OpenPanel(PanelType.Lobby);
    }

    public override void OnLeftLobby()
    {
        PanelManager.Instance.OpenPanel(PanelType.Menu);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        this.roomList = roomList;
        PanelManager.Instance.lobby.UpdateRoomList();
    }


}