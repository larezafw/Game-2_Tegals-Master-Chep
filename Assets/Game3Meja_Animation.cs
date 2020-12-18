using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game3Meja_Animation : MonoBehaviour
{
    public Game3Meja meja;
    Animator anim;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        SetToDefaultAnimation();
    }

    public void StartAnimationDisplay(string resultName)
    {
        if (resultName == KeyWord.GAME3_HASIL_KUAH) anim.SetBool(KeyWord.GAME3_ANIMASI_KUAH_STEP0, true);
        else if (resultName == KeyWord.GAME3_KUPAT_GLABED) anim.SetBool(KeyWord.GAME3_ANIMASI_KUPAT_STEP0, true);
        else Debug.LogError("Result name not found!");
    }
    
    public void UpdateAnimationDisplay()
    {
        if (meja.OnStep2Kuah()) anim.SetBool(KeyWord.GAME3_ANIMASI_KUAH_STEP1, true);
        else if (meja.OnStep3Kuah()) anim.SetBool(KeyWord.GAME3_ANIMASI_KUAH_STEP2, true);
        else if (meja.OnStep8Kuah()) anim.SetBool(KeyWord.GAME3_ANIMASI_KUAH_STEP7, true);
        else if (meja.OnStep2Kupat()) anim.SetBool(KeyWord.GAME3_ANIMASI_KUPAT_STEP1, true);
        else if (meja.OnStep4Kupat()) anim.SetBool(KeyWord.GAME3_ANIMASI_KUPAT_STEP3, true);
        else Debug.Log("Update Animasion Not Identified!");
    }

    public void PlaySliderAnimation()
    {
        if (meja.OnStep3Kuah()) anim.SetBool(KeyWord.GAME3_ANIMASI_KUAH_STEP3, true);
        else if (meja.OnStep8Kuah()) anim.SetBool(KeyWord.GAME3_ANIMASI_KUAH_STEP8, true);
        else if (meja.OnStep4Kupat()) anim.SetBool(KeyWord.GAME3_ANIMASI_KUPAT_STEP4, true);
        else if (meja.OnStep5Kupat()) anim.SetBool(KeyWord.GAME3_ANIMASI_KUPAT_STEP5, true);
        else Debug.Log("Slider Animas");
    }

    public void AnimationSliderResult()
    {
        anim.SetBool(KeyWord.GAME3_ANIMASI_KUAH_STEP3, false);
        anim.SetBool(KeyWord.GAME3_ANIMASI_KUAH_STEP8, false);
        anim.SetBool(KeyWord.GAME3_ANIMASI_KUPAT_STEP5, false);
        meja.EventStepComplete();
    }

    public void SetToDefaultAnimation()
    {
        anim.SetBool(KeyWord.GAME3_ANIMASI_KUAH_STEP0, false);
        anim.SetBool(KeyWord.GAME3_ANIMASI_KUAH_STEP1, false);
        anim.SetBool(KeyWord.GAME3_ANIMASI_KUAH_STEP2, false);
        anim.SetBool(KeyWord.GAME3_ANIMASI_KUAH_STEP3, false);
        anim.SetBool(KeyWord.GAME3_ANIMASI_KUAH_STEP7, false);
        anim.SetBool(KeyWord.GAME3_ANIMASI_KUAH_STEP8, false);

        anim.SetBool(KeyWord.GAME3_ANIMASI_KUPAT_STEP0, false);
        anim.SetBool(KeyWord.GAME3_ANIMASI_KUPAT_STEP1, false);
        anim.SetBool(KeyWord.GAME3_ANIMASI_KUPAT_STEP3, false);
        anim.SetBool(KeyWord.GAME3_ANIMASI_KUPAT_STEP4, false);
        anim.SetBool(KeyWord.GAME3_ANIMASI_KUPAT_STEP5, false);
    }

    public bool IsAnimating()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(KeyWord.ANIMASI_CLIP_KUAH_3B)
            || anim.GetCurrentAnimatorStateInfo(0).IsName(KeyWord.ANIMASI_CLIP_KUAH_4B)
            || anim.GetCurrentAnimatorStateInfo(0).IsName(KeyWord.ANIMASI_CLIP_KUPAT_3B);
    }
}
