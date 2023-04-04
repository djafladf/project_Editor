using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TreeEditor;

public class InstallIcon : MonoBehaviour
{
    public InstallManager IM;
    public InstallO_C OC;
    public GameObject Inp;
    public Color Af;
    Color bf;

    GameObject Oup = null;
    Vector3 AnchorGap;

    private void Awake()
    {
        if (GetComponent<EventTrigger>() == null) gameObject.AddComponent<EventTrigger>();
        EventTrigger eventTrigger = GetComponent<EventTrigger>();
        MyUi.ButtonInit(eventTrigger, OnPointer, OutPointer, null);
        MyUi.AddEvent(eventTrigger, EventTriggerType.BeginDrag, DragOn);
        MyUi.AddEvent(eventTrigger, EventTriggerType.Drag, DragPointer);
        MyUi.AddEvent(eventTrigger, EventTriggerType.EndDrag, DragEnd);
        bf = GetComponent<Image>().color;
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
        Oup = Instantiate(Inp, transform.parent.parent.parent);
        Oup.transform.position = transform.position;
        Oup.transform.SetAsFirstSibling();
        IM.gameObject.SetActive(true);
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
        if (IM.CurLay == null) Destroy(Oup);
        else
        {
            IM.cnt = false;
            Oup.transform.position = IM.CurLay.transform.position;
            Oup.GetComponent<Installation>().InLayer = IM.CurLay;
            IM.CurLay.GetComponent<InstallLayer>().OutPointer(null);
            IM.CurLay.GetComponent<InstallLayer>().IsInstall = true;
            IM.CurLay.SetActive(false);
            IM.CurLay = null;
            IM.cnt = true;
        }
        IM.gameObject.SetActive(false);
    }
}
