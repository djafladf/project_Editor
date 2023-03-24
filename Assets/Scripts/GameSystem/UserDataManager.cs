﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    /**
    * 플레이어 데이터를 저장, 로드
    * initNewPlayerData
    * SavePlayerData
    * LoadPlayerData
    */
    private class PlayerData
    {
        public int year;
        public int month;
        public int day;
        public int time;
        public int renown;
    }

    private PlayerData playerData = new PlayerData(); 
    private string path = "/Resources/Saves/";

    public void InitNewPlayerData()
    {   
        {
            playerData.year = 17;
            playerData.month = 12;
            playerData.day = 13;
            playerData.time = 0;
            playerData.renown = 0;
        }
        
        asyncPlayerPrefs();
    }

    public void SavePlayerData(string saveFileName)
    {
        asyncPlayerData();

        string jsonObjectData = JsonUtility.ToJson(playerData);
        
        FileStream fileStream = new FileStream(Application.dataPath + path + saveFileName + ".json", FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonObjectData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }

    public void LoadPlayerData(string saveFileName)
    { 
        FileStream fileStream = new FileStream(Application.dataPath + path + saveFileName + ".json", FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();

        string jsonObjectData = Encoding.UTF8.GetString(data);
        playerData = JsonUtility.FromJson<PlayerData>(jsonObjectData);

        asyncPlayerPrefs();
        asyncSceneData();
    }

    private void asyncPlayerPrefs()
    {
        PlayerPrefs.SetInt("Year", playerData.year);
        PlayerPrefs.SetInt("Month", playerData.month);
        PlayerPrefs.SetInt("Day", playerData.day);
        PlayerPrefs.SetInt("Time", playerData.time);
        PlayerPrefs.SetInt("Renown", playerData.renown);
    }

    public void asyncSceneData()
    {
        GameObject.FindObjectOfType<Chat>().FileName += PlayerPrefs.GetInt("Day").ToString() + "_" + PlayerPrefs.GetInt("Time").ToString();
    }

    private void asyncPlayerData()
    {
        playerData.year = PlayerPrefs.GetInt("Year");
        playerData.month = PlayerPrefs.GetInt("Month");
        playerData.day = PlayerPrefs.GetInt("Day");
        playerData.time = PlayerPrefs.GetInt("Time");
        playerData.renown = PlayerPrefs.GetInt("Renown");
    }
}
