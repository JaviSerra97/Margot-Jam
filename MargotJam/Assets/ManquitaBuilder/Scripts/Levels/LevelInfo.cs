using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Custom/Level")]
public class LevelInfo : ScriptableObject
{
    public Sprite Sprite;
    public string Info;
    public int sceneIndex;
}
