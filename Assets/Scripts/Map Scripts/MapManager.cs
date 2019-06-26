using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{

    public GameObject recipeGameObject;

    public string[] countryNames;
    //List Of Country that sells these fruits
    public string[] Banana;
    public string[] Apple;

    //End Of List  Here

    public Sprite[] flags;
    public Sprite[] fruits;
    public Animator myAnim;

    public Transform map, mapNames, recipeFieldContainer;

    public Text moneyValueTxt;

    public CountryInfoPanel countryInfoPanelScript;
    public Dictionary<string, CountryBtn> countryDic = new Dictionary<string, CountryBtn>();
    public Dictionary<string, int> stock = new Dictionary<string, int>();

    public static int orderNum = 1;
    public static int moneyValue = 1000;
    
    Dictionary<string, Sprite> fruitDic = new Dictionary<string, Sprite>();

    List<Image> targetImgsEnabled = new List<Image>();
    List<Recipe> recipeList = new List<Recipe>();

    StockTxtManager stockTxtMan;

    bool isCollapsed = false;

    void Start(){
        SetMoneyValueTxt();
        stockTxtMan = GetComponent<StockTxtManager>();
        InitVars();
    }

    void Update()
    {
        if (map.localScale.x >= 4) mapNames.gameObject.SetActive(true);
        else mapNames.gameObject.SetActive(false);
    }

    void InitVars()
    {
        fruitDic.Add("Banana", fruits[0]);
        fruitDic.Add("Apple", fruits[1]);

        stock.Add("Banana", 50);
        UpdateStockTxt("Banana");

        stock.Add("Apple", 50);
        UpdateStockTxt("Apple");

    }

#region public_Methods
    public void SelectCountry(string countryName)
    {
        int countryNameIndex = 0;
        myAnim.Play("CountryPanelScale",-1,0f);
        while (countryNames[countryNameIndex] != countryName)
        {
            countryNameIndex++;
        }

        countryInfoPanelScript.SetCountryNameAndFlag(flags[countryNameIndex], countryNames[countryNameIndex],
            countryDic[countryName].transform);

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
                    EnableFruitLocationImgs(item);
                }break;
            case "Apple": foreach (string item in Apple)
                {
                    EnableFruitLocationImgs(item);
                } break;

            default: Debug.Log("Item NOT Exist"); break;

        }


    }

    public void ShowOrCollapseRecipeField()
    {
        if (!isCollapsed)
        {
            isCollapsed = true;
            myAnim.Play("ShowRecipeField", -1, 0);
        }
        else
        {
            isCollapsed = false;
            myAnim.Play("CollapseRecipeField", -1, 0);
        }
    }

    public void ExitBtn()
    {
        countryInfoPanelScript.DeselectCountry();
        myAnim.Play("CountryPanelShrink");
    }

    public void GenerateRecipe()
    {
        Vector3 randomRotation = new Vector3(0, 0, Random.Range(-2f, 2f));
        GameObject newRecipe = Instantiate(recipeGameObject, recipeFieldContainer.position, Quaternion.Euler(randomRotation), recipeFieldContainer);
        Recipe newDoughRecipe = newRecipe.GetComponent<Recipe>();

        newDoughRecipe.setOrderInfo(orderNum, 0);
        newDoughRecipe.setRequirment("Banana", fruits[0], Random.Range(3, 10));
        newDoughRecipe.setRequirment("Apple", fruits[1], Random.Range(1, 5) * 25);

        recipeList.Add(newDoughRecipe);

        orderNum++; 
    }

    public void SetMoneyValueTxt()
    {
        moneyValueTxt.text = moneyValue.ToString(); 
    }

    public void UpdateStockTxt(string fruitName)
    {
        stockTxtMan.changeStocktxt(fruitName, stock[fruitName]);
    }

#endregion

    //Here Where The Magic Happens
    void ShowCountryOffers(string countryName)
    {

        switch (countryName)
        {
            case "United States": countryInfoPanelScript.GenerateOffer(this, "Banana", fruitDic["Banana"], 100, 500); break;
            case "Frensh": countryInfoPanelScript.GenerateOffer(this, "Apple", fruitDic["Apple"], 75, 200); break;
            case "Brazil": countryInfoPanelScript.GenerateOffer(this, "Banana", fruitDic["Banana"], 100, 500); break;     
        }

    }

    void EnableFruitLocationImgs(string item)
    {
        countryDic[item].EnableImgTarget();
        targetImgsEnabled.Add(countryDic[item].targetImg);
    }
    
}
