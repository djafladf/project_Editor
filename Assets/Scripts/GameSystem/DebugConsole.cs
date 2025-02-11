using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugConsole : MonoBehaviour
{
    public GameObject console;
    public Text consoleText;
    public InputField input;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("ConsoleClicked");
            console.SetActive(!console.activeSelf);
        }
    }
    public void GetConsoleInput()
    {
        string output = "";
        switch(input.text)
        {
            case "todayData" :
                DailyData todayData = GameSystem.Instance.todayData;
                output += $"{todayData.date.year}년 {todayData.date.month}월 {todayData.date.day}일\n";
                output += $"오늘의 업무 현황\n";
                foreach(var work in todayData.workData.Keys)
                {
                    output += $"WORK: {work.Item1}, Stage: {work.Item2} = Done: {todayData.workData[work]}";
                }
                output += "\n";
                break;
            case "playerData" :
                SaveData player = GameSystem.Instance.player;
                output += $"Current Date Index: {player.dateIndex}\n";
                output += $"Current location: {player.location}\n";
                output += $"Current time: {player.time}\n";
                output += $"Current renown: {player.renown}\n";
                break;
            case "help" :
                output += $"todayData: show today's Date, Work Status\n";
                output += $"playerData: show current date Index, location, time and renown\n";
                break;
        }

        consoleText.text = "";
        consoleText.text += output;
    }
}
