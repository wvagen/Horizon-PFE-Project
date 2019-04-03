using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{

    public string[] countryNames;

    //List Of Country that sells these fruits
    public string[] Banana;

    //End Of List  Here

    public Sprite[] flags;
    public Sprite[] fruits;
    public Animator myAnim;

    public CountryInfoPanel countryInfoPanelScript;
    public Dictionary<string, CountryBtn> countryDic = new Dictionary<string, CountryBtn>();
    
    Dictionary<string, Sprite> fruitDic = new Dictionary<string, Sprite>();

    List<Image> targetImgsEnabled = new List<Image>();


    void Start(){

        fruitDic.Add("Banana", fruits[0]);
        //fruitDic.Add("Apple", fruits[1]);

    }
    public void SelectCountry(string countryName)
    {
        int countryNameIndex = 0;
        myAnim.Play("CountryPanelScale",-1,0f);
        while (countryNames[countryNameIndex] != countryName)
        {
            countryNameIndex++;
        }

        countryInfoPanelScript.SetCountryNameAndFlag(flags[countryNameIndex], countryNames[countryNameIndex],
            EventSystem.current.currentSelectedGameObject.transform);

        ShowCountryOffers(countryName);
    }

    public void ShowFruitsLocation(string fruitName)
    {

        foreach (Image i in targetImgsEnabled) i.enabled = false;
        targetImgsEnabled = new List<Image>();
        switch (fruitName)
        {
            case "Banana": foreach (string item in Banana)
                {
                    Debug.Log(countryDic[item].gameObject.name);
                    countryDic[item].targetImg.enabled = true;
                    targetImgsEnabled.Add(countryDic[item].targetImg);
                }break;

            default: Debug.Log("Item NOT Exist"); break;

        }

    }

    public void ExitBtn()
    {
        countryInfoPanelScript.DeselectCountry();
        myAnim.Play("CountryPanelShrink");
    }

    //Here Where The Magic Happens
    void ShowCountryOffers(string countryName)
    {

        switch (countryName)
        {
            case "United States": countryInfoPanelScript.GenerateBuyQuanityTypePanel(fruitDic["Banana"], 100, 500); break;
                
        }

    }
    
}
