using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatinText : MonoBehaviour
{ 
    TextMeshProUGUI text;
    bool startFloating;

    private void Update()
    {
        if (text.alpha > 0f && startFloating) 
        {
            transform.localPosition += new Vector3 (0f, 150 * Time.deltaTime,0f);
            text.alpha -= 0.5f * Time.deltaTime;
        }
        else if(startFloating)
        {
            Destroy(gameObject);
        }
    }
    
    public void Setup(Vector2 position, Color color, string caption)
    {
        transform.localPosition = position;
        text = GetComponent<TextMeshProUGUI>();
        text.SetText(caption);
        text.color = color;
        text.alpha = 1f;
        startFloating = true;

        if (color == Color.green) AudioManager.audioManager.SoundOn(MusikName.SuccessDrop);
        else AudioManager.audioManager.SoundOn(MusikName.FailDrop);
    }
}
