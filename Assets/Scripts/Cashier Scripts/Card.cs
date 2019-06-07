using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{

   public GameObject explostionEffect;

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
       }
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
