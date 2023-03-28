using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using System.IO;
using System;
using TMPro;
using Random = UnityEngine.Random;
using System.Threading;
using System.Linq;
using System.Globalization;
using System.Net;
using Unity.VisualScripting;

public class GameManager: MonoBehaviour
{
    [SerializeField]
    //Prefabs
    public BulletManager BM;        // Bullet Create Manager Prefab
    public Player Pl;               // Player Prefab
    public GameObject GameOver;     // GameOver Prefab
    public GameObject GameEnd;      // GameEnd Prefab
    public Transform[] SPR;         // Bullet Spawn Position(Transform) List_Right
    public Transform[] SPL;         // Bullet Spawn Position(Transform) List_Left
    public List<Pattern> PTL;       // List of Patterns
    public TMP_Text Timer;          // Timer Prefab
    //Public variable
    public float RepeatInterv;      // Interval within the Pattern
    public float BulletInterv;      // Interval between Bullets
    public float PatternInterv;     // Interval between Patterns (When Pattern End)
    public int PatternNum;          // Num of Pattern
    public int RepeatNum;           // Num of Repeatition of Pattern
    public int TimeToSurvive;        // How Long Player Have To Survie
    //Private variable
    private float time = 0;         // Current Time (For Timer)
    private int CurIndex;           // Current Index (For Pattern List)
    private int CurRepeat;          // Current Repeatition
    private int CurPattern;         // Current Pattern

    private void Awake()
    {
        PTL = new List<Pattern>();
        PTL.Add(new Pattern());
        ReadPattern();
        Init();
    }

    private void Start()
    {
        Init();
    }

    IEnumerator MakePattern()
    {
        yield return new WaitForSeconds(0.5f);
        for (CurRepeat = 0; CurRepeat < RepeatNum; CurRepeat++)
        {
            Transform[] cnt;
            string Dir;
            if (CurRepeat % 2 == 0) { cnt = SPR; Dir = "R"; }
            else { cnt = SPL; Dir = "L"; }

            for (CurIndex = 0; CurIndex < PTL[CurPattern].PT[0].Length; CurIndex++)
            {
                for (int i = 0; i < PTL[CurPattern].PT.Count; i++)
                {
                    if (PTL[CurPattern].PT[i][CurIndex] == 1)
                    {
                        GameObject tmp = BM.MakeBul(Dir);
                        tmp.transform.position = cnt[i].position;
                    }
                }
                yield return new WaitForSeconds(BulletInterv);
            }
            yield return new WaitForSeconds(RepeatInterv);
        }

        yield return new WaitForSeconds(PatternInterv);
        int PatternCnt = Random.Range(0, PatternNum);
        while(PatternCnt != CurPattern) PatternCnt = Random.Range(0, PatternNum);
        CurPattern = PatternCnt;
        StartCoroutine(MakePattern());
    }

    IEnumerator TImeUpdate()
    {
        for(time = 0; time <= TimeToSurvive + 0.01f; time += 0.01f)
        {
            yield return new WaitForSeconds(0.01f);
            Timer.text = string.Format("{0:0.00}", time);
        }
        StopCoroutine("MakePattern");
    }

    void Init()
    {
        CurPattern = Random.Range(0, PatternNum);
        CurIndex = 0;
        CurRepeat = 0;
        time = 0;
        StartCoroutine(TImeUpdate());
        StartCoroutine(MakePattern());
    }

    // Read Pattern In Resources Folder. Name of The Pattern File Must be Pattern_X ( ex) Pattern_1, Pattern_2)
    void ReadPattern()
    {
        for (int i = 1; i < PatternNum+1; i++)
        {
            string tmp = "Text/Dodge/Pattern_" + i.ToString();
            TextAsset textFile = Resources.Load(tmp) as TextAsset;
            if (textFile == null)
            {
                return;
            }
            StringReader stringReader = new StringReader(textFile.text);
            Pattern Data = new Pattern();
            Data.PT = new List<int[]>();
            Data.repeat = RepeatNum;

            while (stringReader != null)
            {
                string line = stringReader.ReadLine();
                if (line == null) break;
                int[] cnt = Array.ConvertAll(line.Split(' '), int.Parse);
                Data.PT.Add(cnt);
            }
            PTL.Add(Data);
            stringReader.Close();
        }
    }

    void GameOverFunc()
    {
        StopCoroutine("MakePattern");
        StopCoroutine("TimeUpdate");
        GameEnd.SetActive(true);
        Pl.End_Player();
        BM.EndBul();
    }
}
