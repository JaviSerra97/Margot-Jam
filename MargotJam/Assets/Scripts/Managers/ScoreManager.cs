using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI MultiplierText;
    public GameObject FinalScorePanel;
    public GameObject Record;

    public GameObject scoreTextPrefab;
    public Vector3 scorePoint;

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

        UpdateUI(null);
    }

    public void PerfectPlacementScore(Vector3 pos)
    {
        AddPoints(ScoreForPerfect, pos);
    }

    public void SetMultiplier(int difficult)
    {
        _multiplier = (int) Mathf.Pow(2, difficult +1);
        UpdateUI(null);
    }

    public void AddPoints(int points, Vector3 pos)
    {
        var p = points * _multiplier;
        _score += p;

        //Instanciar texto con puntuacion
        var t = Instantiate(scoreTextPrefab, pos, Quaternion.identity);
        //Tween del texto

        //UpdateUI();
    }

    private void UpdateUI(GameObject text)
    {
        ScoreText.text = _score.ToString();
        MultiplierText.text = "x" + _multiplier;

        if (text) { Destroy(text); }
    }

    public int GetFinalScore()
    {
        UpdateUI(null);
        MultiplierText.transform.parent.gameObject.SetActive(false);
        FinalScorePanel.SetActive(true);
        Record.SetActive(false);
        ScoreText.GetComponent<Animator>().SetTrigger("End");
        return _score;
    }

}
