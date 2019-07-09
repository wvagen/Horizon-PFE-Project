using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Client : MonoBehaviour
{

    public GameObject progBarAndEmoji;
    public Transform bullsPanel;
    public Transform maleHair, maleClothes;
    public Transform femaleHair, femaleClothes;
    public Sprite[] emojis;
    public Sprite[] emotions;
    public Sprite moneySprite;
    public Color[] progBarColors;

    public Color[] skinColors,hairColors;

    public Image myEmotion,myEmoji,progrBar;

    public ClientsManager clientMan;

    Animator myAnim;
    Transform chosenBull;
    Vector2 initBullScale, initPos;

    string cakeCode ="";

    float patienceTime,realTime;
    bool isAsked = false,isGivenCake = false,isGivenMoney=false;

    List<GameObject> femaleHairList = new List<GameObject>(),
        femaleClothesList = new List<GameObject>(), maleHairList = new List<GameObject>(), maleClothesList = new List<GameObject>();

    bool canStartWaiting = false;
    bool isClientFedUp = false;
    short nextPatienceIndexValue = 0;

    void Start()
    {
        patienceTime = clientMan.patienceTime;
        realTime = patienceTime;
        ChangeEmojiAndColor();
        initLists();
        RandomMyLook();
    }

    void Update()
    {
        if (realTime >= 0 && canStartWaiting) DecreaseTimer();
    }

    void initLists()
    {
        for (int i = 0; i < maleHair.childCount; i++)
        {
            maleHairList.Add(maleHair.GetChild(i).gameObject);
        }
        for (int i = 0; i < maleClothes.childCount; i++)
        {
            maleClothesList.Add(maleClothes.GetChild(i).gameObject);
        }
        for (int i = 0; i < femaleHair.childCount; i++)
        {
            femaleHairList.Add(femaleHair.GetChild(i).gameObject);
        }
        for (int i = 0; i < femaleClothes.childCount; i++)
        {
            femaleClothesList.Add(femaleClothes.GetChild(i).gameObject);
        }
    }

    void RandomMyLook()
    {
        GameObject tempHair;
        GetComponent<Image>().color = skinColors[Random.Range(0, skinColors.Length)];
        if (Random.Range(0, 3) == 0)
        {
            tempHair = femaleHairList[Random.Range(0, femaleHairList.Count)];
            femaleClothesList[Random.Range(0, femaleClothesList.Count)].SetActive(true);

        }
        else
        {
            tempHair = maleHairList[Random.Range(0, maleHairList.Count)];
            maleClothesList[Random.Range(0, maleClothesList.Count)].SetActive(true);
        }

        tempHair.SetActive(true);
        tempHair.GetComponent<Image>().color = hairColors[Random.Range(0, hairColors.Length)];

    }

    void DecreaseTimer()
    {
        if (isGivenMoney) return;

        realTime -= Time.deltaTime;
        progrBar.fillAmount = realTime / patienceTime;

        if (realTime <= 0)
        {
        if (!isGivenCake) clientMan.cashMan.pauseCanvMan.Increase_Decrease_SatisfactionLevel(false);
            Leave();
        }

        if ((realTime < ((float)(emojis.Length - nextPatienceIndexValue) / emojis.Length) * patienceTime)) 
            ChangeEmojiAndColor();
    }

    void ChangeEmojiAndColor()
    {
       if (!isGivenCake) myEmoji.sprite = emojis[nextPatienceIndexValue];
        myEmotion.sprite = emotions[nextPatienceIndexValue];
        progrBar.color = progBarColors[nextPatienceIndexValue];
        if (nextPatienceIndexValue < emojis.Length - 1) nextPatienceIndexValue++;
    }
    
    #region public_methods

    public void SetClientsValue(Vector2 destination)
    {
        myAnim = GetComponent<Animator>();

        if (destination.x < 0) chosenBull = bullsPanel.transform.GetChild(0);
        else chosenBull = bullsPanel.transform.GetChild(1);

        initBullScale = chosenBull.localScale;
        chosenBull.gameObject.SetActive(false);

        StartCoroutine(ScaleAnimation(true));
    }

    public void WalkToPoint(Vector2 destination)
    {
        initPos = transform.position;
        StartCoroutine(WalkAnimation(destination, destination.x < transform.position.x));
    }

    public void GenerateCakeInBull(GameObject cake,string cakeCode)
    {
        this.cakeCode = cakeCode;
        cake.transform.SetParent(chosenBull);
        cake.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

    }

    public void SelectClient()
    {
        if (isGivenCake)
        {
            clientMan.cashMan.GeneratePattern();
            progBarAndEmoji.SetActive(false);
            isGivenMoney = true;
            Leave();
            return;
        }
        if (!canStartWaiting) return;
        transform.SetSiblingIndex(transform.parent.childCount - 1);
        StartCoroutine(ScaleAnimation(chosenBull.localScale.y >= initBullScale.y));
        
    }

    public void Leave()
    {
        isClientFedUp = true;
        WalkToPoint(initPos);
        GetComponent<Button>().enabled = false;
        Destroy(this.gameObject, 5);
    }

    public void ChangeToMoney()
    {
        if (isClientFedUp) return;
        myEmoji.sprite = moneySprite;
        realTime = 10;
        patienceTime = realTime;
        progrBar.color = progBarColors[0];
        isGivenCake = true;
    }

    public void SelectBulle()
    {
        if (!isAsked)
        clientMan.InvokeCards(cakeCode);
        StartCoroutine(ScaleAnimation(true));
        isAsked = true;
    }

    public float getRealPatienceTime()
    {
        return realTime;
    }

    public string getCakeCode()
    {
        return cakeCode;
    }

    #endregion

    IEnumerator WalkAnimation(Vector2 destination, bool isGoingLeft)
    {
        myAnim.SetBool("isWalking", true);
        if (isGoingLeft)
        {
            while (destination.x < transform.position.x)
            {
                transform.position = Vector2.MoveTowards(transform.position, destination, Time.deltaTime * 5);
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (destination.x > transform.position.x)
            {
                transform.position = Vector2.MoveTowards(transform.position, destination, Time.deltaTime * 5);
                yield return new WaitForEndOfFrame();
            }
        }

        myAnim.SetBool("isWalking", false);
        progBarAndEmoji.SetActive(true);
        canStartWaiting = true;
    }

    IEnumerator ScaleAnimation(bool isShrinking)
    {

        if (isShrinking)
        {
            while (chosenBull.localScale.y > 0)
            {
                chosenBull.localScale = Vector2.MoveTowards(chosenBull.localScale, Vector2.zero, Time.deltaTime * 5);
                yield return new WaitForEndOfFrame();
            }
            chosenBull.gameObject.SetActive(false);
        }
        else
        {
            chosenBull.gameObject.SetActive(true);
            while (chosenBull.localScale.y < initBullScale.y)
            {
                chosenBull.localScale = Vector2.MoveTowards(chosenBull.localScale, initBullScale, Time.deltaTime * 5);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
