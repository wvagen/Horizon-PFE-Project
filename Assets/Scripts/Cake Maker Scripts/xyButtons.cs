using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class xyButtons : MonoBehaviour
{

    public CakeMakerManager man;
    public bool isOnXAxis = false;
    public Animator myAnim;

    public Image arrow;


    public void setMyFunction(int x, int y)
    {

        GetComponent<Button>().onClick.AddListener(delegate { man.ButtonCordinateClicked(x, y); });
        if (isOnXAxis) arrow.transform.eulerAngles = Vector3.forward * -90;

    }

    public void FillingAnimation(bool isShowing)
    {
        if (!man.isCouroutineFinished) return;
        man.isCouroutineFinished = false;
        if (isOnXAxis){
            if (isShowing)
            {
                StartCoroutine(handMoveToButton(man.rightHand, true));
                myAnim.Play("yFillAnim");
                arrow.transform.localScale = new Vector2(-1, 1);
            }
            else{
                man.MoveHandToButtonPosition(true,transform.position);
                myAnim.Play("yFillAnimHide");
                arrow.transform.localScale = Vector2.one;
            }
        }
        else
        {

            if (isShowing)
            {
                StartCoroutine(handMoveToButton(man.leftHand, false));
                myAnim.Play("xFillAnim");
                arrow.transform.localScale = new Vector2(-1, 1);
            }
            else {
                man.MoveHandToButtonPosition(false, transform.position);
                myAnim.Play("xFillAnimHide");
                arrow.transform.localScale = Vector2.one;
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
