using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerManager : MonoBehaviour
{

    public Transform screenLocation;
    public AudioSource audioMan;
    public Image tickImg;

    public CashierManager cashMan;

    public GameObject focusImg,errorImg;

    public string notesRecorded = "";
    public string generatedNotes = "";

    public int generatedNotesLength = 3;

    public bool isPlayingNotes = false;

    public Cake cakeToGenerate;

    float playAnimationSpeed;
    int noteIndexReached = 0;

    List<ComputerBtn> notesList = new List<ComputerBtn>();

    void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            notesList.Add(transform.GetChild(i).GetComponent<ComputerBtn>());
        }
        LevelDependancy();
        gameObject.SetActive(false);
    }

    void generateRandomPattern()
    {
        reset();
        generatedNotes = "";
        for (int i = 0; i < generatedNotesLength; i++)
        {
            generatedNotes += (Random.Range(0, notesList.Count)).ToString();
        }
    }

    void reset()
    {
        foreach (ComputerBtn btn in notesList)
        {
            btn.ResetInsideColorImgAlpha();
            
        }
        noteIndexReached = 0;
        notesRecorded = "";
        tickImg.fillAmount = 0;
    }

    IEnumerator DisableComputer()
    {
        yield return new WaitForSeconds(.5f);
        gameObject.SetActive(false);
    }
    #region public_metehods

    public void PlayGeneratedNotes()
    {
        generateRandomPattern();
        StartCoroutine(PlayGeneratedNotesNumerator());
    }

    public void SaveNote(string note)
    {
        if (generatedNotes[noteIndexReached].ToString() == note)
        {
            noteIndexReached++;
            notesRecorded += note;
            tickImg.fillAmount += 1.0f / generatedNotesLength;
        }
        else
        {
            reset();
            StartCoroutine(ErrorShow());
        }
        if (noteIndexReached == generatedNotesLength)
        {
            cashMan.canComputerStuff = false;
            cashMan.GenerateCakeRecipe();
            StartCoroutine(DisableComputer());
            reset();
            tickImg.fillAmount = 1;
        }
    }


    #endregion

    IEnumerator ErrorShow()
    {
        isPlayingNotes = true;
        errorImg.SetActive(true);
        yield return new WaitForSeconds(.5f);
        errorImg.SetActive(false);
        StartCoroutine(PlayGeneratedNotesNumerator());
    }

    IEnumerator PlayGeneratedNotesNumerator()
    {
        isPlayingNotes = true;
        focusImg.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        focusImg.SetActive(false);
        for (int i = 0; i < generatedNotesLength; i++)
        {
            notesList[int.Parse(generatedNotes[i].ToString())].PressButton(false);
            yield return new WaitForSeconds(playAnimationSpeed);
        }
        isPlayingNotes = false;
    }

    void LevelDependancy()
    {
        switch (CashierManager.level)
        {
            case 1: playAnimationSpeed = .5f; break;
            default: Debug.Log("level not listed yet"); break;
        }
    }

}
