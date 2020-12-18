using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game3IndividuaManager : MonoBehaviourPunCallbacks
{
    public GameObject[] allProperty;
    public GameMaterials hasilKuah;

    public void DisplayProperty()
    {
        foreach (GameObject prop in allProperty) prop.SetActive(true);
    }

    public void HidePropterty()
    {
        foreach (GameObject prop in allProperty) prop.SetActive(false);
    }

    bool KuahValue(ExitGames.Client.Photon.Hashtable item)
    {
        return item.ContainsKey(KeyWord.GAME3_HASIL_KUAH);
    }

    bool GlabedValue(ExitGames.Client.Photon.Hashtable item)
    {
        return item.ContainsKey(KeyWord.GAME3_KUPAT_GLABED);
    }

    #region PUN CALLBACK

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (KuahValue(changedProps))
        {
            hasilKuah.KuahConfiguration(true);
            GameObject.FindGameObjectWithTag(KeyWord.INFO_MANAGER)
                .GetComponent<InfoManager>().AddScoreVisual(targetPlayer.IsLocal);
            GameObject.FindGameObjectWithTag(KeyWord.COMPLETE_TASK_MANAGER).GetComponent<CompleteTaskManager>()
                .UpdateCompletedTask(targetPlayer.IsLocal, KeyWord.GAME3_HASIL_KUAH);

            if (!targetPlayer.IsLocal) AudioManager.audioManager.SoundOn(MusikName.EnemyPoint);
        }
        else if (GlabedValue(changedProps))
        {
            GameObject.FindGameObjectWithTag(KeyWord.INFO_MANAGER)
                .GetComponent<InfoManager>().AddScoreVisual(targetPlayer.IsLocal);
            GameObject.FindGameObjectWithTag(KeyWord.COMPLETE_TASK_MANAGER).GetComponent<CompleteTaskManager>()
                .UpdateCompletedTask(targetPlayer.IsLocal, KeyWord.GAME3_KUPAT_GLABED);

            if (!targetPlayer.IsLocal) AudioManager.audioManager.SoundOn(MusikName.EnemyPoint);
        }
    }

    #endregion
}
