using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CakePreview : MonoBehaviour
{
    public Text orderNumTxt;
    public Image progressBar;
    
    public Transform cakePreviewLocation;

    public GameObject fireWorkEffect;
    public List<CakePart> cakeParts = new List<CakePart>();
    public string cakeCode;
    public float realPatienceTime;

    CakeMakerManager cakeMan;
    float totaltpPatienceTime;

    public void setVars(int orderNum,float timeToWait,string newCakeCode,CakeMakerManager newCakeMan)
    {
        orderNumTxt.text = "Order Num#" + orderNum.ToString();
        totaltpPatienceTime = timeToWait;
        realPatienceTime = totaltpPatienceTime;
        cakeMan = newCakeMan;
        cakeCode = newCakeCode;
    }
    public void DestroyLeMe()
    {
        Destroy(Instantiate(fireWorkEffect, transform.position, Quaternion.identity),3);
        Destroy(this.gameObject);
    }

    void Update()
    {
        DecreaseTimer();
    }

    void DecreaseTimer()
    {
        if (realPatienceTime > 0)
        {
            realPatienceTime -= Time.deltaTime;
            progressBar.fillAmount = realPatienceTime / totaltpPatienceTime;
        }
        else
        {
            Debug.Log("Game Over");
            cakeMan.pauseCan.Increase_Decrease_SatisfactionLevel(false);
            DestroyLeMe();
        }
    }
  
}
