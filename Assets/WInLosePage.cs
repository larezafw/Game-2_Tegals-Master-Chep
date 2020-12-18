using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class WInLosePage : MonoBehaviour
{
    public GameObject gameManager;

    public GameObject winPage;
    public Sprite[] WinLoseSprites;
    public Image image;
    public TextMeshProUGUI text;
    public Button button;

    public GameObject disconnectedPage;

    void Start() => Hide();

    void Hide()
    {
        winPage.SetActive(false);
        disconnectedPage.SetActive(false);
    }
    
    public void SetToWIn(long winStreak)
    {
        AudioManager.audioManager.SoundOn(MusikName.Win);
        image.sprite = WinLoseSprites[0];
        text.SetText(winStreak + " WIN STREAK!");
        text.color = Color.yellow;
        button.interactable = false;
        winPage.SetActive(true);
    }

    public void SetToLose()
    {
        AudioManager.audioManager.SoundOn(MusikName.Lose);
        image.sprite = WinLoseSprites[1];
        text.SetText("YOU LOSE!");
        text.color = Color.red;
        button.interactable = true;
        winPage.SetActive(true);
    }

    public void SetToDisconnect()
    {
        disconnectedPage.SetActive(true);
    }

    public void ActivateButton() => button.interactable = true;

    public void MainMenuButton()
    {
        Photon.Pun.PhotonNetwork.LeaveRoom();
    }
    
    public void DisconnectButton()
    {
        AudioManager.audioManager.AllSoundOff();
        SceneManager.LoadScene(KeyWord.GAME_LOGIN);
        Destroy(gameManager);
    }
}
