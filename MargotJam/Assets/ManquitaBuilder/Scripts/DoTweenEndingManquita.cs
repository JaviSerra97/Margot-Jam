using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoTweenEndingManquita : MonoBehaviour
{
    private float _dotweenDur = 0.85f;

    private Vector3 _pos;
    private Animator _anim;

    void Start()
    {
        _pos = transform.position;
        _anim = GetComponent<Animator>();
        transform.position += new Vector3(0, 50, 0);
        float _delay = (_pos.x + _pos.y * 10) * 0.02f;
        _delay += 1.5f;
        Invoke(nameof(DotweenSeq), _delay);
        Invoke(nameof(Animate), _delay + _dotweenDur);
    }

    void DotweenSeq()
    {
        transform.DOMoveY(_pos.y, _dotweenDur);
    }

    void Animate()
    {
        _anim.SetTrigger("Fall");
    }
    
    public void MenuAppear()
    {
        //MenuManager.Instance.ShowButtons();
    }
}
