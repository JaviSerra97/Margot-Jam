using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    [Header("Condicion de victoria")]
    public int ScoreToBeat = 200000;
    public static bool GameBeated = false;

    public static ScoreManager Instance;

    [Header("Referencias")]
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI MultiplierText;
    public GameObject FinalScorePanel;
    public GameObject Record;

    [Header("DoTween")]
    public GameObject scoreTextPrefab;
    public Transform scorePoint;
    public float scaleDuration;
    public float moveDuration;
    [Header("Canvas")]
    public GameObject canvas;
    [Header("Animator walking man")]
    public Animator SeniorAnim;
    public Animator ShadowAnim;
    [Header("Panels")]
    public GameObject scorePanel;
    public GameObject multiplierPanel;

    private const int ScoreForPerfect = 5000;
    private const int ScoreForPlace = 10000;

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
        var p = points;
        if(points > 0)
            p *= _multiplier;
        
        _score += p;

        //Instanciar texto con puntuacion
        var t = Instantiate(scoreTextPrefab, Camera.main.WorldToScreenPoint(pos), Quaternion.identity, canvas.transform);
        t.GetComponent<TMP_Text>().text = p.ToString();
        if (p > 0)
            t.GetComponent<TMP_Text>().color = Color.blue;
        else
            t.GetComponent<TMP_Text>().color = Color.red;

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

    public void GetFinalScore()
    {
        UpdateUI();
        
        StartCoroutine(ExtraScore());
    }

    private IEnumerator ExtraScore()
    {
        PieceDrop[] pieces = GameObject.FindObjectsOfType<PieceDrop>();

        foreach (PieceDrop piece in pieces)
        {
            if (piece.CheckNeighbour())
                AddPoints(ScoreForPlace, piece.transform.position);
            else
                AddPoints(-ScoreForPlace, piece.transform.position);
            yield return new WaitForSeconds(0.05f);
        }

        if (PlayfabManager.Instance)
        {
            PlayfabManager.Instance.UpdateHighscore(_score);
            //Debug.Log("Final score: " + _score);
        }
        SFX_Manager.Instance.PlayFanfarriaSFX();

        if (_score > ScoreToBeat)
        {
            Debug.Log("Prueba superada"); // ** Cambiar aqui la variable deseada ** //
            GameBeated = true;
        }

        MultiplierText.transform.parent.gameObject.SetActive(false);
        FinalScorePanel.SetActive(true);

        Record.SetActive(true);
        ScoreText.transform.parent.GetComponent<Animator>().SetTrigger("End");
    }

    public void OnBackButton(int index)
    {
        SceneManager.LoadScene(index);
    }

    
}