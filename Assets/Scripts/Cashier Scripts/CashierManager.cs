using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CashierManager : NetworkBehaviour
{
    public PauseCanvasManager pauseCanvMan;

    public CardManager cardMan;
    public ClientsManager clientMan;
    public ComputerManager compMan;
    public PatternManager pattMan;

    public GameObject recipe;

    public GameObject cake,cakePart;
    public Cake cakeScript;
    public Transform cakeSpawnPos,recipeSpawnPos,cakeTableLocation;
    public Color chocolateCol, bananaCol, appleCol;

    public Animator decorationAnimator,canvasAnimator;

    public Text moneyValueTxt,addedMoneyTxt;

    public GameNetworkManager network;

    public int flavorLength = 3;
    public string cakeCode;//exp : 132102000 - 1221231

    /*cakeCode[0] = cake shape
     *cakeCode[1] = xLength parts
     *cakeCode[2] = yLength parts
     * ...
     */

    public static int level = 1;

    GameObject tempCake;

    int moneyValue = 1500;
    int xPartsLength, yPartsLength;

    bool canComputerStuff = false;

    //Bot Stuff
    bool isGenerated = false;
    float timerToWaitForNextRequirementMenu = 20;
    //End Bot Stuff

    void Start()
    {
        moneyValueTxt.text = moneyValue.ToString();
    }

    void Update()
    {
        if (!isGenerated) StartCoroutine(PlayBot());
    }

    IEnumerator PlayBot()
    {
        isGenerated = true;
        GenerateClientAndCake();
        yield return new WaitForSeconds(timerToWaitForNextRequirementMenu);
        isGenerated = false;
    }

#region public_methods

    public void HideShowRecipe()
    {
        canvasAnimator.SetBool("recipePanelIsShown", !canvasAnimator.GetBool("recipePanelIsShown"));
    }

    public void GenerateCakeOnTable(GameObject cake,string CakeCode,Recipe recipeToDestory){
     GameObject tempCakeOnTable = Instantiate(cake,Vector2.one,Quaternion.identity,cakeTableLocation);
     tempCakeOnTable.transform.localScale *= 1.7f;


        Cake tempCakeOnTableScript =tempCakeOnTable.GetComponent<Cake>();

        foreach (Client c in clientMan.clientsList)
        {
            if (c.getCakeCode().Equals(cakeCode))
            {
                tempCakeOnTable.GetComponent<CakeTransmission>().GiveTransmissionAuthority(c, this, recipeToDestory);
                return;
            }
        }
    }

    public void GenerateCake()
    {
        decodeCakeCode();
        GenerateCakeAndCakeParts();
        GenerateCakeDownParts();
        ColorCake();
    }

    public void GenerateRandomCakeCode()
    {
        cakeCode = "122";
        for (int i = 0; i < 4; i++)
        {
            cakeCode += ((int)Random.Range(0, 3)).ToString();
        }
    }

    public void GenerateClientAndCake()
    {
        GenerateRandomCakeCode();
        GenerateCake();
        clientMan.GenerateNewClient(cakeScript.gameObject, cakeCode);
    }

    public void GeneratePattern()
    {
        pattMan.gameObject.SetActive(true);
        pattMan.StartShowingGeneratedCode();
    }

    public void GenerateComputerScreen()
    {
        decorationAnimator.SetBool("isFlashing", true);
        canComputerStuff = true;
    }

    public void ComputerBtn()
    {
        if (!canComputerStuff) return;
        decorationAnimator.SetBool("isFlashing", false);
        compMan.gameObject.SetActive(true);
        compMan.PlayGeneratedNotes();
    }

    public void GenerateCakeRecipe()
    {
        GameObject tempRecipe = Instantiate(recipe,Vector2.one,Quaternion.identity,recipeSpawnPos);
        Recipe tempRecipeSciprt = tempRecipe.GetComponent<Recipe>();

        tempRecipeSciprt.GenerateCake(tempCake,cakeCode,this);

    }


    public void IncreaseMoneyValue(int valueToAdd)
    {
        canvasAnimator.Play("MoneyIncrease");
        addedMoneyTxt.text = "+" + (valueToAdd * level).ToString();
        moneyValue += (valueToAdd * level);
        moneyValueTxt.text = moneyValue.ToString();

    }


    public void ProceedToDoughRole()
    {
        network.CmdGenerateNewOrder();
    }


#endregion
    void decodeCakeCode()
    {
        xPartsLength = int.Parse(cakeCode[1].ToString());
        yPartsLength = int.Parse(cakeCode[2].ToString());
    }

    void GenerateCakeAndCakeParts()
    {
        tempCake = Instantiate(cake, Vector3.zero, Quaternion.identity, cakeSpawnPos);
        cakeScript = tempCake.GetComponent<Cake>();
        RectTransform tempCakeUpperRect = cakeScript.upperCakeTrans.GetComponent<RectTransform>();

        cakeScript.upperCakeTrans.GetComponent<GridLayoutGroup>().cellSize =
            new Vector2(tempCakeUpperRect.rect.width / xPartsLength, tempCakeUpperRect.rect.height / yPartsLength);

        for (int i = 0; i < xPartsLength * yPartsLength; i++)
        {
            GameObject tempCakePart = Instantiate(cakePart, Vector2.zero, Quaternion.identity, cakeScript.upperCakeTrans);
            tempCakePart.name = i.ToString();
            cakeScript.myCakeParts.Add(tempCakePart.GetComponent<CakePart>());
            tempCakePart.GetComponent<CakePart>().setDecorationRectSize(tempCakeUpperRect);
        }
    }

    void GenerateCakeDownParts()
    {
        RectTransform tempCakeDownRect = cakeScript.downCakeTrans.GetComponent<RectTransform>();

        cakeScript.downCakeTrans.GetComponent<GridLayoutGroup>().cellSize =
            new Vector2(tempCakeDownRect.rect.width / xPartsLength, tempCakeDownRect.rect.height);

        for (int i = 0; i < xPartsLength; i++)
        {
            GameObject tempCakeDownPart = Instantiate(cakePart, Vector2.zero, Quaternion.identity, cakeScript.downCakeTrans);
            tempCakeDownPart.name = i.ToString();
            cakeScript.myDownCakeParts.Add(tempCakeDownPart.GetComponent<CakePart>());
            tempCakeDownPart.GetComponent<CakePart>().setDecorationRectSize(tempCakeDownRect);

        }
    }

    void ColorCake()
    {
        for (int i = 0; i < xPartsLength * yPartsLength; i++)
        {
            cakeScript.myCakeParts[i].ChangeMyColor(getFlavorFromIndex(int.Parse(cakeCode[3+i].ToString())),-1);
        }
        for (int i = 0; i < xPartsLength; i++)
        {
            Color cakeDownColor = getFlavorFromIndex(int.Parse(cakeCode[5 + i].ToString()));
            cakeDownColor.a = 0.85f;
            cakeScript.myDownCakeParts[i].ChangeMyColor(cakeDownColor, -1);
        }
    }

    Color getFlavorFromIndex(int index)
    {
        switch (index)
        {
            case 0: return chocolateCol;
            case 1: return bananaCol; 
            case 2: return appleCol;
            default: return Color.white;
        }

    }

}
