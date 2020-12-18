using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameBoard : MonoBehaviour
{
    public Action SendStartGameMessege;
    public GameManager gameManager;
    public GameObject mainBoard;
    public GameObject[] LoadingPages;
    public Image InfoColorImage;
    public TextMeshProUGUI InfoText;

    float startTimer;
    bool countingDown;

    //score
    public GameObject AllScoreText;
    public TextMeshProUGUI PlayerNameText;
    public TextMeshProUGUI EnemyNameText;
    public TextMeshProUGUI PlayerScoreText;
    public TextMeshProUGUI EnemyScoreText;

    //Game Instruction
    public GameObject AllInstuctionText;
    public TextMeshProUGUI IsiInstructionText;



    private void Awake()
    {
        HideAll();
    }

    private void Update()
    {
        if (countingDown && startTimer > 0f) MakeCountdown();
        else if (countingDown) HideScoreBoard();   
    }

    IEnumerator ShowScoreBoardOnStart(MainScore score,string round)
    {
        mainBoard.SetActive(true);
        AllScoreText.SetActive(true);
        AllInstuctionText.SetActive(false);

        PlayerNameText.SetText(score.GetPlayerNickname());
        EnemyNameText.SetText(score.GetEnemyNickname());
        PlayerScoreText.SetText(score.GetPlayerScore());
        EnemyScoreText.SetText(score.GetEnemyScore());

        InfoText.SetText(round);
        InfoColorImage.color = KeyWord.BlueGrey;

        yield return new WaitForSeconds(2f);
        AllScoreText.SetActive(false);
        AllInstuctionText.SetActive(true);
        IsiInstructionText.SetText(SetInstuctionContent(round));

        startTimer = 15f;
        countingDown = true;
    }

    IEnumerator ShowScoreBoardOnEnd(MainScore score, bool isPlayerPoint, string nextRound)
    {
        mainBoard.SetActive(true);
        AllScoreText.SetActive(true);
        AllInstuctionText.SetActive(false);

        PlayerNameText.SetText(score.GetPlayerNickname());
        EnemyNameText.SetText(score.GetEnemyNickname());
        if (isPlayerPoint)
        {
            InfoText.SetText("POIN PLAYER!");
            InfoColorImage.color = KeyWord.GreenField;
            PlayerScoreText.SetText(score.GetPlayerPreviousScore());
            EnemyScoreText.SetText(score.GetEnemyScore());

            yield return new WaitForSeconds(1f);
            PlayerScoreText.SetText(KeyWord.GREEN_COLOR_TAG + score.GetPlayerScore() + KeyWord.CLOSE_COLOR_TAG);
            EnemyScoreText.SetText(score.GetEnemyScore());
        }
        else
        {
            InfoText.SetText("POIN MUSUH!");
            InfoColorImage.color = KeyWord.RedCrimson;
            PlayerScoreText.SetText(score.GetPlayerScore());
            EnemyScoreText.SetText(score.GetEnemyPreviousScore());

            yield return new WaitForSeconds(1f);
            PlayerScoreText.SetText(score.GetPlayerScore());
            EnemyScoreText.SetText(KeyWord.GREEN_COLOR_TAG + score.GetEnemyScore() + KeyWord.CLOSE_COLOR_TAG);
        }

        yield return new WaitForSeconds(2f);
        if (score.GetWinner() == null) gameManager.EnterGame(nextRound);
        else gameManager.SpecifyTheWinner();
    }

    string SetInstuctionContent(string ronde)
    {
        if (ronde == KeyWord.GAME1) return KeyWord.GAME1_INSTRUCTION;
        else if (ronde == KeyWord.GAME2) return KeyWord.GAME2_INSTUCTION;
        else if(ronde== KeyWord.GAME3) return KeyWord.GAME3_INSTRUCTION;
        else return null;
    }

    void MakeCountdown()
    {
        int timeLeft = (int)startTimer;
        if (timeLeft > 5) InfoText.SetText(KeyWord.WHITE_COLOR_TAG + timeLeft.ToString() + KeyWord.CLOSE_COLOR_TAG);
        else if (timeLeft > 0) InfoText.SetText(KeyWord.RED_COLOR_TAG + timeLeft.ToString() + KeyWord.CLOSE_COLOR_TAG);
        else
        {
            InfoText.SetText(KeyWord.GREEN_COLOR_TAG + "Mulai!" + KeyWord.CLOSE_COLOR_TAG);
            if(!AudioManager.audioManager.StartGameSoundisOnPlay()) AudioManager.audioManager.SoundOn(MusikName.StartGame);
        }
        startTimer -= Time.deltaTime * 1;
    }

    public void UpdateScore(MainScore score, bool isPlayerPoint,string nextRound)
    {
        StartCoroutine(ShowScoreBoardOnEnd(score, isPlayerPoint, nextRound));
    }

    void HideScoreBoard() 
    {
        SendStartGameMessege.Invoke();
        AudioManager.audioManager.SoundOn(MusikName.Gameplay);
        mainBoard.SetActive(false);
        countingDown = false;
    }

    public void DisplayLoadingPage(string page)
    {
        foreach (GameObject LP in LoadingPages) 
            if (LP.name == page) LP.SetActive(true);
    }

    public void HideLoadingPageAndStart(MainScore score, string round)
    {
        foreach (GameObject LP in LoadingPages) LP.SetActive(false);
        StartCoroutine(ShowScoreBoardOnStart(score, round));
    }
    public void HideAll()
    {
        mainBoard.SetActive(false);
        foreach (GameObject LP in LoadingPages) LP.SetActive(false);
    }
}
