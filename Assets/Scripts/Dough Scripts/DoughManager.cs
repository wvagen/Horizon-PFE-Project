using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoughManager : MonoBehaviour {

    public Sprite[] requirmentSprites;
    public int[] quantitysToAdd; //0-1 : eggs; 2-3 : flour

    public Color wrongRecipeColor, missingRecipeColor, correctRecipeColor;

    public GameObject doughRequirment;
    public GameObject[] InputPanel;
    public Transform recipeFieldContainer;

    public Bowl bowl;

    public Text eggsSlot1Txt, eggsSlot2Txt;
    public Text flourSlot1Txt, flourSlot2Txt;

    public Animator myAnim;

    public static Bowl draggedBowl = null;
    public static int orderNum = 1;
    public static float clientTimer = 30;

    List<Recipe> recipeList = new List<Recipe>();


	void Start () {
        SetSlotsTxtValues();
	}

    #region Public_Methods

    public void SetSlotsTxtValues()
    {
        eggsSlot1Txt.text = quantitysToAdd[0].ToString();
        eggsSlot2Txt.text = quantitysToAdd[1].ToString();

        flourSlot1Txt.text = quantitysToAdd[2].ToString();
        flourSlot2Txt.text = quantitysToAdd[3].ToString();
    }

    public void GenerateNewRequirmentMenu()
    {
        GameObject newRecipe = Instantiate(doughRequirment, recipeFieldContainer.position, Quaternion.identity, recipeFieldContainer);
        Recipe newDoughRecipe = newRecipe.GetComponent<Recipe>();

        newDoughRecipe.setOrderInfo(orderNum,0);
        newDoughRecipe.setRequirment("Egg", requirmentSprites[0], genrateRandNumDependingOnSlots(quantitysToAdd[0],quantitysToAdd[1]));
        newDoughRecipe.setRequirment("Flour", requirmentSprites[1], Random.Range(1, 5) * 25);

        recipeList.Add(newDoughRecipe);

        orderNum++;
        CheckBowlColor();
    }

    public void AddIngredient(int slotIndex)
    {
        short indexOfFindess = 0 ;

                bowl.setRequirment(TypeNameCase(slotIndex), requirmentSprites[(slotIndex / 2 + 1)-1], quantitysToAdd[slotIndex]);
                if (recipeList.Count == 0) return;
                foreach (Recipe r in recipeList)
                {
                    short newIndexOfFindess = bowl.CheckMenuList(r.reqList,(short)r.reqList.Count);
                    if (newIndexOfFindess == 2) bowl.compatibleList = r;
                    if (newIndexOfFindess > indexOfFindess) indexOfFindess = newIndexOfFindess;
                }

                switch (indexOfFindess)
                {
                    case 0: bowl.ChangeMyColor(wrongRecipeColor); break;
                    case 1: bowl.ChangeMyColor(missingRecipeColor); break;
                    case 2: bowl.ChangeMyColor(correctRecipeColor); break;
                }
        
    }


    public void SwallawBtn()
    {

        myAnim.Play("Swallaw");

    }


    public void levelUptestBtn()
    {
        levelUp();
    }

    public void EnableInputPanel(int indexOfEnabling)
    {
        foreach (GameObject item in InputPanel)
        {
            item.SetActive(false);
        }
        InputPanel[indexOfEnabling].SetActive(true);
    }

    #endregion

    void levelUp()
    {
        myAnim.Play("LevelUp");
        quantitysToAdd[0]++;
        quantitysToAdd[1]++;

        foreach (Recipe r in recipeList)
        {
            if (!(isCombination(quantitysToAdd[0], quantitysToAdd[1], r.reqList[0].quantity)))
            {
                recipeList.Remove(r);
                r.Delete();
            }
        }
    }

    bool isCombination(int a, int b, int c)
    {
        if ((c % a == 0) || (c % b == 0)) return true;
        int maxQuot = c / Mathf.Max(a, b);

        for (int i = 0; i <= maxQuot; i++)
        {
            if ((a * i + b) == c) return true;
            if ((b * i + a) == c) return true;
        }
        return false;

    }

    int genrateRandNumDependingOnSlots(int slot1,int slot2)
    {
        int randNum ;
        do
        {
            randNum = (Random.Range(0, 3) * slot1 + Random.Range(0, 3) * slot2);
        } while (randNum == 0);
        return  randNum;
    }

    void CheckBowlColor()
    {
        short indexOfFindess = 0;
            foreach (Recipe r in recipeList)
            {
                short newIndexOfFindess = bowl.CheckMenuList(r.reqList, (short)r.reqList.Count);
                if (newIndexOfFindess > indexOfFindess) indexOfFindess = newIndexOfFindess;
            }

            switch (indexOfFindess)
            {
                case 0: bowl.ChangeMyColor(wrongRecipeColor); break;
                case 1: bowl.ChangeMyColor(missingRecipeColor); break;
                case 2: bowl.ChangeMyColor(correctRecipeColor); break;
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

