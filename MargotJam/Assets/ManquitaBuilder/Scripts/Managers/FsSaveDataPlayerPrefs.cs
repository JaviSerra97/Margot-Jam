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
    private const string mountName = "BuilderSave";
    private const string fileName = "BuilderData";
    private static readonly string filePath = string.Format("{0}:/{1}", mountName, fileName);
#pragma warning disable 0414
    private nn.fs.FileHandle fileHandle = new nn.fs.FileHandle();
#pragma warning restore 0414
    
    //private const string versionKey = "Version";
    //private const string counterKey = "Counter";

    //private const int saveDataVersion = 1;
    //private int counter = 0;
    //private int saveData = 0;
    //private int loadData = 0;

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
    
    private void Start()
    {
        nn.account.Account.Initialize();
        Debug.Log("Init");
        nn.account.UserHandle userHandle = new nn.account.UserHandle();

        if (!nn.account.Account.TryOpenPreselectedUser(ref userHandle))
        {
            nn.Nn.Abort("Failed to open preselected user.");
        }
        
        nn.Result result = nn.account.Account.GetUserId(ref userId, userHandle);
        result.abortUnlessSuccess();
        result = nn.fs.SaveData.Mount(mountName, userId);
        result.abortUnlessSuccess();

        InitializeSaveData();
        //Load();
        
        UnlockManager.Instance.SetStatesOnStart();
       
        LeaderboardClient.Instance.InitializeLeaderboard(userHandle);
        Debug.Log("Leaderboard");
    }
    
    private void OnDestroy()
    {
#if UNITY_SWITCH && !UNITY_EDITOR
        if (Instance == this)
        {
            nn.fs.FileSystem.Unmount(mountName);
        }
#endif
    }

    public void SetPlayerPrefs(string key, int unlocked)
    {
        PlayerPrefs.SetInt(key, unlocked);
//        Debug.Log("Saved: " + key + ". Value: " + PlayerPrefs.GetInt(key));
        SavePlayerPrefs();
    }

    public void SetPlayerPrefs(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        SavePlayerPrefs();
    }

   private void InitializeSaveData()
    {
        /*
#if !UNITY_SWITCH || UNITY_EDITOR
        
        if (PlayerPrefs.HasKey(versionKey))
        {
            return;
        }
        PlayerPrefs.SetInt(versionKey, saveDataVersion);
        PlayerPrefs.SetInt(counterKey, 0);
        PlayerPrefs.Save();
        
#else*/
        nn.fs.EntryType entryType = 0;
        nn.Result result = nn.fs.FileSystem.GetEntryType(ref entryType, filePath);
        if (result.IsSuccess())
        {
            Load();
            return;
        }
        if (!nn.fs.FileSystem.ResultPathNotFound.Includes(result))
        {
            result.abortUnlessSuccess();
        }

        //PlayerPrefs.SetInt(versionKey, saveDataVersion);
        //PlayerPrefs.SetInt(counterKey, 0);
        byte[] data = UnityEngine.Switch.PlayerPrefsHelper.rawData;
        long saveDataSize = data.LongLength;

        // Nintendo Switch Guideline 0080
        UnityEngine.Switch.Notification.EnterExitRequestHandlingSection();

        result = nn.fs.File.Create(filePath, saveDataSize);
        result.abortUnlessSuccess();
/*
        result = nn.fs.File.Open(ref fileHandle, filePath, nn.fs.OpenFileMode.Write);
        result.abortUnlessSuccess();

        const int offset = 0;
        result = nn.fs.File.Write(fileHandle, offset, data, data.LongLength, nn.fs.WriteOption.Flush);
        result.abortUnlessSuccess();

        nn.fs.File.Close(fileHandle);
        result = nn.fs.FileSystem.Commit(mountName);
        result.abortUnlessSuccess();
*/
        // Nintendo Switch Guideline 0080
        UnityEngine.Switch.Notification.LeaveExitRequestHandlingSection();
//#endif
    }

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

        nn.fs.File.SetSize(fileHandle, saveDataSize);
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

    private void Load()
    {
#if !(!UNITY_SWITCH || UNITY_EDITOR)
        nn.fs.EntryType entryType = 0;
        nn.Result result = nn.fs.FileSystem.GetEntryType(ref entryType, filePath);
        if (nn.fs.FileSystem.ResultPathNotFound.Includes(result)) { return; }
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
        /*
        int version = PlayerPrefs.GetInt(versionKey);
        Debug.Assert(version == saveDataVersion); // Save data version up
        counter = PlayerPrefs.GetInt(counterKey);
        */
    }
/*
    private void ResetSaveData()
    {
        counter = 0;
        SavePlayerPrefs();
        saveData = counter;
    }
    */
}