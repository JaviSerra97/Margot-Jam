using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;
    [Header("Pause Panel")]
    public GameObject pausePanel;
    public Transform layout;
    public float tweenDuration;
    public Ease ease;
    [Header("FadeImage")]
    public Image FadeImage;
    public float FadeDuration;
    [Header("Final Score Text")]
    public TextMeshProUGUI FinalScore_2; 
    
    private bool _isPaused;
    private bool _isLoading;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        _isLoading = false;
        FadeImage.gameObject.SetActive(true);
        FadeImage.DOFade(0, FadeDuration);
    }

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
        
        _isPaused = true;
    }

    public void ResumeGame(bool doTween = true)
    {
        if (doTween)
        {
            pausePanel.transform.DOLocalMoveX(-1460f, tweenDuration).SetEase(ease).SetUpdate(true);
        }
        
        EventSystem.current.SetSelectedGameObject(null);
        Time.timeScale = 1;
        _isPaused = false;
    }

    public void RestartGame()
    {
        if (_isLoading) return;

        FadeImage.DOFade(1f, FadeDuration).SetUpdate(true);
        StartCoroutine(LoadSameScene());
    }

    private IEnumerator LoadSameScene()
    {
        _isLoading = true;
        yield return new WaitForSecondsRealtime(FadeDuration);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        ResumeGame(false); 
    }

    public void ExitGame()
    {
        if (_isLoading) return;

        FadeImage.DOFade(1f, FadeDuration).SetUpdate(true);
        StartCoroutine(LoadMainMenu());
    }
    
    private IEnumerator LoadMainMenu() 
    {
        _isLoading = true;
        yield return new WaitForSecondsRealtime(FadeDuration);
        SceneManager.LoadScene(0); 
        ResumeGame(false); 
    }

    void OnPause()
    {
        if (!_isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame(true);
        }
    }
}
