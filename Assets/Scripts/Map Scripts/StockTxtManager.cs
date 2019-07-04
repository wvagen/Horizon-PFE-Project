using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StockTxtManager : MonoBehaviour
{
    public Text bananaStockTxt;
    public Text strawberryStockTxt;
    public Text chocolatStockTxt;

    public void changeStocktxt(string fruitName,int valueAmount)
    {
        switch (fruitName)
        {
            case "Banana": bananaStockTxt.text = valueAmount.ToString(); break;
            case "Strawberry": strawberryStockTxt.text = valueAmount.ToString(); break;
            case "Chocolat": chocolatStockTxt.text = valueAmount.ToString(); break;
            default: Debug.Log("fruit dosnt exist u kidding ?"); break;
        }
    }

}
