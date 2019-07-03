using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoughManager : MonoBehaviour {

    public PauseCanvasManager pauseMan;

    public Sprite[] requirmentSprites;
    public int[] quantitysToAdd; //0-1 : eggs; 2-3 : flour

    public Color wrongRecipeColor, missingRecipeColor, correctRecipeColor;

    public GameObject confirmBtn, deleteBowlBtn;
    public GameObject doughRequirment;
    public GameObject[] InputPanel;
    public Transform recipeFieldContainer;

    public Bowl bowl;

    public Animator myAnim;

    public Text eggsSlot1Txt, eggsSlot2Txt;
    public Text flourSlot1Txt, flourSlot2Txt;

    public static int orderNum = 1;

    //if player is offline
    float timerToWaitForNextRequirementMenu = 10;
    public static float clientTimer = 60;
    bool isGenerated = false;
    //Bot stuff ends here

    List<Recipe> recipeList = new List<Recipe>();

    Recipe correctRecipe;

	void Start () {
        SetSlotsTxtValues();
	}

    void Update(){
        myAnim.SetBool("isRecipeFound",recipeList.Count != 0);

        if (!isGenerated) StartCoroutine(PlayBot());
    }

    IEnumerator PlayBot()
    {
        isGenerated = true;
        GenerateNewRequirmentMenu();
        yield return new WaitForSeconds(timerToWaitForNextRequirementMenu);
        
        isGenerated = false;
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
        newDoughRecipe.setRequirment("Flour", requirmentSprites[1], genrateRandNumDependingOnSlots(quantitysToAdd[2], quantitysToAdd[3]));
        newDoughRecipe.doughMan = this;


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
                    case 2: bowl.ChangeMyColor(correctRecipeColor); correctRecipe = bowl.compatibleList; confirmBtn.SetActive(true); break;
                }
        
    }

    public void ConfirmBtn()
    {
        if (pauseMan.ScoreIncrement(correctRecipe.remainingClientTime))
        {
            levelUp();
        }
        correctRecipe.Delete();
        confirmBtn.SetActive(false);
        recipeList.Remove(correctRecipe);
        bowl.DeleteRequirments();
        correctRecipe = null;
        CheckBowlColor();
        DeleteUncompatibleCombinationsRecipe();
    }

    public void DeleteBowlRequirments()
    {
        bowl.DeleteRequirments();
    }

    public void DeleteRecipe(Recipe r){
        recipeList.Remove(r);
        r.Delete();
    }

    public void PhoneBtn()
    {
        myAnim.Play("Phone Scaling");      
    }

    public void ClosePhone()
    {
        myAnim.Play("Phone Shrink");
    }

    public void levelUptestBtn()
    {
        levelUp();
    }

    public void EnableInputPanel(int indexOfEnabling)
    {

        deleteBowlBtn.SetActive(false);
        foreach (GameObject item in InputPanel)
        {
            item.SetActive(false);
        }
        InputPanel[indexOfEnabling].SetActive(true);
    }

    #endregion

    void levelUp()
    {
        quantitysToAdd[0]++;
        quantitysToAdd[1]++;

        quantitysToAdd[2] += 10;
        quantitysToAdd[3] += 10;

        if (timerToWaitForNextRequirementMenu > 1f) timerToWaitForNextRequirementMenu -= 0.1f;
        if (clientTimer > 10) clientTimer -= 0.5f;

        myAnim.Play("LevelUp");
        
    }

    void DeleteUncompatibleCombinationsRecipe()
    {
        foreach (Recipe r in recipeList)
        {
            if (!(isCombination(quantitysToAdd[0], quantitysToAdd[1], r.reqList[0].quantity)))
            {
                DeleteRecipe(r);
            }
            if (!(isCombination(quantitysToAdd[2], quantitysToAdd[3], r.reqList[1].quantity)))
            {
                DeleteRecipe(r);
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

        if (bowl.reqList.Count == 0)
        {
            bowl.ChangeMyColor(Color.white);
            return;
        }
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
                case 2: bowl.ChangeMyColor(correctRecipeColor); correctRecipe = bowl.compatibleList; confirmBtn.SetActive(true); break;
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

