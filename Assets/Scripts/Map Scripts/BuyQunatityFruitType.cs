using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyQunatityFruitType : MonoBehaviour
{
    public Image fruitSprite;
    public Text fruitQuantityTxt;
    public Text priceTxt;
    public CountryInfoPanel country;

    int fruitQuantity,price;
    string fruitName;
    MapManager manScript;
    
    public void Set_Quantity_Price_Sprite(MapManager manScript,string fruitName,Sprite fruitSprite, int fruitQuantity, int price )
    {
        this.fruitSprite.sprite = fruitSprite;
        this.fruitQuantity = fruitQuantity;
        this.price = price;
        this.manScript = manScript;
        this.fruitName = fruitName;

        fruitQuantityTxt.text = fruitQuantity.ToString();
        priceTxt.text = price.ToString();

    }

    public void BuyBtn()
    {
        if (price > MapManager.moneyValue) return;
        MapManager.moneyValue -= price;
        manScript.SetMoneyValueTxt();
        manScript.stock[fruitName] += fruitQuantity;
        manScript.UpdateStockTxt(fruitName);
        country.DeleteOffer(this.gameObject);
    }

}
