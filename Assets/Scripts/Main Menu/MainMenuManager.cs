using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Prototype.NetworkLobby;

public class MainMenuManager : MonoBehaviour
{

    public myButton MultiplayerBtn,SoloBtn,MusicBtn,SFXBtn;

    //Order { "DS", "MS", "CMS" , "CS"};

    public Transform[] cardsLocation; //0 : west || 1 : middle || 2 : east
    public Transform[] roles; // 0 : DS || 1 : MS || CMS : 2 || CS : 3

    public Animator myAnim;

    public GameObject ipAdressInput;

    public Text[] bestScoresReached, bestLevelsReached;

    public LobbyMainMenu lobby;

    public static bool sfxEnabled = false, musicEnabled = false;

    public static  bool isMultiplayerButtonClicked = false,isTutorialModeOn = false;

    public static bool isPlayerConnected = false;
    public static int waitTimerMultiplayer = 300; //the total amount of time since cashier scene

    Vector2 wantedScale, initScale;

    int cardTranslateSpeed = 5;
    
    short indexOfCardSelected = 0;
    short timeLimeIndex = 0;

    bool canTranslateCards = true;

    void Start()
    {
        isPlayerConnected = false;
        initScale = new Vector2(1.5f, 1.5f);
        wantedScale = initScale * 1.2f;
        LoadData();
    }

    void LoadData()
    {
        for (int i = 0; i < bestScoresReached.Length; i++)
        {
            bestScoresReached[i].text = PlayerPrefs.GetInt("bestScore" + (i+1).ToString(), 0).ToString();
            bestLevelsReached[i].text = PlayerPrefs.GetInt("bestLevel" + (i + 1).ToString(), 1).ToString();
        }
    }

   public void initVars()
    {
        isMultiplayerButtonClicked = false;
        isTutorialModeOn = false;
        isPlayerConnected = false;
    }

    public void Play()
    {
        myAnim.Play("PlayClicked");
        timeLimeIndex = 0;
    }

    public void LeaderBoardBtn()
    {
        myAnim.Play("LeaderBoardShow");
    }

    public void LeaderBoardExit()
    {
        myAnim.Play("LeaderBoardHide");
    }

    public void LoadSceneWithTutorialEnabled(string sceneName)
    {
        isTutorialModeOn = true;
        SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(string sceneName)
    {
        isTutorialModeOn = false;
        SceneManager.LoadScene(sceneName);
    }
 

    public void ReturnBtn()
    {
        switch (timeLimeIndex)
        {
            case 0: myAnim.Play("Return0"); break;
            case 1: 
                MultiplayerBtn.ReturnBtn();
                SoloBtn.ReturnBtn();
                isMultiplayerButtonClicked = false;
                ipAdressInput.SetActive(false);
                timeLimeIndex = 0;

                break;
            case 2: myAnim.Play("Return1"); timeLimeIndex = 0; isMultiplayerButtonClicked = false; ipAdressInput.SetActive(false); break;
            case 3:
                lobby.lobbyManager.GoBackButton();
                break;
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

        if (!isMultiplayerButtonClicked)
        {
            ipAdressInput.SetActive(true);
            MultiplayerBtn.PlayButtonAnimation();
            SoloBtn.PlayButtonAnimation();
            timeLimeIndex = 1;
        }
        else
        {
            timeLimeIndex = 3;
            isPlayerConnected = true;
        }
        StartCoroutine(enableMultiplayerBoolean());

    }

    IEnumerator enableMultiplayerBoolean()
    {
        yield return new WaitForEndOfFrame();
        isMultiplayerButtonClicked = true;
    }

    public void PlaySoloClicked()
    {
        if (!isMultiplayerButtonClicked)
        {
            timeLimeIndex = 2;
            myAnim.Play("PlaySoloClicked");
        }
        else
        {
            timeLimeIndex = 3;
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
