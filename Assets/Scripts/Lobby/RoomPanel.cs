using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class RoomPanel : UIBase, IInRoomCallbacks
{
    public RectTransform playerContent;
    public PlayerElement playerPrefab;

    public TMP_Text roomNameText;
    public Button startButton;
    public Button readyButton;
    public Button exitButton;

    private List<PlayerElement> playerList;
    private PlayerElement localPlayerElement;

    private void Awake()
    {
        playerList = new List<PlayerElement>(); 
        startButton.onClick.AddListener(OnStartButtonClick);
        readyButton.onClick.AddListener(OnReadyButtonClick);
        exitButton.onClick.AddListener(OnExitButtonClick);

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);

        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        foreach (PlayerElement player in playerList)
        {
            Destroy(player.gameObject);
        }
        playerList.Clear();

        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            PlayerElement playerInstance = Instantiate(playerPrefab, playerContent);
            playerInstance.SetPlayer(player);

            if (player == PhotonNetwork.LocalPlayer)
            {
                localPlayerElement = playerInstance;
                PhotonHashtable propertiesToSet = PhotonNetwork.LocalPlayer.CustomProperties;
                propertiesToSet["Ready"] = PhotonNetwork.IsMasterClient;
                PhotonNetwork.LocalPlayer.SetCustomProperties(propertiesToSet);
            }

            if (player.CustomProperties.ContainsKey("Ready"))
            {
                playerInstance.SetReady((bool)player.CustomProperties["Ready"]);
            }

            playerList.Add(playerInstance);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            readyButton.gameObject.SetActive(false);
            startButton.gameObject.SetActive(true);
            startButton.interactable = CheckCanStart();
        }
        else
        {
            startButton.gameObject.SetActive(false);
            readyButton.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void OnStartButtonClick()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.LoadScene("GameScene");
    }

    private void OnReadyButtonClick()
    {
        PhotonHashtable propertiesToSet = PhotonNetwork.LocalPlayer.CustomProperties;
        propertiesToSet["Ready"] = (bool)propertiesToSet["Ready"] == false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(propertiesToSet);
        localPlayerElement.SetReady((bool)propertiesToSet["Ready"]);
    }

    private void OnExitButtonClick()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnMasterClientSwitched(Player newMasterClient) { }

    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerElement playerInstance = Instantiate(playerPrefab, playerContent);
        playerInstance.SetPlayer(newPlayer);
        playerList.Add(playerInstance);

        startButton.interactable = CheckCanStart();
    }

    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        PlayerElement element = playerList.Find((element) => element.player == otherPlayer);
        playerList.Remove(element);
        Destroy(element.gameObject);

        startButton.interactable = CheckCanStart();
    }

    public void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (targetPlayer == PhotonNetwork.LocalPlayer)
        {
            return;
        }

        PlayerElement element = playerList.Find((element) => element.player == targetPlayer);
        if (changedProps.ContainsKey("Ready"))
        {
            element.SetReady((bool)changedProps["Ready"]);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            startButton.interactable = CheckCanStart();
        }
    }

    public bool CheckCanStart()
    {
        foreach (PlayerElement player in playerList)
        {
            if (player.ready == false)
            {
                return false;
            }
        }

        return true;
    }

    public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged) { }
}
