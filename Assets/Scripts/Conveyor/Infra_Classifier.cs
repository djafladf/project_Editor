using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Infra_Classifier : MonoBehaviour
{
    /*
     * �з����� �۵��� ��� + Setting(InitSetting, DropChange, GetChange)�� �Ϻκ� ����
     */


    // Color, Shpae��  �̸����� ��� ��ųʸ�. ����� ConveyManager�κ��� ������.
    Dictionary<string, List<string>> Options = new Dictionary<string, List<string>>();

    string CurType = "Color";   // ���� �з� Type

    // Setting��
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

    public Transform Out1;      // �ⱸ 1
    public Transform Out2;      // �ⱸ 2
    public Transform Out3;      // �ⱸ 3
    
    Installation Ins;           // Installation.cs
    bool FirstInstall = true;   // ù Enable��(Pooling �� ��) ���� ���ܿ�

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
    /// Pool���� ������ ������ �ʱ�ȭ��Ŵ.
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
    /// Setting �ʱ�ȭ
    /// </summary>
    /// <param name="_Setting">���� Object�� Setting ������ Object</param>
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
    /// ���� Setting Object�� Type ���� �� Drop�� ���빰�� �ʱ�ȭ��Ŵ.
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
    /// Setting�� ������ ������ ���� Infra�� �����Ŵ
    /// </summary>
    /// <param name="Data">�������� �������� �� �Լ����� ������� ����.</param>
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
            Debug.Log("�� �ɼ��� ���� �޶�� �մϴ�.");
        }
    }

    /// <summary>
    /// �з� �� Deque�� ����
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
    /// ���� �۵� �κ�
    /// 0.5�ʸ��� 1���� �۾��� �����ϸ�
    /// ���� Que�� Object�� ���� Infra�� Type�� �����Ͽ� 1,2,3�� �ⱸ �� �� ������ ����.
    /// Type�� ���� �ʴ� ��� ������ 3������ ����.
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
