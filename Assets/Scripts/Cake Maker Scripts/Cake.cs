using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cake : MonoBehaviour
{
    
    public List<Image> myCakeParts = new List<Image>();


    public void ChangePartColor(int x,int y,int xAxeLength,Color col){

        myCakeParts[getImageIndexFromCodinate(x, y, xAxeLength)].color = col;

    }

    int getImageIndexFromCodinate(int x, int y, int xAxeLength)
    {

        return (x * xAxeLength + y);

    }

}
