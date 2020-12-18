using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoManager : MonoBehaviour
{
    public GameObject InfoPanel;
    public RectTransform InfoButton;
    public CanvasGroup TaskCanvas;
    public TextMeshProUGUI InfoText;
    public TextMeshProUGUI PlayerAddScoreText;
    public TextMeshProUGUI EnemyAddScoreText;
    public TextMeshProUGUI TimeLapseText;
    SceneScore sceneScore;
    double firstTimeLapse;
    bool infoStatus;
    bool started;

    private void Awake()
    {
        HideInfo();
        FindMainManager().GetComponent<GameManager>().SendStartSceneMessege += SetSceneScore;
        FindGameBoard().GetComponent<GameBoard>().SendStartGameMessege += StartTimeLapsting;
    }

    private void Update()
    {
        if (started) UpdateTimeLapse();
    }

    private void OnDestroy()
    {
        if (FindMainManager() != null) FindMainManager().GetComponent<GameManager>().SendStartSceneMessege -= SetSceneScore;
    }

    void SetSceneScore(SceneScore item)
    {
        sceneScore = item;
    }

    void StartTimeLapsting()
    {
        firstTimeLapse = PhotonNetwork.Time;
        started = true;
    }

    public void ChangeStatus()
    {
        if (infoStatus) HideInfo();
        else DisplayInfo();
    }

    void DisplayInfo()
    {
        InfoPanel.SetActive(true);
        InfoButton.transform.localScale = new Vector3(1f, 1f, 1f);
        InfoText.SetText("");
        TaskCanvas.alpha = 1f;
        
        infoStatus = true;
    }

    void HideInfo()
    {
        InfoPanel.SetActive(false);
        InfoButton.transform.localScale = new Vector3(1f, -1f, 1f);
        InfoText.SetText("INFO");
        TaskCanvas.alpha = 0f;

        infoStatus = false;
    }

    public void AddScoreVisual(bool isPlayer)
    {
        if (isPlayer) PlayerAddScoreText.SetText(sceneScore.AddScoreAndPrint(isPlayer));
        else EnemyAddScoreText.SetText(sceneScore.AddScoreAndPrint(isPlayer));

        if (sceneScore.PlayerIsLeading())
        {
            PlayerAddScoreText.color = Color.green;
            EnemyAddScoreText.color = Color.red;
        }
        else if (sceneScore.EnemyIsLeading())
        {
            PlayerAddScoreText.color = Color.red;
            EnemyAddScoreText.color = Color.green;
        }
        else
        {
            PlayerAddScoreText.color = Color.white;
            EnemyAddScoreText.color = Color.white;
        }
    }

    void UpdateTimeLapse()
    {
        int timeLapsed = (int)(PhotonNetwork.Time - firstTimeLapse);
        TimeLapseText.SetText("Selang Waktu: \n " + timeLapsed);
    }

    GameObject FindMainManager()
    {
        return GameObject.FindGameObjectWithTag(KeyWord.GAME_MANAGER);
    }

    GameObject FindGameBoard()
    {
        return GameObject.FindGameObjectWithTag(KeyWord.GAME_BOARD);
    }
}

public class SceneScore
{
    protected string doneMessge;
    protected int playerScore;
    protected int enemyScore;
    protected int winningScore;

    public string AddScoreAndPrint(bool isPlayer)
    {
        if (isPlayer)
        {
            playerScore += 1;
            if (GameDone()) SendDoneMessege();
            return playerScore.ToString();
        }
        else
        {
            enemyScore += 1;
            return enemyScore.ToString();
        }
    }

    protected bool GameDone()
    {
        return playerScore >= winningScore;
    }

    public bool PlayerIsLeading()
    {
        return playerScore > enemyScore;
    }

    public bool EnemyIsLeading()
    {
        return enemyScore > playerScore;
    }

    protected void SendDoneMessege()
    {
        ExitGames.Client.Photon.Hashtable messege = new ExitGames.Client.Photon.Hashtable();
        messege.Add(doneMessge, 1);
        PhotonNetwork.LocalPlayer.SetCustomProperties(messege);
    }
}

public class Game1Score : SceneScore
{
    public Game1Score()
    {
        doneMessge = KeyWord.GAME1_DONE;
        playerScore = 0;
        winningScore = 9;
    }
}

public class Game2Score : SceneScore
{
    public Game2Score()
    {
        doneMessge = KeyWord.GAME2_DONE;
        playerScore = 0;
        winningScore = 2;
    }
}

public class Game3Score: SceneScore
{
    public Game3Score()
    {
        doneMessge = KeyWord.GAME3_DONE;
        playerScore = 0;
        winningScore = 2;
    }
}
