using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CompleteTaskManager : MonoBehaviour
{
    public GameObject PlayerParent;
    public GameObject EnemyParent;
    public GameObject PlayerItemPrefab;
    public GameObject EnemyItemPrefab;
    public TextMeshProUGUI TaskTitle;
    string ronde;

    BahanBahan[] Game1AllTask = { new BawangBombai(), new BawangPutih(), new Kemiri(),new Kencur(),
        new Kunir(), new Laos(), new Serai(), new Ketumbar(), new Cabai(), new BawangMerah(),
        new TomatMerah(), new Gula(), new Garam(), new Tempe(), new Air(), new Santan(), new Ketupat() };

    BahanBahan[] Game2AllTask = { new BumbuKuning(), new BumbuMerah()};

    BahanBahan[] Game3AllTask = { new HasilKuah(), new KupatGlabed() };

    private void Start()
    {
        SetupItem();
    }

    private void SetupItem()
    {
        ronde = GameObject.FindGameObjectWithTag(KeyWord.GAME_MANAGER).GetComponent<GameManager>().GetCurrentRound();
        BahanBahan[] currentTask= null;

        if (ronde == KeyWord.GAME1) currentTask = Game1AllTask;
        else if (ronde == KeyWord.GAME2) currentTask = Game2AllTask;
        else if (ronde == KeyWord.GAME3) currentTask = Game3AllTask;

        TaskTitle.SetText(SetTitle());

        for (int i = 0; i < currentTask.Length; i++)
        {
            string codeBahan = currentTask[i].GetCodeBahan();
            string namaBahan = currentTask[i].GetNamaBahan() + " " + currentTask[i].GetMaxJumlah() + "x";

            GameObject playerItemList = Instantiate(PlayerItemPrefab, PlayerParent.transform);
            playerItemList.name = codeBahan;
            playerItemList.transform.localPosition = new Vector3(200, -350 - (50 * i));
            playerItemList.GetComponent<TextMeshProUGUI>().SetText(namaBahan);

            GameObject enemyItemList = Instantiate(EnemyItemPrefab, EnemyParent.transform);
            enemyItemList.name = codeBahan;
            enemyItemList.transform.localPosition = new Vector3(-200, -350 - (50 * i));
            enemyItemList.GetComponent<TextMeshProUGUI>().SetText(namaBahan);
        }
    }

    string SetTitle()
    {
        if (ronde == KeyWord.GAME1) return "KUMPULKAN";
        else if (ronde == KeyWord.GAME2) return "SELESAIKAN";
        else if (ronde == KeyWord.GAME3) return "SELESAIKAN";
        else return null;
    }

    public void UpdateCompletedTask(bool isPlayer,string codeName)
    {
        if (isPlayer)
        {
            TextMeshProUGUI doneText = PlayerParent.transform.Find(codeName).GetComponent<TextMeshProUGUI>();
            doneText.color = Color.green;

            if (ronde == KeyWord.GAME1)
            {
                TextMeshProUGUI expiredText = EnemyParent.transform.Find(codeName).GetComponent<TextMeshProUGUI>();
                expiredText.color = Color.red;
            }
        }
        else
        {
            TextMeshProUGUI doneText = EnemyParent.transform.Find(codeName).GetComponent<TextMeshProUGUI>();
            doneText.color = Color.green;

            if (ronde == KeyWord.GAME1)
            {
                TextMeshProUGUI expiredText = PlayerParent.transform.Find(codeName).GetComponent<TextMeshProUGUI>();
                expiredText.color = Color.red;
            }
        }

    }
}
