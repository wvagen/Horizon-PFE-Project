using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountryInfoPanel : MonoBehaviour
{
    public GameObject BuyImgQuantityPanel;

    public Transform buyImgPanelSpawnLocation;

    public Text countryName;
    public Image countryFlag;
    public LineRenderer line;

    Transform countryLocation;

    bool isCountrySelected = false;
    public void SetCountryNameAndFlag(Sprite countryFlag,string countryName, Transform countryLocation)
    {
        this.countryFlag.sprite = countryFlag;
        this.countryName.text = countryName;
        this.countryLocation = countryLocation;
        isCountrySelected = true;
    }

    public void GenerateBuyQuanityTypePanel(Sprite fruitImg,int fruitQuantity, int price)
    {
        GameObject newPanel = Instantiate(BuyImgQuantityPanel, transform.position, Quaternion.identity, buyImgPanelSpawnLocation);
        BuyQunatityFruitType script = newPanel.GetComponent<BuyQunatityFruitType>();

        script.Set_Quantity_Price_Sprite(fruitImg,fruitQuantity, price);

    }

    void Update()
    {
        if (isCountrySelected)DrawALine();
    }

    void DrawALine()
    {
        line.SetPosition(0,(Vector3) line.transform.position);
        line.SetPosition(1,(Vector3)countryLocation.position);

    }
}
