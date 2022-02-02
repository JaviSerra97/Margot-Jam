using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static PiecesSequence;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PiecesManager : MonoBehaviour
{
    private const float MarginSideColliders = 0.45f;
    private const float PIECE_MARGIN = 0.25f;

    public Image fadeImage;
    public float fadeDuration;

    [Header("Camera Position")]
    public Transform LeftCollider;
    public Transform RightCollider;
    public Camera MainCamera;
    public int LerpSteps = 5;
    public float StartY;
    public float EndY;
    public float StartOrtho;
    public float EndOrtho;
    public float PieceHeight = 1;
    public const float OFFSET_CAMERA = 3.5f;

    [SerializeField] private List<PiecesSequence> sequences;
    [SerializeField] private Transform spawnPoint;
    public float dropDelay;
    private PiecesSequence chosenSequence;
    private bool canDropPiece;

    private int sequenceIndex, pieceIndex = 0;
    private bool _elevate = false;

    private PieceDrop pieceDrop;
    private PlayerInput input;
    private int _lerpCurrentValue;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        SetSequence();
        InitCamera();
    }

    private void InitCamera()
    {
        _lerpCurrentValue = 0;
        MainCamera.transform.position = new Vector3(MainCamera.transform.position.x, StartY, -10f);
        MainCamera.orthographicSize = StartOrtho;
    }

    void SetSequence()
    {
        int rand = Random.Range(0, sequences.Count);
        chosenSequence = sequences[rand];
        
        ShufflePieces();
    }

    public void CreateNextPiece()
    {
        if(pieceIndex < chosenSequence.sequences[sequenceIndex].listOfPieces.Count)
        {
            pieceDrop = Instantiate(chosenSequence.sequences[sequenceIndex].listOfPieces[pieceIndex], spawnPoint.position, spawnPoint.rotation).GetComponent<PieceDrop>();
            pieceIndex++;

            canDropPiece = false;
            Invoke(nameof(AllowDrop), dropDelay);
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
            StartCoroutine(EndGame());
        }
    }

     IEnumerator EndGame()
    {
        yield return new WaitForSeconds(2.5f);
        ScoreManager.Instance.GetFinalScore();
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

        FadeScreen();
    }

    public void CheckMaxHeight(float y_pos)
    {
        Debug.Log(y_pos);
        Debug.Log(y_pos > spawnPoint.position.y - PieceHeight * 1.2f);
        if(y_pos > spawnPoint.position.y - PieceHeight * 1.2f && _lerpCurrentValue < LerpSteps)
        {
            ++_lerpCurrentValue;
            float _valueLerp =(float) _lerpCurrentValue/LerpSteps;
            Debug.Log(_lerpCurrentValue);
            MainCamera.orthographicSize = Mathf.Lerp(StartOrtho, EndOrtho,_valueLerp);
            MainCamera.transform.position = new Vector3(MainCamera.transform.position.x, Mathf.Lerp(StartY, EndY, _valueLerp), -10f);
            LeftCollider.position = MainCamera.ScreenToWorldPoint(new Vector3(MainCamera.pixelWidth * MarginSideColliders, MainCamera.pixelHeight * 0.5f, 10)); // Mitad de pantalla pegado a la izquierda
            RightCollider.position = MainCamera.ScreenToWorldPoint(new Vector3(MainCamera.pixelWidth * (1 - MarginSideColliders), MainCamera.pixelHeight * 0.5f, 10)); // Mitad de pantalla pegado a la derecha
            //spawnPoint.position = MainCamera.ScreenToWorldPoint(new Vector3(MainCamera.pixelWidth * 0.5f, MainCamera.pixelHeight * 0.9f, 10)); // Mitad de la pantalla en el centro arriba
            float Y_Var_spawn = MainCamera.orthographicSize - PieceHeight - PIECE_MARGIN;
            spawnPoint.position = MainCamera.transform.position + new Vector3(0,Y_Var_spawn, 10f);
        }
        else if(y_pos > spawnPoint.position.y - OFFSET_CAMERA && !_elevate) // Un poco de seguridad por si llegan muy alto
        {
            spawnPoint.position += new Vector3(0, 1f, 0);
            MainCamera.orthographicSize += 0.3f;
            MainCamera.transform.position += new Vector3(0, 0.3f, 0);
            _elevate = true;
        }
    }

    void FadeScreen()
    {
        fadeImage.DOFade(0, fadeDuration).SetEase(Ease.Linear).Play();
    }

    public void StartGame()
    {
        CreateNextPiece();
    }

    public bool CanDrop() { return canDropPiece; }

    void AllowDrop() { canDropPiece = true; }

    public void ResetGame()
    {
        TutorialScreen.ShowTuto = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #region Camera
    /*[ContextMenu("Set Initial Pos")]
    public void SetInitialPos()
    {
        if (!MainCamera) { Debug.LogError("[Pieces Manager]: Main Camera no ha sido referenciada en el objeto"); return; }
        InitialPos.Pos = MainCamera.transform.position;
        InitialPos.OrthoSize = MainCamera.orthographicSize;
    }

    [ContextMenu("Set End Position")]
    public void SetEndPos()
    {
        if (!MainCamera) { Debug.LogError("[Pieces Manager]: Main Camera no ha sido referenciada en el objeto"); return; }
        EndPos.Pos = MainCamera.transform.position;
        EndPos.OrthoSize = MainCamera.orthographicSize;
    }

    [ContextMenu("Show Positions")]
    public void ShowPositionsCamera()
    {
        Debug.Log("[Starting Position]:: Position: " + InitialPos.Pos + "\nOrthoSize: " + InitialPos.OrthoSize);
        Debug.Log("[End Position]:: Position: " + EndPos.Pos + "\nOrthoSize: " + EndPos.OrthoSize);
        Debug.Log("[SO] " + Positions.InitialPositionCam.Pos);
        Debug.Log("[SO] " + Positions.EndPositionCam.Pos);

    }

    [ContextMenu("Save Positions into SO")]
    public void SavePosIntoSO()
    {
        Positions.InitialPositionCam = InitialPos;
        Positions.EndPositionCam = EndPos;
        Debug.Log("[SO] " + Positions.InitialPositionCam.Pos);
        Debug.Log("[SO] " + Positions.EndPositionCam.Pos);
        Debug.Log("[SO] " + Positions.InitialPositionCam.OrthoSize);
        Debug.Log("[SO] " + Positions.EndPositionCam.OrthoSize);
    }*/
    #endregion

    #region INPUTS

    void OnDropPiece()
    {
        pieceDrop.DropPiece();
    }

    void OnRotatePiece()
    {
        float v = input.actions["RotatePiece"].ReadValue<float>();
        if (v > 0)
        {
            pieceDrop.TurnRight();
        }
        else if (v < 0)
        {
            pieceDrop.TurnLeft();
        }
    }
    
    #endregion
}

public class CameraPos
{
    public Vector3 Pos;
    public float OrthoSize;

    public CameraPos(Vector3 pos, float ortho)
    {
        Pos = pos;
        OrthoSize = ortho;
    }
}
