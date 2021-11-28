using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class PlayfabManager : MonoBehaviour
{
    [Header("Playfab Variables")]
    public string GameVersion;

    [Header("References")]
    public GameObject LoadingPanel;

    [Header("Control Versiones")]
    [SerializeField]
    private bool _isTest = false;

    private static string PLAYFAB_PROJECTID_TEST = "9EE97";
    private static string PLAYFAB_PROJECTID_RELEASE = "47872";

    private bool _isItemPurchased;

    private int _chessPoints;

    private string _playfabID;

    private void Awake()
    {
        SetupPlayfabServer();
        LoadingPanel.SetActive(true);
    }

    void Start()
    {
        ServerLogin();
    }

    #region PlayFab Server

    private void SetupPlayfabServer()
    {
        PlayFabSettings.TitleId = (_isTest ? PLAYFAB_PROJECTID_TEST : PLAYFAB_PROJECTID_RELEASE);
    }

    public void LoadServerData()
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
        result => {
            LoadGameSetup(result.Data);
        },
        error => {
            Debug.Log("Getting titleData ERROR::" + error.ErrorMessage);
        }
);
    }

    private void LoadGameSetup(Dictionary<string, string> data)
    {
        SetPlayfabVersion(data["ClientVersion"]);
    }

    private void SetPlayfabVersion(string version)
    {
        GameVersion = version;
    }
    #endregion

    #region LOG IN
    private void ServerLogin()
    {
        Login(OnLoginSucces, OnLoginFailed);
    }

    private void Login(Action<LoginResult> onSuccess, Action<PlayFabError> onFail)
    {
        var request = new LoginWithCustomIDRequest
        {
            CreateAccount = true,
            CustomId = SystemInfo.deviceUniqueIdentifier
        };

        PlayFabClientAPI.LoginWithCustomID(request, onSuccess, onFail);
    }

    private void OnLoginSucces(LoginResult loginResult)
    {
        Debug.Log("User: " + loginResult.PlayFabId);
        _playfabID = loginResult.PlayFabId;
        LoadServerData();
        GetLeaderboard();
        UpdateHighscore(0);
        //LoadingPanel.SetActive(false);
        if (LoadingPanel) Destroy(LoadingPanel, 2f);
    }

    private void OnLoginFailed(PlayFabError error)
    {
        Debug.LogError("Login failed: ERROR:: " + error.ErrorMessage);
    }
    #endregion

    #region LEADERBOARDS
    public void UpdateHighscore(int amount)
    {
        var request = new UpdatePlayerStatisticsRequest()
        {
            Statistics = new List<StatisticUpdate>()
                {
                    new StatisticUpdate()
                    {
                        StatisticName = "Highscore",
                        Value = amount,
                    }
                }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request,
            (result) =>
            {
                Debug.Log("Highscore Updated");
                GetLeaderboard();
            },
            (error) =>
            {
                Debug.LogError("ERROR:: " + error.ErrorMessage);
            });
    }

    private void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest()
        {
            StatisticName = "Highscore",

        };

        PlayFabClientAPI.GetLeaderboard(request,
            (result) =>
            {
                foreach (PlayerLeaderboardEntry player in result.Leaderboard)
                {
                    if (player.PlayFabId == _playfabID)
                    {
                        //RankingText.text = (player.Position + 1).ToString();
                    }
                }
            },
            (error) =>
            {
                Debug.LogError("ERROR:: " + error.ErrorMessage);
            });
    }

    #endregion
}
