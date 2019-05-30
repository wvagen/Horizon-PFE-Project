using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Client : MonoBehaviour
{

    public GameObject progBarAndEmoji;
    public Transform bullsPanel;
    public Sprite[] emojis;
    public Color[] progBarColors;

    public float patienceTime = 60f;
    public Image myEmoji,progrBar;

    Animator myAnim;
    Transform chosenBull;
    Vector2 initBullScale;

    string cakeCode ="";

    float realTime;

    bool canStartWaiting = false;

    short nextPatienceIndexValue = 0;

    void Start()
    {
        realTime = patienceTime;
        ChangeEmojiAndColor();
        
    }

    void Update()
    {
        if (realTime >= 0 && canStartWaiting) DecreaseTimer();
    }

    void DecreaseTimer()
    {
        realTime -= Time.deltaTime;
        progrBar.fillAmount = realTime / patienceTime;

        if (realTime < ((float)(emojis.Length - nextPatienceIndexValue) / emojis.Length) * patienceTime) 
            ChangeEmojiAndColor();
    }

    void ChangeEmojiAndColor()
    {
        
        myEmoji.sprite = emojis[nextPatienceIndexValue];
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
        if (!canStartWaiting) return;
        StartCoroutine(ScaleAnimation(chosenBull.localScale.y >= initBullScale.y));
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
