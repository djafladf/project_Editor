using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    /**
    * 스크린 관리 스크립트
    * - 스크린 전원 키고 끄기 (애니메이션)
    * - 업무 착수 및 관리
    */
    
    public GameObject bootPanel;
    public AnimationController bootAnimation;
    public float logoOnSeconds;
    public float cliOnSeconds;
    private GameObject bootLogo;
    private GameObject bootCLI;
    private bool isBoot;


    private void Start() {
        bootLogo = bootPanel.transform.GetChild(1).gameObject;
        bootCLI = bootPanel.transform.GetChild(0).gameObject;

        {
            bootCLI.GetComponent<Text>().fontSize = 12;
            bootCLI.GetComponent<Text>().color = Color.green;
        }
        isBoot = false;
        bootPanel.SetActive(true);
        bootLogo.SetActive(false);
        bootCLI.SetActive(false);
    }

    public void PowerOnScreen()
    {
        if (isBoot == false)
        {
            StartCoroutine("BootScreen");
        }
    }

    // TODO: 텍스트 애니메이션 끝나는 시간 맞춰서 부팅 종료되도록 수정
    IEnumerator BootScreen()
    {
        bootLogo.SetActive(true);
        yield return new WaitForSeconds(logoOnSeconds);
        bootLogo.SetActive(false);
        bootCLI.SetActive(true);
        bootAnimation.Play();

        // yield return bootAnimation.isFinished == true
        yield return new WaitUntil(() => bootAnimation.isFinished == true);
        yield return new WaitForSeconds(0.5f);
        {
            bootCLI.GetComponent<Text>().fontSize = bootCLI.GetComponent<Text>().fontSize * 2;
            bootCLI.GetComponent<Text>().color = Color.red;
            bootCLI.GetComponent<Text>().text = "위대한 아슬라니아를 위하여!!";
            yield return new WaitForSeconds(0.3f);
            bootCLI.GetComponent<Text>().fontSize = bootCLI.GetComponent<Text>().fontSize * 4;
            bootCLI.GetComponent<Text>().color = Color.red;
            yield return new WaitForSeconds(0.3f);
        }

        bootPanel.SetActive(false);
        isBoot = true;
    }
}
