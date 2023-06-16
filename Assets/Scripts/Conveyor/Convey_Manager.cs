using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Convey_Manager : MonoBehaviour
{/**
   * Conveyor ������ �������� �ý����� ����
   */
    [Header("- Scripts")]
    public Infra_Option Option;             // Option Script
    public InstallO_C OC;                   // Install Open&Close Script
    public ConveyTest CT;                   // Initial Point Script
    public Convey_ObjectManager COM;        // Object Manager Script
    [Header("- Prefabs")]
    public TMP_Text PowerText;              // ���� Power�� ��Ÿ���� Text
    public List<Image> HPS;                 // ���� HP�� ��Ÿ���� Image��
    public Image Attacked;                  // HP�� ���� �� ��, ȭ���� �Ӱ� ���ϴ� �����
    public int Power;                       // ���� Power��
    public Sprite HpOff;                    // HP�� ���҉��� ��, HPS�� ��� �� Sprite
    public Transform Rails;                 // Rail���� �߰� �� Empty Object
    public Transform Etc;                   // Infra���� �߰� �� Empty Object

    [Header("Must Names.Count = Detail.Count")]
    [Header("- Colors")]
    public List<string> ColorNames;         // Object ������ ��� �� Color���
    public List<Color> Colors;              // Object ������ ��� �� Color��

    [Header("- Shapes")]
    public List<string> ShapeNames;         // Object ������ ��� �� Shape���
    public List<GameObject> Shapes;         // Object ������ ��� �� Shape��
    public GameObject NoneShape;            // Shape�� ���� �ʴ� Object

    // Colors�� ColorNames�� ������� ���� ��ųʸ�
    public Dictionary<string, Color> ColorType = new Dictionary<string, Color>();
    // Shpaes�� ShapeNames�� ������� ���� ��ųʸ�
    public Dictionary<string, GameObject> ShapeType = new Dictionary<string, GameObject>();

    [HideInInspector]
    public bool OptionAble = true;
    [HideInInspector]
    public bool IsPlaying = false;

    int CurHp = 5;                           // ���� ü��


    // ������ ù ���� �� ��, �̸� Prefabȭ ���� ���� Color, Shpae���� �̸��� �����Ų��.
    private void Awake()
    {
        for (int i = 0; i < ColorNames.Count; i++) ColorType[ColorNames[i]] = Colors[i];
        for (int i = 0; i < ShapeNames.Count; i++) ShapeType[ShapeNames[i]] = Shapes[i];
    }

    /// <summary>
    /// ������ Ű �Է��� ���� �� ��, Option�� ���� ������ Option�� ��
    /// </summary>
    private void LateUpdate()
    {
        if (Input.anyKey && Option.gameObject.activeSelf && !Option.OnOption) Option.gameObject.SetActive(false);
    }

    /// <summary>
    /// ChangeVal ��ŭ Power�� ��ȭ��Ŵ
    /// </summary>
    /// <param name="ChangeVal">��ȭ��</param>
    public void PowerUpdate(int ChangeVal)
    {
        Power += ChangeVal;
        PowerText.text = $"Power : <color=#FFFF00>{Power} W</color>";
    }

    /// <summary>
    /// RollBack�� ������ �ʱ�ȭ ��Ŵ(HP ����)
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
    /// ���� ���� ����, ��� ��ư�� ���� �� ���� ����.
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
    /// Ʋ�� Object�� �������� ���� ���� ��, HP�� ���̸�, ��� HP�� ���̸� ���� ����
    /// </summary>
    public void Errorr()
    {
        StopAllCoroutines();
        if(CurHp == 1)
        {
            Destroy(transform.parent.gameObject);
            print("���߿� ���� �߰���");
        }
        else
        {
            HPS[--CurHp].sprite = HpOff;
            Attacked.color = new Color(1, 0, 0, 0.4f + (6 - CurHp) * 0.1f);
            StartCoroutine(ABC());
        }
    }

    /// <summary>
    /// HP�� ���� �� ��, �Ͻ������� ȭ���� �������� ������ ������.
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
