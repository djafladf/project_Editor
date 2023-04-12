using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Infra_Power : MonoBehaviour
{
    int Fuel = 0;
    public GameObject Message;
    GameObject CurM = null;
    Installation Ins;
    bool MCnt = false;


    private void Awake()
    {
        Ins = GetComponent<Installation>();
        MyUi.ButtonInit(GetComponent<EventTrigger>(), OnPointer, OutPointer, null);
    }
    private void Start()
    {
        StartCoroutine(Work());
    }
    public void AddFuel(GameObject _A)
    {
        if (_A.GetComponent<Convey_Object>().shape != "") ChangeFuel(20);
        if (_A.GetComponent<Convey_Object>().color != "") ChangeFuel(20);
        Destroy(_A);
    }
    void ChangeFuel(int ch) 
    {
        Fuel += ch;
        if(CurM != null) CurM.transform.GetChild(0).GetComponent<TMP_Text>().text = $":{Fuel}";
    }

    void OnPointer(PointerEventData Data)
    {
        if (MCnt)
        {
            StopAllCoroutines();
            StartCoroutine(Work());
            StartCoroutine(ShowMessage());
        }
    }
    void OutPointer(PointerEventData Data)
    {
        if (MCnt)
        {
            StopAllCoroutines();
            StartCoroutine(Work());
            StartCoroutine(EndMessage());
        }
        MCnt = true;
    }

    IEnumerator ShowMessage()
    {
        if (CurM == null)
        {
            CurM = Instantiate(Message, transform);
            CurM.transform.position = gameObject.transform.position;
        }
        for (;CurM.transform.position.y < gameObject.transform.position.y + 100; CurM.transform.position += Vector3.up * 10) yield return new WaitForSeconds(0.02f);
    }
    IEnumerator EndMessage()
    {
        if (!MCnt) MCnt = true;
        for (; CurM.transform.position.y > gameObject.transform.position.y; CurM.transform.position += Vector3.down * 10) yield return new WaitForSeconds(0.02f);
        Destroy(CurM);
        CurM = null;
    }

    IEnumerator Work()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (Fuel != 0) 
            {
                if (Fuel < 10)
                {
                    Ins.CM.PowerUpdate(Fuel);
                    ChangeFuel(-Fuel);
                }
                else
                {
                    Ins.CM.PowerUpdate(10);
                    ChangeFuel(-10);
                }

            }
        }
    }
}
