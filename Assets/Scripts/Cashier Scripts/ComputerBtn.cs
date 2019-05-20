using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerBtn : MonoBehaviour
{
    public Image insideColorImg;
    public AudioClip myAudio;


    AudioSource audioMan;

    void Start(){
        audioMan = Camera.main.GetComponent<AudioSource>();
    }

    public void PressButton()
    {
        audioMan.PlayOneShot(myAudio);
        StartCoroutine(colorFadingAnimation());
    }

    IEnumerator colorFadingAnimation()
    {
        Color myCol = insideColorImg.color;
        float alphaCol = myCol.a;
        myCol.a = 1;
        insideColorImg.color = myCol;

        while (insideColorImg.color.a > alphaCol)
        {
            myCol.a -= Time.deltaTime * .5f;
            Debug.Log("Hey");
            insideColorImg.color = myCol;
            yield return new WaitForEndOfFrame();
        }

    }

}
