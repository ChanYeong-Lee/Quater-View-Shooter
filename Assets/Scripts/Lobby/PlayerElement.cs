using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerElement : MonoBehaviour
{
    public Player player;
    public bool ready;
    public TMP_Text playerNameText;
    public TMP_Text readyIndicatorText;

    public void SetPlayer(Player player)
    {
        this.player = player;
        playerNameText.text = player.NickName;
    }

    public void SetReady(bool ready)
    {
        this.ready = ready;
        Color color = new Color(1.0f, 1.0f, 1.0f, ready ? 1.0f : 10 / 255);
        readyIndicatorText.color = color;
    }
}
