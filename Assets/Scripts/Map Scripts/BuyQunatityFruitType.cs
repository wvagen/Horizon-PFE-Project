using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyQunatityFruitType : MonoBehaviour
{
    public Image fruitSprite;
    public Text fruitQuantityTxt;
    public Text priceTxt;

    int fruitQuantity,price;
    public void Set_Quantity_Price_Sprite(Sprite fruitSprite, int fruitQuantity, int price )
    {
        this.fruitSprite.sprite = fruitSprite;
        this.fruitQuantity = fruitQuantity;
        this.price = price;

        fruitQuantityTxt.text = fruitQuantity.ToString();
        priceTxt.text = price.ToString();

    }

    public void BuyBtn()
    {
        Debug.Log("NOTHING TO SELL");
    }
}
