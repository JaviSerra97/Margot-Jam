using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockManager : MonoBehaviour
{
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
    }

    public void SetCurrentLevel(int i)
    {
        currentLevelIndex = i;
    }

    public void UnlockNextLevel()
    {
        
    }
    
    
}
