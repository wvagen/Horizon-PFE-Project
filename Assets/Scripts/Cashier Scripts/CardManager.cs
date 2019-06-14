using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public GameObject cardGameObject;
    public CashierManager cashMan;

    public int cardsNumber = 3;

    string correctCakeCode;
    List<string> randomGeneratedCodesList = new List<string>();

    #region public_methods

    public void ShowCadPanel()
    {
        this.gameObject.SetActive(true);
    }
    public void GenerateRandomCards()
    {
        short randCorrectCardPos = (short)Random.Range(0, cardsNumber);
        cashMan.cakeScript = new Cake();

        GenerateRandomCodesAndFillList();
        randomGeneratedCodesList[randCorrectCardPos] = cashMan.cakeCode;
        correctCakeCode = cashMan.cakeCode;

        for (int i = 0; i < cardsNumber; i++)
        {
           GameObject newCard = Instantiate(cardGameObject,Vector2.zero,Quaternion.identity,transform);
           cashMan.cakeCode = randomGeneratedCodesList[i];
           cashMan.GenerateCake();
           cashMan.cakeScript.transform.SetParent(newCard.transform);

           Card newCardScript = newCard.GetComponent<Card>();
           newCardScript.correctCakeCode = correctCakeCode;
           newCardScript.myCakeCode = randomGeneratedCodesList[i];
           newCardScript.cardMan = this;

        }
    }
    #endregion


    void GenerateRandomCodesAndFillList()
    {
        randomGeneratedCodesList = new List<string>();
        int xLength = int.Parse(cashMan.cakeCode[1].ToString());
        int yLength = int.Parse(cashMan.cakeCode[2].ToString());

        string ranCode = "";

        for (int j = 0; j < cardsNumber; j++)
        {
        do
        {
            ranCode = "";
            for (int i = 0; i <= 2+(xLength * yLength); i++)
            {
                if (i>3)
                ranCode += Random.Range(1, cashMan.flavorLength + 1);
                else
                ranCode += cashMan.cakeCode[i].ToString();
            }

        } while (randomGeneratedCodesList.Contains(ranCode));

        randomGeneratedCodesList.Add(ranCode);
        }
    }



}
