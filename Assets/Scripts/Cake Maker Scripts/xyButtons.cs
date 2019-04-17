using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class xyButtons : MonoBehaviour
{

    public CakeMakerManager man;
    public bool isOnXAxis = false;
    public Animator myAnim;

    public void setMyFunction(int x, int y)
    {
        Debug.Log("(" + x + "," + y + ")");
        GetComponent<Button>().onClick.AddListener(delegate { man.ButtonCordinateClicked(x, y); });
    }

    public void FillingAnimation(bool isShowing)
    {
        if (isOnXAxis){
            if (isShowing)
                myAnim.Play("yFillAnim");
            else myAnim.Play("yFillAnimHide");

        }
        else
        {
            if (isShowing)
                myAnim.Play("xFillAnim");
            else myAnim.Play("xFillAnimHide");
        }
    }

    


}
