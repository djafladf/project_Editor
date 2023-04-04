using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.EventSystems;

public class Installation : MonoBehaviour
{
    public GameObject BfIn = null;
    public GameObject AfIn = null;

    public bool IsInstalled = false;
    public bool OnWork = false;     // �۵� ����(false�� �ش� Infra�� �۵����� �ʴ´�.)
    public GameObject InLayer;
    public GameObject Option;

    void Awake()
    {
        if (GetComponent<EventTrigger>() == null) gameObject.AddComponent<EventTrigger>();
        EventTrigger eventTrigger = GetComponent<EventTrigger>();
        MyUi.AddEvent(eventTrigger, EventTriggerType.PointerClick, Click);
    }

    public void DelSelf()
    {
        Disconnect();
        InLayer.SetActive(true);
        InLayer.GetComponent<InstallLayer>().IsInstall = false;
        Destroy(gameObject);
    }
    public void Disconnect()
    {
        if (AfIn == null || OnWork == false) return;
        OnWork = false;
        AfIn.GetComponent<Installation>().Disconnect();
    }
    public void Connect()
    {
        if (AfIn == null || BfIn == null) return;
        if (BfIn.GetComponent<Installation>().OnWork == false) return;
        AfIn.GetComponent<Installation>().Connect();
    }

    void Click(PointerEventData Data)
    {
        if(Data.pointerId == -2)
        {
            transform.parent.gameObject.GetComponent<Convey_Manager>().Option.OptionInit(gameObject); 
        }
    }

}
