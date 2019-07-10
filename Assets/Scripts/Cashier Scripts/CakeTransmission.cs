using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CakeTransmission : MonoBehaviour
{
    CashierManager cashMan;
    Recipe recipeToDesroy;

    bool isDown = false,canMove = false;

    Client rightClient;
    Vector2 initPos, mousePos;

    void OnMouseDown()
    {
        Debug.Log("Can Move");
        if (!canMove) return;
        isDown = true;
        initPos = transform.position;
    }

    void OnMouseUp()
    {
        if (!canMove) return;
        isDown = false;
        if (RectTransformUtility.RectangleContainsScreenPoint(rightClient.GetComponent<RectTransform>(), mousePos))
        {
            rightClient.ChangeToMoney();
            cashMan.pauseCanvMan.ScoreIncrement(rightClient.getRealPatienceTime() * 5);
            recipeToDesroy.Delete();
            Destroy(this.gameObject);

        }
        else
            transform.position = initPos;
    }

    public void GiveTransmissionAuthority(Client newClient,CashierManager cashMan,Recipe recipeToDesroy)
    {
        this.cashMan = cashMan;
        this.recipeToDesroy = recipeToDesroy;
        rightClient = newClient;
        canMove = true;
        GetComponent<BoxCollider2D>().enabled = true;
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (isDown)
        {
            transform.position = mousePos;

        }
    }
}
