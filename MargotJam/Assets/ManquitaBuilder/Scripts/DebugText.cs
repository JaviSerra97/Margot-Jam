using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : MonoBehaviour
{
    public static DebugText Instance;

    private TMP_Text text;

    private Image image;
    
    private void Awake()
    {
        text = GetComponent<TMP_Text>();

        image = GetComponent<Image>();
        
        Instance = this;
    }

    public void SetText(string t)
    {
        text.text = t;
    }

    public void SetColor(Color c)
    {
        image.color = c;
    }
}
