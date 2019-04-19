using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CakeMakerManager : MonoBehaviour
{

    public GameObject cake,cakePart,xyButtons;
    public Transform cakePartsLocation, xButtonsLocation, yButtonsLocation,cakePreviewLocation;
    public Color bananaCol, appleCol, strawberryCol, chocolatCol;

    public int xAxeLength = 2, yAxeLength = 2;
    public short level = 1;

    int[,] blockedBtns; //1 value means blocked ... else not blocked
    string[,] cakePartTaste;
    string[] fruitNames = { "banana", "apple", "strawberry", "chocolat" };

    Cake cakeScript;
    List<GameObject> clickedBtnsGameObject = new List<GameObject>();
    List<Cake> cakePreviewsList = new List<Cake>();

    void Start()
    {
        blockedBtns = new int[Mathf.Max(xAxeLength, yAxeLength), Mathf.Max(xAxeLength, yAxeLength)];
        cakePartTaste = new string[Mathf.Max(xAxeLength, yAxeLength), Mathf.Max(xAxeLength, yAxeLength)];
    }

    #region Public_Methods

    public void GenerateCakeAndButtons()
    {
        GeneerateCakeAndParts();
        GenerateButtons();
    }

    public void ButtonCordinateClicked(int x , int y)
    {
        short indexOfBlockness = 0;
        GameObject clickedBtnGO = EventSystem.current.currentSelectedGameObject;
        if (clickedBtnsGameObject.Contains(clickedBtnGO))
        {
            clickedBtnGO.GetComponent<xyButtons>().FillingAnimation(false);
            clickedBtnsGameObject.Remove(clickedBtnGO);
            indexOfBlockness = 0;
        }else{
            clickedBtnGO.GetComponent<xyButtons>().FillingAnimation(true);
            clickedBtnsGameObject.Add (clickedBtnGO);
            indexOfBlockness = 1;
        }

        if (x == 1)
        {
            for (int i = 0; i < xAxeLength; i++)
            {
                Debug.Log("(" + y + "," + i + ")");
                blockedBtns[y, i] = indexOfBlockness;
            }
        }
        else
        {
            for (int i = 0; i < yAxeLength; i++)
            {
                Debug.Log("(" + i + "," + y + ")");
                blockedBtns[i, y] = indexOfBlockness;
            }
        }

    }

    public void ColorBtnTst(string fruitName)
    {

        for (int i = 0; i < Mathf.Min(xAxeLength,yAxeLength); i++)
        {
            for (int j = 0; j < Mathf.Max(xAxeLength, yAxeLength); j++)
            {
                Debug.Log("(" + i + "," + j + ")");
                if (blockedBtns[i, j] != 1)
                {
                    cakePartTaste[i, j] = fruitName;
                    cakeScript.ChangePartColor(i, j, xAxeLength, fruitTasteToColor(fruitName));
                }
            }
        }
    }

    public void GenerateCakePreview()
    {
        Cake newCakeScript = new Cake();
        cakePreviewsList.Add(newCakeScript);
        GeneerateCakeAndPartsForList();
        foreach (Image i in cakePreviewsList[cakePreviewsList.Count - 1].myCakeParts)
        {
            i.color = fruitTasteToColor(fruitNames[Random.Range(0, level+1)]);
        }
    }

    #endregion

    void GeneerateCakeAndParts()
    {
        GameObject tempCake = Instantiate(cake, Vector3.zero, Quaternion.identity, cakePartsLocation);
        cakeScript = tempCake.GetComponent<Cake>();
        RectTransform tempCakeRect = tempCake.GetComponent<RectTransform>();
        tempCakeRect.sizeDelta = cakePartsLocation.GetComponent<RectTransform>().sizeDelta;
        tempCakeRect.anchoredPosition = Vector2.zero;
        tempCakeRect.localScale *= 0.74f;

        tempCake.GetComponent<GridLayoutGroup>().cellSize =
            new Vector2(tempCakeRect.rect.width / xAxeLength, tempCakeRect.rect.height / yAxeLength);

        for (int i = 0; i < xAxeLength * yAxeLength; i++)
        {
            GameObject tempCakePart = Instantiate(cakePart, Vector2.zero, Quaternion.identity, tempCake.transform);
            tempCakePart.name = i.ToString();
            cakeScript.myCakeParts.Add(tempCakePart.GetComponent<Image>());

        }
    }

    void GeneerateCakeAndPartsForList()
    {
        GameObject tempCake = Instantiate(cake, Vector3.zero, Quaternion.identity, cakePreviewLocation);
        cakePreviewsList[cakePreviewsList.Count - 1] = tempCake.GetComponent<Cake>();
        RectTransform tempCakeRect = tempCake.GetComponent<RectTransform>();
        tempCakeRect.sizeDelta = cakePreviewLocation.GetComponent<RectTransform>().sizeDelta;
        tempCakeRect.anchoredPosition = Vector2.zero;
        tempCakeRect.localScale *= 0.74f;

        tempCake.GetComponent<GridLayoutGroup>().cellSize =
            new Vector2(tempCakeRect.rect.width / xAxeLength, tempCakeRect.rect.height / yAxeLength);

        for (int i = 0; i < xAxeLength * yAxeLength; i++)
        {
            GameObject tempCakePart = Instantiate(cakePart, Vector2.zero, Quaternion.identity, tempCake.transform);
            tempCakePart.name = i.ToString();
            cakePreviewsList[cakePreviewsList.Count - 1].myCakeParts.Add(tempCakePart.GetComponent<Image>());
        }
    }

    void GenerateButtons()
    {
        for (int i = 0; i < xAxeLength; i++)
        {
            GameObject tempBtn = Instantiate(xyButtons, Vector2.zero, Quaternion.identity, xButtonsLocation);
            tempBtn.GetComponent<xyButtons>().man = this;
            tempBtn.GetComponent<xyButtons>().isOnXAxis = true;
            tempBtn.GetComponent<xyButtons>().setMyFunction(0, i);
        }
        for (int i = 0; i < yAxeLength; i++)
        {
            GameObject tempBtn = Instantiate(xyButtons, Vector2.zero, Quaternion.identity, yButtonsLocation);
            tempBtn.GetComponent<xyButtons>().man = this;
            tempBtn.GetComponent<xyButtons>().isOnXAxis = false;
            tempBtn.GetComponent<xyButtons>().setMyFunction(1, i);
        }

    }

    Color fruitTasteToColor(string fruitName)
    {
        switch (fruitName)
        {
            case "banana": return bananaCol;
            case "strawberry": return strawberryCol; 
            case "apple": return appleCol;
            case "chocolat": return chocolatCol;
            default: return Color.black;
        }

    }


}
