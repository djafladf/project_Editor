using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSceneManager : MonoBehaviour 
{
    /**
    * MainWorld 씬 매니저
    *   - 시간대 변경
    *   - 날짜, 시간대 맞춰서 월드캔버스 동기화
    *   - WorldCanvas 관리
    *   - 월드캔버스간 이동
    */

    public string worldfilePath = "Prefab/MainWorld/";  // 월드 프리팹 경로
    public WorldCanvas currentWorld = null;    // 현재 월드 오브젝트

    /// <summary>
    /// 플레이어의 위치와 활성화 월드 동기화
    /// </summary>
    /// <remarks>씬 내 WorldCanvas 객체 삭제 후 새로 생성</remarks>
    public void asyncWorldCanvas()
    {
        currentWorld = GameObject.FindObjectOfType<WorldCanvas>();
        if (currentWorld != null)
        {
            Destroy(currentWorld.gameObject);
        }
        GameObject newWorldObject = Instantiate(Resources.Load<GameObject>(worldfilePath + PlayerDataManager.Instance.playerData.location + "Canvas"), transform);
        currentWorld = newWorldObject.GetComponent<WorldCanvas>();
    }

    /// <summary>
    /// 플레이어의 위치와 활성화 월드 동기화
    /// </summary>
    /// <param name="WorldName">이동할 월드캔버스의 이름 입력</param>
    /// <remarks>씬 내 WorldCanvas 객체 삭제 후 새로 생성</remarks>
    public void asyncWorldCanvas(string worldName)
    {
        PlayerDataManager.Instance.playerData.location = worldName;
        currentWorld = GameObject.FindObjectOfType<WorldCanvas>();
        if (currentWorld != null)
        {
            Destroy(currentWorld.gameObject);
        }
        GameObject newWorldObject = Instantiate(Resources.Load<GameObject>(worldfilePath + worldName + "Canvas"));
        currentWorld = newWorldObject.GetComponent<WorldCanvas>();
    }
}