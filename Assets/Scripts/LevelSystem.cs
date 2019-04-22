using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSystem : MonoBehaviour
{

    public Image progBarFiller;
    public Text levelValueTxt;

    int levelValue = 1;
    int xp = 0;

    void Start()
    {
        setLevelValueTxt();
        setProgBar();
    }

    #region public_Methods

    public void IncreaseXP()
    {
        xp++;
        if (xp >= (levelValue + 1)) levelUp();
        setProgBar();
    }

    #endregion

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
