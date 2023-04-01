﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/**
    * 플레이어 데이터를 저장, 로드
    * initNewPlayerData
    * SavePlayerData
    * LoadPlayerData
    */
public class PlayerDataManager : MonoBehaviour
{
    public string savefilePath = "/Resources/Save/";    // 세이브 파일 경로
    public string worldfilePath = "Prefab/MainWorld/";  // 월드 프리팹 경로

    /**
    * 플레이어 데이터 클래스
    *   - 날짜 데이터 (YYYY:DD:MM - Time)
    *   - 위치 데이터
    *   - 현재 명성치
    */
    private class PlayerData
    {
        public int year;
        public int month;
        public int day;
        public int time;
        public string location;
        public int renown;
    }
    private PlayerData playerData = new PlayerData();   // PlayerPrefs와 데이터 연동

    // 데이터 초기 설정
    public void InitNewPlayerData()
    {   
        LoadPlayerData(savefilePath+"initial.json");
        asyncPlayerPrefs();
    }

    /// <summary>
    /// 플레이어 데이터 JSON 저장
    /// </summary>
    /// <param name="SaveFileName">저장 경로 내 파일명</param>
    public void SavePlayerData(string saveFileName)
    {
        asyncPlayerData();

        string jsonObjectData = JsonUtility.ToJson(playerData);
        
        FileStream fileStream = new FileStream(Application.dataPath + savefilePath + saveFileName + ".json", FileMode.Create);
        byte[] data = Encoding.UTF8.GetBytes(jsonObjectData);
        fileStream.Write(data, 0, data.Length);
        fileStream.Close();
    }

    /// <summary>
    /// 플레이어 데이터 JSON 로드
    /// </summary>
    /// <param name="SaveFileName">저장 경로 내 파일명</param>
    public void LoadPlayerData(string saveFileName)
    { 
        FileStream fileStream = new FileStream(Application.dataPath + savefilePath + saveFileName + ".json", FileMode.Open);
        byte[] data = new byte[fileStream.Length];
        fileStream.Read(data, 0, data.Length);
        fileStream.Close();

        string jsonObjectData = Encoding.UTF8.GetString(data);
        playerData = JsonUtility.FromJson<PlayerData>(jsonObjectData);

        asyncPlayerPrefs();
        asyncSceneData();
    }

    /// <summary>
    /// 플레이어의 위치와 활성화 월드 동기화
    /// </summary>
    /// <remarks>씬 내 WorldCanvas 객체 삭제 후 새로 생성</remarks>
    public void asyncSceneData()
    {
        var existWorld = GameObject.FindObjectOfType<WorldCanvas>();
        if (existWorld != null)
        {
            Destroy(existWorld.gameObject);
        }
        GameObject newWorldCanvas = Instantiate(Resources.Load<GameObject>(worldfilePath + playerData.location + "Canvas"));
    }

    
    /**
    * PlayerData -> PlayerPrefs 데이터 동기화
    */
    private void asyncPlayerPrefs()
    {
        PlayerPrefs.SetInt("Year", playerData.year);
        PlayerPrefs.SetInt("Month", playerData.month);
        PlayerPrefs.SetInt("Day", playerData.day);
        PlayerPrefs.SetInt("Time", playerData.time);
        PlayerPrefs.SetString("Location", playerData.location);
        PlayerPrefs.SetInt("Renown", playerData.renown);
    }

    /**
    * PlayerPrefs -> PlayerData 데이터 동기화
    */
    private void asyncPlayerData()
    {
        playerData.year = PlayerPrefs.GetInt("Year");
        playerData.month = PlayerPrefs.GetInt("Month");
        playerData.day = PlayerPrefs.GetInt("Day");
        playerData.time = PlayerPrefs.GetInt("Time");
        playerData.location = PlayerPrefs.GetString("Location");
        playerData.renown = PlayerPrefs.GetInt("Renown");
    }
}
