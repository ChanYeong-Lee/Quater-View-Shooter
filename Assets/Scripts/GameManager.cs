using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviour, IInRoomCallbacks
{
    public static GameManager Instance { get; private set; }

    public PlayerController playerController;
    public CameraController cameraController;
    public PlayerModel playerModel;
    public List<Transform> spawnPoints;
    public List<PlayerModel> wholeModels;

    public int goalKill = 10;

    public int kill;
    public int death;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private IEnumerator Start()
    {
        PhotonHashtable propertiesToSet = PhotonNetwork.LocalPlayer.CustomProperties;
        propertiesToSet["Kill"] = 0;
        propertiesToSet["Death"] = 0;
        PhotonNetwork.LocalPlayer.SetCustomProperties(propertiesToSet);

        yield return new WaitUntil(() => PhotonNetwork.LocalPlayer.GetPlayerNumber() >= 0);

        int random = Random.Range(0, 2);
        Vector3 spawnPoint = spawnPoints[PhotonNetwork.LocalPlayer.GetPlayerNumber()].position;
        
        switch (random)
        {
            case 0:
                playerModel = PhotonNetwork.Instantiate("Prefabs/Player", spawnPoint, Quaternion.identity).GetComponent<PlayerModel>();
                break;
            case 1:
                playerModel = PhotonNetwork.Instantiate("Prefabs/Player2", spawnPoint, Quaternion.identity).GetComponent<PlayerModel>();
                break;
            default:
                playerModel = PhotonNetwork.Instantiate("Prefabs/Player", spawnPoint, Quaternion.identity).GetComponent<PlayerModel>();
                break;
        }

        playerController.model = playerModel;
        cameraController.mainTarget = playerModel.transform;
        cameraController.cam.LookAt(playerModel.transform.position);

        playerController.gameObject.SetActive(true);
        cameraController.gameObject.SetActive(true);
    }

    public void OnPlayerEnteredRoom(Player newPlayer) { }

    public void OnPlayerLeftRoom(Player otherPlayer) { }

    public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged) { }

    public void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        CheckGameWin();
    }

    public void OnMasterClientSwitched(Player newMasterClient) { }

    public void CheckGameWin()
    {
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if (player.CustomProperties.ContainsKey("Kill"))
            {
                if ((int)player.CustomProperties["Kill"] >= goalKill)
                {
                    Debug.Log($"{player} ¥‘¿Ã ¿Ã±‚ºÃΩ¿¥œ¥Ÿ.");
                }
            }
        }
    }

    public void GetKill()
    {
        PhotonHashtable propertiesToSet = PhotonNetwork.LocalPlayer.CustomProperties;
        propertiesToSet["Kill"] = (int)propertiesToSet["Kill"] + 1;
        PhotonNetwork.LocalPlayer.SetCustomProperties(propertiesToSet);
    }

    public void GetDeath()
    {
        PhotonHashtable propertiesToSet = PhotonNetwork.LocalPlayer.CustomProperties;
        propertiesToSet["Death"] = (int)propertiesToSet["Death"] + 1;
        PhotonNetwork.LocalPlayer.SetCustomProperties(propertiesToSet);
    }
}
