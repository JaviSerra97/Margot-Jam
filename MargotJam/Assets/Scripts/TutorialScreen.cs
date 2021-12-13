using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class TutorialScreen : MonoBehaviour
{
    public TMP_Text StartText;
    public float FadeDuration;

    public float minTutorialTime;

    public PiecesManager manager;

    private bool canCloseTutorial = false;
    
    private void Start()
    {
        StartText.DOFade(0, FadeDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InQuad).Play();
        
        Invoke(nameof(AllowCloseTutorial), minTutorialTime);
    }

    public void StartGame()
    {
        if (canCloseTutorial)
        {
            CanvasGroup g = GetComponent<CanvasGroup>();
            g.interactable = false;
            g.DOFade(0, 0.5f).OnComplete(HideScreen);
        }
    }

    void HideScreen()
    {
        gameObject.SetActive(false);
        manager.StartGame();
    }

    void AllowCloseTutorial()
    {
        canCloseTutorial = true;
    }
}
