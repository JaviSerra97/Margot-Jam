using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelInfoSetter : MonoBehaviour, ISelectHandler
{
    public LevelInfo level;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }


    public void OnSelect(BaseEventData eventData)
    {
        MenuManager.Instance.SetLevelIInfo(level.sceneIndex, level.Sprite, level.Info);
    }
}
