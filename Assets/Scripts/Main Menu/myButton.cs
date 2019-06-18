using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class myButton : MonoBehaviour
{
    public Transform lineMkattaa;
    public string[] textsToBeChanged;
    public ArabicText textInside;

    int index;

    Animator myAnim;

    const short sizeRefrence = 95;

    void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    void Update()
    {
        if (lineMkattaa)
        lineMkattaa.Rotate(Vector3.forward * Time.deltaTime * 10);
    }

    public void PlayButtonAnimation()
    {
        index = 0;
        myAnim.Play("Button");
    }

    public void ReturnBtn()
    {
        index = 1;
        myAnim.Play("ButtonIcon2");
    }

    public void ChangeTxtAndIcon()
    {
        textInside.Text = textsToBeChanged[index];
        
    }

}
