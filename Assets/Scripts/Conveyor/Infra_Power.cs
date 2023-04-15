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
    /// <summary>
    /// Object가 Shape값을 가지면 Fuel을 20 추가, Color값을 가지면 Fuel을 20 추가.(중복 적용)
    /// </summary>
    /// <param name="_Color">Object.color</param>
    /// <param name="_Shape">Object.shape</param>
    public void AddFuel(string _Color, string _Shape)
    {
        if (_Color != "") ChangeFuel(20);
        if (_Shape != "") ChangeFuel(20);
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
        var wfs = new WaitForSeconds(0.5f);
        while (true)
        {
            yield return wfs;
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
