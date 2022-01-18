using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    
    [Header("Fade ------------------------------------------------------------")]
    [SerializeField] private Image fadeScreen;
    [SerializeField] private float fadeDuration;

    [Header("Canvas panels ---------------------------------------------------")]
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject settingsCanvas;
    [SerializeField] private GameObject levelsCanvas;
    [SerializeField] private float changePanelDuration;

    [Header("Buttons ---------------------------------------------------------")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button firstLevelButton;

    [Header("Level Info ------------------------------------------------------")] 
    [SerializeField] private Image levelSprite;
    [SerializeField] private TMP_Text levelText;
    
    private int selectedSceneIndex;
    private bool canInteract;

    private bool isOnSettings;
    private bool isOnLevels;

    private PlayerInput input;

    private void Awake()
    {
        Instance = this;
        
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
            firstLevelButton.Select();
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
        playButton.Select();
    }
    
    void FadeIn()
    {
        fadeScreen.DOFade(1, fadeDuration).SetEase(Ease.Linear).OnComplete(LoadScene).Play();
    }

    void FadeOut()
    {
        fadeScreen.DOFade(0, fadeDuration).SetEase(Ease.Linear).OnComplete(AllowInteractions).Play();
    }

    public void LoadLevel()
    {
        FadeIn();
    }

    void LoadScene()
    {
        //SceneManager.LoadScene(selectedSceneIndex);
        Debug.Log("Load scene: " + selectedSceneIndex);
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

    public void SetLevelIInfo(int i, Sprite s, string t)
    {
        selectedSceneIndex = i;
        levelSprite.sprite = s;
        levelText.text = t;
        
        Debug.Log("Selected level index: " + selectedSceneIndex);
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
