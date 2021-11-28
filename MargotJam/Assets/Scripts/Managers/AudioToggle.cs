using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioToggle : MonoBehaviour
{
    [SerializeField] private GameObject soundImage;

    private bool sound = true;

    public void OnAudioToggle()
    {
        sound = !sound;

        soundImage.SetActive(sound);
        Debug.Log("Implementar aqui el mute");
    }
}
