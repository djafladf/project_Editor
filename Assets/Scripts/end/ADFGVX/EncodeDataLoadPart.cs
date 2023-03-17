using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class EncodeDataLoadPart : MonoBehaviour
{
    private ADFGVX adfgvx;

    private InputField_ADFGVX filePath;

    private TextField securityLevel;
    private TextField title;
    private TextField data;
    private TextField sender;
    private TextField senderUI;
    private TextField date;
    private TextField dateUI;

    private string DecodedChiper = "";  //복호화 정답

    private void Start()
    {
        adfgvx = GameObject.Find("GameManager").GetComponent<ADFGVX>();

        filePath = transform.Find("FilePath").GetComponent<InputField_ADFGVX>();

        securityLevel = transform.Find("SecurityLevel").GetComponent<TextField>();
        title = transform.Find("Title").GetComponent<TextField>();
        data = transform.Find("Data").GetComponent<TextField>();
        sender = transform.Find("Sender").GetComponent<TextField>();
        senderUI = transform.Find("SenderUI").GetComponent<TextField>();
        date = transform.Find("Date").GetComponent<TextField>();
        dateUI = transform.Find("DateUI").GetComponent<TextField>();
    }

    public void SetLayer(int layer)//하위 요소의 입력 제어
    {
        transform.Find("FilePath").gameObject.layer = layer;
        transform.Find("Load").gameObject.layer = layer;
    }

    public InputField_ADFGVX GetInputField_filePath()//파일 경로 입력창 반환
    {
        return filePath;
    }

    public TextField GetTextField_SecurityLevel()//보안 등급 텍스트창 반환
    {
        return securityLevel;
    }

    public void LoadEncodeData()//암호화된 데이터를 로드
    {
        if (date.GetIsNowFlowText() || sender.GetIsNowFlowText())//출력 중이 아니었다면
        {
            adfgvx.InformError("현재 암호화 데이터 로드 중 : 종료 시까지 대기 요망");
            return;
        }

        //깜박임 종료
        filePath.StopFlashInputField();

        string FilePath = "";
        FileInfo TxtFile = null;
        string SecurityLevel = "";
        string Title = "";
        string Data = "";
        string SendingDateUI = "";
        string SendingDate = "";
        string SenderUI = "";
        string Sender = "";
        string ReceptionDateUI = "";
        string ReceptionDate = "";
        string ReceiverUI = "";
        string Receiver = "";

        FilePath = "Assets/Resources/Chiper/" + filePath.GetInputString() + ".txt";
        TxtFile = new FileInfo(FilePath);

        if (!TxtFile.Exists)
        {
            //튜토리얼 관련 코드
            if (adfgvx.GetCurrentTutorialPhase() == 0 && adfgvx.CurrentMode == ADFGVX.mode.Decoding)
            {
                adfgvx.DisplayTutorialDialog(11, 0f);
            }

            adfgvx.InformError("'" + filePath.GetInputString() + "' " + "로드 실패 : 유효하지 않은 파일 경로");
            filePath.DisplayErrorInInputField("파일 경로 오류!");
            return;
        }

        //튜토리얼 관련 코드
        if (adfgvx.GetCurrentTutorialPhase() == 0 && adfgvx.CurrentMode == ADFGVX.mode.Decoding)
        {
            if (filePath.GetInputString() == "SI-XI-I")
                adfgvx.MoveToNextTutorialPhase(3f);
            else
                adfgvx.DisplayTutorialDialog(14, 3.1f);
        }
            
        StreamReader Reader = new StreamReader(FilePath, System.Text.Encoding.UTF8);
        SecurityLevel = Reader.ReadLine();
        Title = Reader.ReadLine();
        Data = Reader.ReadLine();
        SendingDateUI = Reader.ReadLine();
        SendingDate = Reader.ReadLine();
        SenderUI = Reader.ReadLine();
        Sender = Reader.ReadLine();
        ReceptionDateUI = Reader.ReadLine();
        ReceptionDate = Reader.ReadLine();
        ReceiverUI = Reader.ReadLine();
        Receiver = Reader.ReadLine();
        DecodedChiper = Reader.ReadLine();
        Reader.Close();

        //입력 차단
        adfgvx.SetPartLayerWaitForSec(0f, 2, 2, 2, 2, 2, 2, 2, 2, 2);

        //입력 회복
        adfgvx.SetPartLayerWaitForSec(3f, 0, 0, 0, 0, 0, 0, 0, 0, 0);

        adfgvx.InformUpdate("'" + filePath.GetInputString() + "' " + "로드 성공 : 총 작업 시간 1ms 이하");

        //업데이트 전 비움
        securityLevel.SetText("");
        title.SetText("");
        data.SetText("");
        dateUI.SetText("");
        date.SetText("");
        senderUI.SetText("");
        sender.SetText("");

        //현재 파트 업데이트
        securityLevel.SetText(SecurityLevel);
        title.FlowText(Title, 3.0f);
        data.FlowText(Data, 3.0f);
        dateUI.SetText(SendingDateUI);
        date.FlowText(SendingDate, 3.0f);
        senderUI.SetText(SenderUI);
        sender.FlowText(Sender, 3.0f);

        //복호화 후 파트 업데이트
        adfgvx.afterDecodingPart.GetSecurityLevel().SetText(SecurityLevel);
        adfgvx.afterDecodingPart.GetTitle().FlowText(Title, 3.0f);
        adfgvx.afterDecodingPart.GetDateUI().SetText(ReceptionDateUI);
        adfgvx.afterDecodingPart.GetDate().FlowText(ReceptionDate, 3.0f);
        adfgvx.afterDecodingPart.GetSenderUI().SetText(ReceiverUI);
        adfgvx.afterDecodingPart.GetSender().FlowText(Receiver, 3.0f);

        //오디오 재생
        adfgvx.SoundFlow(30, 3f);

        //시간 측정 개시
        adfgvx.StartStopWatch();
    }

    public string GetTextField_Data()//데이터 표시창 반환
    {
        return data.GetText();
    }

    public string GetDecodedChiper()//복호화 정답 반환
    {
        return DecodedChiper;
    }
}
