using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowl : MonoBehaviour {

    public GameObject requirment;
    public Transform requirmentsPanelPos;

    List<Requirment> reqList = new List<Requirment>();

    #region Public_Methods
    public void setRequirment(string typeName, Sprite typeSprite, int quantity)
    {
        short newItemIndex = CheckNewItem(typeName) ;
        Debug.Log(newItemIndex);
        if (newItemIndex == -1) GenerateNewRequirment(typeName, typeSprite, quantity);
        else UpdateRequirmentInfo(newItemIndex, quantity);

    }
    #endregion
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
