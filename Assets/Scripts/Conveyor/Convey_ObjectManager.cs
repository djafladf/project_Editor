using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Convey_ObjectManager : MonoBehaviour
{
    public Convey_Manager CM;
    public Dictionary<string, List<GameObject>> Objects = new Dictionary<string, List<GameObject>>();


    private void Awake()
    {
        MakePool();
    }

    void MakePool()
    {
        for (int i = 0; i < CM.ShapeNames.Count; i++) 
        {
            List<GameObject> Cnt = new List<GameObject>();
            for (int x = 0; x < 50; x++)
            {
                GameObject Tmp = Instantiate(CM.Shapes[i], transform);
                Tmp.SetActive(false);
                Cnt.Add(Tmp);
            }
            Objects[CM.ShapeNames[i]] = Cnt;
        }
        List<GameObject> Cnt1 = new List<GameObject>();
        for (int x = 0; x < 50; x++)
        {
            GameObject Tmp = Instantiate(CM.NoneShape, transform);
            Tmp.SetActive(false);
            Cnt1.Add(Tmp);
        }
        Objects["None"] = Cnt1;
    }
    /// <summary>
    /// Object Pooling
    /// </summary>
    /// <param name="_Shape">도형 이름(도형 X는 "None"으로)</param>
    /// <param name="_Color">색 이름(색 X는 "None"으로)</param>
    /// <returns></returns>
    public GameObject ReturnObject(string _Shape, string _Color)
    {
        foreach(var a in Objects[_Shape])
        {
            if (!a.activeSelf)
            {
                a.SetActive(true);
                a.GetComponent<Convey_Object>().color = _Color;
                a.GetComponent<Convey_Object>().shape = _Shape;
                if (_Color != "None")
                {
                    if(_Shape != "None") a.GetComponent<SpriteRenderer>().color = CM.ColorType[_Color];
                    else a.transform.GetChild(0).GetComponent<SpriteRenderer>().color = CM.ColorType[_Color];
                }
                else a.GetComponent<SpriteRenderer>().color = Color.white;
                return a;
            }
        }
        return null;
    }
    public void DelObj()
    {
        foreach(var a in Objects.Values)
        {
            foreach (var b in a) b.SetActive(false);
        }
    }
}
