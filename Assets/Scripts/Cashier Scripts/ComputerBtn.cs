using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerBtn : MonoBehaviour
{
    public Image insideColorImg;
    public AudioClip myAudio;
    public ComputerManager compMan;

    AudioSource audioMan;
    Color myCol;

    void Start(){
        audioMan = Camera.main.GetComponent<AudioSource>();
    }

    public void PressButton(bool isPressedBtn)
    {
        if (isPressedBtn && compMan.isPlayingNotes) return;
        audioMan.PlayOneShot(myAudio);
        if (isPressedBtn) compMan.SaveNote(this.gameObject.name);
        myCol = insideColorImg.color;
        StartCoroutine(colorFadingAnimation());
    }

    IEnumerator colorFadingAnimation()
    {
        float alphaCol = myCol.a;
        myCol.a = 1;
        insideColorImg.color = myCol;

        while (insideColorImg.color.a > alphaCol)
        {
            myCol.a -= Time.deltaTime * .5f;
            insideColorImg.color = myCol;
            yield return new WaitForEndOfFrame();
        }

    }

}
