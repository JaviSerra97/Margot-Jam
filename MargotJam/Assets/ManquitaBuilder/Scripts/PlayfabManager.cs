//using PlayFab;
//using PlayFab.ClientModels;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;
//using TMPro;

//public class PlayfabManager : MonoBehaviour
//{
//    public static PlayfabManager Instance;

//    [Header("Playfab Variables")]
//    public string GameVersion;

//    private bool _isItemPurchased;

//    private int _chessPoints;

//    private string _playfabID;

//    private string playerPosition;

//    private void Awake()
//    {
//        Instance = this;
//        //DontDestroyOnLoad(gameObject);
//    }

//    void Start()
//    {
//        ServerLogin();
//    }

//    #region PlayFab Server


//    public void LoadServerData()
//    {
//        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
//        result => {        },
//        error => {
//            Debug.Log("Getting titleData ERROR::" + error.ErrorMessage);
//        });
//    }
//    #endregion

//    #region LOG IN
//    private void ServerLogin()
//    {
//        Login(OnLoginSucces, OnLoginFailed);
//    }

//    private void Login(Action<LoginResult> onSuccess, Action<PlayFabError> onFail)
//    {
//        var request = new LoginWithCustomIDRequest
//        {
//            CreateAccount = true,
//            CustomId = SystemInfo.deviceUniqueIdentifier
//        };

//        PlayFabClientAPI.LoginWithCustomID(request, onSuccess, onFail);
//    }

//    private void OnLoginSucces(LoginResult loginResult)
//    {
//        Debug.Log("User: " + loginResult.PlayFabId);
//        _playfabID = loginResult.PlayFabId;
//        LoadServerData();
//        GetLeaderboard();
//    }

//    private void OnLoginFailed(PlayFabError error)
//    {
//        Debug.LogError("Login failed: ERROR:: " + error.ErrorMessage);
//    }
//    #endregion

//    #region LEADERBOARDS
//    public void UpdateHighscore(int amount)
//    {
//        Debug.Log("Score " + amount + " Enviado");
//        var request = new UpdatePlayerStatisticsRequest()
//        {
//            Statistics = new List<StatisticUpdate>()
//                {
//                    new StatisticUpdate()
//                    {
//                        StatisticName = "Highscore",
//                        Value = amount,
//                    }
//                }
//        };

//        PlayFabClientAPI.UpdatePlayerStatistics(request,
//            (result) =>
//            {
//                Debug.Log("Highscore Updated");
//                GetLeaderboard();
//            },
//            (error) =>
//            {
//                Debug.LogError("ERROR:: " + error.ErrorMessage);
//            });
//    }

//    public void GetLeaderboard()
//    {
//        var request = new GetLeaderboardRequest()
//        {
//            StatisticName = "Highscore",

//        };

//        PlayFabClientAPI.GetLeaderboard(request,
//            (result) =>
//            {
//                foreach (PlayerLeaderboardEntry player in result.Leaderboard)
//                {
//                    if (player.PlayFabId == _playfabID)
//                    {
//                        playerPosition = (player.Position + 1).ToString();
//                    }
//                }
//            },
//            (error) =>
//            {
//                Debug.LogError("ERROR:: " + error.ErrorMessage);
//            });
//    }

//    public string GetRanking() { return playerPosition; }

//    #endregion
//}
