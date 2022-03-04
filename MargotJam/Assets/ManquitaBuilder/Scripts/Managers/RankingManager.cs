using System.Collections;
using System.Collections.Generic;
using nn.npln.leaderboard;
using TMPro;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance;

    public Transform firstPlayerParent;
    public Transform nearPlayersParent;
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

    public void SetFirstRankingData(IEnumerable<ScoreData> scoreData)
    {
        Debug.Log("Setting first");
        foreach (var data in scoreData)
        {
            firstPlayerParent.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text =
                data.GetRankData().GetRank().ToString();
            firstPlayerParent.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text =
                data.GetUserData().GetDisplayName();
            firstPlayerParent.transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text =
                data.GetScore().ToString();

            /*
            Debug.Log(string.Format("| {0,4} | {1,-22} | {2,7} | {3}",
                scoreData.GetRankData().GetRank(),
                scoreData.GetUserData().GetUserId(),
                scoreData.GetScore(),
                userId == scoreData.GetUserData().GetUserId() ? "** YOUR SCORE **" : ""));*/
        }
    }

    public void SetNearRankingData(IEnumerable<ScoreData> scoreData)
        {
            currentIndex = 0;
        
            foreach (var data in scoreData)
            {
                Debug.Log(data.GetRankData() + " - " + data.GetUserData() + " - " + data.GetScore());
                /*
                nearPlayersParent.transform.GetChild(currentIndex).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = data.GetRankData().GetRank().ToString();
                nearPlayersParent.transform.GetChild(currentIndex).GetChild(1).GetComponent<TMP_Text>().text = data.GetUserData().GetDisplayName();
                nearPlayersParent.transform.GetChild(currentIndex).GetChild(2).GetComponent<TMP_Text>().text = data.GetScore().ToString();
                */
                currentIndex++;
            }
    }

    public void SetNearRankingDataTest(int index, int rank, string id, long score)
    {
        nearPlayersParent.transform.GetChild(index).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = rank.ToString();
        nearPlayersParent.transform.GetChild(index).GetChild(1).GetComponent<TMP_Text>().text = id;
        nearPlayersParent.transform.GetChild(index).GetChild(2).GetComponent<TMP_Text>().text = score.ToString();
    }
}
