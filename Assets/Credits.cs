using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    public GameObject credit;

    private void Awake() => HideCreditButton();

    public void DisplayCreditButton()
    {
        AudioManager.audioManager.SoundOn(MusikName.Button);
        credit.SetActive(true);
    }

    public void HideCreditButton()
    {
        AudioManager.audioManager.SoundOn(MusikName.Button);
        credit.SetActive(false);
    }
}
