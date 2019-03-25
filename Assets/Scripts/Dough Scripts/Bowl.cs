﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowl : MonoBehaviour {

    public GameObject requirment;
    public Transform requirmentsPanelPos;

    public short bowlPos = 0;

    List<Requirment> reqList = new List<Requirment>();


    const short distanceToCompare = 10;


    #region Public_Methods
    public void setRequirment(string typeName, Sprite typeSprite, int quantity)
    {
        short newItemIndex = CheckNewItem(typeName) ;
        if (newItemIndex == -1) GenerateNewRequirment(typeName, typeSprite, quantity);
        else UpdateRequirmentInfo(newItemIndex, quantity);

    }

    public void SlideAnimation(Vector2 destination)
    {
        StartCoroutine(GoToDestinationAnimation(destination));
    }

    #endregion

    IEnumerator GoToDestinationAnimation(Vector2 destination)
    {
        while (Vector2.Distance(GetComponent<RectTransform>().anchoredPosition, destination) > distanceToCompare)
        {
            GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(GetComponent<RectTransform>().anchoredPosition, destination, DoughManager.speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();


        }
    }

    short CheckNewItem(string typeName)
    {
        for (short i = 0; i < reqList.Count; i++)
        {
            if (reqList[i].typeName == typeName) return i;
        }
        return -1 ;//not found in list
    }


    void GenerateNewRequirment(string typeName, Sprite typeSprite, int quantity)
    {
        GameObject newReqGameObject = Instantiate(requirment, requirmentsPanelPos.position, Quaternion.identity, requirmentsPanelPos);
        Requirment newReqScript = newReqGameObject.GetComponent<Requirment>();

        newReqScript.setQuantityInfos(typeSprite, quantity);

        newReqScript.typeName = typeName;
        newReqScript.quantity = quantity;

        reqList.Add(newReqScript);
    }

     void UpdateRequirmentInfo(short itemIndex, int quantity)
    {
        reqList[itemIndex].quantity += quantity;
        reqList[itemIndex].updateQuantityInfos(reqList[itemIndex].quantity);

    }



}
