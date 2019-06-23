using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseCanvasManager : MonoBehaviour
{
    public Image progBarFiller,pauseBtnImg,stars,emoji;
    public Text levelValueTxt;

    public Text infoPanelTitle,scoreTxt;

    public Sprite optionIcon;
    public Sprite[] emojies;

    Animator myAnim;

    float satisfactionLevel = 0.5f; 

    int levelValue = 1;
    int xp = 0;
    int scoreAmount = 0;

    void Start()
    {
        setLevelValueTxt();
        setProgBar();
        setScoreValueTxt();

        myAnim = GetComponent<Animator>();
    }

    #region public_Methods

    public void IncreaseXP()
    {
        xp++;
        if (xp >= (levelValue + 1)) levelUp();
        setProgBar();
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

    public void Increase_Decrease_SatisfactionLevel(bool isIncreasing)
    {
        if (isIncreasing) satisfactionLevel += 0.1f;
        else satisfactionLevel -= 0.1f;

        if (satisfactionLevel > 1) satisfactionLevel = 1;
        if (satisfactionLevel < 0) satisfactionLevel = 0;
        UpdateSatisfactionLevelEmojiAndStars();

    }

    #endregion

    void UpdateSatisfactionLevelEmojiAndStars()
    {
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
}
