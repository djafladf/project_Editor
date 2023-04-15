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
    Image image;
    void Awake()
    {
        if (GetComponent<EventTrigger>() == null) gameObject.AddComponent<EventTrigger>();
        EventTrigger eventTrigger = GetComponent<EventTrigger>();
        MyUi.ButtonInit(eventTrigger, OnPointer, OutPointer, null);
        image = GetComponent<Image>();
    }
    void OnPointer(PointerEventData data)
    {
        if (!IsInstall)
        {
            IM.CurLay = gameObject;
            image.sprite = af;
        }
    }

    public void OutPointer(PointerEventData data)
    {
        if (!IsInstall && IM.cnt)
        {
            IM.CurLay = null;
            image.sprite = bf;
        }
    }
    public void DestroyInstall()
    {
        gameObject.SetActive(true);
        IsInstall = false;
        OutPointer(null);
    }
}
