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
            {
                StartCoroutine(handMoveToButton(man.rightHand, true));
                myAnim.Play("yFillAnim");
            }
            else{
                man.MoveHandToButtonPosition(true,transform.position);
                myAnim.Play("yFillAnimHide");
            }
        }
        else
        {

            if (isShowing)
            {
                StartCoroutine(handMoveToButton(man.leftHand, false));
                myAnim.Play("xFillAnim");
            }
            else {
                man.MoveHandToButtonPosition(false, transform.position);
                myAnim.Play("xFillAnimHide");
            }
        }
    }

    IEnumerator handMoveToButton(Transform hand, bool isRightHand)
    {
        while (Vector2.Distance(hand.position,transform.position) > 0.2f){
            hand.position = Vector2.MoveTowards(hand.position,transform.position,man.handAnimationSpeed *((isRightHand) ? 1.5f:1) * Time.deltaTime);
        yield return new WaitForEndOfFrame();
        }

       man.MoveHandToTargetPosition(isRightHand);
    }




}
