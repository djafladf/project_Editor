using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Infra_Combine : MonoBehaviour
{
    public Transform Inp1;
    public Transform Inp2;
    public Transform Out;

    public GameObject Message;

    bool MCnt = false;

    GameObject CurM = null;

    string Inp1Type = "color";
    string Inp2Type = "shape";

    List<string> ColorList = new List<string>();
    List<string> ShapeList = new List<string>();

    Installation Ins;
    string Cnt1;
    string Cnt2;
    GameObject Cnt;
    Vector3 ObjPos;
    bool FirstInstall = true;

    private void Awake()
    {
        Ins = GetComponent<Installation>();
        MyUi.ButtonInit(GetComponent<EventTrigger>(), OnPointer, OutPointer, null);
    }

    private void OnEnable()
    {
        if (FirstInstall) FirstInstall = false;
        else
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
            ColorList.Clear(); ShapeList.Clear();
            Inp1Type = "color"; Inp2Type = "shape";
            StartCoroutine(Work());
        }
    }
    private void OnDisable()
    {
        if (!FirstInstall)
        {
            StopAllCoroutines();
            CurM = null;
            MCnt = false;
        }
    }

    void OnPointer(PointerEventData Data)
    {
        if (MCnt)
        {
            StopAllCoroutines();
            StartCoroutine(Work());
            StartCoroutine(ShowMessage());
        }
    }
    void OutPointer(PointerEventData Data)
    {
        if (MCnt)
        {
            StopAllCoroutines();
            StartCoroutine(Work());
            StartCoroutine(EndMessage());
        }
        MCnt = true;
    }

    IEnumerator ShowMessage()
    {
        if (CurM == null)
        {
            CurM = Instantiate(Message, transform);
            CurM.transform.position = gameObject.transform.position;
        }
        for (; CurM.transform.position.y < gameObject.transform.position.y + 100; CurM.transform.position += Vector3.up * 10) yield return new WaitForSeconds(0.02f);
    }
    IEnumerator EndMessage()
    {
        for (; CurM.transform.position.y > gameObject.transform.position.y; CurM.transform.position += Vector3.down * 10) yield return new WaitForSeconds(0.02f);
        Destroy(CurM);
        CurM = null;
    }
    /// <summary>
    /// Combine 전 Object의 정보(Color or Shape) Deque에 저장
    /// Color와 Shape값 모두 보유 중(Not "None")이라면 바로 배출.
    /// </summary>
    /// <param name="ObjColor">Object.color</param>
    /// <param name="ObjShape">Object.shape</param>
    /// <param name="_Transform">Object.transform</param>
    public void Combine(string ObjColor, string ObjShape,Transform _Transform)
    {
        ObjPos =_Transform.position;
        if(Vector3.Magnitude(Inp1.position - ObjPos) <= 16)
        {
            if (ObjColor != "None" && ObjShape != "None") { _Transform.position = Out.position; return; }
            if (Inp1Type == "color") 
            {
                if (ObjShape != "None") { _Transform.position = Out.position; return; }
                else ColorList.Add(ObjColor);
            }
            else if (Inp1Type == "shape")
            {
                if (ObjColor != "") {_Transform.position = Out.position; return; }
                else ShapeList.Add(ObjShape);
            }
            _Transform.gameObject.SetActive(false);
        }
        else if(Vector3.Magnitude(Inp2.position - ObjPos) <= 16)
        {
            if (Inp2Type == "color")
            {
                if (ObjShape != "None") { _Transform.transform.position = Out.position; return; }
                else ColorList.Add(ObjColor);
            }
            else if (Inp2Type == "shape")
            {
                if (ObjColor != "None") { _Transform.transform.position = Out.position; return; }
                else ShapeList.Add(ObjShape);
            }
            _Transform.gameObject.SetActive(false);
        }
        
    }

    void SubWork()
    {
        Cnt1 = ColorList[0]; ColorList.RemoveAt(0);
        Cnt2 = ShapeList[0]; ShapeList.RemoveAt(0);
        Cnt = Ins.CM.COM.ReturnObject(Cnt2,Cnt1);
        if(CurM != null)
        {
            CurM.transform.GetChild(0).GetComponent<Image>().color = Ins.CM.ColorType[Cnt1];
            CurM.transform.GetChild(1).GetComponent<Image>().sprite = Ins.CM.ShapeType[Cnt2].GetComponent<SpriteRenderer>().sprite;
        }
        Cnt.transform.position = Out.position;
    }

    IEnumerator Work()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (ColorList.Count == 0 || ShapeList.Count == 0 || !Ins.OnWork || !Ins.CM.IsPlaying) continue;
            SubWork();
        }
    }
}
