using Firebase.Database;
using Photon.Pun.Demo.Cockpit;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderBoard :MonoBehaviour 
{
    public GameObject leaderBoarPage;
    public GameObject board;
    public TextMeshProUGUI[] LeaderBoardNameText;
    public TextMeshProUGUI[] leaderBoardScoreText;
    DatabaseReference database;

    private void Start()
    {
        database = FirebaseDatabase.DefaultInstance.RootReference;
        HideAllButton();
    }

    public void DisplayLeaderBoardButton()
    {
        AudioManager.audioManager.SoundOn(MusikName.Button);
        leaderBoarPage.SetActive(true);
        StartCoroutine(SetupLeaderBoard());
    }

    void ShowBoard (List<object> item)
    {
        int itemCounter = item.Count;
        if (itemCounter > 5) itemCounter = 5;

        for (int i = 0; i < itemCounter; i++)
        {
            Dictionary<string, object> data = item[i] as Dictionary<string, object>;
            string nickname = (string)data[Key_Data.NICKNAME];
            long winScore = (long)data[Key_Data.SCORE];

            LeaderBoardNameText[i].SetText((i + 1) + ". " + nickname);
            leaderBoardScoreText[i].SetText(winScore + " WIN");

            Debug.Log("Player " + i);
        }
        board.SetActive(true);
    }

    public void HideAllButton()
    {
        AudioManager.audioManager.SoundOn(MusikName.Button);
        board.SetActive(false);
        leaderBoarPage.SetActive(false);
    }

    IEnumerator SetupLeaderBoard()
    {
        YieldTask <DataSnapshot>getLeaderBoardTask;
        yield return getLeaderBoardTask = new YieldTask<DataSnapshot>(database.Child(Key_Data.LEADERBOARD).GetValueAsync());
        if (getLeaderBoardTask.IsFailed)
        {
            StartCoroutine(SetupLeaderBoard());
            yield break;
        }
        else ShowBoard(getLeaderBoardTask.Result.Value as List<object>);
    }
}
