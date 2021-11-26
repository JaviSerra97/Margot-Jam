using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesManager : MonoBehaviour
{
    [SerializeField] private List<PiecesSequence> sequences;
    private PiecesSequence chosenSequence;

    private int pieceIndex, sequenceIndex = 0;

    private void Start()
    {
        SetSequence();
    }

    void SetSequence()
    {
        int rand = Random.Range(0, sequences.Count);
        chosenSequence = sequences[rand];
        Debug.Log(chosenSequence.name);
    }

    void CreateNextPiece()
    {
        GetCurrentPiece();
    }

    GameObject GetCurrentPiece()
    {
        return;
    }

    void GetCurrentSequence()
    {
        if(chosenSequence.sequences[sequenceIndex].listOfPieces.Count > 0)
        {

        }
        else if(sequenceIndex < chosenSequence.sequences.Count){ sequenceIndex++; }
    }

}
