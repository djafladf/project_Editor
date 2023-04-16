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
    Infra_Option IO;
    Image image;

    private void Awake()
    {
        if (GetComponent<EventTrigger>() == null) gameObject.AddComponent<EventTrigger>();
        MyUi.ButtonInit(GetComponent<EventTrigger>(), OnPointer, OutPointer, ClickPointer);
        BfColor = GetComponent<Image>().color;
        IO = transform.parent.gameObject.GetComponent<Infra_Option>();
        image = GetComponent<Image>();
    }
    void OnPointer(PointerEventData data)
    {
        image.color = AfColor;
        IO.OnOption = true;
    }
    void OutPointer(PointerEventData data)
    {
        image.color = BfColor;
        IO.OnOption = false;
    }
    void ClickPointer(PointerEventData data)
    {
        image.color = BfColor;
        switch (name)
        {
            case "Rotate":
                IO.Clicked.transform.Rotate(0, 0, -90);
                break;
            case "Delete":
                IO.Clicked.SetActive(false);
                break;
            case "Detail":
                IO.Clicked.GetComponent<Installation>().OpenSetting();
                break;
        }
        Parent.SetActive(false);
    }
}
