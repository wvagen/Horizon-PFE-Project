using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{

   public GameObject explostionEffect;

   public CardManager cardMan;

   public Cake correctCakeScript;

   public string myCakeCode;
   public string correctCakeCode;

   Animator myAnim;

   void Start()
   {
       myAnim = GetComponent<Animator>();
   }

   public void SelectCard()
   {
       myAnim.Play("Card Show");

       if (myCakeCode == correctCakeCode)
       {
           Destroy(Instantiate(explostionEffect, transform.position, Quaternion.identity), 3);
           StartCoroutine(MoveOnToScreen());
           cardMan.cashMan.cakeCode = myCakeCode;
           cardMan.cashMan.cakeScript = correctCakeScript;
       }
   }

   IEnumerator MoveOnToScreen()
   {
       yield return new WaitForSeconds(1);
       cardMan.gameObject.SetActive(false);
       cardMan.cashMan.GenerateComputerScreen();
   }

   public void HideCake()
   {
       transform.GetChild(0).gameObject.SetActive(false);
   }
   public void ShowCake()
   {
       transform.GetChild(0).gameObject.SetActive(true);
   }

}
