using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    
    [Header("Panel")]
    public GameObject pausePanel;
    public Transform layout;
    public float tweenDuration;
    public Ease ease;
    private bool isPaused;
    
    void PauseGame()
    {
        if (DifficultManager.Instance && DifficultManager.Instance.GameEnded) return;

        Time.timeScale = 0;
        
        Sequence seq = DOTween.Sequence();
        seq.SetUpdate(true);
        seq.Append(pausePanel.transform.DOLocalMoveX(-960f, tweenDuration).SetEase(ease));
        seq.Append(pausePanel.transform.DOScaleX(1.15f, 0.1f));
        seq.Append(pausePanel.transform.DOScaleX(1f, 0.1f));
        layout.GetChild(0).GetComponent<Button>().Select();
        
        isPaused = true;
    }

    public void ResumeGame(bool doTween = true)
    {
        if (doTween)
        {
            pausePanel.transform.DOLocalMoveX(-1460f, tweenDuration).SetEase(ease).SetUpdate(true);
        }
        
        EventSystem.current.SetSelectedGameObject(null);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void RestartGame()
    {
        ResumeGame(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        ResumeGame(false);
        SceneManager.LoadScene(0);
    }
    
    void OnPause()
    {
        if (!isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame(true);
        }
    }
}
