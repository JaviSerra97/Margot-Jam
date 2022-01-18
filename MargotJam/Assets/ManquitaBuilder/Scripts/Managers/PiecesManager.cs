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
    public Image fadeImage;
    public float fadeDuration;

    public const float OFFSET_CAMERA = 3.5f;

    public Transform LeftCollider;
    public Transform RightCollider;
    public Camera MainCamera;
    public float MaxHeightY = 80f;

    [SerializeField] private List<PiecesSequence> sequences;
    [SerializeField] private Transform spawnPoint;
    public float dropDelay;
    private PiecesSequence chosenSequence;
    private bool canDropPiece;

    private int sequenceIndex, pieceIndex = 0;
    private bool _elevate = false;

    private CameraPos InitialPos;
    private CameraPos EndPos;

    private PieceDrop pieceDrop;
    private PlayerInput input;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        SetSequence();
    }

    void SetSequence()
    {
        int rand = Random.Range(0, sequences.Count);
        chosenSequence = sequences[rand];

        var init = chosenSequence.initialPrefab;

        Instantiate(init, init.transform.position, init.transform.rotation);

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
        if(y_pos > spawnPoint.position.y - OFFSET_CAMERA && y_pos < MaxHeightY)
        {
            float _valueLerp = y_pos / MaxHeightY;
            MainCamera.orthographicSize = Mathf.Lerp(InitialPos.OrthoSize, EndPos.OrthoSize,_valueLerp);
            MainCamera.transform.position = new Vector3(Mathf.Lerp(InitialPos.Pos.x, EndPos.Pos.x, _valueLerp), Mathf.Lerp(InitialPos.Pos.y, EndPos.Pos.y, _valueLerp), Mathf.Lerp(InitialPos.Pos.z, EndPos.Pos.z, _valueLerp)) ;
            LeftCollider.position = MainCamera.ScreenToWorldPoint(new Vector3(MainCamera.pixelWidth * 0.1f, MainCamera.pixelHeight * 0.5f, 10)); // Mitad de pantalla pegado a la izquierda
            RightCollider.position = MainCamera.ScreenToWorldPoint(new Vector3(MainCamera.pixelWidth * 0.9f, MainCamera.pixelHeight * 0.5f, 10)); // Mitad de pantalla pegado a la derecha
            spawnPoint.position = MainCamera.ScreenToWorldPoint(new Vector3(MainCamera.pixelWidth * 0.5f, MainCamera.pixelHeight * 0.9f, 10)); // Mitad de la pantalla en el centro arriba
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
    [ContextMenu("Set Initial Pos")]
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
    #endregion

    #region INPUTS

    void OnDropPiece()
    {
        pieceDrop.DropPiece();
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
