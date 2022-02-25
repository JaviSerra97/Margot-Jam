using System.Collections;
using System.Collections.Generic;
using nn.npln.leaderboard;
using TMPro;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance;

    public Transform textsParent;
    private int currentIndex;
    
    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void SetRankingData(IEnumerable<ScoreData> scoreData)
    {
        currentIndex = 0;
        
        foreach (var data in scoreData)
        {
            transform.GetChild(currentIndex).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = data.GetRankData().GetRank().ToString();
            transform.GetChild(currentIndex).GetChild(1).GetComponent<TMP_Text>().text = data.GetUserData().GetDisplayName();
            transform.GetChild(currentIndex).GetChild(2).GetComponent<TMP_Text>().text = data.GetScore().ToString();
            
            /*
            Debug.Log(string.Format("| {0,4} | {1,-22} | {2,7} | {3}",
                scoreData.GetRankData().GetRank(),
                scoreData.GetUserData().GetUserId(),
                scoreData.GetScore(),
                userId == scoreData.GetUserData().GetUserId() ? "** YOUR SCORE **" : ""));*/

            currentIndex++;
        }
    }
}
