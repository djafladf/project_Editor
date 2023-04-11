using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Infra_Combine : MonoBehaviour
{
    public Transform Inp1;
    public Transform Inp2;
    public Transform Out;

    public GameObject Obj1;     //Rec
    public GameObject Obj2;     //Star
    public GameObject Obj3;     //Tri
    
    Transform ObjL;

    string Inp1Type = "color";
    string Inp2Type = "shape";

    List<string> ColorList = new List<string>();
    List<string> ShapeList = new List<string>();

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
            { "Star", Obj2 },
            { "Triangle", Obj3 }
        };
        ObjL = transform.parent.parent.GetChild(1);
    }

    private void Start()
    {
        StartCoroutine(Work());
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
        Destroy(Obj);
    }

    IEnumerator Work()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (ColorList.Count == 0 || ShapeList.Count == 0 || !Ins.OnWork) continue;
            string Cnt1 = ColorList[0]; ColorList.RemoveAt(0);
            string Cnt2 = ShapeList[0]; ShapeList.RemoveAt(0);
            GameObject Cnt = Instantiate(ShapeType[Cnt2],ObjL);
            Cnt.GetComponent<Convey_Object>().shape = Cnt2;
            Cnt.GetComponent<Convey_Object>().color = Cnt1;
            Cnt.GetComponent<SpriteRenderer>().color = ColorType[Cnt1];
            Cnt.transform.position = Out.position;
        }
    }
}
