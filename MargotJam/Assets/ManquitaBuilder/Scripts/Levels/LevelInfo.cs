using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Custom/Level")]
public class LevelInfo : ScriptableObject
{
    public string id;
    public Sprite Sprite;
    [TextArea()]
    public string Info;
    public int sceneIndex;
}
