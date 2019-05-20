using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashierManager : MonoBehaviour
{

    public string writtenCode;
    public bool isBeginWritingCode = false;
    public LineRenderer patternLine;

    Vector3 mousePos;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isBeginWritingCode) FollowPatternLine();
    }

    void FollowPatternLine()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        patternLine.SetPosition(patternLine.positionCount - 1, mousePos);

    }

}
