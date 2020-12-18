
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using Photon.Pun;
using ExitGames.Client.Photon;


public class Game1Bag : MonoBehaviour, IDropHandler
{
    List<BahanBahan> allBahan;
    public GameObject RemainingTextPrefabs;
    GameManager gameManager;
    Game1IndividualManager game1Manager;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag(KeyWord.GAME_MANAGER).GetComponent<GameManager>();
        game1Manager = GameObject.FindGameObjectWithTag(KeyWord.GAME1_MANAGER).GetComponent<Game1IndividualManager>();
        allBahan = new List<BahanBahan> 
        { 
            new BawangBombai(), 
            new BawangPutih(),
            new Kemiri(),
            new Kencur(),
            new Kunir(),
            new Laos(),
            new Serai(),
            new Ketumbar(),
            new Cabai(),
            new BawangMerah(),
            new TomatMerah(),
            new Gula(),
            new Garam(),
            new Tempe(),
            new Air(),
            new Santan(),
            new Ketupat()
        };
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameMaterials bahan = eventData.pointerDrag.GetComponent<GameMaterials>();
        if (bahan != null)
        {
            BahanBahan bahanIdentified = GetMaterialFromList(bahan.GetCodeJenisBahan());
            AddingMaterial(bahanIdentified);
        }
    }

    BahanBahan GetMaterialFromList(string name)
    {
        BahanBahan bahan = null;
        allBahan.ForEach(x =>
        {
            if (x.GetCodeBahan() == name)
                bahan = x;
        });
        return bahan;
    }

    void AddingMaterial(BahanBahan bahan)
    {
        if (!bahan.Tercukupi() && !TelahDirebut(bahan))
        {
            allBahan.Remove(bahan);
            bahan.TambahBahan();
            allBahan.Add(bahan);
            if (bahan.Tercukupi())
            {
                gameManager.CreateFloatingText(transform.localPosition, Color.green, "+1 Point!");
                SendDoneMessege(bahan);
            }
            else
            {
                gameManager.CreateFloatingText(transform.localPosition, Color.yellow, bahan.SisaBahan());
            }
        }
        else gameManager.CreateFloatingText(transform.localPosition,Color.red,"Tercukupi!");
    }

    bool TelahDirebut(BahanBahan bahan)
    {
        return game1Manager.MaterialDoned.Contains(bahan.GetCodeBahan());
    }

    void SendDoneMessege(BahanBahan item)
    {
        Hashtable customVal = new Hashtable(1);
        customVal.Add(KeyWord.GAME1_MATERIAL_DONE,item.GetCodeBahan());
        PhotonNetwork.LocalPlayer.SetCustomProperties(customVal);
    }
}
