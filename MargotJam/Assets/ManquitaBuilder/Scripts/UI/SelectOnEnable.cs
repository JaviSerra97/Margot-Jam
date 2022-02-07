using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectOnEnable : MonoBehaviour
{
    private bool init = false;
    
    private void Start()
    {
        GetComponent<Button>().Select();
    }

    private void OnEnable()
    {
        if (init)
        {
            GetComponent<Button>().Select();
        }
        else
        {
            init = true;
        }
    }
}
