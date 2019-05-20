using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PatternScript : MonoBehaviour {

	public PatternManager myClass;
    Image myImg;
    Color myColor;

    bool canColorDissappear = false;
	void Start(){
    myImg = GetComponent<Image>();
    myColor = myImg.color;
    }

    void FixedUpdate()
    {

        if (canColorDissappear && myImg.color != myColor )
        {
            returnPatternToDefaultColorSlowly();

        }
        if (myClass.isCodeGenerated && !myClass.isOnCouroutine && myClass.generatedCode[myClass.codeIndex].ToString() == this.gameObject.name)
            StartCoroutine(FollowGeneratedCode());
    }

    IEnumerator FollowGeneratedCode()
    {
        myClass.isOnCouroutine = true;
            myClass.codeIndex++;
            if (myClass.generatedCode.Length == myClass.codeIndex)
            {
                myClass.isCodeGenerated = false;
                myClass.codeIndex = 0;
            }

            myImg.color = myClass.patternColor;
            canColorDissappear = true;
        yield return new WaitForSeconds(myClass.generatedPatternFadeSpeed);
        myClass.isOnCouroutine = false;
    }

    void returnPatternToDefaultColorSlowly()
    {
        Color newColor = myImg.color;
        
        // InvokeRepeating("returnPatternToDefaultColorSlowly", Time.time, myClass.patternColorDissappearSpeed);
        if (newColor.a>myColor.a) newColor.a -= myClass.patternColorDissappearSpeed * Time.deltaTime;
        if (newColor.r != myColor.r) newColor.r = (newColor.r > myColor.r) ?
            (newColor.r - Time.deltaTime * myClass.patternColorDissappearSpeed) : (newColor.r + Time.deltaTime * myClass.patternColorDissappearSpeed);
        if (newColor.b != myColor.b) newColor.b = (newColor.b > myColor.b) ?
            (newColor.b - Time.deltaTime * myClass.patternColorDissappearSpeed) : (newColor.b + Time.deltaTime * myClass.patternColorDissappearSpeed);
        if (newColor.g != myColor.g) newColor.g = (newColor.g > myColor.g) ?
           (newColor.g - Time.deltaTime * myClass.patternColorDissappearSpeed) : (newColor.g + Time.deltaTime * myClass.patternColorDissappearSpeed);

        myImg.color = newColor;
    }

    public void OnPointerDown()
    {
        myClass.canDrawPatten = true;
        RecordThePattern();
    }



    public void OnPointerExit()
    {
        //canDrawHere = false;
        canColorDissappear = true;
    }

    public void OnPointerUp()
    {
        myClass.canDrawPatten = false;
        myClass.patternCode = myClass.myCode;
        myClass.myCode = "";
    }

	public void OnPointerEnter (){

        canColorDissappear = false;
        if ((myClass.canDrawPatten)) RecordThePattern();
		
    }

    void RecordThePattern()
    {

        myClass.myCode += gameObject.name;
        myImg.color = myClass.patternColor;

    }







	


}
