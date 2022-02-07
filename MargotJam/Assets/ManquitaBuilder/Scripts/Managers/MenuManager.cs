using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    
    [Header("Fade ------------------------------------------------------------")]
    [SerializeField] private Image fadeScreen;
    [SerializeField] private float fadeDuration;

    [Header("Canvas panels ---------------------------------------------------")]
    [SerializeField] private GameObject buttonsPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject levelsPanel;
    [SerializeField] private float changePanelDuration;

    [Header("Level Info ------------------------------------------------------")] 
    [SerializeField] private Image levelSprite;
    [SerializeField] private TMP_Text levelText;
    
    private int selectedSceneIndex;
    private bool canInteract;

    private bool isOnSettings;
    private bool isOnLevels;
    
    private void Awake()
    {
        Instance = this;
        
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
            //ChangePanel(menuCanvas, levelsCanvas);
            //isOnLevels = true;
            //firstLevelButton.Select();
        }
    }

    public void OnSettingsButton()
    {
        if (canInteract)
        {
            ChangePanel(buttonsPanel, settingsPanel);
            isOnSettings = true;
        }
    }

    void OnSettingsBack()
    {
        ChangePanel(settingsPanel, buttonsPanel);
        isOnSettings = false;
    }

    void OnLevelsBack()
    {
        //ChangePanel(levelsCanvas, menuCanvas);
        //isOnLevels = false;
        //playButton.Select();
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
        openPanel.transform.localScale = Vector3.zero;
        openPanel.SetActive(true);
        
        Sequence seq = DOTween.Sequence().OnComplete(() => closePanel.SetActive(false));
        seq.Append(closePanel.transform.DOScale(0, changePanelDuration/2).SetEase(Ease.InBack));
        seq.Append(openPanel.transform.DOScale(1, changePanelDuration/2).SetEase(Ease.OutBack));
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
