using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PiecesSequence;

public class PiecesManager : MonoBehaviour
{
    public const float OFFSET_CAMERA = 2f;

    public Transform LeftCollider;
    public Transform RightCollider;
    public Camera MainCamera;

    [SerializeField] private List<PiecesSequence> sequences;
    [SerializeField] private Transform spawnPoint;
    private PiecesSequence chosenSequence;

    private int sequenceIndex, pieceIndex = 0;

    private void Start()
    {
        SetSequence();
    }

    void SetSequence()
    {
        int rand = Random.Range(0, sequences.Count);
        chosenSequence = sequences[rand];
        Debug.Log(chosenSequence.name);

        ShufflePieces();
    }

    public void CreateNextPiece()
    {
        if(pieceIndex < chosenSequence.sequences[sequenceIndex].listOfPieces.Count)
        {
            Instantiate(chosenSequence.sequences[sequenceIndex].listOfPieces[pieceIndex], spawnPoint.position, spawnPoint.rotation);
            pieceIndex++;
        }
        else { CheckSequences(); }
    }


    void CheckSequences()
    {
        if(sequenceIndex < chosenSequence.sequences.Count - 1)
        {
            sequenceIndex++;
            pieceIndex = 0;
            CreateNextPiece();
        }
        else 
        { 
            Debug.Log("Ya");
            ScoreManager.Instance.GetFinalScore();
        }
    }

    void ShufflePieces()
    {
        for (int i = 0; i < chosenSequence.sequences.Count; i++)
        {
            if (chosenSequence.sequences[i].doShuffle)
            {
                var count = chosenSequence.sequences[i].listOfPieces.Count;

                for (int j = 0; j < count; j++)
                {
                    int rand = Random.Range(j, count);
                    var temp = chosenSequence.sequences[i].listOfPieces[j];

                    chosenSequence.sequences[i].listOfPieces[j] = chosenSequence.sequences[i].listOfPieces[rand];
                    chosenSequence.sequences[i].listOfPieces[rand] = temp;
                }
            }
        }

        CreateNextPiece(); //Iniciar el juego
    }

    public void CheckMaxHeight(float y_pos)
    {
        if(y_pos > spawnPoint.position.y - OFFSET_CAMERA)
        {
            MainCamera.orthographicSize += 1;
            MainCamera.transform.position += new Vector3(0, 1, 0);
            LeftCollider.position += new Vector3(-1, 0, 0);
            RightCollider.position += new Vector3(1, 0, 0);
            spawnPoint.position += new Vector3(0, 1, 0);
        }
    }

}
