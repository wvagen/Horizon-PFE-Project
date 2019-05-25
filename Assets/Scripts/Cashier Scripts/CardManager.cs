﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public GameObject cardGameObject;
    public CashierManager cashMan;

    public int cardsNumber = 3;


    List<string> randomGeneratedCodesList = new List<string>();

    #region public_methods
    public void GenerateRandomCards()
    {
        short randCorrectCardPos = (short)Random.Range(0, cardsNumber);
        cashMan.cakeScript = new Cake();

        GenerateRandomCodesAndFillList();
        randomGeneratedCodesList[randCorrectCardPos] = cashMan.cakeCode;

        for (int i = 0; i < cardsNumber; i++)
        {
            Debug.Log("HEY1");
           GameObject newCard = Instantiate(cardGameObject,Vector2.zero,Quaternion.identity,transform);
           cashMan.cakeCode = randomGeneratedCodesList[i];
           cashMan.GenerateCake();
           cashMan.cakeScript.transform.SetParent(newCard.transform);

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