using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InstallO_C : MonoBehaviour
{
    public Color Af;
    Color bf;
    bool IsOpen = false;
    private void Awake()
    {
        if (GetComponent<EventTrigger>() == null) gameObject.AddComponent<EventTrigger>();
        MyUi.ButtonInit(GetComponent<EventTrigger>(), OnPointer, OutPointer, Click);
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

    void Click(PointerEventData Data)
    {
        StopAllCoroutines();
        StartCoroutine(InstallOC());
    }
    public IEnumerator InstallOC()
    {
        GetComponent<RectTransform>().Rotate(0, 0, 180);
        float G = 0;
        float dx;
        Transform Parent = transform.parent;
        if (IsOpen)
        {
            G = 1000;
            dx = 1;
        }
        else
        {
            G = 200;
            dx = -1;
        }
        IsOpen = !IsOpen;
        for (;Parent.transform.position.x != G;)
        {
            Vector3 VA = new Vector3(Parent.transform.position.x+dx * 80, Parent.transform.position.y,Parent.transform.position.z);
            Parent.transform.position = VA;
            yield return new WaitForSeconds(0.01f);
        }
        yield break;
    }
}
