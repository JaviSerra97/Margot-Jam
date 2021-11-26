using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesManager : MonoBehaviour
{
    [SerializeField] private List<PiecesSequence> sequences;
    [SerializeField] private Transform spawnPoint;
    private PiecesSequence chosenSequence;

    private int sequenceIndex = 0;

    private void Start()
    {
        SetSequence();
    }

    void SetSequence()
    {
        int rand = Random.Range(0, sequences.Count);
        chosenSequence = sequences[rand];
        Debug.Log(chosenSequence.name);

        CreateNextPiece(); //Iniciar el juego
    }

    public void CreateNextPiece()
    {
        var p = Instantiate(chosenSequence.sequences[GetCurrentSequence()].listOfPieces[0], spawnPoint.position, spawnPoint.rotation);
        chosenSequence.sequences[sequenceIndex].listOfPieces.Remove(p);
    }


    int GetCurrentSequence()
    {
        if(chosenSequence.sequences[sequenceIndex].listOfPieces.Count > 0 &&
            sequenceIndex < chosenSequence.sequences.Count - 1)
        {
            sequenceIndex++;
        }

        return sequenceIndex;
    }

}
