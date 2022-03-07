using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankingInfo : MonoBehaviour
{
    public TMP_Text rankingText;
    public TMP_Text nameText;
    public TMP_Text scoreText;

    public void SetInfo(string ranking, string displayName, string score)
    {
        rankingText.text = ranking;
        nameText.text = displayName;
        scoreText.text = score;
    }
}
