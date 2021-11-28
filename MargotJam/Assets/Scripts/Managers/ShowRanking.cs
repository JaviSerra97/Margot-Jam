using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowRanking : MonoBehaviour
{
    public TMP_Text text;
    public void SetRanking()
    {
        text.text = "Ranking Mundial: " + PlayfabManager.Instance.GetRanking();
    }

    void OnEnable() { SetRanking(); }
}
