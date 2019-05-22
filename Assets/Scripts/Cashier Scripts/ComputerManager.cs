using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerManager : MonoBehaviour
{

    public Image screenImg;

    public string notesRecorded = "";
    public string generatedNotes = "";

    public int generatedNotesLength = 3;

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
    }

    void generateRandomPattern()
    {
        generatedNotes = "";
        for (int i = 0; i < generatedNotesLength; i++)
        {
            generatedNotes += (Random.Range(0, notesList.Count)).ToString();
        }
    }

    void reset()
    {
        screenImg.fillAmount = 0;
        noteIndexReached = 0;
        notesRecorded = "";
    }

    public void PlayGeneratedNotes()
    {
        generateRandomPattern();
        StartCoroutine(PlayGeneratedNotesNumerator());
    }

    public void SaveNote(string note)
    {
        if (noteIndexReached == generatedNotes.Length) reset();
        if (generatedNotes[noteIndexReached].ToString() == note)
        {
            noteIndexReached++;
            screenImg.fillAmount += 1f / generatedNotesLength;
            notesRecorded += note;
        }
        
    }

    IEnumerator PlayGeneratedNotesNumerator()
    {
        for (int i = 0; i < generatedNotesLength; i++)
        {
            notesList[int.Parse(generatedNotes[i].ToString())].PressButton(false);
            yield return new WaitForSeconds(playAnimationSpeed);
        }
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
