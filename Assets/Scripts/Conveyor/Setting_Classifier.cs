using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Setting_Classifier : MonoBehaviour
{
    public Infra_Classifier Parent;
    public Dropdown Out1;
    public Dropdown Out2;
    public Dropdown Out3;

    private void Start()
    {
        if(GetComponent<Dropdown>()!=null) GetComponent<Dropdown>().onValueChanged.AddListener(test);
        else { return; }
    }
    
    void test(int text)
    {
        Debug.Log(text);
    }
    
}
