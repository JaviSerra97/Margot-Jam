using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using nn.npln;
using nn.npln.leaderboard;
public class LeaderboardClient : MonoBehaviour
{
    public static LeaderboardClient Instance;

    private IEnumerable<ScoreData> scoreData;

    private UserContext userContext;
    #region NPLN Logic
    //-------------------------------------------------------------------------------------------------
    // 定数定義
    //-------------------------------------------------------------------------------------------------

    /// <summary>
    /// サンプルの接続先を表すテナントIDソースです。
    /// 実際のアプリケーションコードでは、アプリケーションごとに割り当てられたテナントIDソースに置き換えてください。
    /// </summary>
    private const string TENANT_ID_SOURCE = nns.npln.Common.DEFAULT_TENANT_ID_SOURCE;

    /// <summary>
    /// サンプルで使用するリーダーボードのカテゴリタイプ名です。
    /// 本サンプルでは、NPLNダッシュボードでカテゴリタイプを1つも登録していないときにのみ使用可能な
    /// サンプル用のカテゴリタイプを利用します。詳細はNPLNプログラミングマニュアルを参照してください。
    /// </summary>
    private const string CATEGORY_TYPE_NAME = "Highscore";

    /// <summary>
    /// サンプルで使用するリーダーボードのカテゴリIDです。
    /// 詳細はNPLNプログラミングマニュアルを参照してください。
    /// </summary>
    private const int CATEGORY_ID = 0;

    /// <summary>
    /// サンプルで使用するチャート名です。
    /// 本サンプルでは、NPLNダッシュボードでカテゴリタイプを1つも登録していないときにのみ使用可能な
    /// サンプル用のチャートを利用します。詳細はNPLNプログラミングマニュアルを参照してください。
    /// </summary>
    private const string CHART_NAME = "SampleChart";


    //-------------------------------------------------------------------------------------------------
    // Leaderboardサービスのサンプルコード
    //-------------------------------------------------------------------------------------------------

    /// <summary>
    /// Leaderboardサービスで使用するNPLNユーザーの情報を登録するサンプルコードです。
    /// ユーザー情報の登録は必須ではありませんが、登録した情報はリーダーボードの取得時にスコアに
    /// 付随する情報として取得することができます。
    /// </summary>
    private async Task<bool> SetUserDataAsync(UserContext userContext, string displayName, MapValue applicationData = null)
    {
        // Leaderboardサービスのサービスクライアントを生成します。
        var client = new nn.npln.leaderboard.LeaderboardClient(userContext);

        // ユーザー情報を登録するためのリクエストオブジェクトです。
        var request = new SetUserDataRequest();

        // ユーザー情報として任意の表示名やnn.npln.MapValueを登録することが可能です。
        request.SetDisplayName(displayName);
        if (applicationData != null)
        {
            request.SetApplicationData(applicationData);
        }

        // ユーザー情報を非同期的に登録します。
        var rpcResult = await client.SetUserDataAsync(request);

        // 登録が正常に完了したかどうかを確認します。
        if (!rpcResult.IsOk())
        {
            Debug.Log($"Failed to set user data: {rpcResult.Dump()}");
            return false;
        }

        Debug.Log("User data set successfully.");
        return true;
    }

    /// <summary>
    /// リーダーボードにスコアを登録するサンプルコードです。
    /// </summary>
    private async Task<bool> SetScoreAsync(UserContext userContext, long score, int categoryID ,MapValue applicationData = null)
    {
        // Leaderboardサービスのサービスクライアントを生成します。
        var client = new nn.npln.leaderboard.LeaderboardClient(userContext);

        // スコアを登録するためには、最初にスコアの登録先となるリーダーボードを決定しなければなりません。
        // 登録先のリーダーボードはカテゴリ名で識別します。まずはカテゴリ名を生成します。
        var categoryName = $"{CATEGORY_TYPE_NAME}_{categoryID:x8}";

        // 次にLeaderboardClient.CreateLeaderboardReference()を呼び出し、登録先のリーダーボードを参照するLeaderboardReference
        // オブジェクトを生成します。この時、引数としてリーダーボードのカテゴリ名を与えます。
        var leaderboardRef = client.CreateLeaderboardReference(categoryName);

        // スコアを登録するためのリクエストオブジェクトです。
        var request = new SetScoreRequest();

        // スコア値を設定します。この値の大小によってリーダーボード上の順位が決定されます。
        request.SetScore(score);

        // スコアに対して任意のnn.npln.MapValueを登録することも可能です。
        // このMapValueはスコア毎に登録が可能なものであり、ユーザー情報の登録時に設定したMapValueとは別に管理されます。
        // ユーザー情報と同様にリーダーボードの取得時に読み取り可能です。
        // 順位には影響しません。
        if (applicationData != null)
        {
            request.SetApplicationData(applicationData);
        }

        // スコアを非同期的に登録します。
        var rpcResult = await leaderboardRef.SetScoreAsync(request);

        // 登録が正常に完了したかどうかを確認します。
        if (!rpcResult.IsOk())
        {
            Debug.Log($"Failed to set user score: {rpcResult.Dump()}");
            return false;
        }

        Debug.Log("Score set successfully.");
        return true;
    }

    /// <summary>
    /// リーダーボードを取得してログ出力するサンプルコードです。
    /// </summary>
    private async Task<bool> GetAndPrintLeaderboardAsync(UserContext userContext)
    {
        // メソッド内で自分自身のNPLNユーザーIDを使用するため、まずはそれを取得します。
        var userId = default(string);
        {
            // NPLNユーザーIDを非同期的に取得します。
            var rpcResult = await userContext.WaitForAuthenticationAsync();

            // 正常に取得できたかどうかを確認します。
            if (!rpcResult.IsOk())
            {
                Debug.Log($"Failed to authenticate NPLN user: {rpcResult.Dump()}");
                return false;
            }

            userId = rpcResult.GetResponse();
        }

        // 取得したリーダーボードを格納するオブジェクトです。
        var snapshot = default(LeaderboardSnapshot);
        {
            // Leaderboardサービスのサービスクライアントを生成します。
            var client = new nn.npln.leaderboard.LeaderboardClient(userContext);

            // 取得するリーダーボードを参照するLeaderboardReferenceオブジェクトを生成します。
            var categoryName = $"{CATEGORY_TYPE_NAME}_{CATEGORY_ID:x8}";
            var leaderboardRef = client.CreateLeaderboardReference(categoryName);

            // このサンプルでは、周辺リーダーボードを取得することにします。
            var request = new GetNearLeaderboardRequest();

            // 自分のスコアを中心として合計9つのスコアを取得するようリクエストオブジェクトを生成します。
            request.SetPageSize(9);
            request.SetUserIdToCenter(userId);

            // リーダーボードを非同期的に取得します。
            var rpcResult = await leaderboardRef.GetLeaderboardAsync(request);

            // 正常に取得できたかどうかを確認します。
            if (!rpcResult.IsOk())
            {
                Debug.Log($"Failed to get near leaderboard: {rpcResult.Dump()}");
                return false;
            }

            // 取得したリーダーボードの情報を保存します。
            snapshot = rpcResult.GetResponse();
        }

        // 取得したリーダーボードのスコアをログ出力します。
        // まずは、スコア表のヘッダを出力します。
        Debug.Log("| Rank | NPLN User ID           |   Score |");
        Debug.Log("|------|------------------------|---------|");

        // 取得したスコアをログ出力します。
        foreach (var scoreData in snapshot.GetScoreDataList())
        {
            Debug.Log(string.Format("| {0,4} | {1,-22} | {2,7} | {3}",
                scoreData.GetRankData().GetRank(),
                scoreData.GetUserData().GetUserId(),
                scoreData.GetScore(),
                userId == scoreData.GetUserData().GetUserId() ? "** YOUR SCORE **" : ""));
        }

        // 以下のメソッドがtrueを返す場合は、より下位や上位のスコアデータを続けて取得することができます。
        // - snapshot.CanRequestNextPage()
        // - snapshot.CanRequestPreviousPage()
        // スコアデータを続けて取得するには以下のメソッドを呼び出してください。
        // - snapshot.GetNextPageAsync()
        // - snapshot.GetPreviousPageAsync()

        Debug.Log("Leaderboard fetched successfully.");
        return true;
    }
    
    private async Task<bool> GetNearLeaderboardAsync(UserContext userContext, int categoryID)
    {
        // メソッド内で自分自身のNPLNユーザーIDを使用するため、まずはそれを取得します。
        var userId = default(string);
        {
            // NPLNユーザーIDを非同期的に取得します。
            var rpcResult = await userContext.WaitForAuthenticationAsync();

            // 正常に取得できたかどうかを確認します。
            if (!rpcResult.IsOk())
            {
                Debug.Log($"Failed to authenticate NPLN user: {rpcResult.Dump()}");
                return false;
            }

            userId = rpcResult.GetResponse();
        }

        // 取得したリーダーボードを格納するオブジェクトです。
        var snapshot = default(LeaderboardSnapshot);
        {
            // Leaderboardサービスのサービスクライアントを生成します。
            var client = new nn.npln.leaderboard.LeaderboardClient(userContext);

            // 取得するリーダーボードを参照するLeaderboardReferenceオブジェクトを生成します。
            var categoryName = $"{CATEGORY_TYPE_NAME}_{categoryID:x8}";
            var leaderboardRef = client.CreateLeaderboardReference(categoryName);

            // このサンプルでは、周辺リーダーボードを取得することにします。
            var request = new GetNearLeaderboardRequest();

            // 自分のスコアを中心として合計9つのスコアを取得するようリクエストオブジェクトを生成します。
            request.SetPageSize(5);
            request.SetUserIdToCenter(userId);
            
            //Range
            //request.SetOffset(100);
            //request.SetPageSize(50);

            // リーダーボードを非同期的に取得します。
            var rpcResult = await leaderboardRef.GetLeaderboardAsync(request);

            // 正常に取得できたかどうかを確認します。
            if (!rpcResult.IsOk())
            {
                Debug.Log($"Failed to get near leaderboard: {rpcResult.Dump()}");
                return false;
            }

            // 取得したリーダーボードの情報を保存します。
            snapshot = rpcResult.GetResponse();
        }
        
        RankingManager.Instance.SetRankingData(snapshot.GetScoreDataList());

        Debug.Log("Leaderboard fetched successfully.");
        return true;
    }

    /// <summary>
    /// リーダーボードのヒストグラム (チャート) を取得してログ出力するサンプルコードです。
    /// </summary>
    private async Task<bool> GetAndPrintChartAsync(UserContext userContext)
    {
        // Leaderboardサービスのサービスクライアントを生成します。
        var client = new nn.npln.leaderboard.LeaderboardClient(userContext);

        // 取得するリーダーボードを参照するLeaderboardReferenceオブジェクトを生成します。
        var categoryName = $"{CATEGORY_TYPE_NAME}_{CATEGORY_ID:x8}";
        var leaderboardRef = client.CreateLeaderboardReference(categoryName);

        // 取得したチャートを格納するオブジェクトです。
        var chartData = default(ChartData);
        {
            // チャート名を指定してリクエストオブジェクトを生成します。
            var request = new GetChartDataRequest(CHART_NAME);

            // チャートを非同期的に取得します。
            var rpcResult = await leaderboardRef.GetChartDataAsync(request);

            // 正常に取得できたかどうかを確認します。
            if (!rpcResult.IsOk())
            {
                Debug.Log($"Failed to get leaderboard chart: {rpcResult.Dump()}");
                return false;
            }

            // 取得したチャートの情報を保存します。
            chartData = rpcResult.GetResponse();
        }

        // いくつかの理由により、チャートは未生成の場合があります。
        // - チャートはサーバ上で周期的に生成されるため、タイミングによっては未生成の場合があります。
        // - チャートの集計範囲内にスコアが1つも存在しない場合、チャートは生成されません。
        // もしチャートが未生成の場合は、必要に応じてアプリケーションで代替表示を行ってください。
        if (!chartData.IsGenerated())
        {
            Debug.Log("Chart is not generated yet. Try again later.");
            return true;
        }

        // 本サンプルでは、チャート表示時に自分のスコアを含む区間を強調表示することにします。
        // このために、あらかじめ自分のスコアをサーバから取得しておきます。
        long myScore = 0;
        {
            // スコアを非同期的に取得します。
            var rpcResult = await leaderboardRef.GetScoreAsync();

            // 正常に取得できたかどうかを確認します。
            switch (rpcResult.GetStatusCode())
            {
                // 正常に取得できました。
                case StatusCode.Ok:
                    myScore = rpcResult.GetResponse().GetScore();
                    break;
                // スコアが未登録の可能性があります。本サンプルでは無視することにします。
                case StatusCode.NotFound:
                    break;
                // 想定していないエラーが返りました。
                default:
                    Debug.Log($"Failed to get your score: {rpcResult.Dump()}");
                    return false;
            }
        }

        // ここからチャートをログに出力していきます。
        // まずはチャート表のヘッダを出力します。
        Debug.Log("| Score Range       | Height               | Quantities |");
        Debug.Log("|-------------------|----------------------|------------|");

        // チャートの高さを表す定数です。
        const long heightSteps = 20;

        // チャートの各区間のうち、最も多くのスコアが分類されている区間のスコア数を取得します。
        var maxBinQuantity = chartData.GetBinQuantities().Max();

        // 本サンプルでは分かりやすさのため、スコアが昇順で並べられるのか、または降順で並べられるのか
        // によってチャートの表示方法を分岐することにします。まずは昇順の場合です。
        if (chartData.IsScoreOrderAscending())
        {
            // 各区間に分類されるスコアの範囲を[leftInclusive, rightExclusive)と表すことにします。
            // スコアを昇順で並べるため、最上位区間のleftInclusiveの値は最高区間スコアと同値です。
            var leftInclusive = chartData.GetHighestBinsScore();

            // 最上位区間のrightExclusiveの値は、最高区間スコアに区間幅を足した値となります。
            var rightExclusive = leftInclusive + chartData.GetBinWidth();

            // 最上位区間、すなわちスコアが小さい区間から順にログ出力していきます。
            foreach (var quantity in chartData.GetBinQuantities())
            {
                var line = new System.Text.StringBuilder();

                // スコアの範囲を出力します。rightExclusiveは範囲に含まれませんので1を引きます。
                line.Append($"| {leftInclusive,7} ~ {rightExclusive - 1,7} | ");

                // 区間に分類されているスコア数に比例した高さのバーを描きます。
                for (var i = 0; i < heightSteps; i++)
                {
                    if (quantity > i * maxBinQuantity / heightSteps)
                    {
                        line.Append("*");
                    }
                    else
                    {
                        line.Append(" ");
                    }
                }

                // 区間に分類されているスコア数を出力します。
                line.Append($" | {quantity,10} |");

                // 区間に自分のスコアが分類されている場合は、それを出力します。
                // leftInclusiveは範囲に含まれます。rightExclusiveは範囲に含まれません。
                if (leftInclusive <= myScore && myScore < rightExclusive)
                {
                    line.Append(" ** YOU ARE HERE **");
                }
                Debug.Log(line.ToString());

                // 1つ下位の区間、すなわちスコアがより大きい区間に進みます。
                // leftInclusive, rightExclusiveの値をそれぞれ区間幅だけずらします。
                leftInclusive += chartData.GetBinWidth();
                rightExclusive += chartData.GetBinWidth();
            }
        }
        // 次は、スコアが降順に並べられる場合のチャート表示です。
        else
        {
            // 各区間に分類されるスコアの範囲を(leftExclusive, rightInclusive]と表すことにします。
            // スコアを降順で並べるため、最上位区間のrightInclusiveの値は最高区間スコアと同値です。
            var rightInclusive = chartData.GetHighestBinsScore();

            // 最上位区間のleftExclusiveの値は、最高区間スコアから区間幅を引いた値となります。
            var leftExclusive = rightInclusive - chartData.GetBinWidth();

            // 最上位区間、すなわちスコアが大きい区間から順にログ出力していきます。
            foreach (var quantity in chartData.GetBinQuantities())
            {
                var line = new System.Text.StringBuilder();

                // スコアの範囲を出力します。leftExclusiveは範囲に含まれませんので1を足します。
                line.Append($"| {leftExclusive + 1,7} ~ {rightInclusive,7} | ");

                // 区間に分類されているスコア数に比例した高さのバーを描きます。
                for (var i = 0; i < heightSteps; i++)
                {
                    if (quantity > i * maxBinQuantity / heightSteps)
                    {
                        line.Append("*");
                    }
                    else
                    {
                        line.Append(" ");
                    }
                }

                // 区間に分類されているスコア数を出力します。
                line.Append($" | {quantity,10} |");

                // 区間に自分のスコアが分類されている場合は、それを出力します。
                // leftExclusiveは範囲に含まれません。rightInclusiveは範囲に含まれます。
                if (leftExclusive < myScore && myScore <= rightInclusive)
                {
                    line.Append(" ** YOU ARE HERE **");
                }
                Debug.Log(line.ToString());

                // 1つ下位の区間、すなわちスコアがより小さい区間に進みます。
                // rightInclusive, leftExclusiveの値をそれぞれ区間幅だけずらします。
                rightInclusive -= chartData.GetBinWidth();
                leftExclusive -= chartData.GetBinWidth();
            }
        }

        Debug.Log("Chart fetched successfully.");
        return true;
    }


    //-------------------------------------------------------------------------------------------------
    // サンプルプログラム用ユーティリティメソッドの定義
    //-------------------------------------------------------------------------------------------------

    /// <summary>
    /// サンプルプログラムで登録したスコアを削除するユーティリティメソッドです。
    /// </summary>
    private async Task<bool> DeleteScoreAsync(UserContext userContext)
    {
        // Leaderboardサービスのサービスクライアントを生成します。
        var client = new nn.npln.leaderboard.LeaderboardClient(userContext);

        // リーダーボードを参照するLeaderboardReferenceオブジェクトを生成します。
        var categoryName = $"{CATEGORY_TYPE_NAME}_{CATEGORY_ID:x8}";
        var leaderboardRef = client.CreateLeaderboardReference(categoryName);

        // スコアを非同期的に削除します。
        var rpcResult = await leaderboardRef.DeleteScoreAsync();

        // 正常に削除できたかどうかを確認します。
        if (!rpcResult.IsOk())
        {
            Debug.Log($"Failed to delete score: {rpcResult.Dump()}");
            return false;
        }

        Debug.Log("User score deleted successfully.");
        return true;
    }
    #endregion

    #region Unity Logic
    private Task<bool> m_InitTask;
    private Task<bool> m_LeaderboardTask;
    private bool m_SampleCompleted;

    void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            DestroyImmediate(this);
    }
    private void Start()
    {
        m_InitTask= Task.Run(async () =>
        {
            if (!nns.npln.Common.TryOpenUser(out var userHandle))
            {
                return false;
            }

            userContext = UserContext.Create(TENANT_ID_SOURCE, NsaIdTokenRetriever.Create(userHandle), UserConfig.CreateForPrearrangedUser(), nns.npln.Common.ExecutorHolder);

            // Leaderboardサービスを使用するNPLNユーザーの情報を登録します。
            if (!await SetUserDataAsync(userContext, "My Name"))
            {
                return false;
            }

            return true;
        });
    }

    public void SetLeaderboardScore(int score, int categoryID)
    {
        if(!m_InitTask.IsCompleted) return;
        
        m_LeaderboardTask = Task.Run(async () =>
        {
            if (!await SetScoreAsync(userContext,score, categoryID))
            {
                return false;
            }
            if (!await GetNearLeaderboardAsync(userContext, categoryID))
            {
                return false;
            }

            return true;
        });
    }
    #endregion
}
