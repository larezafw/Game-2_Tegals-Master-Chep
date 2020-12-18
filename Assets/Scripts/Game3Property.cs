using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Game3Property : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public Sprite[] GambarProperty;
    [SerializeField] PropertyType TipeProperty;
    [SerializeField] Image image;
    Property property;

    Vector2 startPoint;
    Canvas canvas;
    RectTransform rect;
    CanvasGroup canvasGroup;

    private void OnEnable() 
    { 
        if (canvas != null) SetToDefaultConfiguration();
    }

    private void Start() => FirstSetup();

    void FirstSetup()
    {
        property = IdentiyProperty(TipeProperty);
        canvas = GameObject.FindGameObjectWithTag(KeyWord.MAIN_CANVAS).GetComponent<Canvas>();
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        startPoint = rect.anchoredPosition;
        image.sprite = GambarProperty[property.GetSpriteID()];
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.5f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        rect.anchoredPosition = startPoint;
        canvasGroup.alpha = 1f;
    }

    Property IdentiyProperty(PropertyType item)
    {
        switch (item)
        {
            case PropertyType.Penggorengan: return new Penggorengan();
            case PropertyType.Mangkok: return new Mangkok();
            default: return null;
        }
    }

    void SetToDefaultConfiguration()
    {
        canvasGroup.blocksRaycasts = true;
        rect.anchoredPosition = startPoint;
        canvasGroup.alpha = 1f;
    }

    public Property GetProperty()
    {
        return property;
    }
}

public enum PropertyType
{
    Penggorengan,
    Mangkok
}

public class Property
{
    protected string doneMessege;
    protected int spriteID;
    protected string[] correctStep;

    public int GetSpriteID()
    {
        return spriteID;
    }

    public string[] GetCorrectList()
    {
        return correctStep;
    }

    public string GetDoneMessege()
    {
        return doneMessege;
    }

    public bool BikinKuah()
    {
        return doneMessege == KeyWord.GAME3_HASIL_KUAH;
    }

    public bool BikinKupat()
    {
        return doneMessege == KeyWord.GAME3_KUPAT_GLABED;
    }
}

public class Penggorengan : Property
{
    public Penggorengan()
    {
        doneMessege = KeyWord.GAME3_HASIL_KUAH;
        spriteID = 0;
        correctStep = new string[] {KeyWord.GAME3_KUAH_STEP1,KeyWord.GAME3_KUAH_STEP2,
        KeyWord.GAME3_KUAH_STEP3,KeyWord.GAME3_KUAH_STEP4,KeyWord.GAME3_KUAH_STEP5,
        KeyWord.GAME3_KUAH_STEP6,KeyWord.GAME3_KUAH_STEP7,KeyWord.GAME3_KUAH_STEP8 };
    }
}

public class Mangkok : Property
{
    public Mangkok()
    {
        doneMessege = KeyWord.GAME3_KUPAT_GLABED;
        spriteID = 1;
        correctStep = new string[] {KeyWord.GAME3_KUPAT_GLABED_STEP1,KeyWord.GAME3_KUPAT_GLABED_STEP2,
        KeyWord.GAME3_KUPAT_GLABED_STEP3,KeyWord.GAME3_KUPAT_GLABED_STEP4,KeyWord.GAME3_KUPAT_GLABED_STEP5};
    }
}

