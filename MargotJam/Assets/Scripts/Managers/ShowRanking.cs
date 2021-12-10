using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowRanking : MonoBehaviour
{
    
    
    public TMP_Text text;
    private float _timer;
    public void SetRanking()
    {
        text.text = "Ranking: " + PlayfabManager.Instance.GetRanking();
    }

    void OnEnable() { SetRanking(); }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > 1)
        {
            SetRanking();
            _timer = 0;
        }
    }
}
