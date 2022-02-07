using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectOnEnable : MonoBehaviour
{
    public bool init = false;
    
    private void Start()
    {
        //GetComponent<Button>().Select();
    }

    private void OnEnable()
    {
        /*if (init)
        {
            GetComponent<Button>().Select();
        }
        else
        {
            init = true;
        }*/
        GetComponent<Button>().Select();
        //GetComponent<SelectedButton>().SelectButton();
    }
}
