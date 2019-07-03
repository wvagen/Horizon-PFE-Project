using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Recipe : MonoBehaviour {

    public Text orderNumTxt;
    public Image progressBar;
    public GameObject requirment;
    public Transform requirmentsPanelPos;

    public DoughManager doughMan;
    
    public GameObject fireWorkEffect;

    public List<Requirment> reqList = new List<Requirment>();

    public float remainingClientTime ;
    float initClientTimer;

    bool isTimeOver = false;

	void Start () {
        initClientTimer = DoughManager.clientTimer;
        remainingClientTime = initClientTimer;
    }

    void Update()
    {
        if (remainingClientTime > 0)
        {
            remainingClientTime -= Time.deltaTime;
            progressBar.fillAmount = remainingClientTime / initClientTimer;
        }
        else if (!isTimeOver)
        {
            isTimeOver = true;
            //Time over state
            doughMan.pauseMan.Increase_Decrease_SatisfactionLevel(false);
            doughMan.DeleteRecipe(this);
            Debug.Log("Time OVER!");
        }
    }

    #region Public_Methods

    public void setOrderInfo(int orderNum,float progressBarTime)
    {

        orderNumTxt.text = "Order Num#" + orderNum.ToString();

    }

    public void setRequirment(string typeName,Sprite typeSprite, int quantity)
    {
        GameObject newReqGameObject = Instantiate(requirment, requirmentsPanelPos.position, Quaternion.identity, requirmentsPanelPos);
        Requirment newReqScript = newReqGameObject.GetComponent<Requirment>();

        newReqScript.setQuantityInfos(typeSprite, quantity);

        newReqScript.typeName = typeName;
        newReqScript.quantity = quantity;

        reqList.Add(newReqScript);

    }
    public void Delete()
    {

        Destroy(Instantiate(fireWorkEffect, (Vector2) transform.position, Quaternion.identity), 2);
        StartCoroutine(fadeOutAnimation());

    }
        
    #endregion

    IEnumerator fadeOutAnimation()
    {
        Image[] imgsToFade = GetComponentsInChildren<Image>();
        Text[] txtsToFade = GetComponentsInChildren<Text>();
        Color colToFadeImg = imgsToFade[0].color;
        Color colToFadeTxt = txtsToFade[0].color;

        while (imgsToFade[0].color.a > 0)
        {
            colToFadeImg.a -= Time.deltaTime * 3;
            colToFadeTxt.a -= Time.deltaTime * 3;
            foreach (Image i in imgsToFade) i.color = colToFadeImg;
            foreach (Text t in txtsToFade) t.color = colToFadeImg;

            yield return new WaitForEndOfFrame();
        }
        Destroy(this.gameObject);
    }

}
