using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class ChiperPart : MonoBehaviour
{
    private ADFGVX adfgvx;

    private TextMeshPro partTitle;                  //파트 타이틀
    private TextMeshPro chiperUI;
    private TextMeshPro chiperTitle;                //암호문 제목
    private TextMeshPro chiper;                     //암호문 내용
    private TextMeshPro inputField;                 //암호문 검색창
    private TextMeshPro dateUI;                     //날짜 제목
    private TextMeshPro date;                       //날짜
    private TextMeshPro senderUI;                   //작성자 제목
    private TextMeshPro sender;                     //작성자

    [Header("로드 or 저장 버튼")]
    public Button_LoadOrSave button_LoadOrSave;    

    private SpriteRenderer inputFieldColor;         //검색창 배경 스프라이트
    private string inputString;                     //플레이어가 검색창에 입력한 내용
    private bool isReadyForInput;                    //검색 준비가 되어 있는가?
    private bool isCursorOverInputField;            //검색창에 커서가 올라갔는가?
    private bool isOnPrintFlow;                     //흐름 출력 작업 중 여부
    private const int InpuField_MAX = 18;           //검색창 최대 입력

    private bool isFlash;                           //깜박임
    private bool skipOneFlash;                      //true면 깜박임 한번 건너뛴다

    private void Start()
    {
        adfgvx = GameObject.Find("ADFGVX").GetComponent<ADFGVX>();

        partTitle = GetComponentsInChildren<TextMeshPro>()[0];
        chiperUI = GetComponentsInChildren<TextMeshPro>()[1];
        chiperTitle = GetComponentsInChildren<TextMeshPro>()[2];
        chiper = GetComponentsInChildren<TextMeshPro>()[3];
        inputField = GetComponentsInChildren<TextMeshPro>()[4];
        dateUI = GetComponentsInChildren<TextMeshPro>()[5];
        date = GetComponentsInChildren<TextMeshPro>()[6];
        senderUI = GetComponentsInChildren<TextMeshPro>()[7];
        sender = GetComponentsInChildren<TextMeshPro>()[8];

        inputFieldColor = GetComponentsInChildren<SpriteRenderer>()[0];
        inputFieldColor.color = new Color(0, 1, 0, 0);
        inputString = "";
        isFlash = false;

        ClearChiperAll();
        InitializeChiperPartAll();
        StartCoroutine(FlashinputField());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isCursorOverInputField && !isReadyForInput)
            {
                inputFieldColor.color = new Color(0, 1, 0, 0);
                inputField.text = inputString;
                isReadyForInput = true;
                isFlash = true;
            }
            else if (isCursorOverInputField && isReadyForInput)
            {

            }
            else
            {
                if (inputString == "")
                    inputField.text = "클릭하여 입력…";
                isReadyForInput = false;
                isFlash = false;
            }
        }
    }

    private void OnMouseEnter()
    {
        if(!isReadyForInput)
            inputFieldColor.color = new Color(0, 1, 0, 0.15f);
        isCursorOverInputField = true;
    }

    private void OnMouseExit()
    {
        inputFieldColor.color = new Color(0, 1, 0, 0);
        isCursorOverInputField = false;
    }

    public bool GetIsReadyForInput()//입력 준비 상태
    {
        return isReadyForInput;
    }

    public void SetLayer(int layer)//모든 입력 차단
    {
        this.gameObject.layer = layer;
        GameObject.Find("LoadOrSave").layer = layer;
    }

    IEnumerator FlashinputField()//검색창을 깜박이게 만든다
    {
        if (inputString.Length <= InpuField_MAX && isReadyForInput && !skipOneFlash)//입력창 길이를 넘기거나, 입력 중이 아니거나, 스킵 명령이 있다면 건너뛴다
        {
            if (isFlash)
            {
                inputField.text = inputString;
                isFlash = false;
            }
            else
            {
                inputField.text = inputString + "…";
                isFlash = true;
            }
        }
        else if (!isReadyForInput && inputString != "")//입력 중이 아니나 빈칸이 아니라면, 입력 상태를 유지한다
            inputField.text = inputString;

        skipOneFlash = false;//이번 턴에 스킵했으니 다음 번에는 깜박여야 한다
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FlashinputField());
    }

    private void DelayFlashinputField()//깜박임을 0.5초 막는다
    {
        skipOneFlash = true;
    }

    public void AddInputField(string value)//입력창에 한 단어 추가한다
    {
        if (!isReadyForInput)
            return;
        else if (inputString.Length > InpuField_MAX)
        {
            adfgvx.InformError("파일 경로 입력 최대 : 입력 불가");
            return;
        }
        DelayFlashinputField();
        inputString = inputString + value;
        inputField.text = inputString;
    }

    public void DeleteInputField()//입력창에서 한 단어 지운다
    {
        if (!isReadyForInput)
            return;
        else if(inputString.Length < 1)
        {
            adfgvx.InformError("파일 경로 입력 최소 : 삭제 불가");
            return;
        }
        DelayFlashinputField();
        inputString = inputString.Substring(0, inputString.Length - 1);
        inputField.text = inputString;
    }

    public void UpdateChiperTitleAndText()//inputstring에 따라서 암호문을 불러온다
    {
        if (isOnPrintFlow)//아직 전에 명령받은 파일 불러오기 작업이 끝나지 않음
        {
            adfgvx.InformError("파일 불러오기 불가 : 작업 진행 중");
            return;
        }
        else if (adfgvx.currentmode == ADFGVX.mode.Encoding)
        {
            adfgvx.InformError("파일 불러오기 불가: 현재 모드 암호화");
            return;
        }

        //return 키를 눌렀으니 검색창 선택과 깜박임을 비활성화한다
        isReadyForInput = false;
        isFlash = false;

        string FilePath = "";
        FileInfo TxtFile = null;
        string TxtchiperUI = "";
        string TxtchiperTitle = "";
        string Txtchiper = "";
        string TxtdateUI = "";
        string Txtdate = "";
        string TxtsenderUI = "";
        string Txtsender = "";
        string TxtDecodedChiper = "";

        //ArrayNum에 따라서 각기 다른 표의 FilePath가 저장된다
        FilePath = "Assets/Workspace_LeeJungWoo/Prefab/ADFGVX/ChipersTxt/" + inputString + ".txt";                  
        TxtFile = new FileInfo(FilePath);
        
        if (TxtFile.Exists)//Filepath가 유효하다면
        {
            StreamReader Reader = new StreamReader(FilePath, System.Text.Encoding.UTF8);
            TxtchiperUI = Reader.ReadLine();
            TxtchiperTitle = Reader.ReadLine();
            Txtchiper = Reader.ReadLine();
            TxtdateUI = Reader.ReadLine();
            Txtdate = Reader.ReadLine();
            TxtsenderUI = Reader.ReadLine();
            Txtsender = Reader.ReadLine();
            TxtDecodedChiper = Reader.ReadLine();
            Reader.Close();
        }
        else//Filepath가 유효하지 않다면
        {
            adfgvx.InformError("'" + inputString + "' " + "접근 실패 : 유효하지 않은 경로");
            DisplayErrorInInputField("파일 접근 불가!");
            return;
        }

        //모든 파트 입력 차단
        adfgvx.SetPartLayer(2, 2, 2, 2);

        adfgvx.InformUpdate("'" + inputString + "' " + "접근 성공 : 도달 시간 1ms 이하");

        //흐름 출력 시작
        ClearChiperAll();
        chiperUI.text = TxtchiperUI;
        FlowPrint(chiperTitle, TxtchiperTitle, 3.0f);
        FlowPrint(chiper, Txtchiper, 3.0f);
        dateUI.text = TxtdateUI;
        FlowPrint(date, Txtdate, 3.0f);
        senderUI.text = TxtsenderUI;
        FlowPrint(sender, Txtsender, 3.0f);

        //작업 종료 시까지 깜박임, 새로운 작업 차단
        isOnPrintFlow = true;
        Invoke("SetisOnPrintFlowFalse", 3.0f);

        //intermediatechiper에도 흐름 출력 시작
        adfgvx.intermediatepart.ClearIntermediateChiperAll();
        adfgvx.intermediatepart.chiperUI.text = TxtchiperUI;
        FlowPrint(adfgvx.intermediatepart.chiperTitle, TxtchiperTitle, 3.0f);
        adfgvx.intermediatepart.dateUI.text = TxtdateUI;
        FlowPrint(adfgvx.intermediatepart.date, Txtdate, 3.0f);
        adfgvx.intermediatepart.senderUI.text = TxtsenderUI;
        FlowPrint(adfgvx.intermediatepart.sender, Txtsender, 3.0f);

        //adfgvx에 해독된 데이터 저장
        adfgvx.SetDecodedChiper(TxtDecodedChiper);

        //사운드 재생
        adfgvx.soundFlow(30, 0, 3.0f);
    }

    private void DisplayErrorInInputField(string value)//검색창에 에러 메세지를 띄운다
    {
        inputField.text = value;
        inputString = "";
        isReadyForInput = false;
        isFlash = false;
    }

    public void ClearChiperAll()//불러와져 있던 암호문을 비운다
    {
        chiperUI.text = "";
        chiperTitle.text = "";
        chiper.text = "";
        dateUI.text = "";
        date.text = "";
        senderUI.text = "";
        sender.text = "";
    }

    public string GetChiperText()//불라와져 있던 암호문을 반환한다
    {
        return chiper.text;
    }

    public void InitializeChiperPartAll()//ChiperPart의 각종 수치를 초기화한다
    {
        if (adfgvx.currentmode == ADFGVX.mode.Encoding)
        {
            partTitle.text = "암호화 데이터 저장";
            if (chiperUI.text == "")
                chiperUI.text = "[보안 등급]";
            if (chiperTitle.text == "")
                chiperTitle.text = "[파일의 제목]";
            if (chiper.text == "")
                chiper.text = "[파일의 내용]";
            if (dateUI.text == "")
                dateUI.text = "[작성일]";
            if (date.text == "")
                date.text = "…";
            if (senderUI.text == "")
                senderUI.text = "[작성자]";
            if (sender.text == "")
                sender.text = "…";
        }
        else
        {
            partTitle.text = "암호화 데이터 로드";
            if (chiperUI.text == "")
                chiperUI.text = "[보안 등급]";
            if (chiperTitle.text == "")
                chiperTitle.text = "[파일의 제목]";
            if (chiper.text == "")
                chiper.text = "[파일의 내용]";
            if (dateUI.text == "")
                dateUI.text = "[작성일]";
            if (date.text == "")
                date.text = "…";
            if (senderUI.text == "")
                senderUI.text = "[작성자]";
            if (sender.text == "")
                sender.text = "…";
        }
    }

    private void FlowPrint(TextMeshPro target, string value, float endTime)//tartget에 value를 endTime안에 순차적으로 채워 넣는다
    {
        StartCoroutine(FlowPrintIEnumerator(target, value, 0, endTime));
    }

    private IEnumerator FlowPrintIEnumerator(TextMeshPro target, string value, int idx, float endTime)//FlowPrint 재귀
    {
        if (idx >= value.Length)
        {
            adfgvx.SetPartLayer(0, 0, 0, 0);
            if (button_LoadOrSave.GetIsOver())
                button_LoadOrSave.EnableClickSprite();
            else
                button_LoadOrSave.DisableClickSprite();
            yield break;
        }
        target.text += value.Substring(idx,1);
        yield return new WaitForSeconds(endTime/value.Length);
        StartCoroutine(FlowPrintIEnumerator(target,value,idx+1,endTime));
    }

    private void SetisOnPrintFlowFalse()//isOnPrintFlow변수를 거짓으로 한다_Invoke용
    {
        isOnPrintFlow = false;
    }

    public void ReturnEncodingResult()//이번 암호화 결과물을 제출합니다
    {
        if(adfgvx.currentmode == ADFGVX.mode.Decoding)
        {
            adfgvx.InformError("파일 저장하기 불가: 현재 모드 복호화");
            return;
        }
        Debug.Log("암호화 결과물을 제출합시다!");
    }
}
