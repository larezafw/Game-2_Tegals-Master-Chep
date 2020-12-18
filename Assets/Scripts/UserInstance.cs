using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using TMPro;
using Firebase.Extensions;
using Firebase.Database;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;
using Photon.Pun;
using Photon.Realtime;

public class UserInstance : MonoBehaviourPunCallbacks
{
    public GameObject InputContent;
    public TextMeshProUGUI emailInputText;
    public TextMeshProUGUI nameInputText;
    public TextMeshProUGUI LogText;

    FirebaseUser user;
    FirebaseAuth Auth;
    DatabaseReference database;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (NoName()) PlayerPrefs.SetString(KeyWord.NAME, "No Name");
        if (FirebaseAuth.DefaultInstance.CurrentUser == null) FirebaseAuth.DefaultInstance.SignOut();

        Auth = FirebaseAuth.DefaultInstance;
        database = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void LoginButton()
    {
        AudioManager.audioManager.SoundOn(MusikName.Button);
        StartCoroutine(AsyncLogin());
    }

    IEnumerator AsyncLogin()
    {
        HideInputContent();

        YieldTask<FirebaseUser> task;
        yield return task = new YieldTask<FirebaseUser> (Auth.SignInWithEmailAndPasswordAsync(emailInputText.text, nameInputText.text));
        if (task.IsFailed) AuthError(task.exception);
        else
        {
            user = task.Result;
            SceneManager.LoadScene(KeyWord.MAINMENU);
        }
    }

    public void RegisterButton()
    {
        AudioManager.audioManager.SoundOn(MusikName.Button);
        StartCoroutine(AsyncRegistration());
    }
    
    IEnumerator AsyncRegistration()
    {
        HideInputContent();

        string inputName = nameInputText.text;
        if (DigitMeet(inputName))
        {
            PlayerPrefs.SetString(KeyWord.NAME, inputName);

            YieldTask<FirebaseUser> task;
            yield return task = new YieldTask<FirebaseUser>(Auth.CreateUserWithEmailAndPasswordAsync(emailInputText.text, nameInputText.text));
            if (task.IsFailed) AuthError(task.exception);
            else
            {
                user = task.task.Result;
                StartCoroutine(SetDatabasePath(inputName));
            }
        }
        else
        {
            LogText.SetText(KeyWord.RED_COLOR_TAG + "Username harus lebih dari 3 dan kurang dari 10!" + KeyWord.CLOSE_COLOR_TAG);
            DisplayInputContent();
        }
    }

    IEnumerator SetDatabasePath(string username)
    {
        LogText.SetText(KeyWord.YELLOW_COLOR_TAG + "Setup Database path!" + KeyWord.CLOSE_COLOR_TAG);
        Dictionary<string, object> FirstInput = new Dictionary<string, object>();
        FirstInput[Key_Data.USER_ID] = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        FirstInput[Key_Data.NICKNAME] = username;
        FirstInput[Key_Data.SCORE] = (long)0;

        YieldTask task;
        yield return task = new YieldTask(database.Child(Key_Data.USER).
            Child(user.UserId).SetValueAsync(FirstInput));
        if (task.IsFailed) AuthError(task.exception);
        else SceneManager.LoadScene(KeyWord.MAINMENU);
    }

    void HideInputContent()
    {
        LogText.SetText(KeyWord.YELLOW_COLOR_TAG + "Menunggu" + KeyWord.CLOSE_COLOR_TAG);
        InputContent.SetActive(false);
    }

    void DisplayInputContent() => InputContent.SetActive(true);

    bool NoName()
    {
        string defaultName = PlayerPrefs.GetString(KeyWord.NAME);
        return defaultName == null || defaultName == "";
    }

    bool DigitMeet(string value)
    {
        if (value.Length > 5 && value.Length < 10) return true;
        else return false;
    }

    void AuthError(System.AggregateException e)
    {
        FirebaseException ex = (FirebaseException)e.Flatten().InnerExceptions[0];
        LogText.SetText(KeyWord.RED_COLOR_TAG + "Error: " + ((AuthError)ex.ErrorCode).ToString() + KeyWord.CLOSE_COLOR_TAG);
        DisplayInputContent();
    }
    public FirebaseUser GetUser()
    {
        return user;
    }

    #region PUN CALLBACKS

    public override void OnDisconnected(DisconnectCause cause)
    {
        Destroy(gameObject);
    }

    #endregion
}

public class YieldTask<T>: CustomYieldInstruction
{
    public Task<T> task { get; }
    public bool IsCompleted { get { return task.IsCompleted; } }
    public bool IsFailed { get { return task.IsFaulted || task.IsCanceled; } }
    public T Result { get { return task.Result; } }
    public AggregateException exception { get { return task.Exception; } }

    public YieldTask(Task<T> value)
    {
        task = value;
    }

    public override bool keepWaiting => !task.IsCompleted;
}

public class YieldTask: CustomYieldInstruction
{
    public Task task { get; }
    public bool IsCompleted { get { return task.IsCompleted; } }
    public bool IsFailed { get { return task.IsFaulted || task.IsCanceled; } }
    public AggregateException exception { get { return task.Exception; } }

    public YieldTask(Task value)
    {
        task = value;
    }

    public override bool keepWaiting => !task.IsCompleted;
}
