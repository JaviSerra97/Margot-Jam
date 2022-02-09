using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [System.Serializable]
    public class Level
    {
        public LevelInfo levelInfo;
        public GameObject mapMarker;
        public bool unlocked = false;
    }

    [Header("Levels list -----------------------------------------------------")]
    public List<Level> listOfLevels;

    [Header("Level Info ------------------------------------------------------")] 
    [SerializeField] private Image levelSprite;
    [SerializeField] private TMP_Text levelName;
    [SerializeField] private TMP_Text levelInfo;

    private int levelIndex = 0;
    private int listIndex = 0;

    private GameObject currentMarker;
    private bool canPlay;

    private void Start()
    {
        ShowSelectedLevel();
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

    void OnSwitchLevel(InputValue value)
    {
        if(!MenuManager.Instance.OnLevels()){return;}
        
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
        if(!MenuManager.Instance.OnLevels()){return;}
        if (canPlay)
        {
            canPlay = false;
            SceneManager.LoadScene(levelIndex);
        }
    }
}
