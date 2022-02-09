using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Parent : MonoBehaviour
{
    public static NPC_Parent Instance;
    public float Speed = 5f;

    public RuntimeAnimatorController MozoAnimator;
    public RuntimeAnimatorController MozaAnimator;

    public float startPosX;
    public float endPosX;

    public GameObject PJ;
    public GameObject Shadow;

    private bool _movingRight;
    private float _currentSpeed;

    void Start()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(this);

        SelectAnimator();
        _movingRight = true;
        _currentSpeed = Speed;
    }

    private void SelectAnimator()
    {
        if (Random.value > 0.5)
        {
            PJ.GetComponent<Animator>().runtimeAnimatorController = MozoAnimator;
            Shadow.GetComponent<Animator>().runtimeAnimatorController = MozoAnimator;
        }
        else
        {
            PJ.GetComponent<Animator>().runtimeAnimatorController = MozaAnimator;
            Shadow.GetComponent<Animator>().runtimeAnimatorController = MozaAnimator;
        }
    }

    private void FlipSprites(bool value)
    {
        PJ.GetComponent<SpriteRenderer>().flipX = value;
        Shadow.GetComponent<SpriteRenderer>().flipX = value;
    }

    void Update()
    {
        float _dir = _movingRight ? 1 : -1;
        transform.Translate(new Vector3(1, 0, 0) * _dir * _currentSpeed * Time.deltaTime);

        if(_movingRight && transform.position.x > endPosX)
        {
            _movingRight = false;
            FlipSprites(!_movingRight);
        }
        if(!_movingRight && transform.position.x < startPosX)
        {
            _movingRight = true;
            FlipSprites(!_movingRight);
        }
    }

    public void SetAnimation()
    {
        CancelInvoke(nameof(ResetSpeed));
        PJ.GetComponent<Animator>().SetTrigger("Sorpresa");
        Shadow.GetComponent<Animator>().SetTrigger("Sorpresa");
        _currentSpeed = 0;
        Invoke(nameof(ResetSpeed), 1.2f);
    }

    public void ResetSpeed()
    {
        _currentSpeed = Speed;
    }
}
