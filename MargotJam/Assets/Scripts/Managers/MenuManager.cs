using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Image fadeScreen;

    [Header("Buttons")]
    [SerializeField] private CanvasGroup buttonsCanvasGroup;
    [SerializeField] private float fallDuration = 0.5f;
    [SerializeField] private float startYPos;
    [SerializeField] private GameObject mainButtons;
    private float mainButtonsPos;
    [SerializeField] private GameObject logoImage;
    private float logoImagePos;
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
        mainButtonsPos = mainButtons.transform.position.y;
        mainButtons.transform.position += new Vector3(0, startYPos, 0);

        logoImagePos = logoImage.transform.position.y;
        logoImage.transform.position += new Vector3(0, startYPos, 0);

        exitButtonPos = exitButton.transform.position.y;
        exitButton.transform.position += new Vector3(0, startYPos, 0);

        audioButtonPos = audioButton.transform.position.y;
        audioButton.transform.position += new Vector3(0, startYPos, 0);

        creditsButtonPos = creditsButton.transform.position.y;
        creditsButton.transform.position += new Vector3(0, startYPos, 0);

        creditsPanel.transform.localScale = Vector3.zero;
    }

    private void Start()
    {
        Invoke(nameof(ShowButtons), 2);
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

    public void ShowButtons()
    {
        Sequence seq = DOTween.Sequence();

        //Fade + Caer jugar, tuto, creditos
        seq.Append(fadeScreen.DOFade(0.5f, 0.5f));
        seq.Join(creditsButton.transform.DOMoveY(creditsButtonPos, fallDuration).SetEase(Ease.InQuad));
        seq.Join(mainButtons.transform.DOMoveY(mainButtonsPos, fallDuration).SetEase(Ease.InQuad));
            

        //Caer audio, salir
        seq.Append(exitButton.transform.DOMoveY(exitButtonPos, fallDuration).SetEase(Ease.InQuad));
        seq.Join(audioButton.transform.DOMoveY(audioButtonPos, fallDuration).SetEase(Ease.InQuad));

        //Caer logo
        seq.Append(logoImage.transform.DOMoveY(logoImagePos, fallDuration).SetEase(Ease.InQuad));

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
