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

    public Animator SeniorAnim;
    public Animator ShadowAnim;

    public GameObject scorePanel;
    public GameObject multiplierPanel;

    private const int ScoreForPerfect = 2000;
    private const int ScoreForPlace = 500;

    private int _score;
    private int _multiplier;

    private int _scoreSuelo, _scorePared, _scoreDetalle, _scoreTecho;

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
        SeniorAnim.SetTrigger("Sorpresa");
        ShadowAnim.SetTrigger("Sorpresa");
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
        seq.Append(t.transform.DOScale(1, scaleDuration * Random.Range(0.8f, 1.2f)).SetEase(Ease.InQuad));
        seq.Append(t.transform.DOMove(scorePoint.position, moveDuration * Random.Range(0.8f, 1.2f)).SetEase(Ease.OutQuad));
        seq.Join(t.transform.DOBlendableLocalMoveBy(new Vector3(Random.Range(-1, 1), 0, 0), moveDuration));
        seq.OnComplete(UpdateUI);

        Destroy(t, scaleDuration + moveDuration + 0.1f);

        //UpdateUI();
    }

    private void UpdateUI()
    {
        ScoreText.text = _score.ToString();
        scorePanel.transform.DOPunchScale(Vector3.one * 0.15f, 0.25f);
    }

    void UpdateMultiplier()
    {
        MultiplierText.text = "x" + _multiplier;
        multiplierPanel.transform.DOPunchScale(Vector3.one * 0.25f, 0.25f);
    }

    public int GetFinalScore()
    {
        UpdateUI();
        
        PieceDrop[] pieces = GameObject.FindObjectsOfType<PieceDrop>();

        _scoreDetalle = 0;
        _scorePared = 0;
        _scoreSuelo = 0;
        _scoreTecho = 0;

        foreach (PieceDrop piece in pieces)
        {
            switch (piece.ubicacion)
            {
                case PieceDrop.UbicacionSprite.Suelo:
                    if (piece.transform.position.y < -1)
                        _scoreSuelo += ScoreForPlace;
                    break;
                case PieceDrop.UbicacionSprite.Pared:
                    if (piece.transform.position.y > -1 && piece.transform.position.y < 3)
                        _scorePared += ScoreForPlace;
                    break;
                case PieceDrop.UbicacionSprite.Detalles:
                    if (piece.transform.position.y > -2 && piece.transform.position.y < 4 && piece.transform.position.x > -6 && piece.transform.position.x < 6)
                        _scoreDetalle += ScoreForPlace;
                        break;
                case PieceDrop.UbicacionSprite.Techo:
                    if (piece.transform.position.y > 3)
                        _scoreTecho += ScoreForPlace;
                    break;
            }
        }
        StartCoroutine(ExtraScore());

        MultiplierText.transform.parent.gameObject.SetActive(false);
        FinalScorePanel.SetActive(true);
        
        Record.SetActive(false);
        ScoreText.transform.parent.GetComponent<Animator>().SetTrigger("End");
        
        return _score;
    }

    private IEnumerator ExtraScore()
    {
        yield return new WaitForSeconds(2f);
        AddPoints(_scoreSuelo, new Vector3(0, -3, 0));
        yield return new WaitForSeconds(0.5f);
        AddPoints(_scorePared, new Vector3(3, 0, 0));
        yield return new WaitForSeconds(0.5f);
        AddPoints(_scoreTecho, new Vector3(0, 6, 0));
        yield return new WaitForSeconds(0.5f);
        AddPoints(_scoreDetalle, new Vector3(-3, 0, 0));
        yield return new WaitForSeconds(0.5f);
        if(PlayfabManager.Instance)
            PlayfabManager.Instance.UpdateHighscore(_score);
    }
}