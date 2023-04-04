using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Infra_OptionButton : MonoBehaviour
{
    public GameObject Parent;
    public Color AfColor;
    Color BfColor;

    private void Awake()
    {
        if (GetComponent<EventTrigger>() == null) gameObject.AddComponent<EventTrigger>();
        MyUi.ButtonInit(GetComponent<EventTrigger>(), OnPointer, OutPointer, ClickPointer);
        BfColor = GetComponent<Image>().color;
    }
    void OnPointer(PointerEventData data)
    {
        GetComponent<Image>().color = AfColor;
    }
    void OutPointer(PointerEventData data)
    {
        GetComponent<Image>().color = BfColor;
    }
    void ClickPointer(PointerEventData data)
    {
        GetComponent<Image>().color = BfColor;
        switch (name)
        {
            case "Rotate":
                transform.parent.GetComponent<Infra_Option>().Clicked.transform.Rotate(0, 0, -90);
                break;
            case "Delete":
                Destroy(transform.parent.GetComponent<Infra_Option>().Clicked);
                break;
            case "Setting":
                break;
        }
        Parent.SetActive(false);
    }
}
