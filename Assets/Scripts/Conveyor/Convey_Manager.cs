using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Convey_Manager : MonoBehaviour
{
    public Infra_Option Option;
    public InstallO_C OC;
    public ConveyTest CT;
    public TMP_Text PowerText;
    public List<Image> HPS;
    public Image Attacked;
    public int Power;
    public bool OptionAble = true;
    public bool IsPlaying = false;
    public Sprite HpOff;

    int CurHp = 5;


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
        for (int i = 1; i < transform.childCount - 1; i++)
        {
            for (int x = 0; x < transform.GetChild(i).childCount; x++)
            {
                transform.GetChild(i).GetChild(x).gameObject.SetActive(false);
                Destroy(transform.GetChild(i).GetChild(x).gameObject);
            }
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

    public void Errorr()
    {
        StopAllCoroutines();
        if(CurHp == 1)
        {
            Destroy(transform.parent.gameObject);
            print("나중에 연출 추가함");
        }
        else
        {
            HPS[--CurHp].sprite = HpOff;
            Attacked.color = new Color(1, 0, 0, 0.4f + (6 - CurHp) * 0.1f);
            StartCoroutine(ABC());
        }
    }

    IEnumerator ABC()
    {
        for(int i = 0; i < 100; i++)
        {
            Attacked.color -= new Color(0, 0, 0, 0.04f);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
