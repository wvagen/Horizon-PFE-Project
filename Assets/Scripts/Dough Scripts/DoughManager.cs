using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoughManager : MonoBehaviour {

    public Sprite[] requirmentSprites;

    public GameObject doughRequirment;
    public Transform recipeFieldContainer;

    public static int orderNum = 1;

    List<Recipe> doughList = new List<Recipe>();

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

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

    #endregion
}
