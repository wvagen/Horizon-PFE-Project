using UnityEngine;
using UnityEngine.UI;

public class Requirment : MonoBehaviour {

    public Text quantityTxt;
    public Image typeImg;

    public string typeName;
    public int quantity = 0; 


    #region Public_Methods

    public void setQuantityInfos(Sprite typeSprite,int quantity)
    {

        typeImg.sprite = typeSprite;
        quantityTxt.text = "x" + quantity.ToString();

    }

    public void updateQuantityInfos(int quantity)
    {
        quantityTxt.text = "x" + quantity.ToString();
    }


    #endregion
}
