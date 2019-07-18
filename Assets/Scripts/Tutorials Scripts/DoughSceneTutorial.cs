using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoughSceneTutorial : MonoBehaviour
{

    public DoughManager doughMan;
    public Bowl bowlScript;
    public GameObject blockerUI;
    

    Animator myAnim;
    int phaseSwitch = 0;

    void Start()
    {
        if (!MainMenuManager.isTutorialModeOn)
        {
            this.gameObject.SetActive(false);
            return;
        }
        myAnim = GetComponent<Animator>();
    }

    public void GenerateOrder()
    {
        doughMan.GenerateNewRequirmentMenu(null);
    }

    public void ScalePhone()
    {
        doughMan.PhoneBtn();
        myAnim.Play("Phase3");
        StartCoroutine(PlayOrderAnimationAfterAWhile());
    }

    public void EnableEggsInput()
    {
        doughMan.EnableInputPanel(0);
        myAnim.Play("Phase6");
    }


    public void ShowTrashIcon()
    {
        bowlScript.BowlBtn();
        myAnim.Play("Phase8");
    }

    public void DeleteBtn()
    {
        doughMan.DeleteBowlRequirments();
        myAnim.Play("Phase8_1");
    }

    public void AddIngredientAmount(int slotNum)
    {
        doughMan.AddIngredient(slotNum);
        DisableAllChildrenGameObjectsAndEnableBlockerUI();
        myAnim.Play("Phase6_1");
    }

    IEnumerator PlayOrderAnimationAfterAWhile()
    {
        yield return new WaitForSeconds(1);
        DisableAllChildrenGameObjectsAndEnableBlockerUI();
        myAnim.Play("Phase4");
    }

    public void NextBtn()
    {
        phaseSwitch++;
        DisableAllChildrenGameObjectsAndEnableBlockerUI();
        CheckAnimationCase();
    }

    void CheckAnimationCase()
    {
        switch (phaseSwitch)
        {
            case 1: myAnim.Play("Phase 1"); break;
            case 2: myAnim.Play("Phase2"); break;
            case 3: myAnim.Play("Phase5"); break;
            case 4: myAnim.Play("Phase7"); break;
            case 5: myAnim.Play("Phase9"); break;
            case 6: MainMenuManager.isTutorialModeOn = false; this.gameObject.SetActive(false); break;
        }
    }

    void DisableAllChildrenGameObjectsAndEnableBlockerUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        blockerUI.SetActive(true);
    }

}
