using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infra_Extractor : MonoBehaviour
{
    public Transform Inp;
    public Transform Out;

    List<Tuple<string, string>> ObjList = new List<Tuple<string, string>>();
    // Color, Shape;

    Installation Ins;

    private void Awake()
    {
        Ins = GetComponent<Installation>();
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
        Obj.SetActive(false);
    }

    IEnumerator Work()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (ObjList.Count == 0 || !Ins.OnWork) continue;
            var cnt = ObjList[0]; ObjList.RemoveAt(0);

            // Color Extract
            GameObject Cnt = Ins.CM.COM.ReturnObject("None",cnt.Item1);
            Cnt.transform.position = Out.position;

            yield return new WaitForSeconds(0.5f);
            // Shape Extract
            GameObject Cnt2 = Ins.CM.COM.ReturnObject(cnt.Item2,"None");
            Cnt2.transform.position = Out.position;
        }
    }
}
