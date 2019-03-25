using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoughManager : MonoBehaviour {

    public Sprite[] requirmentSprites;

    public GameObject doughRequirment,bowl;
    public Transform recipeFieldContainer,bowlField;

    public static int orderNum = 1;
    public static float speed = 20f;

    List<Recipe> doughList = new List<Recipe>();
    List<Bowl> bowlList = new List<Bowl>();

    List<RectTransform> bowlsTransPos = new List<RectTransform>();
        
    short eggsQuantityToAdd = 5;

	void Start () {
        fillBowlsTransPos();
	}
	
	void Update () {

    }


    void fillBowlsTransPos()
    {
        for (int i = 0; i < bowlField.GetChild(0).childCount; i++)
        {
            bowlsTransPos.Add(bowlField.GetChild(0).GetChild(i).GetComponent<RectTransform>());
        }
    }

    #region Public_Methods

    public void GenerateNewRequirmentMenu()
    {
        
        GameObject newRecipe = Instantiate(doughRequirment, recipeFieldContainer.position, Quaternion.identity, recipeFieldContainer);
        Recipe newDoughRecipe = newRecipe.GetComponent<Recipe>();

        newDoughRecipe.setOrderInfo(orderNum,0);
        newDoughRecipe.setRequirment("Egg", requirmentSprites[0], Random.Range(0, 6));

        doughList.Add(newDoughRecipe);

        orderNum++;
    }

    public void GenerateNewBowl()
    {

        foreach (Bowl b in bowlList)
        {
            if (b.bowlPos == 0) return;
        }
        GameObject newBowl = Instantiate(bowl, Vector2.zero, Quaternion.identity, bowlField);
        Bowl newBowlScript = newBowl.GetComponent<Bowl>();

        newBowlScript.bowlPos = 0;
        newBowlScript.GetComponent<RectTransform>().anchoredPosition = bowlsTransPos[0].anchoredPosition;
        bowlList.Add(newBowlScript);

    }

    public void AddEggsBtn()
    {
        foreach (Bowl b in bowlList)
        {
            if (b.bowlPos == 1)
            b.setRequirment("Egg", requirmentSprites[0], eggsQuantityToAdd);
        }
        
    }

    public void RightBtn()
    {
        foreach(Bowl b in bowlList){
            if (b.bowlPos < bowlsTransPos.Count - 1)
            {
                b.SlideAnimation(bowlsTransPos[b.bowlPos + 1].anchoredPosition);
                b.bowlPos++;
            } 
        }
    }

    public void LeftBtn()
    {
        foreach (Bowl b in bowlList)
        {
            if (b.bowlPos > 0)
            {
                b.SlideAnimation(bowlsTransPos[b.bowlPos - 1].anchoredPosition);
                b.bowlPos--;
            } 
        }
    }

    #endregion
}

