using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Convey_Manager : MonoBehaviour
{
    public Infra_Option Option;
    public InstallO_C OC;
    public ConveyTest CT;
    public TMP_Text PowerText;
    public int Power;
    public bool OptionAble = true;
    public bool IsPlaying = false;

    private void LateUpdate()
    {
        if (Input.anyKey && Option.gameObject.activeSelf && !Option.OnOption) Option.gameObject.SetActive(false);
    }

    public void PowerUpdate(int ChangeVal)
    {
        Power += ChangeVal;
        PowerText.text = $"Power : <color=#FFFF00>{Power} W</color>";
    }

    public void RollBack()
    {
        PowerUpdate(-Power + 300);
        IsPlaying = false;
        OptionAble = true;
        OC.OpenAble = true;
        CT.StopWork();
        for (int i = 1; i < transform.childCount-1; i++)
        {
            for (int x = 0; x < transform.GetChild(i).childCount; x++) Destroy(transform.GetChild(i).GetChild(x).gameObject);
        }
    }
    public void PlayStart()
    {
        IsPlaying = true;
        OptionAble = false;
        OC.OpenAble = false;
        CT.StartWork();
        if (OC.IsOpen) OC.Click(null);
    }

}
