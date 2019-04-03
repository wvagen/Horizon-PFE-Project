using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Lean.Touch;

public class MapManager : MonoBehaviour
{

    public string[] countryNames;
    public Sprite[] flags;

    public CountryInfoPanel countryInfoPanelScript;

    public void SelectCountry(string countryName)
    {
        int countryNameIndex = 0;
        while (countryNames[countryNameIndex] != countryName)
        {
            countryNameIndex++;
        }

        countryInfoPanelScript.SetCountryNameAndFlag(flags[countryNameIndex], countryNames[countryNameIndex],
            EventSystem.current.currentSelectedGameObject.transform);


    }
    
}
