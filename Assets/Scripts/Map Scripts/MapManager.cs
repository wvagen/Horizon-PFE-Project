using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public PauseCanvasManager pauseMan;
    public GameObject recipeGameObject;

    public string[] countryNames;
    //List Of Country that sells these fruits
    public string[] Banana;
    public string[] Strawberry;
    public string[] Chocolat;

    //End Of List  Here

    public Sprite[] flags;
    public Sprite[] fruits;
    public Animator myAnim;

    public Transform map, mapNames, recipeFieldContainer;

    public Text moneyValueTxt,moneyValueAdded;

    public CountryInfoPanel countryInfoPanelScript;
    public Dictionary<string, CountryBtn> countryDic = new Dictionary<string, CountryBtn>();
    public Dictionary<string, int> stock = new Dictionary<string, int>();

    public static int orderNum = 1;
    public static int moneyValue = 10000;
    
    Dictionary<string, Sprite> fruitDic = new Dictionary<string, Sprite>();

    List<Image> targetImgsEnabled = new List<Image>();
    List<Recipe> recipeList = new List<Recipe>();

    StockTxtManager stockTxtMan;

    bool isCollapsed = false;
    bool isMoneyGenerated = false;
    int level = 1;

    //bot stuff
    float patienceTime = 30;
    bool isStockDeacreased = false;
    float durationOfStockDecreasing = .5f;
    float recipeGenerationRate = 10;
    bool isRecipeGenerated = false;
    //bot end stuff

    void Start(){
        SetMoneyValueTxt();
        stockTxtMan = GetComponent<StockTxtManager>();
        InitVars();
    }

    void Update()
    {
        if (map.localScale.x >= 4) mapNames.gameObject.SetActive(true);
        else mapNames.gameObject.SetActive(false);
        if (!isStockDeacreased) StartCoroutine(DecreaseStockQuantity());
        if (!isMoneyGenerated) StartCoroutine(moneyGenerator());
        if (!isRecipeGenerated) StartCoroutine(AutoGenerateRecipe());
    }

    IEnumerator AutoGenerateRecipe()
    {
        isRecipeGenerated = true;
        GenerateRecipe();
        yield return new WaitForSeconds(recipeGenerationRate);
        isRecipeGenerated = false;
    }

    IEnumerator DecreaseStockQuantity()
    {
        isStockDeacreased = true;

        stock["Banana"] -= Random.Range(0, 3) ;
        stock["Strawberry"] -= Random.Range(0, 3);
        stock["Chocolat"] -= Random.Range(0, 3);

        if (stock["Banana"] < 0) stock["Banana"] = 0;
        if (stock["Strawberry"] < 0) stock["Strawberry"] = 0;
        if (stock["Chocolat"] < 0) stock["Chocolat"] = 0;

        UpdateStockTxt("Banana");
        UpdateStockTxt("Strawberry");
        UpdateStockTxt("Chocolat");
        
        myAnim.SetBool("bananaEmpty", stock["Banana"] <= 5);
        myAnim.SetBool("strawberryEmpty", stock["Strawberry"] <= 5);
        myAnim.SetBool("chocolatEmpty", stock["Chocolat"] <= 5);

        yield return new WaitForSeconds(durationOfStockDecreasing);
        isStockDeacreased = false;
    }

    IEnumerator moneyGenerator()
    {
        isMoneyGenerated = true;
        moneyValue += level * 10;
        SetMoneyValueTxt();
        yield return new WaitForSeconds(1);
        isMoneyGenerated = false;

    }

    void InitVars()
    {
        fruitDic.Add("Banana", fruits[0]);
        fruitDic.Add("Strawberry", fruits[1]);
        fruitDic.Add("Chocolat", fruits[2]);

        stock.Add("Banana", 50);
        UpdateStockTxt("Banana");

        stock.Add("Strawberry", 50);
        UpdateStockTxt("Strawberry");


        stock.Add("Chocolat", 50);
        UpdateStockTxt("Chocolat");
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
            case "Strawberry": foreach (string item in Strawberry)
                {
                    EnableFruitLocationImgs(item);
                } break;

            case "Chocolat": foreach (string item in Chocolat)
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

    public bool CheckResourceAvailibilityBeforeDecrease(string fruitName, int quantityToDecrease)
    {
        //true means can Deacrease false cant
        return (stock[fruitName] >= quantityToDecrease);
    }

    public void DecreaseFruitQuantity(string fruitName,int quantityToDecrease)
    {
        stock[fruitName] -= quantityToDecrease;
        UpdateStockTxt(fruitName);
    }
    public void SuccessfulTransaction(float patienceTime)
    {
        if (pauseMan.ScoreIncrement(patienceTime)) LevelUp();
    }
    public void DeleteRecipe(Recipe r)
    {
        recipeList.Remove(r);
        r.Delete();
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
        newDoughRecipe.mapMan = this;
        newDoughRecipe.GetComponent<Button>().enabled = true;
        newDoughRecipe.GetComponent<Button>().onClick.AddListener(newDoughRecipe.GiveFruitRecipe);
        newDoughRecipe.initClientTimer = patienceTime;

        newDoughRecipe.setOrderInfo(orderNum, 0);

        int ranNum = Random.Range(0, 1000);
        if (ranNum <= 400)
        {
            if (ranNum % 3 == 0) newDoughRecipe.setRequirment("Banana", fruits[0], Random.Range(3,10));
            else if (ranNum % 2 == 0) newDoughRecipe.setRequirment("Chocolat", fruits[2], Random.Range(3, 10));
            else newDoughRecipe.setRequirment("Strawberry", fruits[1], Random.Range(3, 10));
        }
        else if (ranNum <= 900)
        {
            if (ranNum % 2 == 0)
            {
                newDoughRecipe.setRequirment("Banana", fruits[0], Random.Range(3, 10));
                newDoughRecipe.setRequirment("Chocolat", fruits[2], Random.Range(3, 10));
            }
            else
            {
                newDoughRecipe.setRequirment("Strawberry", fruits[1], Random.Range(3, 10));
                newDoughRecipe.setRequirment("Chocolat", fruits[2], Random.Range(3, 10));
            }
        }
        else
        {
            newDoughRecipe.setRequirment("Banana", fruits[0], Random.Range(3, 10));
            newDoughRecipe.setRequirment("Strawberry", fruits[1], Random.Range(3, 10));
            newDoughRecipe.setRequirment("Chocolat", fruits[2], Random.Range(3, 10));

        }

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
            //Banana
            case "India": countryInfoPanelScript.GenerateOffer(this, "Banana", fruitDic["Banana"], 150, 1000); break;
            case "China": countryInfoPanelScript.GenerateOffer(this, "Banana", fruitDic["Banana"], 100, 500); break;
            case "Philippines": countryInfoPanelScript.GenerateOffer(this, "Banana", fruitDic["Banana"], 75, 150); break;
            case "Ecuador": countryInfoPanelScript.GenerateOffer(this, "Banana", fruitDic["Banana"], 50, 140); break;
            case "Indonesia": countryInfoPanelScript.GenerateOffer(this, "Banana", fruitDic["Banana"], 40, 50); break;
            case "Brazil": countryInfoPanelScript.GenerateOffer(this, "Banana", fruitDic["Banana"], 10, 10); break;
        }
        switch (countryName)
        {
            //ENd banana

            //Strawberry
            case "Spain": countryInfoPanelScript.GenerateOffer(this, "Strawberry", fruitDic["Strawberry"], 200, 1200); break;
            case "United States": countryInfoPanelScript.GenerateOffer(this, "Strawberry", fruitDic["Strawberry"], 180, 1000); break;
            case "Mexico": countryInfoPanelScript.GenerateOffer(this, "Strawberry", fruitDic["Strawberry"], 80, 800); break;
            case "Netherlands": countryInfoPanelScript.GenerateOffer(this, "Strawberry", fruitDic["Strawberry"], 70, 500); break;
            case "Belgium": countryInfoPanelScript.GenerateOffer(this, "Strawberry", fruitDic["Strawberry"], 50, 140); break;
            case "Egypt": countryInfoPanelScript.GenerateOffer(this, "Strawberry", fruitDic["Strawberry"], 25, 100); break;
        }
        switch (countryName)
        {
            //End Strawberry

            //Chocolat
            case "Germany": countryInfoPanelScript.GenerateOffer(this, "Chocolat", fruitDic["Chocolat"], 200, 1200); break;
            case "Belgium": countryInfoPanelScript.GenerateOffer(this, "Chocolat", fruitDic["Chocolat"], 180, 1000); break;
            case "Italy": countryInfoPanelScript.GenerateOffer(this, "Chocolat", fruitDic["Chocolat"], 80, 800); break;
            case "Netherlands": countryInfoPanelScript.GenerateOffer(this, "Chocolat", fruitDic["Chocolat"], 70, 500); break;
            case "Poland": countryInfoPanelScript.GenerateOffer(this, "Chocolat", fruitDic["Chocolat"], 50, 140); break;
            case "United States": countryInfoPanelScript.GenerateOffer(this, "Chocolat", fruitDic["Chocolat"], 25, 100); break;
            //End Chocolat
        }
    }

    void EnableFruitLocationImgs(string item)
    {
        countryDic[item].EnableImgTarget();
        targetImgsEnabled.Add(countryDic[item].targetImg);
    }

    void LevelUp()
    {
        moneyValue += level * 100;
        moneyValueAdded.text = "+" + (level * 100).ToString();
        myAnim.Play("LevelUp");
        level++;
        SetMoneyValueTxt();
    }
    
}
