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

    private void Awake()
    {
        Ins = GetComponent<Installation>();
        MyUi.ButtonInit(GetComponent<EventTrigger>(), OnPointer, OutPointer, null);
    }

    private void Start()
    {
        StartCoroutine(Work());
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

    public void Combine(GameObject Obj)
    {
        string ObjColor = Obj.GetComponent<Convey_Object>().color;
        string ObjShape = Obj.GetComponent<Convey_Object>().shape;
        if (ObjColor != "" && ObjShape != "") { Obj.transform.position = Out.position; return; }

        Vector3 ObjPos = Obj.transform.position;
        if(Vector3.Magnitude(Inp1.position - ObjPos) <= 15)
        {
            if (Inp1Type == "color") 
            {
                if (ObjShape != "") { Obj.transform.position = Out.position; return; }
                else ColorList.Add(ObjColor);
            }
            else if (Inp1Type == "shape")
            {
                if (ObjColor != "") { Obj.transform.position = Out.position; return; }
                else ShapeList.Add(ObjShape);
            }
        }
        else if(Vector3.Magnitude(Inp2.position - ObjPos) <= 15)
        {
            if (Inp2Type == "color")
            {
                if (ObjShape != "") { Obj.transform.position = Out.position; return; }
                else ColorList.Add(ObjColor);
            }
            else if (Inp2Type == "shape")
            {
                if (ObjColor != "") { Obj.transform.position = Out.position; return; }
                else ShapeList.Add(ObjShape);
            }
        }
        Obj.SetActive(false);
    }

    void SubWork()
    {
        string Cnt1 = ColorList[0]; ColorList.RemoveAt(0);
        string Cnt2 = ShapeList[0]; ShapeList.RemoveAt(0);
        GameObject Cnt = Ins.CM.COM.ReturnObject(Cnt2,Cnt1);
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
