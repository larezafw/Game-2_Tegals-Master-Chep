using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using Photon.Realtime;
using TMPro;
using Photon.Pun.UtilityScripts;
using Firebase.Auth;
using Firebase;
using Firebase.Database;
using UnityEngine.UI;

public class GameLauncher : MonoBehaviourPunCallbacks
{
    public GameObject MenuHidangan;
    public TextMeshProUGUI ErrorMessegeText;
    public Button startButton;
    string gameVersion = "1";
    DatabaseReference database;
    string userId;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        MenuHidangan.SetActive(false);
    }
    

    private void Start()
    {
        MasterConnection();
        database = FirebaseDatabase.DefaultInstance.RootReference;
        userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
    }
    
    private void MasterConnection()
    {
        if(!PhotonNetwork.IsConnected)
        {
            ErrorMessegeText.SetText(KeyWord.YELLOW_COLOR_TAG + "Menghubungkan..." + KeyWord.CLOSE_ALL_TAG);
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
            startButton.interactable= false;
        }
    }

    public void ConnectButton()
    {
        AudioManager.audioManager.SoundOn(MusikName.Button);
        PhotonNetwork.JoinRandomRoom();
        startButton.interactable = false;
        MenuHidangan.SetActive(false);
    }

    public void ChooseHidanganButton()
    {
        AudioManager.audioManager.SoundOn(MusikName.Button);
        MenuHidangan.SetActive(true);
    }

    IEnumerator PlayGame()
    {
        YieldTask<DataSnapshot> task;
        yield return task = new YieldTask<DataSnapshot>(database.Child(Key_Data.USER).Child(userId).GetValueAsync());
        if (task.IsFailed)
        {
            StartCoroutine(PlayGame());
            yield break;
        }
        else
        {
            Dictionary<string, object> userData = task.Result.Value as Dictionary<string, object>;
            Debug.Log(userData);
            PhotonNetwork.LocalPlayer.NickName = (string)userData[Key_Data.NICKNAME];
            GameObject.FindGameObjectWithTag(KeyWord.GAME_MANAGER).GetComponent<GameManager>().FirstEnterGame(userData);
        }
    }

    void MainMenuErrors(int err)
    {
        switch (err)
        {
            case 1:
                ErrorMessegeText.SetText("Huruf harus lebih dari 3 dan kurang dari 10!");
                ErrorMessegeText.color = Color.red;
                break;

            case 2:
                ErrorMessegeText.SetText("Koneksi terganggu!");
                ErrorMessegeText.color = Color.red;
                break;

            case 11:
                ErrorMessegeText.SetText("Berhasil menemukan musuh!");
                ErrorMessegeText.color = Color.green;
                break;

            case 12:
                ErrorMessegeText.SetText("Mencari musuh...");
                ErrorMessegeText.color = Color.yellow;
                break;

            default:
                ErrorMessegeText.SetText("Masalah tidak diketahui");
                ErrorMessegeText.color = Color.red;
                break;

        }
    }

    #region PUN Callbacks
    public override void OnConnectedToMaster()
    {
        ErrorMessegeText.SetText("");
        startButton.interactable = true;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        MainMenuErrors(2);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Enemy Entered. Player in room: "+PhotonNetwork.CurrentRoom.PlayerCount);

        MainMenuErrors(11);
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2) StartCoroutine(PlayGame());
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room. Player in room " + PhotonNetwork.CurrentRoom.PlayerCount);

        MainMenuErrors(12);
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2) StartCoroutine(PlayGame());
    }
    #endregion
}
