/*--------------------------------------------------------------------------------*
  Copyright (C)Nintendo All rights reserved.

  These coded instructions, statements, and computer programs contain proprietary
  information of Nintendo and/or its licensed developers and are protected by
  national and international copyright laws. They may not be disclosed to third
  parties or copied or duplicated in any form, in whole or in part, without the
  prior written consent of Nintendo.

  The content herein is highly confidential and should be handled accordingly.
 *--------------------------------------------------------------------------------*/

using System;
using System.IO;
using UnityEngine;

public class FsSaveDataPlayerPrefs : MonoBehaviour
{
    public static FsSaveDataPlayerPrefs Instance;
    
    private nn.account.Uid userId;
    private const string mountName = "MySave";
    private const string fileName = "MySaveData";
    private static readonly string filePath = string.Format("{0}:/{1}", mountName, fileName);
#pragma warning disable 0414
    private nn.fs.FileHandle fileHandle = new nn.fs.FileHandle();
#pragma warning restore 0414
    
    private void Awake()
    {
        #region SINGLETON
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        #endregion
    }

    private void Start()
    {
#if UNITY_SWITCH && !UNITY_EDITOR
        
        nn.account.Account.Initialize();
        nn.account.UserHandle userHandle = new nn.account.UserHandle();

        if (!nn.account.Account.TryOpenPreselectedUser(ref userHandle))
        {
            nn.Nn.Abort("Failed to open preselected user.");
        }
        nn.Result result = nn.account.Account.GetUserId(ref userId, userHandle);
        result.abortUnlessSuccess();
        result = nn.fs.SaveData.Mount(mountName, userId);
        result.abortUnlessSuccess();

        //InitializeSaveData();
        //Load();
#endif
    }

    public void SetInt(string id, int value)
    {
        PlayerPrefs.SetInt(id, value);
        SavePlayerPrefs();
    }
    
    private void OnDestroy()
    {
#if UNITY_SWITCH && !UNITY_EDITOR
        nn.fs.FileSystem.Unmount(mountName);
#endif
    }

    /*
    private void InitializeSaveData()
    {
#if !UNITY_SWITCH || UNITY_EDITOR
        if (PlayerPrefs.HasKey(versionKey))
        {
            return;
        }
        PlayerPrefs.SetInt(versionKey, saveDataVersion);
        PlayerPrefs.SetInt(counterKey, 0);
        PlayerPrefs.Save();
#else
        nn.fs.EntryType entryType = 0;
        nn.Result result = nn.fs.FileSystem.GetEntryType(ref entryType, filePath);
        if (result.IsSuccess())
        {
            return;
        }
        if (!nn.fs.FileSystem.ResultPathNotFound.Includes(result))
        {
            result.abortUnlessSuccess();
        }

        PlayerPrefs.SetInt(versionKey, saveDataVersion);
        PlayerPrefs.SetInt(counterKey, 0);
        byte[] data = UnityEngine.Switch.PlayerPrefsHelper.rawData;
        long saveDataSize = data.LongLength;

        // Nintendo Switch Guideline 0080
        UnityEngine.Switch.Notification.EnterExitRequestHandlingSection();

        result = nn.fs.File.Create(filePath, saveDataSize);
        result.abortUnlessSuccess();

        result = nn.fs.File.Open(ref fileHandle, filePath, nn.fs.OpenFileMode.Write);
        result.abortUnlessSuccess();

        const int offset = 0;
        result = nn.fs.File.Write(fileHandle, offset, data, data.LongLength, nn.fs.WriteOption.Flush);
        result.abortUnlessSuccess();

        nn.fs.File.Close(fileHandle);
        result = nn.fs.FileSystem.Commit(mountName);
        result.abortUnlessSuccess();

        // Nintendo Switch Guideline 0080
        UnityEngine.Switch.Notification.LeaveExitRequestHandlingSection();
#endif
    }*/

    private void SavePlayerPrefs()
    {
#if !UNITY_SWITCH || UNITY_EDITOR
        PlayerPrefs.Save();
#else
        byte[] data = UnityEngine.Switch.PlayerPrefsHelper.rawData;
        long saveDataSize = data.LongLength;

        // Nintendo Switch Guideline 0080
        UnityEngine.Switch.Notification.EnterExitRequestHandlingSection();

        nn.Result result = nn.fs.File.Open(ref fileHandle, filePath, nn.fs.OpenFileMode.Write);
        result.abortUnlessSuccess();

        const int offset = 0;
        result = nn.fs.File.Write(fileHandle, offset, data, data.LongLength, nn.fs.WriteOption.Flush);
        result.abortUnlessSuccess();

        nn.fs.File.Close(fileHandle);
        result = nn.fs.FileSystem.Commit(mountName);
        result.abortUnlessSuccess();

        // Nintendo Switch Guideline 0080
        UnityEngine.Switch.Notification.LeaveExitRequestHandlingSection();
#endif
    }

    public int LoadInt(string id)
    {
#if !(!UNITY_SWITCH || UNITY_EDITOR)
        nn.fs.EntryType entryType = 0;
        nn.Result result = nn.fs.FileSystem.GetEntryType(ref entryType, filePath);
        if (nn.fs.FileSystem.ResultPathNotFound.Includes(result)) { return 0; }
        result.abortUnlessSuccess();

        result = nn.fs.File.Open(ref fileHandle, filePath, nn.fs.OpenFileMode.Read);
        result.abortUnlessSuccess();

        long fileSize = 0;
        result = nn.fs.File.GetSize(ref fileSize, fileHandle);
        result.abortUnlessSuccess();

        byte[] data = new byte[fileSize];
        result = nn.fs.File.Read(fileHandle, 0, data, fileSize);
        result.abortUnlessSuccess();

        nn.fs.File.Close(fileHandle);

        UnityEngine.Switch.PlayerPrefsHelper.rawData = data;
#endif
        return PlayerPrefs.GetInt(id);
    }
}
