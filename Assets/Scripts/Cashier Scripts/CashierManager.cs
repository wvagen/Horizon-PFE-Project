using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CashierManager : MonoBehaviour
{

    public GameObject cake,cakePart;
    public Transform cakeSpawnPos;
    public Color chocolateCol, bananaCol, appleCol;

    public string cakeCode;//exp : 132102000 - 1221231

    public static int level = 1;

    Cake cakeScript;
    int xPartsLength, yPartsLength;

#region public_methods
    public void GenerateCake()
    {
        decodeCakeCode();
        GenerateCakeAndCakeParts();
        GenerateCakeDownParts();
        ColorCake();
    }

#endregion

    void decodeCakeCode()
    {
        xPartsLength = int.Parse(cakeCode[1].ToString());
        yPartsLength = int.Parse(cakeCode[2].ToString());
    }

    void GenerateCakeAndCakeParts()
    {
        GameObject tempCake = Instantiate(cake, Vector3.zero, Quaternion.identity, cakeSpawnPos);
        cakeScript = tempCake.GetComponent<Cake>();
        RectTransform tempCakeUpperRect = cakeScript.upperCakeTrans.GetComponent<RectTransform>();

        cakeScript.upperCakeTrans.GetComponent<GridLayoutGroup>().cellSize =
            new Vector2(tempCakeUpperRect.rect.width / xPartsLength, tempCakeUpperRect.rect.height / yPartsLength);

        for (int i = 0; i < xPartsLength * yPartsLength; i++)
        {
            GameObject tempCakePart = Instantiate(cakePart, Vector2.zero, Quaternion.identity, cakeScript.upperCakeTrans);
            tempCakePart.name = i.ToString();
            cakeScript.myCakeParts.Add(tempCakePart.GetComponent<CakePart>());
            tempCakePart.GetComponent<CakePart>().setDecorationRectSize(tempCakeUpperRect);
        }
    }

    void GenerateCakeDownParts()
    {
        RectTransform tempCakeDownRect = cakeScript.downCakeTrans.GetComponent<RectTransform>();

        cakeScript.downCakeTrans.GetComponent<GridLayoutGroup>().cellSize =
            new Vector2(tempCakeDownRect.rect.width / xPartsLength, tempCakeDownRect.rect.height);

        for (int i = 0; i < xPartsLength; i++)
        {
            GameObject tempCakeDownPart = Instantiate(cakePart, Vector2.zero, Quaternion.identity, cakeScript.downCakeTrans);
            tempCakeDownPart.name = i.ToString();
            cakeScript.myDownCakeParts.Add(tempCakeDownPart.GetComponent<CakePart>());
            tempCakeDownPart.GetComponent<CakePart>().setDecorationRectSize(tempCakeDownRect);

        }
    }

    void ColorCake()
    {
        for (int i = 0; i < xPartsLength * yPartsLength; i++)
        {
            cakeScript.myCakeParts[i].ChangeMyColor(getFlavorFromIndex(int.Parse(cakeCode[3+i].ToString())),-1);
        }
        for (int i = 0; i < xPartsLength; i++)
        {
            Color cakeDownColor = getFlavorFromIndex(int.Parse(cakeCode[5 + i].ToString()));
            cakeDownColor.a = 0.85f;
            cakeScript.myDownCakeParts[i].ChangeMyColor(cakeDownColor, -1);
        }
    }

    Color getFlavorFromIndex(int index)
    {
        Debug.Log(index);
        switch (index)
        {
            case 1: return chocolateCol;
            case 2: return bananaCol; 
            case 3: return appleCol;
            default: return Color.white;
        }

    }



}
