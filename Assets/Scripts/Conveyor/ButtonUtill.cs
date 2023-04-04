using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonUtill : MonoBehaviour
{
     public static void AddEvent(EventTrigger eventTrigger, EventTriggerType Type, Action<PointerEventData> Event)
     {
         EventTrigger.Entry entry = new EventTrigger.Entry();
         entry.eventID = Type;
         entry.callback.AddListener((data) => { Event((PointerEventData)data); });
         eventTrigger.triggers.Add(entry);
     }

     public static void ButtonInit(EventTrigger eventTrigger, Action<PointerEventData> OnPointer, Action<PointerEventData> OutPointer, Action<PointerEventData> ClickPointer)
     {
         //data.pointerId : left -> -1, right -> -2, Wheel -> -3;
         if (OnPointer != null) AddEvent(eventTrigger, EventTriggerType.PointerEnter, OnPointer);

         if (OutPointer != null) AddEvent(eventTrigger, EventTriggerType.PointerExit, OutPointer);

         if (ClickPointer != null) AddEvent(eventTrigger, EventTriggerType.PointerClick, ClickPointer);
     }
}
