using KrillAudio.Krilloud;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultManager : MonoBehaviour
{
    public static DifficultManager Instance;

    //layer1 = 0,1,2

    [Header("Snap")]
    public float SnapEasy = 0.6f;
    public float SnapHard = 0.35f;
    public int FailsForEasy = 2;

    [Header("Streak")]
    public int StreakMedium = 5;
    public int StreakHard = 10;

    [Header("Speed")]
    public float speedEasy = 4f;
    public float speedHard = 10f;

    private int _difficult; // 0 easy, 1 medium, 2 hard
    private int _failsInEasy;
    private int _streak;

    private KLAudioSource _source;

    private Vector3 _lastPos = new Vector3(0,0,0);
    private void Awake()
    {
        Instance = this;
        _difficult = 0;
        _failsInEasy = 0;
        _source = GetComponent<KLAudioSource>();
        _source.Play("game_ost");
    }

    public void PerfectPlacement(Vector3 pos)
    {
        _lastPos = pos;

        _streak++;

        if (_failsInEasy > 0)
            _failsInEasy--;

        if (_streak > StreakHard)
        {
            _difficult = 2;
            _source.SetFloatVar("layer1", 0);
            _source.SetFloatVar("layer2", 0);
            _source.SetFloatVar("layer3", 1);
        }
        else if (_streak > StreakMedium)
        {
            _difficult = 1;
            _source.SetFloatVar("layer1", 0);
            _source.SetFloatVar("layer2", 1);
            _source.SetFloatVar("layer3", 0);
        }
        else
        {
            _difficult = 0;
            _source.SetFloatVar("layer1", 1);
            _source.SetFloatVar("layer2", 0);
            _source.SetFloatVar("layer3", 0);
        }

        EvaluateDifficult();
        SendScore(true);
    }

    public void Fail()
    {
        if (_difficult > 0)
        {
            _difficult--;
            if (_difficult == 0)
                _streak = 0;
            else if (_difficult == 1)
                _streak = StreakMedium;
        }
        else
        {
            _failsInEasy++;
        }

        EvaluateDifficult();
        SendScore(false);
    }

    private void EvaluateDifficult()
    {
        if(_difficult == 0)
        {
            PieceMove.speed = speedEasy;
            if (_failsInEasy > FailsForEasy)
                PieceDrop.SNAP_THRESHOLD = SnapEasy;
            else
                PieceDrop.SNAP_THRESHOLD = SnapHard;
        }
        else if(_difficult == 1)
        {
            _failsInEasy = 0;
            PieceMove.speed = (speedEasy + speedHard)/ 2;
            PieceDrop.SNAP_THRESHOLD = SnapHard;
        }
        else if(_difficult == 2)
        {
            PieceMove.speed = speedHard;
            PieceDrop.SNAP_THRESHOLD = SnapHard;
        }

        
    }

    private void SendScore(bool perfect)
    {
        if(perfect)
        {
            ScoreManager.Instance.PerfectPlacementScore(_lastPos);
            ScoreManager.Instance.SetMultiplier(_difficult);
        }
        else
        {
            if (_difficult == 0 && _failsInEasy > 0)
                ScoreManager.Instance.SetMultiplier(-1);
            else
                ScoreManager.Instance.SetMultiplier(_difficult);
        }
    }
}
