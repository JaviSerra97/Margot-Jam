using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    public AudioSource Music;
    public Image ButtonImage;
    private bool _muted = false;
    public GameObject SoundGO;
    public void SwitchMute()
    {
        _muted = !_muted;
        SoundGO.SetActive(_muted);
        SFX_Manager.Instance.MuteSounds(_muted);
        Music.mute = _muted;

        /*if (_muted)
            ButtonImage.color = Color.red;
        else
            ButtonImage.color = Color.white;*/
    }
}
