using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Game2Blender : MonoBehaviour, IDropHandler
{
    public Animator anim;
    public Button OnButton;
    public Button CancelButton;
    public GameObject ItemPrefab;
    public RectTransform[] ItemsDisplay;
    List<BahanBumbuHalus> itemsInBlender;
    List<string> itemsCode;
    List<string> bumbuDoned;
    GameManager gameManager;

    void Start()
    {
        FirstSetup();
        SetMainManager();               
    }

    void FirstSetup()
    {
        bumbuDoned = new List<string>();
        ResetItemDisplay();
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameMaterials bahan = eventData.pointerDrag.GetComponent<GameMaterials>();
        if (bahan != null && OnDefaultANimation())
        {
            BahanBahan jenisBahan = bahan.GetJenisBahan();
            if (!itemsCode.Contains(jenisBahan.GetCodeBahan()))
            {
                if (itemsCode.Count < 8)
                {
                    AudioManager.audioManager.SoundOn(MusikName.Button);
                    itemsCode.Add(jenisBahan.GetCodeBahan());
                    itemsInBlender.Add(new BahanBumbuHalus(jenisBahan));
                    UpdateItemDisplay();
                }
                else
                {
                    gameManager.CreateFloatingText(transform.localPosition, Color.red, "Blender Penuh!");
                }
            }
            else
            {
                AudioManager.audioManager.SoundOn(MusikName.Button);
                itemsInBlender.ForEach(x =>
                {
                    if (x.GetCodeBahan() == jenisBahan.GetCodeBahan())
                        x.TambahBahan();
                });
                UpdateItemDisplay();
            }
        }
        else if (bahan != null) gameManager.CreateFloatingText(transform.localPosition, Color.red, "Sedang memblender!");
    }

   public void ResepIntructionButton()
    {
        if (!AudioManager.audioManager.ResepSoundIsOnPlay()) AudioManager.audioManager.SoundOn(MusikName.Resep);
        else gameManager.CreateFloatingText(transform.localPosition, Color.red, "Sedang Memainkan!");
    }

    public void CancelingButton()
    {
        AudioManager.audioManager.SoundOn(MusikName.Button);
        ResetItemDisplay();
    }

    public void TurnOnBlenderButton()
    {
        if (IsCodeBumbuKuning())
        {
            if (CheckBumbuKuningList()) anim.SetBool(KeyWord.GAME2_ANIM_PROP_BLENDER_BUMBU_KUNING, true);
            else anim.SetBool(KeyWord.GAME2_ANIM_PROP_BLENDER_GAGAL, true);
        }
        else if (IsCodeBumbuMerah())
        {
            if (ChechBumbuMerahList()) anim.SetBool(KeyWord.GAME2_ANIM_PROP_BLENDER_BUMBU_MERAH, true);
            else anim.SetBool(KeyWord.GAME2_ANIM_PROP_BLENDER_GAGAL, true);
        }
        else anim.SetBool(KeyWord.GAME2_ANIM_PROP_BLENDER_GAGAL, true);

        AudioManager.audioManager.SoundOn(MusikName.Button);
        AudioManager.audioManager.SoundOn(MusikName.Blender);
        ResetItemDisplay();
    }

    void UpdateItemDisplay()
    {
        for (int i = 0; i < itemsCode.Count; i++)
        {
            ItemsDisplay[i].GetComponent<GameMaterials>().LateIdentify(itemsInBlender[i], false);
            ItemsDisplay[i].gameObject.SetActive(true);
        }
        OnButton.interactable = true;
        CancelButton.interactable = true;
    }

    void ResetItemDisplay()
    {
        itemsInBlender = new List<BahanBumbuHalus>();
        itemsCode = new List<string>();
        foreach (RectTransform item in ItemsDisplay)
            item.gameObject.SetActive(false);
        OnButton.interactable = false;
        CancelButton.interactable = false;
    }

    public void AnimationStateResult()
    {
        if (anim.GetBool(KeyWord.GAME2_ANIM_PROP_BLENDER_BUMBU_KUNING))
        {
            string doneItem = KeyWord.GAME2_BUMBU_KUNING;
            if (!bumbuDoned.Contains(doneItem))
            {
                SendBumbuDoneMessege(doneItem);
                gameManager.CreateFloatingText(transform.localScale, Color.green, "+1 Point!");
                bumbuDoned.Add(doneItem);
            }
            else gameManager.CreateFloatingText(transform.localPosition, Color.red, "Bumbu sudah dibuat!");
            
        }
        else if(anim.GetBool(KeyWord.GAME2_ANIM_PROP_BLENDER_BUMBU_MERAH))
        {
            string doneItem = KeyWord.GAME2_BUMBU_MERAH;
            if (!bumbuDoned.Contains(doneItem))
            {
                SendBumbuDoneMessege(doneItem);
                gameManager.CreateFloatingText(transform.localPosition,Color.green,"+1 Point");
                bumbuDoned.Add(doneItem);
            }
            else gameManager.CreateFloatingText(transform.localPosition, Color.red, "Sudah dibuat!");
            
        }
        else gameManager.CreateFloatingText(transform.localPosition, Color.red, "Resep gagal!");

        AudioManager.audioManager.SoundOff(MusikName.Blender);
        ResetAnimationState();
    }

    void ResetAnimationState()
    {
        anim.SetBool(KeyWord.GAME2_ANIM_PROP_BLENDER_GAGAL, false);
        anim.SetBool(KeyWord.GAME2_ANIM_PROP_BLENDER_BUMBU_KUNING, false);
        anim.SetBool(KeyWord.GAME2_ANIM_PROP_BLENDER_BUMBU_MERAH, false);
    }

    bool OnDefaultANimation()
    {
        return (!anim.GetBool(KeyWord.GAME2_ANIM_PROP_BLENDER_GAGAL))
            && (!anim.GetBool(KeyWord.GAME2_ANIM_PROP_BLENDER_BUMBU_KUNING))
            && (!anim.GetBool(KeyWord.GAME2_ANIM_PROP_BLENDER_BUMBU_MERAH));
    }

    bool CheckBumbuKuningList()
    {
        bool result = itemsInBlender.Count == 8;
        itemsInBlender.ForEach(x =>
        {
            if (!x.BumbuKuning())
                result = false;
        });
        return result;
    }

    bool ChechBumbuMerahList()
    {
        bool result = itemsInBlender.Count == 7;
        itemsInBlender.ForEach(x =>
        {
            if (!x.BumbuMerah())
                result = false;
        });
        return result;
    }

    bool IsCodeBumbuKuning()
    {
        bool result = true;
        itemsCode.ForEach(x =>
        {
            if (!CodeBumbuKuning(x))
                result = false;
        });
        return result;
    }

    bool IsCodeBumbuMerah()
    {
        bool result = true;
        itemsCode.ForEach(x =>
        {
            if (!CodeBumbuMerah(x))
                result = false;
        });
        return result;
    }

    bool CodeBumbuKuning(string code)
    {
        return (code == KeyWord.GAME1_BAWANG_BOMBAI)
            || (code == KeyWord.GAME1_BAWANG_PUTIH)
            || (code == KeyWord.GAME1_KEMIRI)
            || (code == KeyWord.GAME1_KENCUR)
            || (code == KeyWord.GAME1_KUNIR)
            || (code == KeyWord.GAME1_LAOS)
            || (code == KeyWord.GAME1_SERAI)
            || (code == KeyWord.GAME1_KETUMBAR);
    }

    bool CodeBumbuMerah(string code)
    {
        return (code == KeyWord.GAME1_CABAI)
            || (code == KeyWord.GAME1_BAWANG_MERAH)
            || (code == KeyWord.GAME1_BAWANG_PUTIH)
            || (code == KeyWord.GAME1_LAOS)
            || (code == KeyWord.GAME1_TOMAT_MERAH)
            || (code == KeyWord.GAME1_GULA)
            || (code == KeyWord.GAME1_GARAM);
    }

    void SendBumbuDoneMessege(string item)
    {
        ExitGames.Client.Photon.Hashtable doneMessege = new ExitGames.Client.Photon.Hashtable();
        doneMessege.Add(KeyWord.GAME2_BUMBU_DONE, item);
        PhotonNetwork.LocalPlayer.SetCustomProperties(doneMessege);
    }

    void SetMainManager()
    {
        gameManager = GameObject.FindGameObjectWithTag(KeyWord.GAME_MANAGER)
            .GetComponent<GameManager>();
    }
}

public class BahanBumbuHalus : BahanBahan
{
    public BahanBumbuHalus(BahanBahan bahan)
    {
        Jumlah = 1;
        CodeBahan = bahan.GetCodeBahan();
        SpriteID = bahan.GetSpriteID();
    }

    public bool BumbuKuning()
    {
        return (CodeBahan == KeyWord.GAME1_BAWANG_BOMBAI && Jumlah == 1)
            || (CodeBahan == KeyWord.GAME1_BAWANG_PUTIH && Jumlah == 5)
            || (CodeBahan == KeyWord.GAME1_KEMIRI && Jumlah == 3)
            || (CodeBahan == KeyWord.GAME1_KENCUR && Jumlah == 2)
            || (CodeBahan == KeyWord.GAME1_KUNIR && Jumlah == 2)
            || (CodeBahan == KeyWord.GAME1_LAOS && Jumlah == 1)
            || (CodeBahan == KeyWord.GAME1_SERAI && Jumlah == 1)
            || (CodeBahan == KeyWord.GAME1_KETUMBAR && Jumlah == 1);
    }

    public bool BumbuMerah()
    {
        return (CodeBahan == KeyWord.GAME1_CABAI && Jumlah == 5)
            || (CodeBahan == KeyWord.GAME1_BAWANG_MERAH && Jumlah == 7)
            || (CodeBahan == KeyWord.GAME1_BAWANG_PUTIH && Jumlah == 3)
            || (CodeBahan == KeyWord.GAME1_LAOS && Jumlah == 1)
            || (CodeBahan == KeyWord.GAME1_TOMAT_MERAH && Jumlah == 1)
            || (CodeBahan == KeyWord.GAME1_GULA && Jumlah == 1)
            || (CodeBahan == KeyWord.GAME1_GARAM && Jumlah == 1);
    }
}

