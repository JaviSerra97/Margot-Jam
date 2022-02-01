using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManquitaBuilder
{
    public class InitMenu : MonoBehaviour
    {
        void Start()
        {
            MenuManager.Instance.ShowButtons();
        }
    }
}