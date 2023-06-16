using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Infra_Classifier : MonoBehaviour
{
    /*
     * 분류기의 작동을 담당 + Setting(InitSetting, DropChange, GetChange)도 일부분 포함
     */


    // Color, Shpae의  이름들을 담는 딕셔너리. 목록은 ConveyManager로부터 가져옴.
    Dictionary<string, List<string>> Options = new Dictionary<string, List<string>>();

    string CurType = "Color";   // 현재 분류 Type

    // Setting용
    int CurTypeBal = 0;
    List<string> CurDetail = new List<string>();
    int[] CurDetailVal = new int[] {0,1,2};
    TMP_Dropdown Type_D;
    TMP_Dropdown Out1_D;
    TMP_Dropdown Out2_D;
    TMP_Dropdown Out3_D;
    EventTrigger Apply;
    GameObject Setting;

    List<Tuple<string,string>> InpQueue = new List<Tuple<string,string>>();

    public Transform Out1;      // 출구 1
    public Transform Out2;      // 출구 2
    public Transform Out3;      // 출구 3
    
    Installation Ins;           // Installation.cs
    bool FirstInstall = true;   // 첫 Enable시(Pooling 될 때) 연산 제외용

    private void Awake()
    {
        Ins = GetComponent<Installation>();
        
    }
    private void Start()
    {
        Options["Color"] = new List<string>(Ins.CM.ColorNames) { "Cnt" };
        Options["Shape"] = new List<string>(Ins.CM.ShapeNames) { "Cnt" };
    }

    /// <summary>
    /// Pool에서 꺼내질 때마다 초기화시킴.
    /// </summary>
    private void OnEnable()
    {
        if (FirstInstall)
        {
            FirstInstall = false;
        }
        else
        {
            CurType = "Color";
            CurTypeBal = 0;
            CurDetailVal[0] = 0; CurDetailVal[1] = 1; CurDetailVal[2] = 2;
            transform.rotation = new Quaternion(0, 0, 0, 0);
            for (int i = 0; i < 3; i++) CurDetail.Add(Ins.CM.ColorNames[i]);
            StartCoroutine(WorkGap());
        }
    }

    private void OnDisable()
    {
        if (!FirstInstall)
        {
            StopAllCoroutines();
            CurDetail.Clear();
            InpQueue.Clear();
        }
    }

    /// <summary>
    /// Setting 초기화
    /// </summary>
    /// <param name="_Setting">현재 Object의 Setting 설정용 Object</param>
    public void InitSetting(GameObject _Setting)
    {
        Setting = _Setting;
        Type_D = _Setting.transform.GetChild(0).GetComponent<TMP_Dropdown>();
        Type_D.value = CurTypeBal;
        Out1_D = _Setting.transform.GetChild(1).GetComponent<TMP_Dropdown>();
        Out2_D = _Setting.transform.GetChild(2).GetComponent<TMP_Dropdown>();
        Out3_D = _Setting.transform.GetChild(3).GetComponent<TMP_Dropdown>();
        DropChange(CurTypeBal);
        Out1_D.value = CurDetailVal[0];
        Out2_D.value = CurDetailVal[1];
        Out3_D.value = CurDetailVal[2];
        Apply = _Setting.transform.GetChild(4).GetComponent<EventTrigger>();

        Type_D.onValueChanged.AddListener(DropChange);
        MyUi.AddEvent(Apply, EventTriggerType.PointerClick, GetChange);
    }

    /// <summary>
    /// 현재 Setting Object의 Type 변경 시 Drop의 내용물을 초기화시킴.
    /// </summary>
    /// <param name="Ind">0 : Color, 1 : Shpae</param>
    void DropChange(int Ind)
    {
        string cnt = Type_D.options[Ind].text;
        Out1_D.ClearOptions();
        Out1_D.AddOptions(Options[cnt]);
        Out2_D.ClearOptions();
        Out2_D.AddOptions(Options[cnt]);
        Out3_D.ClearOptions();
        Out3_D.AddOptions(Options[cnt]);
    }
    
    /// <summary>
    /// Setting에 적용한 내용을 실제 Infra에 적용시킴
    /// </summary>
    /// <param name="Data">포인터의 값이지만 이 함수에선 사용하지 않음.</param>
    public void GetChange(PointerEventData Data)
    {
        if (Out1_D.value != Out2_D.value && Out1_D.value != Out3_D.value && Out2_D.value != Out3_D.value)
        {
            CurType = Type_D.captionText.text;
            CurTypeBal = Type_D.value;
            CurDetail[0] = Out1_D.captionText.text;
            CurDetailVal[0] = Out1_D.value;
            CurDetail[1] = Out2_D.captionText.text;
            CurDetailVal[1] = Out2_D.value;
            CurDetail[2] = Out3_D.captionText.text;
            CurDetailVal[2] = Out3_D.value;
            Ins.CM.OptionAble = true;
            Destroy(Setting);
        }
        else
        {
            Debug.Log("세 옵션은 서로 달라야 합니다.");
        }
    }

    /// <summary>
    /// 분류 전 Deque에 삽입
    /// </summary>
    /// <param name="_Color">Object.color</param>
    /// <param name="_Shape">Object.shape</param>
    /// <param name="_Transform">Object.transform</param>
    public void Classify(string _Color,string _Shape,Transform _Transform)
    {
        if (Vector3.Magnitude(_Transform.position - transform.GetChild(0).position) > 16) return;
        InpQueue.Add(new Tuple<string, string>(_Color,_Shape));
        _Transform.gameObject.SetActive(false);
    }

    /// <summary>
    /// 실제 작동 부분
    /// 0.5초마다 1번의 작업을 수행하며
    /// 현재 Que의 Object와 현재 Infra의 Type을 대조하여 1,2,3번 출구 중 한 곳으로 보냄.
    /// Type이 맞지 않는 경우 무조건 3번으로 보냄.
    /// </summary>
    /// <returns></returns>
    IEnumerator WorkGap()
    {
        Tuple<string, string> CurWork;
        string c = "";
        var wfs = new WaitForSeconds(0.5f);
        while (true)
        {
            yield return wfs;
            if (InpQueue.Count == 0 || !Ins.OnWork) { continue; }
            CurWork = InpQueue[0]; InpQueue.RemoveAt(0);
            GameObject cnt = Ins.CM.COM.ReturnObject(CurWork.Item2,CurWork.Item1);
            if (CurType == "Color") c = CurWork.Item1;
            else if (CurType == "Shape") c = CurWork.Item2;
            if (CurDetail[0] == c) cnt.transform.position = Out1.position;
            else if (CurDetail[1] == c) cnt.transform.position = Out2.position;
            else if (CurDetail[2] == c) cnt.transform.position = Out3.position;
            else
            {
                if (CurDetail[0] == "Cnt") cnt.transform.position = Out1.position;
                else if (CurDetail[1] == "Cnt") cnt.transform.position = Out2.position;
                else cnt.transform.position = Out3.position;
            }
        }
    }
}
