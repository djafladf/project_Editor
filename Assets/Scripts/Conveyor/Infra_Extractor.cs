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

    /// <summary>
    /// Object의 Color, Shape를 Deque에 저장.
    /// Object가 Color, Shape값을 하나라도 가지지 않으면 바로 배출
    /// </summary>
    /// <param name="_color">Object.color</param>
    /// <param name="_shape">Object.shape</param>
    /// <param name="_Transform">Object.transform</param>
    public void Extract(string _color, string _shape,Transform _Transform)
    {
        if (Vector3.Magnitude(Inp.position - _Transform.position) > 16) return;
        if(_color == "None" || _shape == "None")
        {
            _Transform.position = Out.position; return;
        }
        ObjList.Add(new Tuple<string,string> (_color,_shape));
        _Transform.gameObject.SetActive(false);
    }

    IEnumerator Work()
    {
        Tuple<string, string> cnt;
        GameObject Cnt1;
        GameObject Cnt2;
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (ObjList.Count == 0 || !Ins.OnWork) continue;
            cnt = ObjList[0]; ObjList.RemoveAt(0);

            // Color Extract
            Cnt1 = Ins.CM.COM.ReturnObject("None",cnt.Item1);
            Cnt1.transform.position = Out.position;

            yield return new WaitForSeconds(0.5f);
            // Shape Extract
            Cnt2 = Ins.CM.COM.ReturnObject(cnt.Item2,"None");
            Cnt2.transform.position = Out.position;
        }
    }
}
