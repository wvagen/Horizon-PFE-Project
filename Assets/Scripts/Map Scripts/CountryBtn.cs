using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountryBtn : MonoBehaviour
{

//Ya Ramy Piece OF SHit
    MapManager man; 
    public Image targetImg;
    public string countryName;
    public bool isMainBtn = false;

    void Start()
    {
            man = GameObject.Find("GameManager").GetComponent<MapManager>();

            if (isMainBtn)
            man.countryDic.Add(countryName, this);
    }

    public void SelectCountry()
    {
        man.SelectCountry(countryName);
    }

    public void EnableImgTarget()
    {
        StartCoroutine(TargetImgAnimation());
    }

    IEnumerator TargetImgAnimation()
    {
        float imgFill = 0;
        targetImg.enabled = true;
        targetImg.fillAmount = imgFill;

        while (imgFill < 1)
        {
            imgFill += Time.deltaTime * 5;
            targetImg.fillAmount = imgFill;
            yield return new WaitForEndOfFrame();
        }
    }
       
}
