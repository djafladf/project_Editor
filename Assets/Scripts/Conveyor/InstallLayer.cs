using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TreeEditor;

public class InstallLayer : MonoBehaviour
{
    public InstallManager IM;
    public Sprite bf;
    public Sprite af;
    public bool IsInstall = false;
    void Awake()
    {
        if (GetComponent<EventTrigger>() == null) gameObject.AddComponent<EventTrigger>();
        EventTrigger eventTrigger = GetComponent<EventTrigger>();
        MyUi.ButtonInit(eventTrigger, OnPointer, OutPointer, null);
    }
    void OnPointer(PointerEventData data)
    {
        if (!IsInstall)
        {
            IM.CurLay = gameObject;
            GetComponent<Image>().sprite = af;
        }
    }

    public void OutPointer(PointerEventData data)
    {
        if (!IsInstall && IM.cnt)
        {
            IM.CurLay = null;
            GetComponent<Image>().sprite = bf;
        }
    }
}
