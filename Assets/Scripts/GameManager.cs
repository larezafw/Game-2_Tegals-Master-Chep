using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.Audio;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameBoard board;
    MainScore mainScore;
    SceneScore sceneScore;

    Dictionary<string, object> userData;
    Dictionary<string, object> enemyData;
    DatabaseReference database;
    string userId;

    int ReadyPlayerNumber;

    public Action<string> SendChatMessege;
    public Action<SceneScore> SendStartSceneMessege;
    public Action<string> SendGameDoneMessge;

    string NextRound;
    string CurrentRound;
    bool Game1Played;
    bool Game2Played;
    bool Game3PLayed;
    bool Game1Doned;
    bool Game2Doned;
    bool Game3Doned;

    bool isPlayed;
    bool isOver;

    public GameObject FloatingTextPrefabs;
    public WInLosePage winLosePage;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        PlayTitleSound();
        SceneManager.sceneLoaded += OnSceneFinishLoaded;
        database = FirebaseDatabase.DefaultInstance.RootReference;
        userData = new Dictionary<string, object>();
        enemyData = new Dictionary<string, object>();
        userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
    }
    private void Update()
    {
        if (ReadyPlayerNumber >= 2 && AllPlayerReady()) StartTheGame();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    void OnSceneFinishLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == KeyWord.GAME1 && !Game1Played && !Game1Doned)
        {
            PlayFirstPreSceneStartSound();
            CurrentRound = KeyWord.GAME1;
            NextRound = KeyWord.GAME2;
            mainScore = new MainScore();
            sceneScore = new Game1Score();

            Game1Played = true;
            StartCoroutine(SendReadyMessege(KeyWord.GAME1_READY));
        }
        else if (scene.name == KeyWord.GAME2 && !Game2Played && !Game2Doned)
        {
            PlayPreSceneStartSound();
            CurrentRound = KeyWord.GAME2;
            NextRound = KeyWord.GAME3;
            sceneScore = new Game2Score();

            Game2Played = true;
            StartCoroutine(SendReadyMessege(KeyWord.GAME2_READY));
        }
        else if (scene.name == KeyWord.GAME3 && !Game3PLayed && !Game3Doned)
        {
            PlayPreSceneStartSound();
            CurrentRound = KeyWord.GAME3;
            NextRound = null;
            sceneScore = new Game3Score();

            Game3PLayed = true;
            StartCoroutine(SendReadyMessege(KeyWord.GAME3_READY));
        }
    }

    public void FirstEnterGame(Dictionary<string, object> data)
    {
        userData = data;
        EnterGame(KeyWord.GAME1);
    }
    public void EnterGame(string sceneName)
    {
        Debug.Log("enter game");
        StartCoroutine(LoadGame(sceneName));
    }

    IEnumerator LoadGame(string sceneName)
    {
        board.DisplayLoadingPage(sceneName + KeyWord.LOADING_PAGE);

        while (PhotonNetwork.PlayerListOthers[0].NickName == ""
            || PhotonNetwork.PlayerListOthers[0].NickName == null) yield return null;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator SendReadyMessege(string readyMessege)
    {
        while (!AllManagerReady()) yield return null;
        Dictionary<string, object> messegeValue = new Dictionary<string, object>();
        if (readyMessege == KeyWord.GAME1_READY) messegeValue = userData;

        ExitGames.Client.Photon.Hashtable ready = new ExitGames.Client.Photon.Hashtable();
        ready.Add(readyMessege, messegeValue);
        PhotonNetwork.LocalPlayer.SetCustomProperties(ready);
    }

    void Ready()
    {
        ReadyPlayerNumber += 1;
    }

    bool AllPlayerReady()
    {
        return (CurrentRound == KeyWord.GAME1 && Game1Played)
            || (CurrentRound == KeyWord.GAME2 && Game2Played)
            || (CurrentRound == KeyWord.GAME3 && Game3PLayed);
    }

    void StartTheGame()
    {
        if (enemyData == null || userData == null)
        {
            PhotonNetwork.Disconnect();
            return;
        }
        if (!isPlayed) isPlayed = true;
        ReadyPlayerNumber = 0;
        board.HideLoadingPageAndStart(mainScore, CurrentRound);
        SendStartSceneMessege.Invoke(sceneScore);
    }

    void UpdateMainScore(bool isLocal)
    {
        mainScore.UpdateScore(isLocal);
        board.UpdateScore(mainScore, isLocal, NextRound);
    }

    public void SpecifyTheWinner()
    {
        if (isOver) return;
        Player player = mainScore.GetWinner();

        if (player.IsLocal)
        {
            winLosePage.SetToWIn((long)userData[Key_Data.SCORE]+1);
            StartCoroutine(UpdatePlayerDatabase());
        }
        else winLosePage.SetToLose();

        isOver = true;
    }

    IEnumerator UpdatePlayerDatabase()
    {
        // UPDATE PLAYER DATABASE
        long updatedScore = (long)userData[Key_Data.SCORE] + 1;
        userData[Key_Data.SCORE] = updatedScore;

        YieldTask updateScoreTask;
        yield return updateScoreTask = new YieldTask(database.Child(Key_Data.USER).Child((string)userData[Key_Data.USER_ID]).SetValueAsync(userData));
        if (updateScoreTask.IsFailed)
        {
            StartCoroutine(UpdatePlayerDatabase());
            yield break;
        }
        else StartCoroutine(UpdateLeaderBoardDatabase(updatedScore));
    }

    IEnumerator UpdateLeaderBoardDatabase(long updatedScore)
    {
        // GET LEADERBOARD DATA
        List<object> leaderBoardList;
        YieldTask<DataSnapshot> getDataTask;
        yield return getDataTask = new YieldTask<DataSnapshot>(database.Child(Key_Data.LEADERBOARD).GetValueAsync());
        if (getDataTask.IsFailed)
        {
            Debug.Log("Error while get data");
            StartCoroutine(UpdateLeaderBoardDatabase(updatedScore));
            yield break;
        }
        else leaderBoardList = getDataTask.Result.Value as List<object>;

        // setup updating
        if (leaderBoardList == null)
        {
            Debug.Log("Null");
            leaderBoardList = new List<object>();
        }
        else if (leaderBoardList.Count < 5)
        {
            Dictionary<string, object> minValue = null;
            Debug.Log("Data kurang dari 5 (" + leaderBoardList.Count + ")");

            foreach (Dictionary<string, object> list in leaderBoardList)
            {
                string objectUserId = (string)list[Key_Data.USER_ID];
                long objectScore = (long)list[Key_Data.SCORE];

                if (objectUserId == userId)
                {
                    if (objectScore >= updatedScore)
                    {
                        Debug.Log("tingkat 1 Score player terdaftar dan lebih rendah");
                        StartCoroutine(UpdateEnemyDatabase());
                        yield break;
                    }
                    else minValue = list;
                }
            }
            if (minValue != null)
            {
                Debug.Log("tingkat 1 Removed");
                leaderBoardList.Remove(minValue);
            }
        }
        else if (leaderBoardList.Count >= 5)
        {
            long minScore = long.MaxValue;
            Dictionary<string, object> minValue = null;
            Debug.Log("Data lebih dari 5 ("+ leaderBoardList.Count+")");

            foreach (Dictionary<string, object> list in leaderBoardList)
            {
                string objectUserId = (string)list[Key_Data.USER_ID];
                long objectScore = (long)list[Key_Data.SCORE];

                if (objectUserId == userId)
                {
                    if (objectScore >= updatedScore)
                    {
                        Debug.Log("Tingkat 2 score player terdaftar dan lebih Rendah");
                        StartCoroutine(UpdateEnemyDatabase());
                        yield break;
                    }
                    else
                    {
                        minScore = objectScore;
                        minValue = list;
                        break;
                    }
                }
                else if (objectScore < minScore)
                {
                    minScore = objectScore;
                    minValue = list;
                }
            }
            if (minScore >= updatedScore) 
            {
                Debug.Log("Tingkat 2 Score Lebih Rendah dari daftar leaderboard");
                StartCoroutine(UpdateEnemyDatabase());
                yield break;
            }
            leaderBoardList.Remove(minValue);
        }
        Dictionary<string, object> userUpdatedData = new Dictionary<string, object>(userData);
        leaderBoardList.Add(userUpdatedData);
        List<object> orderedList = new List<object>();
        Debug.Log("Jumlah List Saat ini: " + leaderBoardList.Count);

        while (leaderBoardList.Count > 0)
        {
            long BiggestScore = -1;
            Dictionary<string, object> MaxValue = null;

            foreach (Dictionary<string, object> list in leaderBoardList)
            {
                long objectScore = (long)list[Key_Data.SCORE];
                if (objectScore > BiggestScore)
                {
                    BiggestScore = objectScore;
                    MaxValue = list;
                }
            }
            orderedList.Add(MaxValue);
            leaderBoardList.Remove(MaxValue);
            Debug.Log("List ordered. List Left " + leaderBoardList.Count);
        }

        // UPDATE LEADERBOARD DATABASE
        YieldTask UpdateLeaderBoardTask;
        yield return UpdateLeaderBoardTask = new YieldTask(database.Child(Key_Data.LEADERBOARD).SetValueAsync(orderedList));
        if (UpdateLeaderBoardTask.IsFailed)
        {
            Debug.Log("Error While Updating");
            StartCoroutine(UpdateLeaderBoardDatabase(updatedScore));
            yield break;
        }
        else StartCoroutine(UpdateEnemyDatabase());
        
    }
    
    IEnumerator UpdateEnemyDatabase()
    {
        enemyData[Key_Data.SCORE] = (long)0;

        YieldTask task;
        yield return task = new YieldTask(database.Child(Key_Data.USER).Child((string)enemyData[Key_Data.USER_ID]).SetValueAsync(enemyData));
        if (task.IsFailed)
        {
            StartCoroutine(UpdateEnemyDatabase());
            yield break;
        }
        else winLosePage.ActivateButton();
    }

    public void CreateFloatingText(Vector2 position, Color color, string caption)
    {
        Transform parent = GameObject.FindGameObjectWithTag(KeyWord.MAIN_CANVAS).transform;
        FloatinText floatingText = Instantiate(FloatingTextPrefabs, parent).GetComponent<FloatinText>();
        floatingText.Setup(position, color, caption);
    }

    public string GetCurrentRound()
    {
        return CurrentRound;
    }

    bool AllManagerReady()
    {
        return ((GameObject.FindGameObjectWithTag(KeyWord.GAME_MANAGER) != null) 
            && (GameObject.FindGameObjectWithTag(KeyWord.GAME1_MANAGER) != null
            || GameObject.FindGameObjectWithTag(KeyWord.GAME2_MANAGER) != null
            || GameObject.FindGameObjectWithTag(KeyWord.GAME3_MANAGER) != null));
    }

    bool ChatValue(ExitGames.Client.Photon.Hashtable item)
    {
        return item.ContainsKey(KeyWord.CHAT);
    }

    bool ReadyMessegeValue(ExitGames.Client.Photon.Hashtable item)
    {
        return (item.ContainsKey(KeyWord.GAME1_READY))
            || (item.ContainsKey(KeyWord.GAME2_READY))
            || (item.ContainsKey(KeyWord.GAME3_READY));
    }

    bool EnemyIdValue(bool isLocal)
    {
        return CurrentRound == KeyWord.GAME1 && !isLocal;
    }

    bool Game1DoneValue(ExitGames.Client.Photon.Hashtable item)
    {
        return item.ContainsKey(KeyWord.GAME1_DONE);
    }

    bool Game2DoneValue(ExitGames.Client.Photon.Hashtable item)
    {
        return item.ContainsKey(KeyWord.GAME2_DONE);
    }

    bool Game3DoneValue(ExitGames.Client.Photon.Hashtable item)
    {
        return item.ContainsKey(KeyWord.GAME3_DONE);
    }

    #region SOUND EFFECT

    void PlayTitleSound()
    {
        AudioManager.audioManager.SoundOn(MusikName.TitleScreen);
    }

    void PlayFirstPreSceneStartSound()
    {
        AudioManager.audioManager.SoundOff(MusikName.TitleScreen);
        AudioManager.audioManager.SoundOn(MusikName.PreStartScene);
    }
    void PlayPreSceneStartSound()
    {
        AudioManager.audioManager.SoundOn(MusikName.PreStartScene);
    }

    void PlayEndSceneSound()
    {
        AudioManager.audioManager.SoundOff(MusikName.Gameplay);
        AudioManager.audioManager.SoundOn(MusikName.EndScene);
    }
    #endregion

    #region PUN CALLBACKS

    public override void OnDisconnected(DisconnectCause cause)
    {
        StopAllCoroutines();
        winLosePage.SetToDisconnect();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (isPlayed) SpecifyTheWinner();
    }

    public override void OnLeftRoom()
    {
        AudioManager.audioManager.AllSoundOff();
        SceneManager.LoadScene(KeyWord.MAINMENU);
        StopAllCoroutines();
        Destroy(gameObject);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (ReadyMessegeValue(changedProps) && EnemyIdValue(targetPlayer.IsLocal))
        {
            enemyData = changedProps[KeyWord.GAME1_READY] as Dictionary<string, object>;
            Ready();
        }
        else if (ReadyMessegeValue(changedProps))
        {
            Ready();
        }
        else if (ChatValue(changedProps))
        {
            string chat = targetPlayer.NickName + ": " + (string)changedProps[KeyWord.CHAT];
            if (targetPlayer.IsLocal)
            {
                AudioManager.audioManager.SoundOn(MusikName.Chat);
                chat = KeyWord.YELLOW_COLOR_TAG + chat + KeyWord.CLOSE_COLOR_TAG;
            }
            else chat = KeyWord.MAGENTA_COLOR_TAG + chat + KeyWord.CLOSE_COLOR_TAG;
            SendChatMessege.Invoke(chat);
        }
        else if (Game1DoneValue(changedProps) && !Game1Doned)
        {
            PlayEndSceneSound();
            Game1Doned = true;
            UpdateMainScore(targetPlayer.IsLocal);
        }
        else if (Game2DoneValue(changedProps) && !Game2Doned)
        {
            PlayEndSceneSound();
            Game2Doned = true;
            UpdateMainScore(targetPlayer.IsLocal);
        }
        else if (Game3DoneValue(changedProps) && !Game3Doned)
        {
            PlayEndSceneSound();
            Game3Doned = true;
            UpdateMainScore(targetPlayer.IsLocal);
        }
    }
    #endregion
}

public class MainScore 
{
    string playerNickname;
    string enemyNickname;
    int playerScore;
    int enemyScore;

    public MainScore()
    {
        playerNickname = PhotonNetwork.LocalPlayer.NickName;
        enemyNickname = PhotonNetwork.PlayerListOthers[0].NickName;
        playerScore = 0;
        enemyScore = 0;
    }

    public void UpdateScore(bool isLocal)
    {
        if (isLocal) playerScore += 1;
        else enemyScore += 1;
    }

    public string GetPlayerNickname()
    {
        return playerNickname;
    }

    public string GetEnemyNickname()
    {
        return enemyNickname;
    }

    public string GetPlayerScore()
    {
        return playerScore.ToString();
    }

    public string GetEnemyScore()
    {
        return enemyScore.ToString();
    }

    public string GetPlayerPreviousScore()
    {
        return (playerScore - 1).ToString();
    }

    public string GetEnemyPreviousScore()
    {
        return (enemyScore - 1).ToString();
    }

    public Player GetWinner()
    {
        if (PhotonNetwork.PlayerListOthers.Length <= 0) return PhotonNetwork.LocalPlayer;
        else if (playerScore >= 2) return PhotonNetwork.LocalPlayer;
        else if (enemyScore >= 2) return PhotonNetwork.PlayerListOthers[0];
        else return null;

    }
}
