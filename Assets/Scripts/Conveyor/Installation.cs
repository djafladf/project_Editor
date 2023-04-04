using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.EventSystems;

public class Installation : MonoBehaviour
{
    public bool IsInstalled = false;
    public bool OnWork = false;
    public GameObject InLayer;
    Vector3 AnchorGap;
    void Awake()
    {
        if (GetComponent<EventTrigger>() == null) gameObject.AddComponent<EventTrigger>();
        EventTrigger eventTrigger = GetComponent<EventTrigger>();
    }
}
