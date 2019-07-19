using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseCanvasManager : MonoBehaviour
{
    public Image progBarFiller,pauseBtnImg,stars,emoji;

    public Image musicImg, sfxImg;

    public GameObject GameOverPanel,HomeBtnGameObject;
    public GameObject bestScoreCrown,bestLevelCrown;

    public AudioSource musicAudioSource;

    public Text levelValueTxt,starsUpTxt;
    public Text infoPanelTitle,scoreTxt;
    public Text finalScoreTxt,finalLevelReachedTxt;
    public Text bestScoreTxt,bestLevelTxt;

    public Sprite optionIcon;
    public Sprite musicOnSprite, musicOffSprite, sfxOnSprite, sfxOffSprite;
    public Sprite[] emojies;

    Animator myAnim;

    float satisfactionLevel = 0.5f; 

    int bestScore,bestLevelReached;
    int levelValue = 1,scoreAmount = 0;

    int xp = 0;
    bool isGameOver = false;
    short activeSceneIndex ; //1 : Dough Scene || 2: Cashier Scene || 3: Cake Maker Scene || 4: Map Scene

    void Start()
    {
        setLevelValueTxt();
        setProgBar();
        setScoreValueTxt();

        activeSceneIndex =(short) SceneManager.GetActiveScene().buildIndex;

        bestScore = PlayerPrefs.GetInt("bestScore" + activeSceneIndex ,0);
        bestLevelReached = PlayerPrefs.GetInt("bestLevel" + activeSceneIndex,1);
        myAnim = GetComponent<Animator>();

        if (MainMenuManager.musicEnabled)
        {
            musicImg.sprite = musicOnSprite;
            musicAudioSource.enabled = true;
        }
        else
        {
            musicImg.sprite = musicOffSprite;
            musicAudioSource.enabled = false;
        }

        if (MainMenuManager.sfxEnabled)
        {
            sfxImg.sprite = sfxOnSprite;
        }
        else
        {
            sfxImg.sprite = sfxOffSprite;
        }


    }

    #region public_Methods


    public void MusicBtn()
    {
        if (MainMenuManager.musicEnabled)
        {
            MainMenuManager.musicEnabled = false;
            musicImg.sprite = musicOffSprite;
            musicAudioSource.enabled = false;
        }
        else
        {
            MainMenuManager.musicEnabled = true;
            musicImg.sprite = musicOnSprite;
            musicAudioSource.enabled = true;
        }
    }

    public void SfxBtn()
    {
        if (MainMenuManager.sfxEnabled)
        {
            MainMenuManager.sfxEnabled = false;
            sfxImg.sprite = sfxOffSprite;
        }
        else
        {
            MainMenuManager.sfxEnabled = true;
            sfxImg.sprite = sfxOnSprite;
        }
    } 



    public bool ScoreIncrement(float timeValue)
    {
        //return true if levelUp
        bool isLevelUp = IncreaseXP();
        int scoreToBeAdded = (levelValue * 10) + (int)(timeValue);
        starsUpTxt.text ="+" + scoreToBeAdded.ToString();
        scoreAmount += scoreToBeAdded;
        Increase_Decrease_SatisfactionLevel(true);
        setScoreValueTxt();
        return isLevelUp;
    }

    public void OptionBtn()
    {
        myAnim.Play("pause");
    }

    public void ResumeBtn()
    {
        myAnim.Play("PauseHide");
    }

    public void InfoPanelShow(string title_infos)
    {

        infoPanelTitle.text = title_infos;

        myAnim.Play("InfoPanelShow");
    }

    public void InfoPanelHide()
    {
        myAnim.Play("InfoPanelHide");
    }

    public void HomeBtn()
    {
        myAnim.Play("ConfirmationShow");
    }

    public void ConfirmationPanelHide()
    {
        myAnim.Play("ConfirmationHide");
    }

    public void ConfirmationPanelYes()
    {
        SceneManager.LoadScene(0);
    }

    public void GameOver(){
        GameOverPanel.SetActive(true);
        bestScoreTxt.text = bestScore.ToString();
        bestLevelTxt.text = bestLevelReached.ToString();
        if (scoreAmount > bestScore)
        {
            PlayerPrefs.SetInt("bestScore" + activeSceneIndex, scoreAmount);
            bestScoreTxt.text = scoreAmount.ToString();
            Debug.Log(bestScoreTxt.text);
        }
        if (levelValue > bestLevelReached)
        {
            PlayerPrefs.SetInt("bestLevel" + activeSceneIndex, levelValue);
            bestLevelTxt.text = levelValue.ToString();
        }

        myAnim.Play("GameOver");
    }

    public void StartScoreCountDown(){
    
    StartCoroutine (GoToValueAnimation(scoreAmount,finalScoreTxt));

    }

    public void StartLevelReachedCountDown(){
    
    StartCoroutine (GoToValueAnimation(levelValue,finalLevelReachedTxt));

    }


    public void Increase_Decrease_SatisfactionLevel(bool isIncreasing)
    {
        if (isIncreasing)
        {
            myAnim.Play("StarsUp");
            satisfactionLevel += 0.1f;
        }
        else
        {
            myAnim.Play("StarsDown");
            satisfactionLevel -= 0.1f;
        }
        if (satisfactionLevel > 1) satisfactionLevel = 1;
        if (satisfactionLevel < 0 && !isGameOver)
        {
            isGameOver = true;
            GameOver();
            satisfactionLevel = 0;
        }
         UpdateSatisfactionLevelEmojiAndStars();

    }

    public void EnableHomeBtn(){
        HomeBtnGameObject.SetActive(true);
    }

    #endregion

    bool IncreaseXP()
    {
        xp++;
        if (xp >= (levelValue + 1))
        {
            levelUp();
            setProgBar();
            return true;
        }
        setProgBar();
        return false;
        
    }


    void UpdateSatisfactionLevelEmojiAndStars()
    {
        if (satisfactionLevel < 0 || satisfactionLevel > 1) return;
        stars.fillAmount = satisfactionLevel;
        if (satisfactionLevel == 1) emoji.sprite = emojies[4];
        else emoji.sprite = emojies[(int)(satisfactionLevel * 10) / 2];

    }

     void setScoreValueTxt()
    {
        scoreTxt.text = scoreAmount.ToString();
    }

    void setLevelValueTxt()
    {
        levelValueTxt.text = levelValue.ToString();
    }

    void setProgBar()
    {
        progBarFiller.fillAmount = ((float)xp / (levelValue + 1));
    }

    void levelUp()
    {
        levelValue++;
        xp = 0;
        setLevelValueTxt();
        setProgBar();
    }

    IEnumerator GoToValueAnimation(int value,Text valueTxt){

        float realTimeValue = 0;
        while ((int)realTimeValue < value){
            realTimeValue += Time.deltaTime * value ;
            valueTxt.text = ((int) realTimeValue).ToString();
            yield return new WaitForEndOfFrame();
        }
        myAnim.SetBool("isScoreAnimationOver",!myAnim.GetBool("isScoreAnimationOver"));
        if (myAnim.GetBool("isScoreAnimationOver")){
            if (scoreAmount > bestScore){
            bestScoreCrown.SetActive (true);
            bestScore = scoreAmount;
            }
        }else{
            
            if (levelValue > bestLevelReached){
            bestLevelCrown.SetActive (true);
            bestLevelReached = levelValue;
            }
        } 



    } 

}
