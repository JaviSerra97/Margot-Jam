using System;
using System.Collections;
using System.Collections.Generic;
using OWO;
using UnityEngine;

public class OWOManager : MonoBehaviour
{
    public static OWOManager Instance;
    
    private OWOController controller;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(this);

        controller = new OWOController();
    }

    private void Start()
    {
        controller.FindServersInLANAndConnect();
    }

    public void SendSensation(SensationId ID, OWOMuscle muscle)
    {
        controller.SendSensation(ID, muscle);
    }
}
