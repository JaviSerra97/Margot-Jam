using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using nn.npln;

namespace nns.npln
{
    public static class Common
    {
        /// <summary>
        /// サンプルプログラム用のテナントIDソースです。
        /// 実際のアプリケーションコードでは、アプリケーションごとに割り当てられたテナントIDソースを使用してください。
        /// </summary>
        public const string DEFAULT_TENANT_ID_SOURCE = "t-713a1c66";

        /// <summary>
        /// サンプルプログラムで使用するExecutorHolderです。
        /// </summary>
        /// <remarks>
        /// NPLN内部の非同期タスクを実行するスレッドリソースを管理するオブジェクトです。
        /// このオブジェクトはアプリケーションの終了まで破棄されないようstatic変数などで管理することを推奨します。
        /// 詳細はNPLNプログラミングマニュアルを参照してください。
        /// </remarks>
        public static ExecutorHolder ExecutorHolder => s_ExecutorHolder.Value;

        private static Lazy<ExecutorHolder> s_ExecutorHolder = new Lazy<ExecutorHolder>(() => ExecutorHolder.Create());

        /// <summary>
        /// サンプルプログラムで使用するユーザーアカウントをOpen状態にします。
        /// </summary>
        public static bool TryOpenUser(out nn.account.UserHandle userHandle)
        {
            
            nn.account.Account.Initialize();

            userHandle = default;
            if (nn.account.Account.TryOpenPreselectedUser(ref userHandle))
            {
                return true;
            }
            
            userHandle = default;

#if NN_ACCOUNT_OPENUSER_ENABLE
            Debug.Log($"{nameof(nn.account.Account.TryOpenPreselectedUser)}() failed. Will try to open the first discovered user instead.");

            var uids = new nn.account.Uid[nn.account.Account.UserCountMax];
            var length = default(int);
            {
                var result = nn.account.Account.ListAllUsers(ref length, uids);
                if (!result.IsSuccess())
                {
                    Debug.Log($"{nameof(nn.account.Account.ListAllUsers)}() failed ({result}).");
                    return false;
                }
            }
            if (length == 0)
            {
                Debug.Log("No user found on the DevKit.");
                return false;
            }
            {
                var result = nn.account.Account.OpenUser(ref userHandle, uids[0]);
                if (!result.IsSuccess())
                {
                    Debug.Log($"{nameof(nn.account.Account.OpenUser)}() failed ({result}).");
                    return false;
                }
            }
            return true;
#else
            Debug.Log($"{nameof(nn.account.Account.TryOpenPreselectedUser)}() failed.");
            return false;
#endif
        }
    }
}
