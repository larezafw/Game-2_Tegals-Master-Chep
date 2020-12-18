using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AllPlayerName : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI playerText;
    public TextMeshProUGUI enemyText;
    Player player;
    Player Enemy;
    void Start()
    {
        SetPlayerName();
        SetEnemyName();
    }

    void SetPlayerName()
    {
        player = PhotonNetwork.LocalPlayer;
        playerText.SetText(player.NickName) ;
        playerText.color = Color.yellow;
    }
    void SetEnemyName()
    {
        Enemy = PhotonNetwork.PlayerListOthers[0];
        enemyText.SetText(Enemy.NickName);
        enemyText.color = Color.white;
    }

}
