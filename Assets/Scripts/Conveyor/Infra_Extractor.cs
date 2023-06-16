using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infra_Extractor : MonoBehaviour
{
    /// <summary>
    /// ������� �۵��� ����
    /// Object�� Shape�� Color�� ��������.
    /// TODO : ���� � Object�� �۾��� ���������� ��Ÿ���� Message �߰� ����
    /// </summary>
    public Transform Inp;
    public Transform Out;

    List<Tuple<string, string>> ObjList = new List<Tuple<string, string>>();
    // Color, Shape;

    Installation Ins;

    bool FirstInstall = true;

    private void Awake()
    {
        Ins = GetComponent<Installation>();
    }
    private void OnEnable()
    {
        if (FirstInstall) FirstInstall = false;
        else
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
            ObjList.Clear();
            StartCoroutine(Work());
        }
    }

    /// <summary>
    /// Object�� Color, Shape�� Deque�� ����.
    /// Object�� Color, Shape���� �ϳ��� ������ ������ �ٷ� ����
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

    /// <summary>
    /// Que�� Shape�� Color�� �� Object�� �����ؼ� Out���� �����Ŵ.
    /// </summary>
    /// <returns></returns>
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
