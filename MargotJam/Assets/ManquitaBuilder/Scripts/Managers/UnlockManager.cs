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
        SetStatesOnStart();
    }

    void SetStatesOnStart()
    {
        //Unlock first level
        PlayerPrefs.SetInt(unlocksList[0].id, 1);
        
        foreach (LevelState s in unlocksList)
        {
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
    }

    //Llamar desde ScoreManager al superar la puntuación.
    public void CompleteThisLevel(int sceneIndex)
    {
        unlocksList[sceneIndex].state = true; //El indice 0 del menu se compensa con el indice 0 de la lista
    }

    public void SetLevelsState(LevelManager manager)
    {
        for (int i = 0; i < unlocksList.Count; i++)
        {
            var s = 0;
            if (unlocksList[i].state)
            {
                s = 1;
            }
            PlayerPrefs.SetInt(unlocksList[i].id, s);
            
            manager.UnlockLevel(i, unlocksList[i].state);
        }
    }
}
