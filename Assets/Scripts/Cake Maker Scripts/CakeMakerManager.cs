using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CakeMakerManager : MonoBehaviour
{
    public PauseCanvasManager pauseCan;
    public GameObject cake,cakePart,xyButtons,cakePreview;
    public Transform rightHand, leftHand;
    public Transform cakePartsLocation, xButtonsLocation, yButtonsLocation,cakePreviewLocation;
    public Color bananaCol, appleCol, chocolatCol;
    public Animator filterAnim,canvasAnim;

    public int xAxeLength = 1, yAxeLength = 1;
    public short level = 1;
    public short handAnimationSpeed = 50;

    public GameNetworkManager network;

    int[,] blockedBtns; //1 value means blocked ... else not blocked
    string[,] cakePartTaste;
    string[] fruitNames = { "banana", "strawberry", "chocolat" };
    short cakeFilter = -1;//0 : star filter ....
    int orderNum = 1;

    Cake cakeScript;
    List<GameObject> clickedBtnsGameObject = new List<GameObject>();
    List<CakePreview> cakePreviewsList = new List<CakePreview>();
    List<GameObject> generatedBtns = new List<GameObject>();
    string myCakeCode,generatedcakeCode ="1220000" ; //exp : 132102000 - 1221231

    /*cakeCode[0] = cake shape
     *cakeCode[1] = xLength parts
     *cakeCode[2] = yLength parts
     * ...
     */

    Vector2 initRightHandPos, initLeftHandPos;

    //Bot Stuff

    float patienceTime = 30;
    float timerToWaitForNextRequirementMenu = 5;
    bool isGenerated = false;

    //End Bot Stuff

    const float southEdgeCakeYValue = -3.13f;
    const float eastEdgeCakeXValue = 3;
    void Start()
    {
        GenerateCakeAndButtons();
        initRightHandPos = rightHand.position;
        initLeftHandPos = leftHand.position;
    }

    void Update() {
        canvasAnim.SetBool("isRecipeEmpty", cakePreviewsList.Count == 0);
        if (!isGenerated && !MainMenuManager.isPlayerConnected) StartCoroutine(PlayBot());
    }

   public  void CheckCodeAndCakePreviewMatch()
    {
        foreach (CakePreview cakePreview in cakePreviewsList)
        {
            if (myCakeCode.Equals(cakePreview.cakeCode) )
            {
                //Increment Score Phase
                if (pauseCan.ScoreIncrement(cakePreview.realPatienceTime)) levelUp();
                GenerateRandomCakeCode();
                DecodeXandYLength(generatedcakeCode);
                GenerateCakeAndButtons();
                cakePreviewsList.Remove(cakePreview);
                cakePreview.DestroyLeMe();
                break;
            }
        }
    }

  public void GenerateRandomCakeCode()
    {
        generatedcakeCode = "1";
        if (level == 1) generatedcakeCode += "11" + Random.Range(0, fruitNames.Length).ToString();
        else if (level <= 3)
        {
            generatedcakeCode += "22";
            for (int i = 0; i < 4; i++)
            {
                generatedcakeCode += Random.Range(0, fruitNames.Length).ToString();
            }
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                generatedcakeCode += Random.Range(0, fruitNames.Length).ToString();
            }
        }
    }

    IEnumerator PlayBot()
    {
        isGenerated = true;
        GenerateRandomCakeCode();
        GenerateCakePreview();
        yield return new WaitForSeconds(timerToWaitForNextRequirementMenu);
        isGenerated = false;
    }

    void levelUp()
    {
        level++;
        if (timerToWaitForNextRequirementMenu > 1f) timerToWaitForNextRequirementMenu -= 0.1f;
        if (patienceTime > 10) patienceTime -= 0.5f;
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
                    char[] ch = myCakeCode.ToCharArray();
                    ch[3 + i * xAxeLength + j] = char.Parse(fruitTasteToIndex(fruitName).ToString()); // index starts at 0!
                    myCakeCode = new string(ch);
                    Debug.Log(myCakeCode);
                }
            }
        }
    }

   public void GenerateCakePreview()
    {
        DecodeXandYLength(generatedcakeCode);
        GeneerateCakeAndPartsForListAndPreview();
        /*foreach (CakePart i in cakePreviewsList[cakePreviewsList.Count - 1].myCakeParts)
        {
            i.ChangeMyColor(fruitTasteToColor(fruitNames[Random.Range(0, level + 1)]), -1);
            i.ChangeMyColor(fruitTasteToColor(fruitNames[Random.Range(0, level + 1)]), cakeFilter);
        }*/
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

    public void ScalePhone()
    {
        canvasAnim.Play("PhoneScale");
    }
    public void ShrinkPhone()
    {
        canvasAnim.Play("PhoneShrink");
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
        if (cakeScript != null) Destroy(cakeScript.gameObject);
        GameObject tempCake = Instantiate(cake, Vector3.zero, Quaternion.identity, cakePartsLocation);
        cakeScript = tempCake.GetComponent<Cake>();
        myCakeCode = "";
        myCakeCode = "1" + xAxeLength.ToString() + yAxeLength.ToString();
        RectTransform tempCakeRect = tempCake.GetComponent<RectTransform>();
        SetRectToMiddle(tempCakeRect);
        tempCakeRect.localScale *= 0.74f;

        tempCake.GetComponent<GridLayoutGroup>().cellSize =
            new Vector2(tempCakeRect.rect.width / xAxeLength, tempCakeRect.rect.height / yAxeLength);

        for (int i = 0; i < xAxeLength * yAxeLength; i++)
        {
            GameObject tempCakePart = Instantiate(cakePart, Vector2.zero, Quaternion.identity, tempCake.transform);
            tempCakePart.name = i.ToString();
            myCakeCode += "0";
            cakeScript.myCakeParts.Add(tempCakePart.GetComponent<CakePart>());
            tempCakePart.GetComponent<CakePart>().setDecorationRectSize(tempCakeRect);
        }
    }

    
    void GeneerateCakeAndPartsForListAndPreview()
    {
        GameObject newCakePreviw = Instantiate(cakePreview, Vector2.zero,Quaternion.identity, cakePreviewLocation);
        CakePreview cakePreviewScript = newCakePreviw.GetComponent<CakePreview>();
        GameObject tempCake = Instantiate(cake, Vector3.zero, Quaternion.identity, cakePreviewScript.cakePreviewLocation);

        cakePreviewScript.setVars(orderNum, patienceTime, generatedcakeCode, this);
        orderNum++;
        cakePreviewsList.Add(cakePreviewScript);
        RectTransform tempCakeRect = tempCake.GetComponent<RectTransform>();
        SetRectToMiddle(tempCakeRect);
        tempCakeRect.localScale *= .2f;

        tempCake.GetComponent<GridLayoutGroup>().cellSize =
            new Vector2(tempCakeRect.rect.width / xAxeLength, tempCakeRect.rect.height / yAxeLength);

        for (int i = 0; i < xAxeLength * yAxeLength; i++)
        {
            GameObject tempCakePart = Instantiate(cakePart, Vector2.zero, Quaternion.identity, tempCake.transform);
            CakePart cakePartScript = tempCakePart.GetComponent<CakePart>();
            cakePreviewScript.cakeParts.Add(cakePartScript);
            tempCakePart.name = i.ToString();
            cakePartScript.ChangeMyColor(fruitTasteToColor(fruitNames[int.Parse(generatedcakeCode[3 + i].ToString())]), -1);
           // tempCakePart.GetComponent<CakePart>().setDecorationRectSize(tempCakeRect);
        }


    }

    void SetRectToMiddle(RectTransform rect)
    {
        Vector2 halfVector = new Vector2(.5f, .5f);
        rect.pivot = halfVector;
        rect.anchorMin = halfVector;
        rect.anchorMax = halfVector;
        rect.anchoredPosition = Vector2.zero;
    }

    void DecodeXandYLength(string cakeCode)
    {
        xAxeLength = int.Parse(cakeCode[1].ToString());
        yAxeLength = int.Parse(cakeCode[2].ToString());
    }

    void GenerateButtons()
    {
        deleteGeneratedBtns();
        for (int i = 0; i < xAxeLength; i++)
        {
            GameObject tempBtn = Instantiate(xyButtons, Vector2.zero, Quaternion.identity, xButtonsLocation);
            tempBtn.GetComponent<xyButtons>().man = this;
            tempBtn.GetComponent<xyButtons>().isOnXAxis = true;
            tempBtn.GetComponent<xyButtons>().setMyFunction(0, i);
            generatedBtns.Add(tempBtn);
        }
        for (int i = 0; i < yAxeLength; i++)
        {
            GameObject tempBtn = Instantiate(xyButtons, Vector2.zero, Quaternion.identity, yButtonsLocation);
            tempBtn.GetComponent<xyButtons>().man = this;
            tempBtn.GetComponent<xyButtons>().isOnXAxis = false;
            tempBtn.GetComponent<xyButtons>().setMyFunction(1, i);
            generatedBtns.Add(tempBtn);
        }
        blockedBtns = new int[Mathf.Max(xAxeLength, yAxeLength), Mathf.Max(xAxeLength, yAxeLength)];
        cakePartTaste = new string[Mathf.Max(xAxeLength, yAxeLength), Mathf.Max(xAxeLength, yAxeLength)];
    }

    void deleteGeneratedBtns()
    {
        foreach (GameObject btn in generatedBtns)
        {
            Destroy(btn);
        }
        generatedBtns = new List<GameObject>();
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
    int fruitTasteToIndex(string fruitName)
    {
        switch (fruitName)
        {
            case "banana": return 0;
            case "strawberry": return 1;
            case "chocolat": return 2;
            default: return -1;
        }
    }


}
