using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Convey_Manager : MonoBehaviour
{
    [Header("- Scripts")]
    public Infra_Option Option;
    public InstallO_C OC;
    public ConveyTest CT;
    public Convey_ObjectManager COM;
    [Header("- Prefabs")]
    public TMP_Text PowerText;
    public List<Image> HPS;
    public Image Attacked;
    public int Power;
    public Sprite HpOff;
    public Transform Rails;
    public Transform Etc;

    [Header("Must Names.Count = Detail.Count")]
    [Header("- Colors")]
    public List<string> ColorNames;
    public List<Color> Colors;

    [Header("- Shapes")]
    public List<string> ShapeNames;
    public List<GameObject> Shapes;
    public GameObject NoneShape;

    public Dictionary<string, Color> ColorType = new Dictionary<string, Color>();
    public Dictionary<string, GameObject> ShapeType = new Dictionary<string, GameObject>();

    [HideInInspector]
    public bool OptionAble = true;
    [HideInInspector]
    public bool IsPlaying = false;


    int CurHp = 5;

    private void Awake()
    {
        for (int i = 0; i < ColorNames.Count; i++) ColorType[ColorNames[i]] = Colors[i];
        for (int i = 0; i < ShapeNames.Count; i++) ShapeType[ShapeNames[i]] = Shapes[i];
    }


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
        COM.DelObj();
        for(int i = 0; i < Rails.childCount; i++)
        {
            Rails.GetChild(i).gameObject.SetActive(false);
            Destroy(Rails.GetChild(i).gameObject);
        }
        for(int i = 0; i < Etc.childCount; i++)
        {
            Etc.GetChild(i).gameObject.SetActive(false);
            Destroy(Etc.GetChild(i).gameObject);
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
