using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Recipe : MonoBehaviour {

    public Text orderNumTxt;
    public GameObject requirment;
    public Transform requirmentsPanelPos;

    public List<Requirment> reqList = new List<Requirment>();

	void Start () {

    }

    #region Public_Methods

    public void setOrderInfo(int orderNum,float progressBarTime)
    {

        orderNumTxt.text = "Order Num#" + orderNum.ToString();

    }

    public void setRequirment(string typeName,Sprite typeSprite, int quantity)
    {
        GameObject newReqGameObject = Instantiate(requirment, requirmentsPanelPos.position, Quaternion.identity, requirmentsPanelPos);
        Requirment newReqScript = newReqGameObject.GetComponent<Requirment>();

        newReqScript.setQuantityInfos(typeSprite, quantity);

        newReqScript.typeName = typeName;
        newReqScript.quantity = quantity;

        reqList.Add(newReqScript);

    }

    #endregion


}
