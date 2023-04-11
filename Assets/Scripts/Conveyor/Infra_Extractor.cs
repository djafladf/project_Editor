using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class Infra_Extractor : MonoBehaviour
{
    public Transform Inp;
    public Transform Out;

    public GameObject Obj1;     //Rec
    public GameObject Obj2;     //Star
    public GameObject Obj3;     //Tri
    public GameObject NoneObj;  //Contain Color : Circle

    Transform ObjL;



    List<Tuple<string, string>> ObjList = new List<Tuple<string, string>>();
    // Color, Shape;

    Dictionary<string, Color> ColorType = new Dictionary<string, Color>()
    {
        {"Red",Color.red },
        {"Blue",Color.blue },
        {"Green",Color.green }
    };
    Dictionary<string, GameObject> ShapeType;

    Installation Ins;

    private void Awake()
    {
        Ins = GetComponent<Installation>();
        ShapeType = new Dictionary<string, GameObject>()
        {
            { "Rectangle", Obj1 },
            { "Triangle", Obj2 },
            { "Star", Obj3 }
        };
        ObjL = transform.parent.parent.GetChild(1);
    }
    private void Start()
    {
        StartCoroutine(Work());
    }

    public void Extract(GameObject Obj)
    {
        if (Vector3.Magnitude(Inp.position - Obj.transform.position) > 15) return;
        string ObjColor = Obj.GetComponent<Convey_Object>().color;
        string ObjShape = Obj.GetComponent<Convey_Object>().shape;
        if(ObjColor == "" || ObjShape == "")
        {
            Obj.transform.position = Out.position; return;
        }
        ObjList.Add(new Tuple<string,string> (ObjColor,ObjShape));
        Destroy(Obj);
    }

    IEnumerator Work()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (ObjList.Count == 0 || !Ins.OnWork) continue;
            var cnt = ObjList[0]; ObjList.RemoveAt(0);

            // Color Extract
            GameObject Cnt = Instantiate(NoneObj,ObjL);
            Cnt.GetComponent<Convey_Object>().color = cnt.Item1;
            Cnt.GetComponent<SpriteRenderer>().color = ColorType[cnt.Item1];
            Cnt.transform.position = Out.position;

            yield return new WaitForSeconds(0.5f);
            // Shape Extract
            GameObject Cnt2 = Instantiate(ShapeType[cnt.Item2], ObjL);
            Cnt2.GetComponent<Convey_Object>().shape = cnt.Item2;
            Cnt2.transform.position = Out.position;
            Cnt2.transform.position = Out.position;
        }
    }
}
