using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Manager : MonoBehaviour
{
    public static SFX_Manager Instance;

    public AudioClip HitSFX, FailSFX, BounceSFX, HoverSFX, ClickSFX, CounterSFX, Fanfarria;
    public List<AudioClip> PerfectSFX;

    private AudioSource _source;


    private void Awake()
    {
        Instance = this;
        _source = GetComponent<AudioSource>();
    }

    public void PlayHitSound()
    {
        _source.PlayOneShot(HitSFX, 0.3f);
    }

    public void PlayFailSound()
    {
        _source.PlayOneShot(FailSFX);
    }

    public void PlayBounceSFX()
    {
        _source.PlayOneShot(BounceSFX, 0.3f);
    }

    public void PlayHoverSFX()
    {
        _source.PlayOneShot(HoverSFX);
    }

    public void PlayClickSFX()
    {
        _source.PlayOneShot(ClickSFX);
    }

    public void PlayPerfectSFX()
    {
        _source.PlayOneShot(PerfectSFX[Random.Range(0, PerfectSFX.Count)]);
    }

    public void PlayFanfarriaSFX()
    {
        _source.PlayOneShot(Fanfarria);
    }

    public void PlayCounterSFX()
    {
        _source.PlayOneShot(CounterSFX);
    }
    
    public void MuteSounds(bool value)
    {
        _source.mute = value;
    }

}
