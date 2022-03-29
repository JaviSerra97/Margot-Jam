using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockManager : MonoBehaviour
{
    [System.Serializable]
    public class LevelState
    {
        public string id;
        public bool state;
    }
    
    public List<LevelState> unlocksList;
    public static UnlockManager Instance;

    private int currentLevelIndex;

    public static bool init = false;
    
    private void Awake()
    {
        #region SINGLETON
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        #endregion
    }
    
    public void SetStatesOnStart()
    {
        //Unlock first level
        string key = "Level_" + unlocksList[0].id;
        FsSaveDataPlayerPrefs.Instance.SetPlayerPrefs(key, 1);

        foreach (LevelState s in unlocksList)
        {
            //int state = FsSaveDataPlayerPrefs.Instance.LoadInt(s.id);
            int state = PlayerPrefs.GetInt(s.id);

            switch (state)
            {
                case 0:
                    s.state = false;
                    break;
                case 1:
                    s.state = true;
                    break;
            }
        }
        
        SetLevelsState();
    }

    public void SetStatesOnReload()
    {
        if (init)
        {
            foreach (LevelState s in unlocksList)
            {
                //int state = FsSaveDataPlayerPrefs.Instance.LoadInt(s.id);
                int state = PlayerPrefs.GetInt(s.id);

                switch (state)
                {
                    case 0:
                        s.state = false;
                        break;
                    case 1:
                        s.state = true;
                        break;
                }
                //Debug.Log(s.id + ": " + s.state);
            }

            SetLevelsState();
        }
        else
        {
            init = true;
        }
    }
    
    //Llamar desde ScoreManager al superar la puntuación.
    public void CompleteThisLevel(int sceneIndex)
    {
        string key = "Level_" + sceneIndex;
        FsSaveDataPlayerPrefs.Instance.SetPlayerPrefs(key, 1);
        
        //unlocksList[sceneIndex].state = true;
/*
        foreach (var l in unlocksList)
        {
            Debug.Log(l.id + ":" + l.state);
        }*/
    }

    public void SetLevelsState()
    {
        for (int i = 1; i < unlocksList.Count; i++)
        {
            /*
            var s = 0;
            if (unlocksList[i].state)
            {
                s = 1;
            }
            FsSaveDataPlayerPrefs.Instance.SetPlayerPrefs(unlocksList[i].id, s);
            
            LevelManager.Instance.UnlockLevel(i, unlocksList[i].state);
            */
            string key = "Level_" + i;
            LevelManager.Instance.UnlockLevel(i, key);
        }
    }
}
