using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;
    [Header("Audio Manager")]
    public Slider MusicSlider;
    public Slider SFXSlider;

    public AudioMixer Mixer;
    
    [Header("Buttons")]
    public AudioSource SFX_Button;

    public AudioClip Hover;
    public AudioClip Click;
    public AudioClip Slider;

    private float _sliderValue;

    public static string GENERAL_VOLUME_TAG = "GeneralVolume";
    public static string MUSIC_VOLUME_TAG = "MusicVolume";
    public static string SFX_VOLUME_TAG = "EffectVolume";

    private void Awake()
    {
        Instance = this;
        MusicSlider.onValueChanged.AddListener(setMusicVolume);
        SFXSlider.onValueChanged.AddListener(setEffectVolume);
    }

    void Start()
    {
        Invoke(nameof(SetSliders), 2f);
    }


    #region Audio Mixer
    public void setGeneralVolume(float v)
    {
        v = Mathf.Clamp(v, 0.001f, Mathf.Infinity);
        Mixer.SetFloat(GENERAL_VOLUME_TAG, Mathf.Log10(v) * 20);
    }

    public void setMusicVolume(float v)
    {
        v = Mathf.Clamp(v, 0.001f, Mathf.Infinity);
        Mixer.SetFloat(MUSIC_VOLUME_TAG, Mathf.Log10(v) * 20);
        FsSaveDataPlayerPrefs.Instance.SetPlayerPrefs(MUSIC_VOLUME_TAG,v);
    }

    public void setEffectVolume(float v)
    {
        v = Mathf.Clamp(v, 0.001f, Mathf.Infinity);
        Mixer.SetFloat(SFX_VOLUME_TAG, Mathf.Log10(v) * 20);
        FsSaveDataPlayerPrefs.Instance.SetPlayerPrefs(MUSIC_VOLUME_TAG, v);
    }
    #endregion

    #region SLIDERS

    private void SetSliders()
    {
        //Mixer.GetFloat(MUSIC_VOLUME_TAG, out _sliderValue); // Quitar comentario para volver a lo anterior
        _sliderValue = PlayerPrefs.GetFloat(MUSIC_VOLUME_TAG); // Comentar para volver a lo anterior
        Mixer.SetFloat(MUSIC_VOLUME_TAG, Mathf.Log10(_sliderValue) * 20); // Comentar para volver a lo anterior
        MusicSlider.value = LogConversion(_sliderValue);

        //Mixer.GetFloat(SFX_VOLUME_TAG, out _sliderValue); // Quitar comentario para volver a lo anterior
        _sliderValue = PlayerPrefs.GetFloat(SFX_VOLUME_TAG); // Comentar para volver a lo anterior
        Mixer.SetFloat(SFX_VOLUME_TAG, Mathf.Log10(_sliderValue) * 20); // Comentar para volver a lo anterior
        SFXSlider.value = LogConversion(_sliderValue);
    }


    private float LogConversion(float value)
    {
        return Mathf.Pow(10, value / 20);
    }
    #endregion

    public void PlayHover()
    {
        SFX_Button.PlayOneShot(Hover);
    }

    public void PlayClick()
    {
        SFX_Button.PlayOneShot(Click);
    }
    public void PlaySlider()
    {
        SFX_Button.PlayOneShot(Slider);
    }
}
