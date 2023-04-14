using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Infra_Classifier : MonoBehaviour
{
    Dictionary<string, List<string>> Options = new Dictionary<string, List<string>>();

    string CurType = "Color";
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

    public Transform Out1;
    public Transform Out2;
    public Transform Out3;
    
    Installation Ins;

    private void Awake()
    {
        Ins = GetComponent<Installation>();

        Options["Color"] = new List<string>(Ins.CM.ColorType.Keys){ "Cnt" };
        Options["Shape"] = new List<string>(Ins.CM.ShapeType.Keys){ "Cnt" };
        for (int i = 0; i < 3; i++) CurDetail.Add(Options["Color"][i]);
    }

    private void Start()
    {
        StartCoroutine(WorkGap());
    }
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

    public void Classify(GameObject Inp)
    {
        if (Vector3.Magnitude(Inp.transform.position - transform.GetChild(0).position) > 15) return;
        InpQueue.Add(new Tuple<string, string>(Inp.GetComponent<Convey_Object>().color,Inp.GetComponent<Convey_Object>().shape));
        Inp.SetActive(false);
    }

    IEnumerator WorkGap()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (InpQueue.Count == 0 || !Ins.OnWork) { continue; }
            var CurWork = InpQueue[0]; InpQueue.RemoveAt(0);
            GameObject cnt = Ins.CM.COM.ReturnObject(CurWork.Item2,CurWork.Item1);

            string c = "";
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
