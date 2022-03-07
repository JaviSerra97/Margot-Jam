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
            listOfPrefabs[0].SetInfo(data.GetRankData().GetRank().ToString(), data.GetUserData().GetDisplayName(), data.GetScore().ToString());
        }
    }

    public void SetNearRankingData(IEnumerable<ScoreData> scoreData)
    {
        int i = 1;
        
        foreach (var data in scoreData)
        {
            Debug.Log("Index: " + i);
            /*nearPlayersParent.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text =
                data.GetRankData().GetRank().ToString();
            nearPlayersParent.transform.GetChild(i).GetChild(1).GetComponent<TMP_Text>().text =
                data.GetUserData().GetDisplayName();
            nearPlayersParent.transform.GetChild(i).GetChild(2).GetComponent<TMP_Text>().text =
                data.GetScore().ToString();*/

            listOfPrefabs[i].SetInfo(data.GetRankData().GetRank().ToString(), data.GetUserData().GetDisplayName(), data.GetScore().ToString());
            
            i++;
        }

    }
}

