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

    public bool init = false;
    
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
        FsSaveDataPlayerPrefs.Instance.SetPlayerPrefs(unlocksList[0].id, 1);

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

        if (!init)
        {
            init = true;
        }
    }

    public void SetStatesOnReload()
    {
        if(!init) {return;}
        
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
    
    //Llamar desde ScoreManager al superar la puntuación.
    public void CompleteThisLevel(int sceneIndex)
    {
        unlocksList[sceneIndex].state = true;

        foreach (var l in unlocksList)
        {
            Debug.Log(l.id + ":" + l.state);
        }
    }

    public void SetLevelsState()
    {
        for (int i = 0; i < unlocksList.Count; i++)
        {
            var s = 0;
            if (unlocksList[i].state)
            {
                s = 1;
            }
            FsSaveDataPlayerPrefs.Instance.SetPlayerPrefs(unlocksList[i].id, s);
            
            LevelManager.Instance.UnlockLevel(i, unlocksList[i].state);
        }
    }
}
