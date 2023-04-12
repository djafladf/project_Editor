using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Infra_Goal : MonoBehaviour
{
    public Sprite ShapeSprite;
    public Color Color;
    public string ShapeG;
    public string ColorG;
    public GameObject Message;
    public int Goal;

    int CurGoal = 0;

    GameObject CurM = null;
    Convey_Manager CM;

    private void Awake()
    {
        MyUi.ButtonInit(GetComponent<EventTrigger>(), OnPointer, OutPointer, null);
    }

    private void Start()
    {
        CM = GetComponent<Installation>().CM;
    }

    public void JudgeObj(GameObject _A)
    {
        Convey_Object Cnt = _A.GetComponent<Convey_Object>();
        if(Cnt.color == ColorG && Cnt.shape == ShapeG)
        {
            CurGoal++;
            if (CurM != null) CurM.transform.GetChild(1).GetComponent<TMP_Text>().text = $"{CurGoal} / {Goal}";
        }
        else
        {
            CM.Errorr();
        }
        Destroy(_A);
    }

    void OnPointer(PointerEventData Data)
    {
        StopAllCoroutines();
        StartCoroutine(ShowMessage());
    }
    void OutPointer(PointerEventData Data)
    {
        StopAllCoroutines();
        StartCoroutine(EndMessage());
    }

    IEnumerator ShowMessage()
    {
        if (CurM == null)
        {
            CurM = Instantiate(Message, transform);
            CurM.transform.GetChild(0).GetComponent<Image>().sprite = ShapeSprite;
            CurM.transform.GetChild(0).GetComponent<Image>().color = Color;
            CurM.transform.GetChild(1).GetComponent<TMP_Text>().text = $"{CurGoal} / {Goal}";
            CurM.transform.position = gameObject.transform.position;
        }
        for (; CurM.transform.position.y < gameObject.transform.position.y + 100; CurM.transform.position += Vector3.up * 10) yield return new WaitForSeconds(0.02f);
    }
    IEnumerator EndMessage()
    {
        for (; CurM.transform.position.y > gameObject.transform.position.y; CurM.transform.position += Vector3.down * 10) yield return new WaitForSeconds(0.02f);
        Destroy(CurM);
        CurM = null;
    }
}
