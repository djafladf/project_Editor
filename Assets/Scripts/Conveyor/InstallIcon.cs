using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;

public class InstallIcon : MonoBehaviour
{
    public InstallManager IM;
    public InstallO_C OC;
    public GameObject Inp;
    public GameObject Inp_Img;
    public Transform Infras;
    public Color Af;
    public int MaxInstall;

    Color bf;

    GameObject Oup;
    Vector3 AnchorGap;
    GameObject[] Pools;

    bool j;

    private void Awake()
    {
        if (GetComponent<EventTrigger>() == null) gameObject.AddComponent<EventTrigger>();
        EventTrigger eventTrigger = GetComponent<EventTrigger>();
        MyUi.ButtonInit(eventTrigger, OnPointer, OutPointer, null);
        MyUi.AddEvent(eventTrigger, EventTriggerType.BeginDrag, DragOn);
        MyUi.AddEvent(eventTrigger, EventTriggerType.Drag, DragPointer);
        MyUi.AddEvent(eventTrigger, EventTriggerType.EndDrag, DragEnd);
        bf = GetComponent<Image>().color;

        Oup = Instantiate(Inp_Img, transform.parent.parent.parent);
        Oup.SetActive(false);

        Pools = new GameObject[MaxInstall];
        for (int i = 0; i < MaxInstall; i++)
        {
            Pools[i] = Instantiate(Inp, Infras);
            Pools[i].SetActive(false);
        }
    }

    void OnPointer(PointerEventData data)
    {
        GetComponent<Image>().color = Af;
    }

    void OutPointer(PointerEventData data)
    {
        GetComponent<Image>().color = bf;
    }

    void DragOn(PointerEventData Data)
    {
        Oup.SetActive(true);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); mousePos.z = 0;
        Oup.transform.position = mousePos;
        StartCoroutine(OC.InstallOC());
    }
    void DragPointer(PointerEventData data)
    {
        MyUi.DragUI(Oup, Vector3.zero);
    }
    void DragEnd(PointerEventData data)
    {
        Oup.SetActive(false);
        j = false;
        if(IM.CurLay != null)
        {
            IM.cnt = false;
            foreach(var _A in Pools)
            {
                if (!_A.activeSelf)
                {
                    _A.SetActive(true);
                    _A.GetComponent<Installation>().InLayer = IM.CurLay;
                    _A.transform.position = IM.CurLay.transform.position;
                    IM.CurLay.GetComponent<InstallLayer>().OutPointer(null);
                    IM.CurLay.GetComponent<InstallLayer>().IsInstall = true;
                    IM.CurLay.SetActive(false);
                    IM.CurLay = null;
                    IM.cnt = true;
                    j = true;
                    break;
                }
            }
            if (!j)
            {
                Debug.Log("설치 가능 용량 초과!");
            }
        }

    }
}
