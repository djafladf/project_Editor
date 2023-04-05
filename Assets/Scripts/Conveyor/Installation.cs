using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.EventSystems;

public class Installation : MonoBehaviour
{
    public List<GameObject> BfInfra = new List<GameObject>();
    public List<GameObject> AfInfra = new List<GameObject>();

    public bool IsInstalled = false;
    public bool OnWork = false;     // 작동 여부(false면 해당 Infra는 작동하지 않는다.)
    public int type;

    public GameObject InLayer;
    public GameObject Setting;

    public Convey_Manager CM;

    public bool TouchAble = true;
    void Awake()
    {
        if (GetComponent<EventTrigger>() == null) gameObject.AddComponent<EventTrigger>();
        EventTrigger eventTrigger = GetComponent<EventTrigger>();
        MyUi.AddEvent(eventTrigger, EventTriggerType.PointerClick, Click);
        if(type != 5) CM = transform.parent.parent.GetComponent<Convey_Manager>();
    }

    public void DelSelf()
    {
        Disconnect();
        InLayer.SetActive(true);
        InLayer.GetComponent<InstallLayer>().IsInstall = false;
        InLayer.GetComponent<InstallLayer>().OutPointer(null);
        Destroy(gameObject);
    }
    public void Disconnect()
    {
        if (AfInfra.Count == 0 || OnWork == false) return;
        OnWork = false;
        foreach(var a in AfInfra) a.GetComponent<Installation>().Disconnect();
    }
    public void Connect()
    {
        if (AfInfra.Count == 0 || BfInfra.Count == 0) return;
        if (OnWork == false) return;
        foreach(var a in AfInfra)
        {
            Installation b = a.GetComponent<Installation>();
            if (b.OnWork == true) continue;
            b.OnWork = true; b.Connect();
        }
    }

    public void OpenSetting()
    {
        GameObject Cnt = Instantiate(Setting, transform.parent);
        if (type == 1)   // 분류기일때
            GetComponent<Infra_Classifier>().InitSetting(Cnt);
        Cnt.transform.position = transform.position;
        CM.OptionAble = false;
    }

    void Click(PointerEventData Data)
    {
        if (!TouchAble) return;
        if(Data.pointerId == -2)
        {
            CM.Option.OptionInit(gameObject); 
        }
    }

}
