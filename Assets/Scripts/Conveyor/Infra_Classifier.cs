using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class Infra_Classifier : MonoBehaviour
{
    public Dictionary<string,List<string>> Options = new Dictionary<string, List<string>>
    { {"Color", new List<string>() {"Red","Green","Blue" } }, 
      {"Shape", new List<string>() {"Rectangle","Triangle","Star"} } };
    public Tuple<string, string> Top = new Tuple<string, string>("Color","Red");
    public Tuple<string, string> Mid = new Tuple<string, string>("Color","Green");
    public Tuple<string, string> Bottom = new Tuple<string, string>("Color","Blue");
    public Transform Out1;
    public Transform Out2;
    public Transform Out3;
    public List<GameObject> InpQueue = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(WorkGap());
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
            if (Compare(Top, cnt)) CurWork.transform.position = Out1.position;
            if (Compare(Mid, cnt)) CurWork.transform.position = Out2.position;
            if (Compare(Bottom, cnt)) CurWork.transform.position = Out3.position;
            CurWork.SetActive(true);
        }
    }

    bool Compare(Tuple<string,string> a,Convey_Object b)
    {
        if (a == null) return false;
        string c = "";
        if (a.Item1 == "Color") c = b.color;
        else if (a.Item1 == "Shape") c = b.shape;
        return a.Item2 == c;
    }
}
