using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class xyButtons : MonoBehaviour
{

    public CakeMakerManager man;

    public void setMyFunction(int x, int y)
    {
        Debug.Log("(" + x + "," + y + ")");
        GetComponent<Button>().onClick.AddListener(delegate { man.ButtonCordinateClicked(x, y); });
    }

}
