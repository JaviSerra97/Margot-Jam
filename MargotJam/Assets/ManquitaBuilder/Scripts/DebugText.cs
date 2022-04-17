using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugText : MonoBehaviour
{
    public static DebugText Instance;

    private TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();

        Instance = this;
    }

    public void SetText(string t)
    {
        text.text = t;
    }
}
