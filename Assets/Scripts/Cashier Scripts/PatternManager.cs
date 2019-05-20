using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoBehaviour
{

    public string generatedCode = "", patternCode = "", myCode = "";
    public bool canDrawPatten = false, isCodeGenerated = false, isOnCouroutine = false;
    public Color patternColor;
    public float patternColorDissappearSpeed = 1f, generatedPatternFadeSpeed;
    public int codeIndex;

    public int generatedPatternLength;


    void Start()
    {
        LevelDependancy();
    }

    #region public_methods
    public void StartShowingGeneratedCode()
    {
        GenerateRandomPattern();
        codeIndex = 0;
        isCodeGenerated = true;
    }

    #endregion

    void GenerateRandomPattern()
    {
        generatedCode = "";
        int randPatternIndex = Random.Range(1, 10);
        for (int i = 0; i < generatedPatternLength; i++)
        {
            generatedCode += randPatternIndex.ToString();
            randPatternIndex = patternCodeDepdendancy(randPatternIndex);
        }
    }

    int patternCodeDepdendancy(int x)
    {
        List<int> numList = new List<int>();
        switch (x)
        {
            case 1: numList.Add(2); numList.Add(4); break;
            case 2: numList.Add(1); numList.Add(5); numList.Add(3); break;
            case 3: numList.Add(2); numList.Add(6); break;
            case 4: numList.Add(1); numList.Add(5); numList.Add(7); break;
            case 5: numList.Add(2); numList.Add(4); numList.Add(6); numList.Add(8); break;
            case 6: numList.Add(3); numList.Add(5); numList.Add(9); break;
            case 7: numList.Add(4); numList.Add(8); break;
            case 8: numList.Add(5); numList.Add(7); numList.Add(9); break;
            case 9: numList.Add(6); numList.Add(8); break;
        }
        return numList[Random.Range(0, numList.Count)];
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
