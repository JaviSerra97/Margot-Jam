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
        //Cambiar ranking por el de NEX
        //text.text = "Ranking: " + PlayfabManager.Instance.GetRanking();
    }

    void OnEnable() { SetRanking(); Invoke(nameof(UpdateLeaderboards), 5f); }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > 1)
        {
            SetRanking();
            _timer = 0;
        }
    }

    private void UpdateLeaderboards()
    {
        // Obtener ranking de NEX
        //if(PlayfabManager.Instance?.GetRanking() == null)
        //    PlayfabManager.Instance?.GetLeaderboard();
    }
}
