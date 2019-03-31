using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Recipe : MonoBehaviour {

    public Text orderNumTxt;
    public GameObject requirment;
    public Transform requirmentsPanelPos;

    List<Requirment> reqList = new List<Requirment>();

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

   public short checkRecipeAndBowlCorresspandencie(List<Requirment> bowlReqList)
    {
        //tchouf kan el items fil menu 9add elli fil bowl w kan 9ad 9ad traja3lek 2 sinon kanhi 9rib
        //traja3lek 1 sinon 0 i4a diffrent par default kan el bowl far8a traja3lek 0
       //sinon traja3lek 3 ya3ni menu mafiha 7atta effet
        short valueToReturn = 3;
        short indexReqFound = -1;
        short matchListCount = 0;

        for (int i = 0; i < bowlReqList.Count; i++)
        {
            indexReqFound = isRequriementExists(bowlReqList[i]);
            if (indexReqFound == -1) valueToReturn = 0;
            else
            {
                if (bowlReqList[i].quantity > reqList[indexReqFound].quantity)
                {
                    return 0;
                }
                else if (bowlReqList[i].quantity == reqList[indexReqFound].quantity
                    && valueToReturn != 1)
                {
                    matchListCount++;
                    valueToReturn = 1;
                }
                else valueToReturn = 1;
            }
            
        }

        if (matchListCount == reqList.Count) valueToReturn = 2;
        return valueToReturn;

    }

   public void changeMyColor(Color newColor)
   {
       GetComponent<Image>().color = newColor;
   }

    #endregion

    short isRequriementExists(Requirment req){

        for (short i = 0; i < (short)reqList.Count; i++){
			 if (req.typeName == reqList[i].typeName) return i;
			}

        return -1;
    }


}
