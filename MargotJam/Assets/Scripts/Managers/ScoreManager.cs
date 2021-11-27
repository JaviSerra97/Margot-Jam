using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI MultiplierText;
    public GameObject FinalScorePanel;
    public GameObject Record;

    public GameObject scoreTextPrefab;
    public Transform scorePoint;
    public float scaleDuration;
    public float moveDuration;

    public GameObject canvas;

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
        UpdateMultiplier();
    }

    public void PerfectPlacementScore(Vector3 pos)
    {
        AddPoints(ScoreForPerfect, pos);
    }

    public void SetMultiplier(int difficult)
    {
        _multiplier = (int) Mathf.Pow(2, difficult +1);
        UpdateMultiplier();
    }

    public void AddPoints(int points, Vector3 pos)
    {
        var p = points * _multiplier;
        _score += p;

        //Instanciar texto con puntuacion
        var t = Instantiate(scoreTextPrefab, Camera.main.WorldToScreenPoint(pos), Quaternion.identity, canvas.transform);
        t.GetComponent<TMP_Text>().text = p.ToString();

        //Tween del texto
        Sequence seq = DOTween.Sequence();
        seq.Append(t.transform.DOScale(1, scaleDuration).SetEase(Ease.InQuad));
        seq.Append(t.transform.DOMove(scorePoint.position, moveDuration).SetEase(Ease.OutQuad));
        seq.OnComplete(UpdateUI);

        Destroy(t, scaleDuration + moveDuration + 0.1f);

        //UpdateUI();
    }

    private void UpdateUI()
    {
        ScoreText.text = _score.ToString();
    }

    void UpdateMultiplier()
    {
        MultiplierText.text = "x" + _multiplier;
    }

    public int GetFinalScore()
    {
        UpdateUI();
        MultiplierText.transform.parent.gameObject.SetActive(false);
        FinalScorePanel.SetActive(true);
        Record.SetActive(false);
        ScoreText.GetComponent<Animator>().SetTrigger("End");
        return _score;
    }

}
