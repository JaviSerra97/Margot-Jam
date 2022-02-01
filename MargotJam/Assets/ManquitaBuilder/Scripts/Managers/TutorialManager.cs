using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ManquitaBuilder
{
    public class TutorialManager : MonoBehaviour
    {
        public void LoadSceneByIndex(int index)
        {
            SceneManager.LoadScene(index);
        }
    }
}