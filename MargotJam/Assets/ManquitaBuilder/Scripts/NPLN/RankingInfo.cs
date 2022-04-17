using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankingInfo : MonoBehaviour
{
    public TMP_Text rankingText;
    public TMP_Text nameText;
    public TMP_Text scoreText;

    public Color userColor;
    public Color normalColor;
    
    public void SetInfo(LeaderboardClient.RankingData data)
    {
        rankingText.text = data.rank.ToString();
        nameText.text = data.displayName;
        scoreText.text = data.score.ToString("N0");

        if (data.isUser)
        {
            nameText.color = userColor;
        }
        else
        {
            nameText.color = normalColor;
        }
    }
}
