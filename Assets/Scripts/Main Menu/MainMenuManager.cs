using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{

    public myButton MultiplayerBtn,SoloBtn,MusicBtn,SFXBtn;

    public Animator myAnim;

    public static bool sfxEnabled = false, musicEnabled = false;
    int indexOfTimline = 0;

    public void Play()
    {
        myAnim.Play("PlayClicked");
        
    }

    public void MultiplaeryBtn()
    {
        if (indexOfTimline == 0)
        {
            MultiplayerBtn.PlayButtonAnimation();
            SoloBtn.PlayButtonAnimation();
            indexOfTimline++;
        }
    }

    public void ReturnBtn()
    {
        switch (indexOfTimline)
        {
            case 0: myAnim.Play("Return0"); break;
            case 1: 
                MultiplayerBtn.ReturnBtn();
                SoloBtn.ReturnBtn();
                indexOfTimline--;break;
            default: Debug.Log("State not registred"); break;
        }
    }
    public void SFX()
    {
        if (sfxEnabled)
        {
            SFXBtn.ReturnBtn();
            sfxEnabled = false;
        }
        else
        {
            SFXBtn.PlayButtonAnimation();
            sfxEnabled = true;
        }
    }

    public void Music()
    {
        if (musicEnabled)
        {
            MusicBtn.ReturnBtn();
            musicEnabled = false;
        }
        else
        {
            MusicBtn.PlayButtonAnimation();
            musicEnabled = true;
        }
    }

    public void OptionsBtn()
    {
        myAnim.Play("Setting");
    }

    public void OptionsClose()
    {
        myAnim.Play("SettingsClose");
    }

}
