using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI MultiplierText;

    private const int ScoreForPerfect = 2000;

    private int _score;
    private int _multiplier;

    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        _score = 0;
        _multiplier = 1;

        UpdateUI();
    }

    public void PerfectPlacementScore()
    {
        AddPoints(ScoreForPerfect);
    }

    public void SetMultiplier(int difficult)
    {
        _multiplier = (int) Mathf.Pow(2, difficult +1);
        UpdateUI();
    }

    public void AddPoints(int points)
    {
        _score += points * _multiplier;
        UpdateUI();
    }

    private void UpdateUI()
    {
        ScoreText.text = _score.ToString();
        MultiplierText.text = "x" + _multiplier;
    }

}
