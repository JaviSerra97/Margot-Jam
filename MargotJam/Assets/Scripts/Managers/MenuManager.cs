using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private CanvasGroup buttonsCanvasGroup;
    [SerializeField] private float fallDuration = 0.5f;
    [SerializeField] private float startYPos;
    [SerializeField] private Button playButton;
    private float playButtonPos;
    [SerializeField] private Button tutorialButton;
    private float tutorialButtonPos;
    [SerializeField] private Button exitButton;
    private float exitButtonPos;
    [SerializeField] private Button audioButton;
    private float audioButtonPos;
    [SerializeField] private Button creditsButton;
    private float creditsButtonPos;

    [Header("Credits")]
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private float creditsTweenDuration;
    [SerializeField] private float creditsLockDuration;
    private bool canExitCredits;
    private bool exitCreditsInput;
    private float timer;

    private void Awake()
    {
        playButtonPos = playButton.transform.position.y;
        playButton.transform.position += new Vector3(0, startYPos, 0);

        tutorialButtonPos = tutorialButton.transform.position.y;
        tutorialButton.transform.position += new Vector3(0, startYPos, 0);

        exitButtonPos = exitButton.transform.position.y;
        exitButton.transform.position += new Vector3(0, startYPos, 0);

        audioButtonPos = audioButton.transform.position.y;
        audioButton.transform.position += new Vector3(0, startYPos, 0);

        creditsButtonPos = creditsButton.transform.position.y;
        creditsButton.transform.position += new Vector3(0, startYPos, 0);

        creditsPanel.transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        if (canExitCredits)
        {
            if (Input.GetMouseButtonDown(0)) { exitCreditsInput = true; }

            timer += Time.deltaTime;
            if(timer > creditsLockDuration && exitCreditsInput)
            {
                ExitCredits();
            }

        }


    }

    [ContextMenu("Fall")]
    public void ShowButtons()
    {
        Sequence seq = DOTween.Sequence();

        //Caer audio, creditos, tuto
        seq.Append(audioButton.transform.DOMoveY(audioButtonPos, fallDuration).SetEase(Ease.InQuad));
        seq.Join(creditsButton.transform.DOMoveY(creditsButtonPos, fallDuration).SetEase(Ease.InQuad));
        seq.Join(tutorialButton.transform.DOMoveY(tutorialButtonPos, fallDuration).SetEase(Ease.InQuad));
            
        //Caer jugar
        seq.Append(playButton.transform.DOMoveY(playButtonPos, fallDuration).SetEase(Ease.InQuad));

        //Caer titulo 1
        //seq.Append

        //Caer titulo 2

        //Caer salir
        seq.Append(exitButton.transform.DOMoveY(exitButtonPos, fallDuration).SetEase(Ease.InQuad));

        seq.Play();
    }

    public void OnPlayButton()
    {
        if (PlayerPrefs.GetFloat("ShowTutorial") == 1)
        {
            //Jugar
            LoadSceneByIndex(2);
        }
        else
        {
            //Tutorial
            PlayerPrefs.SetFloat("ShowTutorial", 1);
            PlayerPrefs.Save();

            LoadSceneByIndex(1);
        }
    }

    void LoadSceneByIndex(int index) { SceneManager.LoadScene(index); }

    public void OnTutorialButton()
    {
        LoadSceneByIndex(1);
    }

    public void OnAudioToggle()
    {
        //Mutear/desmutear
    }

    public void OnExitButton()
    {
        Application.Quit();
    }

    public void OnCreditsButton()
    {
        //Mostrar creditos
        buttonsCanvasGroup.DOFade(0, creditsTweenDuration).SetEase(Ease.Linear).Play();
        creditsPanel.transform.DOScale(1, creditsTweenDuration).SetEase(Ease.Linear).Play();

        buttonsCanvasGroup.interactable = false;

        canExitCredits = true;
    }

    void ExitCredits()
    {
        creditsPanel.transform.DOScale(0, creditsTweenDuration).SetEase(Ease.Linear).Play();
        buttonsCanvasGroup.DOFade(1, creditsTweenDuration).SetEase(Ease.Linear).OnComplete(AllowButtonsInteraction).Play();

        canExitCredits = false;
        exitCreditsInput = false;
        timer = 0;
    }

    void AllowButtonsInteraction()
    {
        buttonsCanvasGroup.interactable = true;
    }
}
