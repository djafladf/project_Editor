using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class ADFGVX : MonoBehaviour
{
    public enum mode//ADFGVX모드
    { Encoding,Decoding};
    public mode currentmode;//현재 ADFGVX모드

    [Header("뒤로가기 버튼")]
    public Button_QuitADFGVX quitButton;
    [Header("정보 창 파트")]
    public InfoPart infoPart;
    [Header("화살표 파트")]
    public GuideArrowPart guidearrowpart;
    [Header("이중 문자 치환 파트")]
    public BiliteralSubstitutionPart biliteralsubstitutionpart;
    [Header("키 순위 전치 파트")]
    public TranspositionPart transpositionpart;
    [Header("복호화 후 파트")]
    public AfterDecodingPart afterDecodingPart;
    [Header("암호화 전 파트")]
    public BeforeEncodingPart beforeEncodingPart;
    [Header("암호화 데이터 로드 파트")]
    public EncodeDataLoadPart encodeDataLoadPart;
    [Header("암호화 데이터 저장 파트")]
    public EncodeDataSavePart encodeDataSavePart;
    [Header("확인 창 파트")]
    public VerificationPanelPart verificationpart;

    public enum Audio
    {
        TextFlow, AddChar, DeleteChar, MouseClick, Error, DataProcessing,
    }
    private AudioSource audioSource;
    public AudioClip[] audioClips;

    private float totalElapsedTime;

    private void Start()
    {
        InformUpdate("아츠토츠카 표준 암호 시스템 V7 가동 정상");

        if(currentmode == mode.Decoding)
        {
            afterDecodingPart.VisiblePart();
            encodeDataLoadPart.VisiblePart();
        }
        else if(currentmode == mode.Encoding)
        {
            guidearrowpart.Rotate180();

            beforeEncodingPart.VisiblePart();
            encodeDataSavePart.VisiblePart();
        }

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        StopWatch();
    }

    public void SetPartLayer(int layer_ADFGVX, int layer_BiliteralSubstitution, int layer_Transposition, int layer_AfterDecode, int layer_BeforeEncode, int layer_EncodeDataSave, int layer_EncodeDataLoad, int layer_Verification)//모든 파트 레이어 설정
    {
        quitButton.gameObject.layer = layer_ADFGVX;
        biliteralsubstitutionpart.SetLayer(layer_BiliteralSubstitution);
        transpositionpart.SetLayer(layer_Transposition);
        afterDecodingPart.SetLayer(layer_AfterDecode);
        beforeEncodingPart.SetLayer(layer_BeforeEncode);
        encodeDataSavePart.SetLayer(layer_EncodeDataSave);
        encodeDataLoadPart.SetLayer(layer_EncodeDataLoad);
        verificationpart.SetLayer(layer_Verification);
    }

    public void SetPartLayerWaitForSec(float endTime, int layer_ADFGVX, int layer_BiliteralSubstitution, int layer_Transposition, int layer_AfterDecode, int layer_BeforeEncode, int layer_EncodeDataSave, int layer_EncodeDataLoad, int layer_Verification)//모든 파트 레이어 설정
    {
        StartCoroutine(SetPartLayerWaitForSecIEnumerator(endTime, 0, layer_ADFGVX, layer_BiliteralSubstitution, layer_Transposition, layer_AfterDecode, layer_BeforeEncode, layer_EncodeDataSave, layer_EncodeDataLoad, layer_Verification));
    }

    private IEnumerator SetPartLayerWaitForSecIEnumerator(float endTime, float currentTime, int layer_ADFGVX, int layer_BiliteralSubstitution, int layer_Transposition, int layer_AfterDecode, int layer_BeforeEncode, int layer_EncodeDataSave, int layer_EncodeDataLoad, int layer_Verification)//모든 파트 레이어 설정
    {
        currentTime += endTime / 100;
        if(currentTime>=endTime)
        {
            quitButton.gameObject.layer = layer_ADFGVX;
            biliteralsubstitutionpart.SetLayer(layer_BiliteralSubstitution);
            transpositionpart.SetLayer(layer_Transposition);
            afterDecodingPart.SetLayer(layer_AfterDecode);
            beforeEncodingPart.SetLayer(layer_BeforeEncode);
            encodeDataSavePart.SetLayer(layer_EncodeDataSave);
            encodeDataLoadPart.SetLayer(layer_EncodeDataLoad);
            verificationpart.SetLayer(layer_Verification);
            yield break;
        }

        yield return new WaitForSeconds(endTime / 100);
        StartCoroutine(SetPartLayerWaitForSecIEnumerator(endTime, currentTime, layer_ADFGVX, layer_BiliteralSubstitution, layer_Transposition, layer_AfterDecode, layer_BeforeEncode, layer_EncodeDataSave, layer_EncodeDataLoad, layer_Verification));
    }

    public void StartStopWatch()//스탑워치 재시작
    {
        totalElapsedTime = 0;
    }

    private void StopWatch()//스탑워치
    {
        totalElapsedTime += Time.deltaTime;
    }

    public float GetTotalElapsedTime()//totalElapsedTime을 반환합니다
    {
        return totalElapsedTime;
    }

    public bool ReturnDecodeScore()//복호화 결과를 반환합니다
    {
        
        return EditStirng.CollectEnglishUpperAlphabet(encodeDataLoadPart.GetDecodedChiper()) == EditStirng.CollectEnglishUpperAlphabet(afterDecodingPart.GetInputField_Data().GetInputString());
    }

    public bool ReturnEncodeScore()//암호화 결과를 반환합니다
    {
        string original = EditStirng.CollectEnglishUpperAlphabet(beforeEncodingPart.GetInputField_Data().GetInputString());
        string keyword = EditStirng.CollectEnglishUpperAlphabet(transpositionpart.GetInputField_keyword().GetInputString());
        int[] place = new int[keyword.Length];
        place = transpositionpart.GetPriority();

        string encode = EditStirng.CollectEnglishUpperAlphabet(encodeDataSavePart.GetInputField_Data().GetInputString());

        char[] answer = new char[original.Length];

        for (int i = 0; i < place.Length; i++)
        {
            for (int j = 0; j < original.Length / place.Length; j++)
            {
                answer[(original.Length / place.Length) * (place[i] - 1) + j] += original[place.Length * j + i];
            }
        }

        return answer.ArrayToString() == encode;
    }

    public void InformUpdate(string value)//InfoBox에 업데이트한 사항을 띄운다
    {
        infoPart.UpdateText(value);
    }

    public void InformError(string value)//InfoBox에 에러 발생 사항을 띄운다
    {
        infoPart.UpdateText(value);
        PlayAudioSource(Audio.Error);
    }

    public void PlayAudioSource(Audio value)//선택한 사운드 재생
    {
        audioSource.clip = audioClips[((int)value)];
        audioSource.Play();
    }

    public void soundFlow(int length, float endTime)//사운드 흐름 재생
    {
        StartCoroutine(soundFlowIEnumerator(length, 0, endTime));
    }

    private IEnumerator soundFlowIEnumerator(int length, int idx, float endTime)//사운드 흐름 재생 재귀
    {
        if (idx >= length)
            yield break;
        PlayAudioSource(Audio.TextFlow);
        yield return new WaitForSeconds(endTime / length);
        StartCoroutine(soundFlowIEnumerator(length, idx + 1, endTime));
    }
}
