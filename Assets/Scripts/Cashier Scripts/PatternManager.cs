using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoBehaviour
{

    public CashierManager cashMan;
    public string generatedCode = "", myCode = "";
    public bool canDrawPatten = false, isCodeGenerated = false, isOnCouroutine = false;
    public Color patternColor,correctPatternColor,falsePatternColor;
    public LineRenderer line;
    public float patternColorDissappearSpeed = 1f, generatedPatternFadeSpeed;
    public int codeIndex;

    public List<PatternScript> activatedPatterns = new List<PatternScript>();

    public int generatedPatternLength;

    Material lineMat;
    
    Vector2 mousePos;
    void Start()
    {
        LevelDependancy();
        line.numCornerVertices = 5;
        lineMat = line.material;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (canDrawPatten)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            line.SetPosition(line.positionCount - 1, mousePos);
        }
    }

    IEnumerator CorrectLine(bool isPatternCorrect)
    {
        ColorMyLine(correctPatternColor, isPatternCorrect);
        yield return new WaitForSeconds(1);
        cashMan.IncreaseMoneyValue(500);
        this.gameObject.SetActive(false);
    }

    #region public_methods
    public void StartShowingGeneratedCode()
    {
        lineMat.color = patternColor;
        ReactivateAlfaLineColor();
        GenerateRandomPattern();
        line.positionCount = 0;
        codeIndex = 0;
        isCodeGenerated = true;
    }

    public void OnPatternsUp()
    {
        canDrawPatten = false;
        line.positionCount--;
        if (generatedCode == myCode)
        {
            StartCoroutine(CorrectLine(true));
        }
        else
        {
            ColorMyLine(falsePatternColor,false);
        }
        myCode = "";
        //line.positionCount = 0;
        foreach (PatternScript pattern in activatedPatterns)
        {
            pattern.canDrawOver = true;
        }
    }

    public void RecordPattern(PatternScript patt)
    {
        myCode += patt.gameObject.name;
        Vector2 myPos = patt.transform.position;
        line.SetPosition(line.positionCount - 1, myPos);
        line.positionCount++;
        activatedPatterns.Add(patt);
    }

    public void FadeLine(bool isPatternCorrect)
    {
        StartCoroutine(fadeLineCoroutine(isPatternCorrect));
    }

    public void ReactivateAlfaLineColor()
    {
        Color myCol = lineMat.color;
        myCol.a = 1;
        lineMat.color = myCol;
    }

    #endregion

    IEnumerator fadeLineCoroutine(bool isPatternCorrect)
    {
        Color myCol = lineMat.color;
        while (myCol.a > 0)
        {
            myCol.a -= Time.deltaTime;
            lineMat.color = myCol;
          yield  return new WaitForEndOfFrame();
        }
        line.positionCount = 0;
        isCodeGenerated = false;
        if (!isPatternCorrect)
            StartShowingGeneratedCode();
    }

    void ColorMyLine(Color newLineCol,bool isPatternCorrect)
    {
        lineMat.color = newLineCol;
        FadeLine(isPatternCorrect);
    }

    void GenerateRandomPattern()
    {
        generatedCode = "";
        int randPatternIndex = Random.Range(1, 10);
        for (int i = 0; i < generatedPatternLength; i++)
        {
            generatedCode += randPatternIndex.ToString();
            do
            {
                randPatternIndex = randomPatternCodeDependancy(randPatternIndex);
            } while (generatedCode.Contains(randPatternIndex.ToString()));
        }
    }

    int randomPatternCodeDependancy(int value)
    {
       List<int> randomNum = new List<int>();
        switch (value)
        {
            case 1: randomNum.Add(2); randomNum.Add(4); randomNum.Add(5); break;
            case 2: randomNum.Add(1); randomNum.Add(4); randomNum.Add(5); randomNum.Add(6); randomNum.Add(3); break;
            case 3: randomNum.Add(2); randomNum.Add(5); randomNum.Add(6);  break;
            case 4: randomNum.Add(1); randomNum.Add(2); randomNum.Add(5); randomNum.Add(7); randomNum.Add(8); break;
            case 5: randomNum.Add(1); randomNum.Add(2); randomNum.Add(3); randomNum.Add(4); randomNum.Add(7); randomNum.Add(8); randomNum.Add(9); randomNum.Add(6); break;
            case 6: randomNum.Add(2); randomNum.Add(3); randomNum.Add(5); randomNum.Add(9); randomNum.Add(8); break;
            case 7: randomNum.Add(4); randomNum.Add(5); randomNum.Add(8); break;
            case 8: randomNum.Add(7); randomNum.Add(4); randomNum.Add(5); randomNum.Add(6); randomNum.Add(9); break;
            case 9: randomNum.Add(8); randomNum.Add(5); randomNum.Add(6); break;
        }

        return randomNum[Random.Range(0, randomNum.Count)];
    }

    void LevelDependancy()
    {
        switch (CashierManager.level)
        {
            case 1: generatedPatternFadeSpeed = .5f; break;
            default: Debug.Log("level not listed yet"); break;
        }
    }
}
