using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Game3Meja : MonoBehaviourPunCallbacks,IDropHandler,IPointerDownHandler
{
    public Slider slider;
    [SerializeField] float sliderAcceleration;
    Image sliderFillImage;
    int sliderClickCounter;

    public Game3Meja_Animation AnimManager;
    public TextMeshProUGUI StepText;
    public Image PropertyImage;
    Property propertyInUse;
    List<string> StepList;
    List<string> doneMessegeList;
    string[] correctStepList;
    Game3IndividuaManager game3Manager;
    GameManager gameManager;
    bool sedangMemasak;
    bool sliderStepEvent;

    private void Start()
    {
        FirstSetup();
    }

    private void Update()
    {
        if (sliderStepEvent)
        {
            PlaySliderValue();
            SliderAdjustment();
        }
    }

    void FirstSetup()
    {
        doneMessegeList = new List<string>();
        game3Manager = GameObject.FindGameObjectWithTag(KeyWord.GAME3_MANAGER)
            .GetComponent<Game3IndividuaManager>();
        gameManager = GameObject.FindGameObjectWithTag(KeyWord.GAME_MANAGER)
            .GetComponent<GameManager>();
        ResetToDefault();
        HideSliderStepEvent();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (AnimManager.IsAnimating()) 
        {
            gameManager.CreateFloatingText(transform.localPosition, Color.red, "Tunggu Animasi!");
            return;
        }

        if (!sedangMemasak)
        {
            if (isProperty(eventData)) 
            {
                Property prop = eventData.pointerDrag.GetComponent<Game3Property>().GetProperty();
                string doneMessege = prop.GetDoneMessege();
                if (!doneMessegeList.Contains(doneMessege))
                {
                    AudioManager.audioManager.SoundOn(MusikName.Button);
                    propertyInUse = prop;
                    correctStepList = propertyInUse.GetCorrectList();
                    StepText.SetText(correctStepList[StepList.Count]);
                    StepText.gameObject.SetActive(true);
                    game3Manager.HidePropterty();
                    AnimManager.StartAnimationDisplay(propertyInUse.GetDoneMessege());

                    sedangMemasak = true;
                }
                else gameManager.CreateFloatingText(transform.localPosition,Color.red,"Yuk Buat Kupat!");
                
            }
            else if (isMaterial(eventData)) gameManager
                    .CreateFloatingText(transform.localPosition,Color.red,"Butuh Property!");
        }
        else
        {
            if (isMaterial(eventData))
            {
                BahanBahan bahan = eventData.pointerDrag.GetComponent<GameMaterials>().GetJenisBahan();
                AddAndCheckStepList(bahan.GetCodeStep());
            }
            else if (isProperty(eventData)) gameManager.CreateFloatingText(transform.localPosition,Color.red,"Properti Siap!");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!sliderStepEvent||AnimManager.IsAnimating()) return;

        if (slider.value >= 0.8f)
        {
            sliderClickCounter -= 1;
            if (sliderClickCounter > 0)
                gameManager.CreateFloatingText(transform.localPosition, Color.yellow, sliderClickCounter + " Lagi!");
            else
            { 
                HideSliderStepEvent();
                AnimManager.PlaySliderAnimation();
            }
        }
        else AddAndCheckStepList("");
    }

    void AddAndCheckStepList(string step)
    {
        StepList.Add(step);
        for (int i = 0; i < StepList.Count; i++)
            if (StepList[i] != correctStepList[i])
            {
                gameManager.CreateFloatingText(transform.localPosition, Color.red, "GAGAL");
                ResetToDefault();
                return;
            }

        if (SomeStepCompleted())
        {
            string doneMessege = propertyInUse.GetDoneMessege();
            Debug.Log("Messege by Game3Meja: " + doneMessege);
            doneMessegeList.Add(doneMessege);
            SendDoneMessege(doneMessege);
            gameManager.CreateFloatingText(transform.localPosition, Color.green, "STEP COMPLETE!");
            ResetToDefault();
            return;
        }
        else if (OnStep3Kuah()) DisplaySliderStepEvent();
        else if (OnStep8Kuah()) DisplaySliderStepEvent();
        else if (OnStep4Kupat()) DisplaySliderStepEvent();
        else if (OnStep5Kupat()) DisplaySliderStepEvent();

        AnimManager.UpdateAnimationDisplay();
        StepText.SetText(correctStepList[StepList.Count]);
        gameManager.CreateFloatingText (transform.localPosition, Color.green, "STEP " + StepList.Count + " Berhasil!");
    }

    public bool SomeStepCompleted()
    {
        return StepList.Count == correctStepList.Length;
    }

    public bool OnStep2Kuah()
    {
        return propertyInUse.GetDoneMessege() == KeyWord.GAME3_HASIL_KUAH
            && StepList.Count == 1;
    }

    public bool OnStep3Kuah()
    {
        return propertyInUse.GetDoneMessege() == KeyWord.GAME3_HASIL_KUAH 
            && StepList.Count == 2;
    }

    public bool OnStep8Kuah()
    {
        return propertyInUse.GetDoneMessege() == KeyWord.GAME3_HASIL_KUAH
          && StepList.Count == 7;
    }

    public bool OnStep2Kupat()
    {
        return propertyInUse.GetDoneMessege() == KeyWord.GAME3_KUPAT_GLABED
            && StepList.Count == 1;
    }

    public bool OnStep4Kupat()
    {
        return propertyInUse.GetDoneMessege() == KeyWord.GAME3_KUPAT_GLABED
            && StepList.Count == 3;
    }

    public bool OnStep5Kupat()
    {
        return propertyInUse.GetDoneMessege() == KeyWord.GAME3_KUPAT_GLABED
            && StepList.Count == 4;
    }

    public void EventStepComplete()
    {
        AddAndCheckStepList(correctStepList[StepList.Count]);
    }

    void ResetToDefault()
    {
        sedangMemasak = false;
        StepList = new List<string>();
        game3Manager.DisplayProperty();
        StepText.gameObject.SetActive(false);

        AnimManager.SetToDefaultAnimation();
        HideSliderStepEvent();
    }

    bool isProperty(PointerEventData item)
    {
        return item.pointerDrag.GetComponent<Game3Property>() != null;
    }

    bool isMaterial(PointerEventData item)
    {
        return item.pointerDrag.GetComponent<GameMaterials>() != null;
    }

    void SendDoneMessege(string messege)
    {
        ExitGames.Client.Photon.Hashtable doneMessege = new ExitGames.Client.Photon.Hashtable();
        doneMessege.Add(messege, messege);
        PhotonNetwork.LocalPlayer.SetCustomProperties(doneMessege);
    }

    #region SLIDER CONFIGURATION
    public void DisplaySliderStepEvent()
    {
        if (OnStep3Kuah()) sliderAcceleration = StepList.Count / 2;
        else if (OnStep8Kuah()) sliderAcceleration = StepList.Count / 3;
        else sliderAcceleration = StepList.Count;
        if (sliderAcceleration > 3.5) sliderAcceleration = 3.5f;

        if (OnStep5Kupat()) sliderClickCounter = 1;
        else sliderClickCounter = 3;

        sliderFillImage = slider.fillRect.GetComponent<Image>();
        slider.value = 0.1f;
        slider.gameObject.SetActive(true);
        sliderStepEvent = true;
    }

    void HideSliderStepEvent()
    {
        slider.gameObject.SetActive(false);
        sliderStepEvent = false;
    }

    void PlaySliderValue()
    {
        slider.value += sliderAcceleration * 1.3f * Time.deltaTime;
    }

    public void SliderAdjustment()
    {
        float currentValue = slider.normalizedValue;

        if (currentValue < 0.85f) sliderFillImage.color = Color.red;
        else if (currentValue >= 0.85f) sliderFillImage.color = Color.green;

        if (currentValue >= 1f && !MinusAcceleration()) 
        {
            sliderAcceleration *= -1;
        }
        else if (currentValue <= 0f && MinusAcceleration())
        {
            sliderAcceleration *= -1;
        }
    }

    bool MinusAcceleration()
    {
        return sliderAcceleration <= 0f;
    }
    #endregion
}
