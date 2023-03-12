using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeforeEncodingPart : MonoBehaviour
{
    private ADFGVX adfgvx;

    private InputField_ADFGVX data;
    private TextField block;
    private TextField primeFactor;

    private void Start()
    {
        adfgvx = GameObject.Find("GameManager").GetComponent<ADFGVX>();

        data = transform.Find("Data").GetComponent<InputField_ADFGVX>();
        block = transform.Find("Block").GetComponent<TextField>();
        primeFactor = transform.Find("PrimeFactor").GetComponent<TextField>();
    }

    public void SetLayer(int layer)//이 게임오브젝트 하위 요소의 레이어 제어
    {
        transform.Find("Data").gameObject.layer = layer;
    }

    public void VisiblePart()//파트 가시 모드
    {
        this.transform.localPosition = new Vector3(96.3f, -19f, 0);
    }

    public void UnvisiblePart()//파트 비가시 모드
    {
        this.transform.localPosition = new Vector3(96.3f, 200, 0);
    }

    public InputField_ADFGVX GetInputField_Data()//데이터 인풋 필드 반환
    {
        return data;
    }

    public void AddInputField_Data(string value)//data에 value에 대응하는 값을 입력
    {
        //튜토리얼 관련 코드
        if(adfgvx.GetCurrentTutorialPhase() == 1 && adfgvx.CurrentMode == ADFGVX.mode.Encoding)
        {
            if (adfgvx.biliteralsubstitutionpart.GetCurrentArrayNum() != 0)
            {
                adfgvx.DisplayTutorialDialog(44, 0f);
                return;
            }
            else
                adfgvx.MoveToNextTutorialPhase(2.0f);
        }

        char[] array = new char[6] { 'A', 'D', 'F', 'G', 'V', 'X' };
        int idx_row = 6;
        int idx_line = 6;
        for (int idx = 0; idx < 36; idx++)
        {
            if (adfgvx.biliteralsubstitutionpart.elementButtons[idx].GetButtonText() == value)
            {
                idx_row = idx / 6;
                idx_line = idx % 6;
            }
        }

        if(idx_row == 6 && idx_line == 6)//해당하는 엘리먼트를 찾지 못했습니다
        {
            adfgvx.InformError("ADFGVX배열에 대응하지 않는 입력 : 입력 불가");
            return;
        }

        data.AddInputField(array[idx_row].ToString() + array[idx_line].ToString() + " ");
        UpdateRecommendKeyword();
    }

    public void DeleteInputField_Data()//keyboard 입력의 의해 data에 블록을 삭제
    {        
        data.DeleteInputField(3);
        UpdateRecommendKeyword();
    }

    private void UpdateRecommendKeyword()//추천 키워드 업데이트
    {
        string number = ("암호화 대상 총 글자 수 : " + data.GetMarkText().Length / 3 * 2).ToString();

        string prime = "추천 암호 키 글자 수 : ";

        if (data.GetMarkText().Length / 3 * 2 == 0)
            prime += "NULL";
        else
        {
            prime += "1";
            int max = ((data.GetMarkText().Length / 3 * 2) < 9) ? (data.GetMarkText().Length / 3 * 2) : 9;
            for (int i = 2; i <= max; i++)
            {
                if ((data.GetMarkText().Length / 3 * 2) % i == 0)
                    prime += ", " + i.ToString();
            }
        }

        block.SetText(number);
        primeFactor.SetText(prime);
    }
}
