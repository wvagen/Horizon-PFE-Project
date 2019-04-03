using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountryBtn : MonoBehaviour
{
    MapManager man; 
    public Image targetImg;
    public string countryName;

    void Start()
    {
        man = GameObject.Find("GameManager").GetComponent<MapManager>();
        man.countryDic.Add(countryName, this);
    }

    public void SelectCountry()
    {
        man.SelectCountry(countryName);
    }
       
}
