using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoughManager : MonoBehaviour {

    public Sprite[] requirmentSprites;
    public int[] quantitysToAdd; //0-1 : eggs; 2-3 : flour

    public Color wrongRecipeColor, missingRecipeColor, correctRecipeColor;

    public GameObject doughRequirment,bowl;
    public Transform recipeFieldContainer,bowlField;

    public Text eggsSlot1Txt, eggsSlot2Txt;
    public Text flourSlot1Txt, flourSlot2Txt;

    public static Bowl draggedBowl = null;
    public static int orderNum = 1;
    public static float speed = 20f;
    public static bool onAnimation = false;

    List<Recipe> doughList = new List<Recipe>();
    List<Bowl> bowlList = new List<Bowl>();

    List<RectTransform> bowlsTransPos = new List<RectTransform>();

	void Start () {
        fillBowlsTransPos();
        SetInitialValues();
	}

    void SetInitialValues()
    {
        eggsSlot1Txt.text = quantitysToAdd[0].ToString();
        eggsSlot2Txt.text = quantitysToAdd[1].ToString();

        flourSlot1Txt.text = quantitysToAdd[2].ToString();
        flourSlot2Txt.text = quantitysToAdd[3].ToString();
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
        Vector3 randomRotation = new Vector3(0, 0, Random.Range(-5f, 5f));
        GameObject newRecipe = Instantiate(doughRequirment, recipeFieldContainer.position, Quaternion.Euler(randomRotation), recipeFieldContainer);
        Recipe newDoughRecipe = newRecipe.GetComponent<Recipe>();

        newDoughRecipe.setOrderInfo(orderNum,0);
        newDoughRecipe.setRequirment("Egg", requirmentSprites[0], Random.Range(3, 10));
        newDoughRecipe.setRequirment("Flour", requirmentSprites[1], 100);

        doughList.Add(newDoughRecipe);

        orderNum++;
        CheckBowlColor();
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

    public void AddIngredient(int slotIndex)
    {
        short indexOfFindess = 0 ;
        if (onAnimation) return;
        foreach (Bowl b in bowlList)
        {
            if (((slotIndex / 2) + 1) == b.bowlPos)
            {
                b.setRequirment(TypeNameCase(slotIndex), requirmentSprites[(slotIndex / 2 + 1)-1], quantitysToAdd[slotIndex]);
                if (doughList.Count == 0) return;
                foreach (Recipe r in doughList)
                {
                    short newIndexOfFindess = b.CheckMenuList(r.reqList,(short)r.reqList.Count);
                    if (newIndexOfFindess > indexOfFindess) indexOfFindess = newIndexOfFindess;
                }

                switch (indexOfFindess)
                {
                    case 0: b.ChangeMyColor(wrongRecipeColor); break;
                    case 1: b.ChangeMyColor(missingRecipeColor); break;
                    case 2: b.ChangeMyColor(correctRecipeColor); break;
                }

            }
        }
        
    }

    public void DeleteBowl()
    {
        bowlList.Remove(draggedBowl);
        Destroy(draggedBowl.gameObject);
        draggedBowl = null;
    }


    public void RightBtn()
    {
        if (onAnimation ) return;
        foreach (Bowl b in bowlList) if (b.bowlPos == bowlsTransPos.Count - 1) return;
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
        if (onAnimation) return;
        foreach (Bowl b in bowlList) if (b.bowlPos == 0) return;
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

    void CheckBowlColor()
    {
        short indexOfFindess = 0;
        foreach (Bowl b in bowlList)
        {
            foreach (Recipe r in doughList)
            {
                short newIndexOfFindess = b.CheckMenuList(r.reqList, (short)r.reqList.Count);
                if (newIndexOfFindess > indexOfFindess) indexOfFindess = newIndexOfFindess;
            }

            switch (indexOfFindess)
            {
                case 0: b.ChangeMyColor(wrongRecipeColor); break;
                case 1: b.ChangeMyColor(missingRecipeColor); break;
                case 2: b.ChangeMyColor(correctRecipeColor); break;
            }
        }

    }

    string TypeNameCase(int slotIndexCases)
    {
        switch (slotIndexCases)
        {
            case 0: return "Egg";
            case 1: return "Egg";

            case 2: return "Flour";
            case 3: return "Flour";

            default: return "NULL";
        }
    }

}

