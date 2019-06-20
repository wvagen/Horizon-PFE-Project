using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Prototype.NetworkLobby;

public class MainMenuManager : MonoBehaviour
{

    public myButton MultiplayerBtn,SoloBtn,MusicBtn,SFXBtn;

    //Order { "DS", "MS", "CMS" , "CS"};

    public Transform[] cardsLocation; //0 : west || 1 : middle || 2 : east
    public Transform[] roles; // 0 : DS || 1 : MS || CMS : 2 || CS : 3

    public Animator myAnim;

    public LobbyMainMenu lobby;

    public static bool sfxEnabled = false, musicEnabled = false;

    public int indexOfTimline = 0;

    Vector2 wantedScale, initScale;

    int cardTranslateSpeed = 5;
    
    short indexOfCardSelected = 0;
    bool canTranslateCards = true;

    void Start()
    {
        initScale = new Vector2(1.5f, 1.5f);
        wantedScale = initScale * 1.2f;
    }

    public void Play()
    {
        myAnim.Play("PlayClicked");
        indexOfTimline++;
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
            case 2: myAnim.Play("Return1"); indexOfTimline = 0;  break;
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

    public void MultiplaeryBtn()
    {
        if (indexOfTimline == 1)
        {
            MultiplayerBtn.PlayButtonAnimation();
            SoloBtn.PlayButtonAnimation();
            indexOfTimline = 2;
            MultiplayerBtn.GetComponent<Button>().onClick.AddListener(lobby.OnClickJoin);
        }


    }

    public void PlaySoloClicked()
    {
        if (indexOfTimline == 1)
        {
            myAnim.Play("PlaySoloClicked");
            indexOfTimline = 2;

        }else if (indexOfTimline == 2)
        {
            SoloBtn.GetComponent<Button>().onClick.AddListener(lobby.OnClickHost);
        }
        
    }

    public void RightBtn()
    {
        if (!canTranslateCards) return;
        StartCoroutine(moveToWardsAPoint((indexOfCardSelected == 3) ? roles[0] : roles[indexOfCardSelected + 1], cardsLocation[1],true));
        if (indexOfCardSelected == 0) indexOfCardSelected = 3;
        else indexOfCardSelected--;

       roles[indexOfCardSelected].SetAsLastSibling();
       StartCoroutine(moveToWardsAPoint(roles[indexOfCardSelected], cardsLocation[1],false));
       StartCoroutine(moveToWardsAPoint((indexOfCardSelected == 3) ? roles[0] : roles[indexOfCardSelected +1], cardsLocation[2],true));
       StartCoroutine(moveToWardsAPoint((indexOfCardSelected == 0) ? roles[3] : roles[indexOfCardSelected - 1], cardsLocation[0],true));
    }

    public void LeftBtn()
    {
        if (!canTranslateCards) return;
        StartCoroutine(moveToWardsAPoint((indexOfCardSelected == 0) ? roles[3] : roles[indexOfCardSelected - 1], cardsLocation[1], true));
        if (indexOfCardSelected == 3) indexOfCardSelected = 0;
        else indexOfCardSelected++;

        roles[indexOfCardSelected].SetAsLastSibling();
        StartCoroutine(moveToWardsAPoint(roles[indexOfCardSelected], cardsLocation[1], false));
        StartCoroutine(moveToWardsAPoint((indexOfCardSelected == 0) ? roles[3] : roles[indexOfCardSelected - 1], cardsLocation[0], true));
        StartCoroutine(moveToWardsAPoint((indexOfCardSelected == 3) ? roles[0] : roles[indexOfCardSelected + 1], cardsLocation[2], true));

    }

    IEnumerator moveToWardsAPoint(Transform card, Transform destination,bool canShrink)
    {
        canTranslateCards = false;
        Vector2 destinationScale;

        if (canShrink)
        {
            card.GetComponent<Button>().enabled = false;
            destinationScale = initScale;
        }
        else
        {
            card.GetComponent<Button>().enabled = true;
            destinationScale = wantedScale;
        }

        while (Vector2.Distance(card.position, destination.position) > 1f)
        {
            card.position = Vector2.Lerp(card.position, destination.position, Time.deltaTime * cardTranslateSpeed);
            card.localScale = Vector2.MoveTowards(card.localScale, destinationScale, Time.deltaTime * cardTranslateSpeed);

            yield return new WaitForEndOfFrame();
        }

        card.localScale = destinationScale;
        canTranslateCards = true;

    }

}
