using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.EventSystems;

public class Belts : MonoBehaviour
{
    public bool IsInstalled = false;
    public GameObject InLayer;
    Vector3 AnchorGap;
    void Awake()
    {
        if (GetComponent<EventTrigger>() == null) gameObject.AddComponent<EventTrigger>();
        EventTrigger eventTrigger = GetComponent<EventTrigger>();
        MyUi.AddEvent(eventTrigger,EventTriggerType.PointerClick,ClickPointer);
    }
    void ClickPointer(PointerEventData data)
    {
        if (data.pointerId == -2)
        {
            transform.Rotate(0,0,-90);
        }
    }
}
