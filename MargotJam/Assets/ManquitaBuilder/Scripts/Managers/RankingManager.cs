using System.Collections;
using System.Collections.Generic;
using System.Linq;
using nn.npln.leaderboard;
using TMPro;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance;

    public List<RankingInfo> listOfPrefabs;

    private int currentIndex;

    //private List<LeaderboardClient.RankingData> currentData;
    
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

    void SetFirstRankingData(List<LeaderboardClient.RankingData> rankingData)
    {
        listOfPrefabs[0].SetInfo(rankingData[0]);
    }

    void SetNearRankingData(List<LeaderboardClient.RankingData> rankingData)
    {
        for (int i = 1; i < rankingData.Count; i++)
        {
            listOfPrefabs[i].SetInfo(rankingData[i]);
        }
    }

    public void SetRankings(List<LeaderboardClient.RankingData> rankingData)
    {
        SetFirstRankingData(rankingData);
        SetNearRankingData(rankingData);
    }
}

