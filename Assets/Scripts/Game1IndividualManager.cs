using JetBrains.Annotations;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Game1IndividualManager : MonoBehaviourPunCallbacks
{
    public List<string> MaterialDoned { get; private set; }

    private void Start()
    {
        MaterialDoned = new List<string>();
    }
    bool MaterialValue(ExitGames.Client.Photon.Hashtable item)
    {
        return item.ContainsKey(KeyWord.GAME1_MATERIAL_DONE);
    }

    #region PUN CALLBACKS

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (MaterialValue(changedProps))
        {
            string bahan = (string)changedProps[KeyWord.GAME1_MATERIAL_DONE];
            if (!MaterialDoned.Contains(bahan))
            {
                MaterialDoned.Add(bahan);
                GameObject.FindGameObjectWithTag(KeyWord.COMPLETE_TASK_MANAGER).GetComponent<CompleteTaskManager>().UpdateCompletedTask(targetPlayer.IsLocal, bahan);
                GameObject.FindGameObjectWithTag(KeyWord.INFO_MANAGER).GetComponent<InfoManager>().AddScoreVisual(targetPlayer.IsLocal);

                if (!targetPlayer.IsLocal) AudioManager.audioManager.SoundOn(MusikName.EnemyPoint);
            }
            else Debug.Log("Material Doned!");
        }
        else Debug.Log("Unknow Player Property Changed!");
    }
    #endregion
}
