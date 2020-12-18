using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioManager;

    AudioSource[] allSound;

    [SerializeField] AudioSource TitleScreenSound;
    [SerializeField] AudioSource StartGameSound;
    [SerializeField] AudioSource GameplaySound;
    [SerializeField] AudioSource WinSound;
    [SerializeField] AudioSource LoseSound;
    [SerializeField] AudioSource EndSceneSound;
    [SerializeField] AudioSource LoadingScreenSound;

    [SerializeField] AudioSource ChatSound;
    [SerializeField] AudioSource ButtonSound;
    [SerializeField] AudioSource CountdownSound;
    [SerializeField] AudioSource SuccessDropSound;
    [SerializeField] AudioSource FailDropSound;
    [SerializeField] AudioSource EnemyPointSound;

    [SerializeField] AudioSource ResepSound;
    [SerializeField] AudioSource BlenderSound;


    private void Awake()
    {
        if (audioManager == null) audioManager = this;
        else if (audioManager != null && audioManager != this) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        allSound = new AudioSource[] {TitleScreenSound, StartGameSound, GameplaySound, WinSound, LoseSound,
                            EndSceneSound,LoadingScreenSound,ChatSound,ButtonSound,CountdownSound,SuccessDropSound,
                            FailDropSound, EnemyPointSound, ResepSound, BlenderSound};
    }
    
    public void SoundOn(MusikName item) => IdentifiedAudioSource(item).Play();

    public void SoundOff(MusikName item) => IdentifiedAudioSource(item).Stop();    

    public void AllSoundOff()
    {
        foreach (AudioSource audSource in allSound)
            audSource.Stop();
    }

    AudioSource IdentifiedAudioSource(MusikName item)
    {
        switch (item)
        {
            case MusikName.TitleScreen: return TitleScreenSound;
            case MusikName.StartGame: return StartGameSound;
            case MusikName.Gameplay: return GameplaySound; 
            case MusikName.Win: return WinSound; 
            case MusikName.Lose: return LoseSound;
            case MusikName.EndScene: return EndSceneSound;
            case MusikName.PreStartScene: return LoadingScreenSound;

            case MusikName.Chat: return ChatSound;
            case MusikName.Button: return ButtonSound;
            case MusikName.Countdown: return CountdownSound;
            case MusikName.SuccessDrop: return SuccessDropSound;
            case MusikName.FailDrop: return FailDropSound;
            case MusikName.EnemyPoint: return EnemyPointSound;

            case MusikName.Resep: return ResepSound;
            case MusikName.Blender: return BlenderSound;
            default: return null;
        }
    }

    public bool ResepSoundIsOnPlay()
    {
        return ResepSound.isPlaying;
    }

    public bool StartGameSoundisOnPlay()
    {
        return StartGameSound.isPlaying;
    }
}
public enum MusikName
{
    TitleScreen,
    StartGame,
    Gameplay,
    Win,
    Lose,
    EndScene,
    PreStartScene,

    Chat,
    Button,
    Countdown,
    SuccessDrop,
    FailDrop,
    EnemyPoint,

    Resep,
    Blender,
}
