using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infra_Option : MonoBehaviour
{
    public GameObject Clicked;
    public bool OnOption = false;

    public void OptionInit(GameObject _Clicked)
    {
        gameObject.SetActive(true);
        Clicked = _Clicked;
        Vector3 CurCamPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); CurCamPos.z = 0;
        transform.position = CurCamPos;
        if(Clicked.tag != "Infra") transform.GetChild(2).gameObject.SetActive(false);
        else transform.GetChild(2).gameObject.SetActive(true);
    }
}
