using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternDot : MonoBehaviour
{
    public char myIndex;
    public bool isVisited = false;
    public CashierManager cashMan;

    public void OnDown()
    {
        LineCodeLogic();
        cashMan.isBeginWritingCode = true;
    }

    public void OnEnter()
    {
        if (isVisited || !cashMan.isBeginWritingCode) return;
        LineCodeLogic();
    }

    void LineCodeLogic()
    {
        isVisited = true;
        cashMan.writtenCode += myIndex;
        Vector3 myPos = transform.position;
        cashMan.patternLine.positionCount++;
        myPos.z = 0;
        cashMan.patternLine.SetPosition(cashMan.patternLine.positionCount - 1, myPos);
        cashMan.patternLine.positionCount++;
    }

}
 