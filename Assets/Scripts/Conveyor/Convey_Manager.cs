using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Convey_Manager : MonoBehaviour
{/**
   * Conveyor 게임의 전반적인 시스템을 조절
   */
    [Header("- Scripts")]
    public Infra_Option Option;             // Option Script
    public InstallO_C OC;                   // Install Open&Close Script
    public ConveyTest CT;                   // Initial Point Script
    public Convey_ObjectManager COM;        // Object Manager Script
    [Header("- Prefabs")]
    public TMP_Text PowerText;              // 현재 Power를 나타내는 Text
    public List<Image> HPS;                 // 현재 HP를 나타내는 Image들
    public Image Attacked;                  // HP가 감소 될 때, 화면이 붉게 변하는 연출용
    public int Power;                       // 현재 Power량
    public Sprite HpOff;                    // HP가 감소됬을 때, HPS를 대신 할 Sprite
    public Transform Rails;                 // Rail들이 추가 될 Empty Object
    public Transform Etc;                   // Infra들이 추가 될 Empty Object

    [Header("Must Names.Count = Detail.Count")]
    [Header("- Colors")]
    public List<string> ColorNames;         // Object 구별에 사용 될 Color명들
    public List<Color> Colors;              // Object 구별에 사용 될 Color들

    [Header("- Shapes")]
    public List<string> ShapeNames;         // Object 구별에 사용 될 Shape명들
    public List<GameObject> Shapes;         // Object 구별에 사용 될 Shape들
    public GameObject NoneShape;            // Shape를 갖지 않는 Object

    // Colors와 ColorNames를 연결시켜 놓은 딕셔너리
    public Dictionary<string, Color> ColorType = new Dictionary<string, Color>();
    // Shpaes와 ShapeNames를 연결시켜 놓은 딕셔너리
    public Dictionary<string, GameObject> ShapeType = new Dictionary<string, GameObject>();

    [HideInInspector]
    public bool OptionAble = true;
    [HideInInspector]
    public bool IsPlaying = false;

    int CurHp = 5;                           // 현재 체력


    // 게임이 첫 시작 될 때, 미리 Prefab화 시켜 놓은 Color, Shpae들을 이름과 연결시킨다.
    private void Awake()
    {
        for (int i = 0; i < ColorNames.Count; i++) ColorType[ColorNames[i]] = Colors[i];
        for (int i = 0; i < ShapeNames.Count; i++) ShapeType[ShapeNames[i]] = Shapes[i];
    }

    /// <summary>
    /// 임의의 키 입력이 감지 될 시, Option이 켜져 있으면 Option을 끔
    /// </summary>
    private void LateUpdate()
    {
        if (Input.anyKey && Option.gameObject.activeSelf && !Option.OnOption) Option.gameObject.SetActive(false);
    }

    /// <summary>
    /// ChangeVal 만큼 Power를 변화시킴
    /// </summary>
    /// <param name="ChangeVal">변화량</param>
    public void PowerUpdate(int ChangeVal)
    {
        Power += ChangeVal;
        PowerText.text = $"Power : <color=#FFFF00>{Power} W</color>";
    }

    /// <summary>
    /// RollBack시 게임을 초기화 시킴(HP 제외)
    /// </summary>
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
        }
        for(int i = 0; i < Etc.childCount; i++)
        {
            Etc.GetChild(i).gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 게임 정지 도중, 배속 버튼을 누를 시 게임 시작.
    /// </summary>
    public void PlayStart()
    {
        IsPlaying = true;
        OptionAble = false;
        OC.OpenAble = false;
        CT.StartWork();
        if (OC.IsOpen) OC.Click(null);
    }

    /// <summary>
    /// 틀린 Object가 목적지에 도착 했을 시, HP가 깍이며, 모든 HP가 깍이면 게임 오버
    /// </summary>
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

    /// <summary>
    /// HP가 감소 될 때, 일시적으로 화면이 빨게졌다 서서히 옅어짐.
    /// </summary>
    /// <returns>None</returns>
    IEnumerator ABC()
    {
        var wfs = new WaitForSeconds(0.01f);
        for(int i = 0; i < 100; i++)
        {
            Attacked.color -= new Color(0, 0, 0, 0.04f);
            yield return wfs;
        }
    }
}
