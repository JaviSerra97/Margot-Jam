using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Image fadeScreen;
    [SerializeField] private float fadeDuration;

    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject settingsCanvas;
    [SerializeField] private GameObject levelsCanvas;
    [SerializeField] private float changePanelDuration;

    private int selectedSceneIndex;
    private bool canInteract;

    private bool isOnSettings;
    private bool isOnLevels;

    private PlayerInput input;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        
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
            ChangePanel(menuCanvas, levelsCanvas);
            isOnLevels = true;
        }
    }

    public void OnSettingsButton()
    {
        if (canInteract)
        {
            ChangePanel(menuCanvas, settingsCanvas);
            isOnSettings = true;
        }
    }

    public void OnSettingsBack()
    {
        ChangePanel(settingsCanvas, menuCanvas);
        isOnSettings = false;
    }

    public void OnLevelsBack()
    {
        ChangePanel(levelsCanvas, menuCanvas);
        isOnLevels = false;
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

    void ChangePanel(GameObject closePanel, GameObject openPanel)
    {
        closePanel.transform.DOScale(0, changePanelDuration).SetEase(Ease.Linear).Play();
        openPanel.transform.DOScale(1, changePanelDuration).SetEase(Ease.Linear).Play();
    }

    #region INPUTS

    void OnBackToMenu()
    {
        if (isOnSettings)
        {
            OnSettingsBack();
        }

        if (isOnLevels)
        {
            OnLevelsBack();
        }
    }

    #endregion
    
}
