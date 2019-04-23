using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class CakePart : MonoBehaviour
{

    public Image decoration;
    public Sprite[] decorationSprites;

    RectTransform cakeTrans;

    public void ChangeMyColor(Color newCol, short filter)
    {
        if (filter < 0)
        {
            GetComponent<Image>().color = newCol;
        }
        else
        {
            decoration.gameObject.SetActive(true);
            decoration.sprite = decorationSprites[filter];
            decoration.color = newCol;
        }
        
    }

    public void setDecorationRectSize(RectTransform cakeTransRect)
    {
        decoration.gameObject.SetActive(false);
        this.cakeTrans = cakeTransRect;
        StartCoroutine(changeCakeTransPos());
       
    }

    IEnumerator changeCakeTransPos()
    {
        yield return new WaitForEndOfFrame();
        decoration.transform.position = cakeTrans.transform.position;
        decoration.GetComponent<RectTransform>().sizeDelta = cakeTrans.sizeDelta;
    }

}
