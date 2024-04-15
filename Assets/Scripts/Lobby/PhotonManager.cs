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
            Debug.Log($"��Ʈ��ũ ���� ���� : {currentState}");
        }
    }

    public override void OnConnected()
    {
        Debug.Log("����Ǿ����ϴ�.");
        PanelManager.Instance.OpenPanel(PanelType.Menu);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"������ �����մϴ�. ���� : {cause.ToString()}");
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
        Debug.Log($"���� �����մϴ�. �� �̸� {PhotonNetwork.CurrentRoom.Name}");
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