using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class VerificationPanelPart : MonoBehaviour
{
    private ADFGVX adfgvx;

    private SpriteRenderer panelBackgroundSprite_D;
    private SpriteRenderer panelGuideSprite;
    private TextMeshPro title;
    private TextMeshPro result;
    private TextMeshPro percentageInfo;
    private TextMeshPro percentage;
    private Gauge gauge;
    private Log log;
    private Button_ADFGVX_QuitVerificationPanel quit;

    private void Awake()
    {
        adfgvx = GameObject.Find("GameManager").GetComponent<ADFGVX>();

        panelBackgroundSprite_D = transform.GetChild(2).GetComponent<SpriteRenderer>();
        panelGuideSprite = transform.GetChild(3).GetComponent<SpriteRenderer>();
        title = transform.GetChild(4).GetComponent<TextMeshPro>();
        result = transform.GetChild(5).GetComponent<TextMeshPro>();
        percentageInfo = transform.GetChild(6).GetComponent<TextMeshPro>();
        percentage = transform.GetChild(7).GetComponent<TextMeshPro>();
        gauge = transform.GetChild(8).GetComponent<Gauge>();
        log = transform.GetChild(9).GetComponent<Log>();
        quit = transform.GetChild(10).GetComponent<Button_ADFGVX_QuitVerificationPanel>();

        UnvisiblePart();
    }

    public void SetLayer(int layer)//하위 요소의 입력 제어
    {
        this.gameObject.layer = layer;
        quit.gameObject.layer = layer;
    }

    public void VisiblePart()//파트 가시
    {
        this.transform.localPosition = new Vector3(-63.6f, -45.7f, 0);
    }

    public void UnvisiblePart()//파트 비가시
    {
        this.transform.localPosition = new Vector3(330, -45.7f, 0);
    }

    public void StartDecodeVerification()//복호화 검증 개시
    {
        StartCoroutine(StartDecodeVerification_IE());
    }

    private IEnumerator StartDecodeVerification_IE()//StartDecodeVerification_IEnumerator
    {
        //에러 발생
        if (adfgvx.encodeDataLoadPart.GetEncodeData().text == "암호화 데이터를 로드하여 시작…")
        {
            adfgvx.InformError("복호화 데이터 무결성 검증 불가 : 암호화 데이터 공백");
            yield break;
        }
        else if(adfgvx.afterDecodingPart.GetInputField_Data().GetInputString() == "")
        {
            adfgvx.InformError("복호화 데이터 무결성 검증 불가 : 복호화 데이터 공백");
            yield break;
        }
        else if(adfgvx.transpositionpart.GetInputField_keyword().GetInputString() == "")
        {
            adfgvx.InformError("복호화 데이터 무결성 검증 불가 : 복호화 전치 키 공백");
            yield break;
        }

        //파트 가시
        VisiblePart();

        adfgvx.InformUpdate("복호화 무결성 검증 작업 개시…");

        //제목 연출
        adfgvx.GetSTRConverter().ConvertTMPFontSize(0f, 117.5f, title);
        adfgvx.GetSTRConverter().ConvertTMPColor(0f, Color.white, title);
        adfgvx.GetSTRConverter().PrintTMPByDuration(0f, "복호화 무결성 검증 진행 중…", title);



        panelBackgroundSprite_D.size = new Vector2(231.8f, 46.3f);
        panelGuideSprite.size = new Vector2(57.9f, 11.7f);
        gauge.VisibleGaugeImediately();



        adfgvx.GetSTRConverter().ConvertTMPColor(0f, Color.white, percentage);
        adfgvx.GetSTRConverter().FillPercentage(3f, percentage);
        
        adfgvx.GetSTRConverter().ConvertTMPColor(0f, Color.white, percentageInfo);
        adfgvx.GetSTRConverter().ConvertTMPColor(0f, Color.clear, result);
        
        
        log.SetColorText(Color.white);
        gauge.FillGaugeBar(3.0f, new Color(0.13f, 0.67f, 0.28f, 1));

        //걸린 시간
        float totalElaspedTime = adfgvx.GetTotalElapsedTime();

        //입력 차단
        adfgvx.SetPartLayerWaitForSec(0f, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2);

        //로그 파일 경로
        string filePath = adfgvx.ReturnDecodeScore() ? "DecodeSuccess" : "DecodeFail";
        log.FillLoadingLog(3.0f, filePath);

        //오디오 재생
        adfgvx.PlayAudioSource(ADFGVX.Audio.DataProcessing);

        //대기
        yield return new WaitForSeconds(4f);
        
        //제목 연출
        adfgvx.GetSTRConverter().ConvertTMPFontSize(1f, 173.5f, title);
        adfgvx.GetSTRConverter().ConvertTMPColor(1f, Color.clear, percentageInfo);
        adfgvx.GetSTRConverter().ConvertTMPColor(1f, Color.clear, percentage);

        gauge.UnvisibleGauge(1f);
        log.HideTextOnly(1f);
        
        //결과 정보
        string info;
        string keyword;
        string time;
        if (adfgvx.ReturnDecodeScore())
        {
            adfgvx.GetSTRConverter().PrintTMPByDuration(0f, "데이터 복호화 작업 성공", title);
            adfgvx.GetSTRConverter().ConvertTMPColor(3f, new Color(0.1f, 0.35f, 0.85f, 1f), title);

            info = "보안 등급 : " + adfgvx.encodeDataLoadPart.GetSecurityLevel().text + " '" + adfgvx.encodeDataLoadPart.GetInputField_FilePath().GetInputString() + "'을\n";
            keyword = "전치 키 : " + EditStirng.CollectEnglishUpperAlphabet(adfgvx.transpositionpart.GetInputField_keyword().GetInputString()) + "로 복호화에 성공했습니다\n";
        }
        else
        {
            adfgvx.GetSTRConverter().PrintTMPByDuration(0f, "데이터 복호화 작업 실패", title);
            adfgvx.GetSTRConverter().ConvertTMPColor(3f, new Color(0.76f, 0.28f, 0.28f, 1f), title);

            info = "보안 등급 : " + adfgvx.encodeDataLoadPart.GetSecurityLevel().text + " '" + adfgvx.encodeDataLoadPart.GetInputField_FilePath().GetInputString() + "'을\n";
            keyword = "전치 키 : " + EditStirng.CollectEnglishUpperAlphabet(adfgvx.transpositionpart.GetInputField_keyword().GetInputString()) + "로 복호화에 실패했습니다\n";
        }
        time = "총 작업 시간 : " + Mathf.FloorToInt(totalElaspedTime / 60).ToString("D2") + ":" + Mathf.FloorToInt(totalElaspedTime % 60).ToString("D2");

        adfgvx.GetSTRConverter().ConvertSpriteRendererSize(1f, new Vector2(231.8f, 57.6f), panelBackgroundSprite_D);
        adfgvx.GetSTRConverter().ConvertSpriteRendererSize(1f, new Vector2(57.9f, 14.6f), panelGuideSprite);

        //결과 안내
        yield return new WaitForSeconds(1.5f);

        adfgvx.GetSTRConverter().ConvertTMPColor(0f, Color.white, result);
        adfgvx.GetSTRConverter().PrintTMPByDuration(3f, info + keyword + time, result);

        //오디오 재생
        adfgvx.SoundFlow(30, 3f);

        //종료
        yield return new WaitForSeconds(4f);
        adfgvx.InformUpdate(adfgvx.ReturnDecodeScore() ? "복호화 데이터 무결성 검증에 성공했습니다" : "복호화 데이터 무결성 검증에 실패했습니다");

        //튜토리얼 관련 코드
        if (adfgvx.chat_ADFGVX.GetCurrentTutorialPhase() == 9 && adfgvx.CurrentMode == ADFGVX.mode.Decoding)
        {
            Debug.Log(adfgvx.ReturnDecodeScore());
            if (adfgvx.ReturnDecodeScore())
                adfgvx.chat_ADFGVX.MoveToNextTutorialPhase(0f);
            else
                adfgvx.chat_ADFGVX.DisplayTutorialDialog(166, 0f);
        }
        else        
        {
           adfgvx.SetPartLayerWaitForSec(0f, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0 ,2);
        }
    }

    public void StartEncodeVerifiaction()//암호화 검증 개시
    {
        StartCoroutine(StartEncodeVerification_IE());
    }

    private IEnumerator StartEncodeVerification_IE()//StartEncodeVerifiaction_IEnumerator
    {
        if(adfgvx.encodeDataSavePart.GetInputField_Data().GetInputString() == "")
        {
            adfgvx.InformError("암호화 데이터 무결성 검증 불가 : 저장 파일 내용 공백");
            yield break;
        }
        if(adfgvx.encodeDataSavePart.GetInputField_Title().GetInputString() == "")
        {
            adfgvx.InformError("암호화 데이터 무결성 검증 불가 : 저장 파일 제목 공백");
            yield break;
        }
        if(adfgvx.transpositionpart.GetInputField_keyword().GetInputString() == "")
        {
            adfgvx.InformError("암호화 데이터 무결성 검증 불가 : 암호화 전치 키 공백");
            yield break;
        }

        //파트 가시
        VisiblePart();

        adfgvx.InformUpdate("암호화 무결성 검증 작업 개시…");

        //연출
        adfgvx.GetSTRConverter().ConvertTMPFontSize(0f, 117.5f, title);
        adfgvx.GetSTRConverter().ConvertTMPColor(0f, Color.white, title);
        adfgvx.GetSTRConverter().PrintTMPByDuration(0f, "암호화 무결성 검증 진행 중…", title);

        panelBackgroundSprite_D.size = new Vector2(231.8f, 46.3f);
        panelGuideSprite.size = new Vector2(57.9f, 11.7f);
        gauge.VisibleGaugeImediately();


        adfgvx.GetSTRConverter().ConvertTMPColor(0f, Color.white, percentage);
        adfgvx.GetSTRConverter().FillPercentage(3f, percentage);
        
        adfgvx.GetSTRConverter().ConvertTMPColor(0f, Color.white, percentageInfo);
        adfgvx.GetSTRConverter().ConvertTMPColor(0f, Color.clear, result);
        
        log.SetColorText(Color.white);
        gauge.FillGaugeBar(3f, new Color(0.13f, 0.67f, 0.28f, 1f));

        //걸린 시간
        float totalElaspedTime = adfgvx.GetTotalElapsedTime();

        //입력 차단
        adfgvx.SetPartLayerWaitForSec(0f, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2);

        //로그 파일 경로
        string filePath = adfgvx.ReturnEncodeScore() ? "EncodeSuccess" : "EncodeFail";
        log.FillLoadingLog(3f, filePath);

        //오디오 재생
        adfgvx.PlayAudioSource(ADFGVX.Audio.DataProcessing);

        //대기
        yield return new WaitForSeconds(4f);
        
        //제목 연출
        adfgvx.GetSTRConverter().ConvertTMPFontSize(1f, 173.5f, title);
        adfgvx.GetSTRConverter().ConvertTMPColor(1f, Color.clear, percentageInfo);
        adfgvx.GetSTRConverter().ConvertTMPColor(1f, Color.clear, percentage);


        gauge.UnvisibleGauge(1f);
        log.HideTextOnly(1f);

        //결과 정보
        string info;
        string keyword;
        string time;
        if (adfgvx.ReturnEncodeScore())
        {
            adfgvx.GetSTRConverter().PrintTMPByDuration(0f, "데이터 암호화 작업 성공", title);
            adfgvx.GetSTRConverter().ConvertTMPColor(3f, new Color(0.1f, 0.35f, 0.85f, 1f), title);

            info = "보안 등급 : " + adfgvx.encodeDataSavePart.GetSecurityLevel() + " '" + adfgvx.encodeDataSavePart.GetInputField_Title().GetInputString() + "'을\n";
            keyword = "전치 키 : " + EditStirng.CollectEnglishUpperAlphabet(adfgvx.transpositionpart.GetInputField_keyword().GetInputString()) + "로 암호화에 성공했습니다\n";
        }
        else
        {
            adfgvx.GetSTRConverter().PrintTMPByDuration(0f, "데이터 암호화 작업 실패", title);
            adfgvx.GetSTRConverter().ConvertTMPColor(3f, new Color(0.76f, 0.28f, 0.28f, 1f), title);

            info = "보안 등급 : " + adfgvx.encodeDataSavePart.GetSecurityLevel() + " '" + adfgvx.encodeDataSavePart.GetInputField_Title().GetInputString() + "'을\n";
            keyword = "전치 키 : " + EditStirng.CollectEnglishUpperAlphabet(adfgvx.transpositionpart.GetInputField_keyword().GetInputString()) + "로 암호화에 실패했습니다\n";
        }

        time = "총 작업 시간 : " + Mathf.FloorToInt(totalElaspedTime / 60).ToString("D2") + ":" + Mathf.FloorToInt(totalElaspedTime % 60).ToString("D2");
        
        adfgvx.GetSTRConverter().ConvertSpriteRendererSize(1f, new Vector2(231.8f, 57.6f), panelBackgroundSprite_D);
        adfgvx.GetSTRConverter().ConvertSpriteRendererSize(1f, new Vector2(57.9f, 14.6f), panelGuideSprite);

        //결과 안내
        yield return new WaitForSeconds(1.5f);

        adfgvx.GetSTRConverter().ConvertTMPColor(0f, Color.white, result);
        adfgvx.GetSTRConverter().PrintTMPByDuration(3f, info + keyword + time, result);

        //오디오 재생
        adfgvx.SoundFlow(30, 3f);

        //종료
        yield return new WaitForSeconds(4f);
        adfgvx.InformUpdate(adfgvx.ReturnDecodeScore() ? "암호화 데이터 무결성 검증에 성공했습니다" : "암호화 데이터 무결성 검증에 실패했습니다");

        //튜토리얼 관련 코드
        if(adfgvx.chat_ADFGVX.GetCurrentTutorialPhase() == 3 && adfgvx.CurrentMode == ADFGVX.mode.Encoding)
        {
            if (adfgvx.ReturnEncodeScore())
                adfgvx.chat_ADFGVX.MoveToNextTutorialPhase(0f);
            else
                adfgvx.chat_ADFGVX.DisplayTutorialDialog(67, 0f);
        }
        else
        {
            adfgvx.SetPartLayerWaitForSec(0f, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 2);
        }
    }
}
