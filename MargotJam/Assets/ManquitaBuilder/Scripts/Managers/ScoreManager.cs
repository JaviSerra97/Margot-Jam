using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using nn.npln.leaderboard;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    private const int POINTS_BITS = 101;
    private const float POINTS_BIT_TIME = 0.002f;
    public const float SCALE_TEXT = 1.65f;

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
        NPC_Parent.Instance.SetAnimation();
    }

    public void SetMultiplier(int difficult)
    {
        _multiplier = (int) Mathf.Pow(2, difficult +1);
        UpdateMultiplier();
    }

    public void AddPoints(int points, Vector3 pos)
    {
        var p = points;
        if (points > 0)
            p *= _multiplier;
            
        //Instanciar texto con puntuacion
        var t = Instantiate(scoreTextPrefab, Camera.main.WorldToScreenPoint(pos), Quaternion.identity, canvas.transform);
        t.GetComponent<TMP_Text>().text = p.ToString();
        if (p > 0)
            t.GetComponent<TMP_Text>().color = Color.white;
        else
            t.GetComponent<TMP_Text>().color = Color.red;

        ////Tween del texto
        //Sequence seq = DOTween.Sequence();
        //seq.Append(t.transform.DOScale(1, scaleDuration * Random.Range(0.8f, 1.2f)).SetEase(Ease.InQuad));
        //seq.Append(t.transform.DOMove(scorePoint.position, moveDuration * Random.Range(0.8f, 1.2f)).SetEase(Ease.OutQuad));
        //seq.Join(t.transform.DOBlendableLocalMoveBy(new Vector3(Random.Range(-1, 1), 0, 0), moveDuration));
        //Destroy(t, scaleDuration + moveDuration + 0.1f);
        //seq.OnComplete(UpdateUI);

        // Nuevo Tween
        StartCoroutine(TextVFX(t));
        if(p>=0)
            StartCoroutine(PointsVFX_Positive(p));
        else
            StartCoroutine(PointsVFX_Negative(p));


        //UpdateUI();
    }

    private IEnumerator PointsVFX_Positive(int points)
    {
        int AllPoints = points;
        //float scale = 1;

        while (points > POINTS_BITS)
        {
            points -= POINTS_BITS;
            _score += POINTS_BITS;
            //text.GetComponent<TMP_Text>().text = points.ToString();
            //scale = Mathf.Max(0.65f, points / AllPoints);
            //text.transform.localScale = Vector3.one * scale;
            UpdateUI();
            yield return new WaitForSeconds(POINTS_BIT_TIME);
        }
        _score += points;
        UpdateUI();
        //yield return new WaitForSeconds(POINTS_BIT_TIME);
        //Destroy(text);
    }

    private IEnumerator PointsVFX_Negative(int points)
    {
        int AllPoints = points;
        //float scale = 1;

        while (points < -POINTS_BITS)
        {
            points += POINTS_BITS;
            _score -= POINTS_BITS;
            //text.GetComponent<TMP_Text>().text = points.ToString();
            //scale = Mathf.Max(0.9f, points / AllPoints);
            //text.transform.localScale = Vector3.one * scale;
            UpdateUI();
            yield return new WaitForSeconds(POINTS_BIT_TIME);
        }
        _score -= points;
        UpdateUI();
        //yield return new WaitForSeconds(POINTS_BIT_TIME);
        //Destroy(text);
    }

    private IEnumerator TextVFX(GameObject text)
    {
        text.transform.localScale = Vector3.zero;
        text.transform.DOScale(SCALE_TEXT, 0.35f);
        yield return new WaitForSeconds(0.7f);
        text.transform.DOScale(0f, 0.35f);
        yield return new WaitForSeconds(0.35f);
        Destroy(text);

    }

    private void UpdateUI()
    {
        _score = Mathf.Max(0, _score);
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
            Debug.Log(piece.name);
            if (piece.CheckNeighbour())
                AddPoints(ScoreForPlace, piece.transform.position);
            else
                AddPoints(-ScoreForPlace, piece.transform.position);
            yield return new WaitForSeconds(0.18f);
        }

        yield return new WaitForSeconds(moveDuration * 1.5f);

        LeaderboardClient.Instance.SetLeaderboardScore(_score, SceneManager.GetActiveScene().buildIndex);

        SFX_Manager.Instance.PlayFanfarriaSFX();

        if (_score > ScoreToBeat)
        {
            Debug.Log("Prueba superada"); // ** Cambiar aqui la variable deseada ** //
            GameBeated = true;
            UnlockManager.Instance?.CompleteThisLevel(SceneManager.GetActiveScene().buildIndex);
        }

        MultiplierText.transform.parent.gameObject.SetActive(false);
        FinalScorePanel.SetActive(true);

        //Record.SetActive(true);
        ScoreText.transform.parent.GetComponent<Animator>().SetTrigger("End");
    }

    public void OnBackButton(int index)
    {
        SceneManager.LoadScene(index);
    }

    
}