using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cake : MonoBehaviour
{

    public List<CakePart> myCakeParts = new List<CakePart>(), myDownCakeParts = new List<CakePart>(); 
    public Transform upperCakeTrans,downCakeTrans;

    public void ChangePartColor(int x,int y,int xAxeLength,Color col,short cakeFilter){

        myCakeParts[getImageIndexFromCodinate(x, y, xAxeLength)].ChangeMyColor(col, cakeFilter);

    }

    int getImageIndexFromCodinate(int x, int y, int xAxeLength)
    {

        return (x * xAxeLength + y);

    }

}
