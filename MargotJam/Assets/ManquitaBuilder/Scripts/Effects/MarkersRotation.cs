using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MarkersRotation : MonoBehaviour
{
    public float duration = 5f;

    private void Start()
    {
        transform.DORotate(new Vector3(0, 180, 360), duration, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }
}