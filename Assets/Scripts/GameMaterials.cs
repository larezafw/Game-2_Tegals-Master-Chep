using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaterials : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public TextMeshProUGUI NameText;
    public Sprite[] GambarBahan;
    [SerializeField] TipeBahan tipeBahan;
    [SerializeField] bool MovingItem;
    [SerializeField] Image image;
    BahanBahan jenisBahan;

    Vector2 startPoint;
    Canvas canvas;
    RectTransform rect;
    CanvasGroup canvasGroup;

    void Start()
    {
        if (MovingItem) InitialIdentify();
        if (tipeBahan == TipeBahan.hasilKuah) KuahConfiguration(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!MovingItem) return;
        canvasGroup.blocksRaycasts = false;
        startPoint = rect.anchoredPosition;
        canvasGroup.alpha = 0.5f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!MovingItem) return;
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!MovingItem) return;
        canvasGroup.blocksRaycasts = true;
        rect.anchoredPosition = startPoint;
        canvasGroup.alpha = 1f;
    }

    BahanBahan IdentifyItem(TipeBahan item)
    {
        switch (item)
        {
            case TipeBahan.bawangBombai: return new BawangBombai();
            case TipeBahan.bawangPutih: return new BawangPutih();
            case TipeBahan.kemiri: return new Kemiri();
            case TipeBahan.kencur: return new Kencur();
            case TipeBahan.kunir: return  new Kunir();
            case TipeBahan.laos: return new Laos();
            case TipeBahan.serai: return new Serai();
            case TipeBahan.ketumbar: return new Ketumbar();
            case TipeBahan.cabai: return new Cabai();
            case TipeBahan.bawangMerah: return new BawangMerah();
            case TipeBahan.tomatMerah: return new TomatMerah();
            case TipeBahan.gula: return new Gula();
            case TipeBahan.garam: return new Garam();
            case TipeBahan.tempe: return new Tempe();
            case TipeBahan.air: return new Air();
            case TipeBahan.santan: return new Santan();
            case TipeBahan.ketupat: return new Ketupat();
            case TipeBahan.bumbuKuning: return new BumbuKuning();
            case TipeBahan.bumbuMerah: return new BumbuMerah();
            case TipeBahan.hasilKuah: return new HasilKuah();
            default: return null;
        }
    }

    void InitialIdentify()
    {
        jenisBahan = IdentifyItem(tipeBahan);
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GameObject.FindGameObjectWithTag(KeyWord.MAIN_CANVAS).GetComponent<Canvas>();
        rect = gameObject.GetComponent<RectTransform>();
        startPoint = new Vector2();

        DisplayNamaBahan();
    }
    public void LateIdentify(BahanBahan bahan, bool displayName)
    {
        jenisBahan = bahan;
        if (displayName) DisplayNamaBahan();
        else DisplayJumlahBahan();
    }

    void DisplayNamaBahan()
    {
        image.sprite = GambarBahan[jenisBahan.GetSpriteID()];
        NameText.SetText(jenisBahan.GetNamaBahan());
    }

    void DisplayJumlahBahan()
    {
        image.sprite = GambarBahan[jenisBahan.GetSpriteID()];
        NameText.SetText(jenisBahan.GetJumlah()+"x");
    }

    public void KuahConfiguration(bool available)
    {
        if (!available)
        {
            canvasGroup.alpha = 0.35f;
            canvasGroup.blocksRaycasts = false;
            MovingItem = false;
        }
        else
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            MovingItem = true;
        }
    }

    public BahanBahan GetJenisBahan()
    {
        return jenisBahan;
    }

    public string GetCodeJenisBahan()
    {
        return jenisBahan.GetCodeBahan();
    }
}
public enum TipeBahan
{
    bawangBombai,
    bawangPutih,
    kemiri,
    kencur,
    kunir,
    laos,
    serai,
    ketumbar,
    cabai,
    bawangMerah,
    tomatMerah,
    gula,
    garam,
    tempe,
    air,
    santan,
    ketupat,

    bumbuKuning,
    bumbuMerah,
    hasilKuah
}
public class BahanBahan
{
    protected string CodeBahan;
    protected string NamaBahan;
    protected string CodeStep;
    protected int Jumlah;
    protected int MaxJumlah;
    protected int SpriteID;

    public bool Tercukupi()
    {
        return Jumlah >= MaxJumlah;
    }

    public void TambahBahan()
    {
        Jumlah += 1;
    }

    public string SisaBahan()
    {
        return (MaxJumlah - Jumlah) + " lagi!";
    }

    public int GetJumlah()
    {
        return Jumlah;
    }

    public int GetMaxJumlah()
    {
        return MaxJumlah;
    }

    public string GetCodeBahan()
    {
        return CodeBahan;
    }

    public string GetNamaBahan()
    {
        return NamaBahan;
    }
    
    public int GetSpriteID()
    {
        return SpriteID;
    }

    public string GetCodeStep()
    {
        return CodeStep;
    }

    public bool BahanKuah()
    {
        return CodeStep == KeyWord.GAME3_KUAH_STEP1
            || CodeStep == KeyWord.GAME3_KUAH_STEP2
            || CodeStep == KeyWord.GAME3_KUAH_STEP3
            || CodeStep == KeyWord.GAME3_KUAH_STEP4
            || CodeStep == KeyWord.GAME3_KUAH_STEP5
            || CodeStep == KeyWord.GAME3_KUAH_STEP6
            || CodeStep == KeyWord.GAME3_KUAH_STEP7
            || CodeStep == KeyWord.GAME3_KUAH_STEP8;
    }

    public bool BahanKupat()
    {
        return CodeStep == KeyWord.GAME3_KUPAT_GLABED_STEP1
            || CodeStep == KeyWord.GAME3_KUPAT_GLABED_STEP2
            || CodeStep == KeyWord.GAME3_KUPAT_GLABED_STEP3
            || CodeStep == KeyWord.GAME3_KUPAT_GLABED_STEP4
            || CodeStep == KeyWord.GAME3_KUPAT_GLABED_STEP4
            || CodeStep == KeyWord.GAME3_KUPAT_GLABED_STEP5;
    }
}

// KELAS BAHAN
public class BawangBombai : BahanBahan
{
    public BawangBombai()
    {
        
        CodeBahan = KeyWord.GAME1_BAWANG_BOMBAI;
        NamaBahan = "Bawang Bombai";
        Jumlah = 0;
        MaxJumlah = 1;
        SpriteID = 0;
    }
}
public class BawangPutih : BahanBahan
{
    public BawangPutih()
    {
        CodeBahan = KeyWord.GAME1_BAWANG_PUTIH;
        NamaBahan = "Bawang Putih";
        Jumlah = 0;
        MaxJumlah = 8;
        SpriteID = 1;
    }
}
public class Kemiri : BahanBahan
{
    public Kemiri()
    {
        CodeBahan = KeyWord.GAME1_KEMIRI;
        NamaBahan = "Kemiri";
        Jumlah = 0;
        MaxJumlah = 3;
        SpriteID = 2;
    }
}
public class Kencur: BahanBahan
{
    public Kencur()
    {
        CodeBahan = KeyWord.GAME1_KENCUR;
        NamaBahan = "Kencur";
        Jumlah = 0;
        MaxJumlah = 2;
        SpriteID = 3;
    }
}

public class Kunir : BahanBahan
{
    public Kunir()
    {
        CodeBahan = KeyWord.GAME1_KUNIR;
        NamaBahan = "Kunir";
        Jumlah = 0;
        MaxJumlah = 2;
        SpriteID = 4;
    }
}

public class Laos : BahanBahan
{
    public Laos()
    {
        CodeBahan = KeyWord.GAME1_LAOS;
        NamaBahan = "Laos";
        Jumlah = 0;
        MaxJumlah = 2;
        SpriteID = 5;
    }
}
    
public class Serai : BahanBahan
{
    public Serai()
    {
        CodeBahan = KeyWord.GAME1_SERAI;
        NamaBahan = "Serai";
        Jumlah = 0;
        MaxJumlah = 1;
        SpriteID = 6;
    }
}

public class Ketumbar : BahanBahan
{
    public Ketumbar()
    {
        CodeBahan = KeyWord.GAME1_KETUMBAR;
        NamaBahan = "Ketumbar";
        Jumlah = 0;
        MaxJumlah = 1;
        SpriteID = 7;
    }
}

public class Cabai : BahanBahan
{
    public Cabai()
    {
        CodeBahan = KeyWord.GAME1_CABAI;
        NamaBahan = "Cabai";
        Jumlah = 0;
        MaxJumlah = 5;
        SpriteID = 8;
    }
}

public class BawangMerah: BahanBahan
{
    public BawangMerah()
    {
        CodeBahan = KeyWord.GAME1_BAWANG_MERAH;
        NamaBahan = "Bawang Merah";
        Jumlah = 0;
        MaxJumlah = 7;
        SpriteID = 9;
    }
}

public class TomatMerah : BahanBahan
{
    public TomatMerah()
    {
        CodeBahan = KeyWord.GAME1_TOMAT_MERAH;
        NamaBahan = "Tomat Merah";
        CodeStep = KeyWord.GAME3_KUAH_STEP4;
        Jumlah = 0;
        MaxJumlah = 1;
        SpriteID = 10; 
    }
}
public class Gula : BahanBahan
{
    public Gula()
    {
        CodeBahan = KeyWord.GAME1_GULA;
        NamaBahan = "Gula";
        CodeStep = KeyWord.GAME3_KUAH_STEP6;
        Jumlah = 0;
        MaxJumlah = 1;
        SpriteID = 11;
    }
}
public class Garam : BahanBahan
{
    public Garam()
    {
        CodeBahan = KeyWord.GAME1_GARAM;
        NamaBahan = "Garam";
        CodeStep = KeyWord.GAME3_KUAH_STEP5;
        Jumlah = 0;
        MaxJumlah = 1;
        SpriteID = 12;
    }
}

public class Tempe : BahanBahan
{
    public Tempe()
    {
        CodeBahan = KeyWord.GAME1_TEMPTE;
        NamaBahan = "Tempe";
        CodeStep = KeyWord.GAME3_KUPAT_GLABED_STEP2;
        Jumlah = 0;
        MaxJumlah = 1;
        SpriteID = 13;
    }
}

public class Air : BahanBahan
{
    public Air()
    {
        CodeBahan = KeyWord.GAME1_AIR;
        NamaBahan = "Air";
        CodeStep = KeyWord.GAME3_KUAH_STEP1;
        Jumlah = 0;
        MaxJumlah = 1;
        SpriteID = 14;
    }
}
public class Santan : BahanBahan
{
    public Santan()
    {
        CodeBahan = KeyWord.GAME1_SANTAN;
        NamaBahan = "Santan";
        Jumlah = 0;
        MaxJumlah = 1;
        SpriteID = 15;
    }
}
public class Ketupat : BahanBahan
{
    public Ketupat()
    {
        CodeBahan = KeyWord.GAME1_KETUPAT;
        NamaBahan = "Ketupat";
        CodeStep = KeyWord.GAME3_KUPAT_GLABED_STEP1;
        Jumlah = 0;
        MaxJumlah = 1;
        SpriteID = 16;
    }
}

// KELAS BUMBU
public class BumbuKuning : BahanBahan
{
    public BumbuKuning()
    {
        CodeBahan = KeyWord.GAME2_BUMBU_KUNING;
        NamaBahan = "Bumbu Kuning";
        CodeStep = KeyWord.GAME3_KUAH_STEP2;
        Jumlah = 0;
        MaxJumlah = 1;
        SpriteID = 17;
    }
}

public class BumbuMerah : BahanBahan
{
    public BumbuMerah()
    {
        CodeBahan = KeyWord.GAME2_BUMBU_MERAH;
        NamaBahan = "Bumbu Merah";
        CodeStep = KeyWord.GAME3_KUAH_STEP7;
        Jumlah = 0;
        MaxJumlah = 1;
        SpriteID = 18;
    }
}

// HASIL
public class HasilKuah: BahanBahan
{
    public HasilKuah()
    {
        CodeBahan = KeyWord.GAME3_HASIL_KUAH;
        NamaBahan = "Kuah";
        CodeStep = KeyWord.GAME3_KUPAT_GLABED_STEP3;
        Jumlah = 0;
        MaxJumlah = 1;
        SpriteID = 19;
    }
}

public class KupatGlabed : BahanBahan
{
    public KupatGlabed()
    {
        CodeBahan = KeyWord.GAME3_KUPAT_GLABED;
        NamaBahan = "Kupat Glabed";
    }
}
