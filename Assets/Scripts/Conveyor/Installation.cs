using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.EventSystems;

public class Installation : MonoBehaviour
{
    public bool IsInstalled = false;
    public bool OnWork = false;     // 작동 여부(false면 해당 Infra는 작동하지 않는다.)
    public bool Manual = false;
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
    private void Start()
    {
        if (name[0] != 'P') StartCoroutine(ConsumePower());

    }

    IEnumerator ConsumePower()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (!CM.IsPlaying) continue;
            if (CM.Power <= 0) OnWork = false;
            else OnWork = true;
            if (OnWork) CM.PowerUpdate(-1);
        }
    }


    public void DelSelf()
    {
        Destroy(gameObject);
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
    private void OnDisable()
    {
        if (InLayer != null)
            InLayer.GetComponent<InstallLayer>().DestroyInstall();
    }
}
