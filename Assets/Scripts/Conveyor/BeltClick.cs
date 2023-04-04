using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.EventSystems;

public class BeltClick : MonoBehaviour
{
    Vector3 AnchorGap;
    void Start()
    {
        if (GetComponent<EventTrigger>() == null) gameObject.AddComponent<EventTrigger>();
        EventTrigger eventTrigger = GetComponent<EventTrigger>();
        MyUi.AddEvent(eventTrigger, EventTriggerType.BeginDrag, DragOn);
        MyUi.AddEvent(eventTrigger, EventTriggerType.Drag, DragPointer);
        MyUi.AddEvent(eventTrigger,EventTriggerType.PointerClick,ClickPointer);
    }
    void DragOn(PointerEventData Data)
    {
        DragSetting();
    }

    void DragPointer(PointerEventData data)
    {
        MyUi.DragUI(gameObject, AnchorGap);
    }

    void DragSetting()
    {
        AnchorGap = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.parent.position;
        AnchorGap.z = 0;
    }
    void ClickPointer(PointerEventData data)
    {
        if (data.pointerId == -2)
        {
            transform.Rotate(0,0,-90);
        }
    }
}
