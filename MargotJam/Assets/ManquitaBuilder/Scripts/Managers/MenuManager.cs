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
    [SerializeField] private GameObject buttonsHeader;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject settingsHeader;
    [SerializeField] private float changePanelDuration;
    [SerializeField] private GameObject levelsPanel;
    [SerializeField] private float showLevelsDuration;
    private Vector2 levelsStartPos;

    [Header("Sound sliders ---------------------------------------------------")]
    [SerializeField] private float valueRate;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Image musicHandler;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Image soundHandler;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color deselectedColor;
    private Slider currentSlider;

    [Header("Level Info ------------------------------------------------------")] 
    [SerializeField] private Image levelSprite;
    [SerializeField] private TMP_Text levelText;
    
    private int selectedSceneIndex;
    private bool canInteract;

    private bool isOnSettings;
    private bool isOnLevels;

    private bool switchSlider = true;
    private bool addValue;
    private bool decreaseValue;
    
    public bool canGetInput;
    private bool onAnimation;
    private void Awake()
    {
        Instance = this;
        
        fadeScreen.gameObject.SetActive(true);

        PlayerInput pl = GetComponent<PlayerInput>();
        pl.actions["SwitchSlider"].canceled += ctx => switchSlider = true;
        pl.actions["SliderValue"].canceled += ctx => addValue = false;
        pl.actions["SliderValue"].canceled += ctx => decreaseValue = false;

        levelsStartPos = levelsPanel.transform.localPosition;
    }

    private void Start()
    {
        FadeOut();
    }

    private void Update()
    {
        if (addValue)
        {
            currentSlider.value += valueRate * Time.deltaTime;
        }

        if (decreaseValue)
        {
            currentSlider.value -= valueRate * Time.deltaTime;
        }
    }

    public void OnPlayButton()
    {
        if (canInteract)
        {
            EventSystem.current.SetSelectedGameObject(null);
            //ChangePanel(menuCanvas, levelsCanvas);
            isOnLevels = true;
            levelsPanel.transform.DOLocalMoveY(0, showLevelsDuration).SetEase(Ease.OutBounce);
        }
    }

    public void OnSettingsButton()
    {
        if (canInteract)
        {
            ChangePanel(buttonsPanel, settingsPanel);
            ChangePanel(buttonsHeader, settingsHeader);
            isOnSettings = true;
            //EventSystem.current.SetSelectedGameObject(null);
            SelectMusicSlider();
        }
    }

    void OnSettingsBack()
    {
        ChangePanel(settingsPanel, buttonsPanel);
        ChangePanel(settingsHeader, buttonsHeader);
        buttonsPanel.GetComponentInChildren<Button>().Select();
        isOnSettings = false;
    }

    public bool OnLevels()
    {
        return isOnLevels;
    }
    
    void OnLevelsBack()
    {
        //ChangePanel(levelsCanvas, menuCanvas);
        levelsPanel.transform.DOLocalMoveY(levelsStartPos.y, showLevelsDuration).SetEase(Ease.OutQuint);
        buttonsPanel.GetComponentInChildren<Button>().Select();
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
        onAnimation = true;
        
        openPanel.transform.localScale = Vector3.zero;
        openPanel.SetActive(true);
        
        Sequence seq = DOTween.Sequence().OnComplete(() => closePanel.SetActive(false));
        seq.Append(closePanel.transform.DOScale(0, changePanelDuration/2).SetEase(Ease.InBack));
        seq.Append(openPanel.transform.DOScale(1, changePanelDuration/2).SetEase(Ease.OutBack)).OnComplete(EndAnimation);
    }

    void EndAnimation()
    {
        onAnimation = false;
    }
    
    public void SetLevelIInfo(int i, Sprite s, string t)
    {
        selectedSceneIndex = i;
        levelSprite.sprite = s;
        levelText.text = t;
        
        Debug.Log("Selected level index: " + selectedSceneIndex);
    }

    void SelectMusicSlider()
    {
        musicHandler.color = selectedColor;
        soundHandler.color = deselectedColor;
        currentSlider = musicSlider;
    }

    void SelectSoundSlider()
    {
        musicHandler.color = deselectedColor;
        soundHandler.color = selectedColor;
        currentSlider = soundSlider;
    }
    
    #region INPUTS

    void OnBackToMenu()
    {
        if(onAnimation || !canGetInput){return;}
        
        if (isOnSettings)
        {
            OnSettingsBack();
        }

        if (isOnLevels)
        {
            OnLevelsBack();
        }
    }

    void OnSwitchSlider()
    {
        if(!isOnSettings || !switchSlider || onAnimation || !canGetInput){ return; }

        switchSlider = false;
        if (currentSlider == musicSlider)
        {
            SelectSoundSlider();
        }
        else
        {
            SelectMusicSlider();
        }
    }

    void OnSliderValue(InputValue value)
    {
        if(!isOnSettings || onAnimation || !canGetInput){return;}

        if (value.Get<float>() > 0)
        {
            addValue = true;
        }
        else if (value.Get<float>() < 0)
        {
            decreaseValue = true;
        }
        
    }

    #endregion
    
}
