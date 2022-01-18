using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Image fadeScreen;
    [SerializeField] private float fadeDuration;

    private int selectedSceneIndex;
    private bool canInteract;

    private void Awake()
    {
        fadeScreen.gameObject.SetActive(true);
    }

    private void Start()
    {
        FadeOut();
    }

    public void OnPlayButton()
    {
        if (canInteract)
        {

        }
    }

    public void OnSettingsButton()
    {
        if (canInteract)
        {
            
        }
    }

    void FadeIn()
    {
        fadeScreen.DOFade(1, fadeDuration).SetEase(Ease.Linear).OnComplete(LoadScene).Play();
    }

    void FadeOut()
    {
        fadeScreen.DOFade(0, fadeDuration).SetEase(Ease.Linear).OnComplete(AllowInteractions).Play();
    }

    public void LoadSceneByIndex(int i)
    {
        selectedSceneIndex = i;
        FadeIn();
    }

    void LoadScene()
    {
        SceneManager.LoadScene(selectedSceneIndex);
    }

    void AllowInteractions()
    {
        canInteract = true;
    }

}
