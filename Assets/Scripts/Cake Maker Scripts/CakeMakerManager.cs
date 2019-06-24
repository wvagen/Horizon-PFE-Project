using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CakeMakerManager : MonoBehaviour
{

    public GameObject cake,cakePart,xyButtons;
    public Transform rightHand, leftHand;
    public Transform cakePartsLocation, xButtonsLocation, yButtonsLocation,cakePreviewLocation;
    public Color bananaCol, appleCol, chocolatCol;
    public Animator filterAnim;

    public int xAxeLength = 2, yAxeLength = 2;
    public short level = 1;
    public short handAnimationSpeed = 50;

    int[,] blockedBtns; //1 value means blocked ... else not blocked
    string[,] cakePartTaste;
    string[] fruitNames = { "banana", "strawberry", "chocolat" };
    short cakeFilter = -1;//0 : star filter ....


    Cake cakeScript;
    List<GameObject> clickedBtnsGameObject = new List<GameObject>();
    List<Cake> cakePreviewsList = new List<Cake>();

    Vector2 initRightHandPos, initLeftHandPos;
    const float southEdgeCakeYValue = -3.13f;
    const float eastEdgeCakeXValue = 3;
    void Start()
    {
        blockedBtns = new int[Mathf.Max(xAxeLength, yAxeLength), Mathf.Max(xAxeLength, yAxeLength)];
        cakePartTaste = new string[Mathf.Max(xAxeLength, yAxeLength), Mathf.Max(xAxeLength, yAxeLength)];

        initRightHandPos = rightHand.position;
        initLeftHandPos = leftHand.position;
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
                    cakeScript.ChangePartColor(i, j, xAxeLength, fruitTasteToColor(fruitName),cakeFilter);
                }
            }
        }
    }

    public void GenerateCakePreview()
    {
        Cake newCakeScript = new Cake();
        cakePreviewsList.Add(newCakeScript);
        GeneerateCakeAndPartsForListAndPreview();
        foreach (CakePart i in cakePreviewsList[cakePreviewsList.Count - 1].myCakeParts)
        {
            i.ChangeMyColor(fruitTasteToColor(fruitNames[Random.Range(0, level + 1)]), -1);
            i.ChangeMyColor(fruitTasteToColor(fruitNames[Random.Range(0, level + 1)]), cakeFilter);
        }
    }

    public void Filter(int filterIndex)
    {
        
        if (filterIndex == cakeFilter)
        {
            switch (filterIndex)
            {
                case 0: filterAnim.Play("FilterStarHide"); cakeFilter = -1; break;
            }
        }
        else
        {
            switch (filterIndex)
            {
                case 0: filterAnim.Play("FilterStarShow"); cakeFilter = (short)filterIndex; break;
            }
        }

    }

    public void returnHandsToInitPosition()
    {
        StartCoroutine(returnHandsToInitPositionIEnumerator());
    }
    public void MoveHandToTargetPosition(bool isRightHand)
    {
        StartCoroutine(MoveHandToTargetPositionIEnumerator(isRightHand));
    }

    public void MoveHandToButtonPosition(bool isRightHand,Vector2 buttonLocation)
    {
        //hiding field
        StartCoroutine(MoveHandToButtonPositionIEnumerator(isRightHand, buttonLocation));
    }

    #endregion


    IEnumerator returnHandsToInitPositionIEnumerator()
    {
        while ((Vector2.Distance(rightHand.position,initRightHandPos) > 0.1f) || Vector2.Distance(leftHand.position,initLeftHandPos) > 0.1f) {
           rightHand.position = Vector2.MoveTowards(rightHand.position,initRightHandPos,handAnimationSpeed*Time.deltaTime);
           leftHand.position = Vector2.MoveTowards(leftHand.position,initLeftHandPos,handAnimationSpeed*Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator MoveHandToTargetPositionIEnumerator(bool isRightHand)
    {
         Vector2 targetLocation ;
         Transform targetHand;

        if (isRightHand){
           targetLocation = new Vector2(rightHand.position.x,southEdgeCakeYValue);
           targetHand = rightHand;
        }else{
           targetLocation = new Vector2(eastEdgeCakeXValue, leftHand.position.y);
           targetHand = leftHand;
        }
            
           while ((Vector2.Distance(targetHand.position , targetLocation) > 0.1f))
           {
               targetHand.position = Vector2.MoveTowards(targetHand.position, targetLocation, handAnimationSpeed / 2 * Time.deltaTime);
               yield return new WaitForEndOfFrame();
           }
        returnHandsToInitPosition();
    }

    IEnumerator MoveHandToButtonPositionIEnumerator(bool isRightHand,Vector2 buttonLocation)
    {
        Vector2 initPos;
        Transform targetHand;
        if (isRightHand) {
            initPos = new Vector2(buttonLocation.x, southEdgeCakeYValue);
            targetHand = rightHand;
        }
        else {
            initPos = new Vector2(eastEdgeCakeXValue, buttonLocation.y);
            targetHand = leftHand;
        }

        for (int i = 0; i < 2; i++)
        {
            while (Vector2.Distance(initPos, targetHand.position) > 0.1f)
            {
                targetHand.position = Vector2.MoveTowards(targetHand.position, initPos, handAnimationSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();
            initPos = buttonLocation;
        }
        returnHandsToInitPosition();
    }

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
            cakeScript.myCakeParts.Add(tempCakePart.GetComponent<CakePart>());
            tempCakePart.GetComponent<CakePart>().setDecorationRectSize(tempCakeRect);
        }
    }

    
    void GeneerateCakeAndPartsForListAndPreview()
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
            cakePreviewsList[cakePreviewsList.Count - 1].myCakeParts.Add(tempCakePart.GetComponent<CakePart>());
            tempCakePart.GetComponent<CakePart>().setDecorationRectSize(tempCakeRect);
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
            case "strawberry": return appleCol;
            case "chocolat": return chocolatCol;
            default: return Color.black;
        }

    }


}
