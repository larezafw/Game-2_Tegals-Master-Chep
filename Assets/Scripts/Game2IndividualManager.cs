using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game2IndividualManager : MonoBehaviourPunCallbacks
{
    bool BumbuValue(ExitGames.Client.Photon.Hashtable item)
    {
        return item.ContainsKey(KeyWord.GAME2_BUMBU_DONE);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (BumbuValue(changedProps))
        {
            string jenisBumbu = (string)changedProps[KeyWord.GAME2_BUMBU_DONE];
            GameObject.FindGameObjectWithTag(KeyWord.COMPLETE_TASK_MANAGER).GetComponent<CompleteTaskManager>()
                .UpdateCompletedTask(targetPlayer.IsLocal, jenisBumbu);
            GameObject.FindGameObjectWithTag(KeyWord.INFO_MANAGER).GetComponent<InfoManager>().AddScoreVisual(targetPlayer.IsLocal);

            if (!targetPlayer.IsLocal) AudioManager.audioManager.SoundOn(MusikName.EnemyPoint);
        }
    }
}
