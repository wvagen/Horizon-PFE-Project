using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bowl : MonoBehaviour {

    public GameObject requirment;
    public Transform requirmentsPanelPos;
    public Recipe compatibleList;
    
    List<Requirment> reqList = new List<Requirment>();
    

    Vector2 startPos;

    const float distanceToCompare = .05f;


    #region Public_Methods
    public void setRequirment(string typeName, Sprite typeSprite, int quantity)
    {
        short newItemIndex = CheckNewItem(typeName) ;
        if (newItemIndex == -1) GenerateNewRequirment(typeName, typeSprite, quantity);
        else UpdateRequirmentInfo(newItemIndex, quantity);

    }

    public void OnDown()
    {
        startPos = transform.position;
        DoughManager.draggedBowl = this;
    }

    public void OnDrag()
    {
        if (DoughManager.draggedBowl)
            transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void OnUp()
    {
        transform.position = startPos;
    }


    public short CheckMenuList(List<Requirment> menuReqList,short recipeReqListLength){

        short colorIndex = 0; //0 means fott el requirment 1 means en train de w mazal famma mat7ott
        //2 means c bn rak l9itha
        short itemsCount = 0;

        
            for (short i = 0; i < reqList.Count; i++)
            {
                colorIndex = 0;
                foreach (Requirment r in menuReqList)
                {
                    if (reqList[i].typeName == r.typeName)
                    {
                        if (reqList[i].quantity > r.quantity) colorIndex = 0;
                        else {
                            colorIndex = 1;
                            if (reqList[i].quantity == r.quantity) itemsCount++;
                        }
                        
                    }
                }
                if (colorIndex == 0) return 0;
            }
            if (itemsCount == recipeReqListLength) colorIndex = 2;

            return colorIndex;
    }

    public void ChangeMyColor(Color newColorToChange)
    {

        GetComponent<Image>().color = newColorToChange;

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
