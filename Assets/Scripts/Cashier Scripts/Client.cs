using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Client : MonoBehaviour
{

    public Sprite[] emojis;
    public Color[] progBarColors;

    public float patienceTime = 60f;
    public Image myEmoji,progrBar;

    float realTime;

    short nextPatienceIndexValue = 0;

    void Start()
    {
        realTime = patienceTime;
        ChangeEmojiAndColor();
    }

    // Update is called once per frame
    void Update()
    {
        if (realTime >= 0) DecreaseTimer();
    }

    void DecreaseTimer()
    {
        realTime -= Time.deltaTime;
        progrBar.fillAmount = realTime / patienceTime;

        if (realTime < ((float)(emojis.Length - nextPatienceIndexValue) / emojis.Length) * patienceTime) 
            ChangeEmojiAndColor();
    }

    void ChangeEmojiAndColor()
    {
        
        myEmoji.sprite = emojis[nextPatienceIndexValue];
        progrBar.color = progBarColors[nextPatienceIndexValue];
        if (nextPatienceIndexValue < emojis.Length - 1) nextPatienceIndexValue++;
    }
    
    #region public_methods



    #endregion

}
