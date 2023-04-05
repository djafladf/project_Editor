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
    Dictionary<string,List<string>> Options = new Dictionary<string, List<string>>
    { {"Color", new List<string>() {"Red","Green","Blue" } }, 
      {"Shape", new List<string>() {"Rectangle","Triangle","Star"} } };

    string CurType = "Color";
    string[] CurDetail = new string[] { "Red", "Green", "Blue" };

    TMP_Dropdown Type_D;
    TMP_Dropdown Out1_D;
    TMP_Dropdown Out2_D;
    TMP_Dropdown Out3_D;
    EventTrigger Apply;
    GameObject Setting;

    public Transform Out1;
    public Transform Out2;
    public Transform Out3;
    public List<GameObject> InpQueue = new List<GameObject>();
    private void Start()
    {
        StartCoroutine(WorkGap());
    }
    public void InitSetting(GameObject _Setting)
    {
        Setting = _Setting;
        Type_D = _Setting.transform.GetChild(0).GetComponent<TMP_Dropdown>();
        Out1_D = _Setting.transform.GetChild(1).GetComponent<TMP_Dropdown>();
        Out2_D = _Setting.transform.GetChild(2).GetComponent<TMP_Dropdown>();
        Out3_D = _Setting.transform.GetChild(3).GetComponent<TMP_Dropdown>();
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
            CurDetail[0] = Out1_D.captionText.text;
            CurDetail[1] = Out2_D.captionText.text;
            CurDetail[2] = Out3_D.captionText.text;
            GetComponent<Installation>().CM.OptionAble = true;
            Destroy(Setting);
        }
        else
        {
            Debug.Log("�� �ɼ��� ���� �޶�� �մϴ�.");
        }
    }

    public void Classify(GameObject Inp)
    {
        if (Vector3.Magnitude(Inp.transform.position - transform.GetChild(0).position) > 15) return;
        Inp.SetActive(false);
        InpQueue.Add(Inp);
    }

    IEnumerator WorkGap()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (InpQueue.Count == 0) { continue; }
            GameObject CurWork = InpQueue[0]; InpQueue.RemoveAt(0);
            Convey_Object cnt = CurWork.GetComponent<Convey_Object>();

            string c = "";
            if (CurType == "Color") c = cnt.color;
            else if (CurType == "Shape") c = cnt.shape;
            if (CurDetail[2] == c) CurWork.transform.position = Out3.position;
            else if (CurDetail[1] == c) CurWork.transform.position = Out2.position;
            else CurWork.transform.position = Out1.position;
            CurWork.SetActive(true);
        }
    }
}
