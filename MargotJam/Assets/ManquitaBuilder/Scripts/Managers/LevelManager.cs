using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    [ExecuteInEditMode]
    [System.Serializable]
    public class Level
    {
        public string id;
        public LevelInfo levelInfo;
        public GameObject mapMarker;
        public bool unlocked = false;
        [TextArea()]
        public string lockedInfo;
    }

    public static LevelManager Instance;
    
    [Header("Levels list -----------------------------------------------------")]
    public List<Level> listOfLevels;

    [Header("Level Info ------------------------------------------------------")] 
    [SerializeField] private Image levelSprite;
    [SerializeField] private TMP_Text levelName;
    [SerializeField] private TMP_Text levelInfo;
    [SerializeField] private TMP_Text unlockInfo;

    public GameObject lockImage;

    [Header("Fade Screen -----------------------------------------------------")]
    public Image fadeImage;
    public float fadeDuration;

    public int levelIndex = 0;
    private int listIndex = 0;

    private GameObject currentMarker;
    public bool canPlay;
    public bool canGetInputs;
    

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        UnlockManager.Instance.SetStatesOnReload();
    }
    
    void ShowSelectedLevel()
    {
        var selectedLevel = listOfLevels[listIndex];
        
        SetLevelInfo(selectedLevel.levelInfo);

        if (currentMarker)
        {
            currentMarker.SetActive(false);
        }
        currentMarker = selectedLevel.mapMarker.transform.GetChild(0).gameObject;
        currentMarker.SetActive(true);

        canPlay = selectedLevel.unlocked;
        lockImage.SetActive(!canPlay);
        if (canPlay)
        {
            unlockInfo.text = "";
        }
        else
        {
            unlockInfo.text = selectedLevel.lockedInfo;
        }
    }
    
    void SetLevelInfo(LevelInfo lv)
    {
        levelName.text = lv.id;
        levelInfo.text = lv.Info;
        levelSprite.sprite = lv.Sprite;

        levelIndex = lv.sceneIndex;
    }

    void GetNextLevel()
    {
        if (listIndex < listOfLevels.Count - 1)
        {
            listIndex++;
        }
        else
        {
            listIndex = 0;
        }
    }

    void GetPreviousLevel()
    {
        if (listIndex > 0)
        {
            listIndex--;
        }
        else
        {
            listIndex = listOfLevels.Count - 1;
        }
    }

    #region INPUTS

    void OnSwitchLevel(InputValue value)
    {
        if(!MenuManager.Instance.OnLevels() || !canGetInputs){return;}
        
        float v = value.Get<float>();
        if (v > 0)
        {
            GetNextLevel();
        }
        else if (v < 0)
        {
            GetPreviousLevel();
        }
        
        ShowSelectedLevel();
    }

    void OnPlayLevel()
    {
        if(!MenuManager.Instance.OnLevels() || !canGetInputs){return;}
        if (canPlay)
        {
            canPlay = false;
            fadeImage.gameObject.SetActive(true);
            fadeImage.DOFade(1, fadeDuration).SetEase(Ease.Linear).Play();
            Invoke(nameof(LoadLevelScene), fadeDuration * 1.1f);
        }
    }
    #endregion

    private void LoadLevelScene()
    {
        SceneManager.LoadScene(levelIndex);
    }

    public void UnlockLevel(int i, string key)
    {
        listOfLevels[i].unlocked = PlayerPrefs.GetInt(key) == 1;
        
        Debug.Log(listOfLevels[i].id + ":" + listOfLevels[i].unlocked);
        
        ShowSelectedLevel();
    }
    
    [ContextMenu("Set ID")]
    public void SetIDInInspector()
    {
        foreach (Level l in listOfLevels)
        {
            l.id = l.levelInfo.id;
        }
    }
}
