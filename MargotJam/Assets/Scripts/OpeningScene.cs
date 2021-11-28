using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningScene : MonoBehaviour
{
    public float nextSceneTime;
    public int sceneIndex;

    void Start()
    {
        Invoke(nameof(LoadNextScene), nextSceneTime);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
