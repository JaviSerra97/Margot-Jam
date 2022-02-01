using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static ManquitaBuilder.PiecesSequence;
using UnityEngine.UI;
using DG.Tweening;

namespace ManquitaBuilder
{

    public class PiecesManager : MonoBehaviour
    {
        public Image fadeImage;
        public float fadeDuration;

        public const float OFFSET_CAMERA = 3.5f;

        public Transform LeftCollider;
        public Transform RightCollider;
        public Camera MainCamera;

        [SerializeField] private List<PiecesSequence> sequences;
        [SerializeField] private Transform spawnPoint;
        public float dropDelay;
        private PiecesSequence chosenSequence;
        private bool canDropPiece;

        private int sequenceIndex, pieceIndex = 0;
        private bool _elevate = false;

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
            if (pieceIndex < chosenSequence.sequences[sequenceIndex].listOfPieces.Count)
            {
                Instantiate(chosenSequence.sequences[sequenceIndex].listOfPieces[pieceIndex], spawnPoint.position,
                    spawnPoint.rotation);
                pieceIndex++;

                canDropPiece = false;
                Invoke(nameof(AllowDrop), dropDelay);
            }
            else
            {
                CheckSequences();
            }
        }


        void CheckSequences()
        {
            if (sequenceIndex < chosenSequence.sequences.Count - 1)
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
            if (y_pos > spawnPoint.position.y - OFFSET_CAMERA && y_pos < 18f)
            {
                MainCamera.orthographicSize += 1;
                MainCamera.transform.position += new Vector3(0, 1.1f, 0);
                LeftCollider.position += new Vector3(-0.5f, 0, 0);
                RightCollider.position += new Vector3(0.5f, 0, 0);
                spawnPoint.position += new Vector3(0, 2f, 0);
            }
            else if (y_pos > spawnPoint.position.y - OFFSET_CAMERA && !_elevate)
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

        public bool CanDrop()
        {
            return canDropPiece;
        }

        void AllowDrop()
        {
            canDropPiece = true;
        }

        public void ResetGame()
        {
            TutorialScreen.ShowTuto = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }
}