using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MazeTimer : MonoBehaviour
{
    public GameObject Warning;
    public GameObject Player;

    TMP_Text Timer;
    RectTransform Rect;
    public double NowTime = 20;
    bool warning = false;

    private void Awake()
    {
        Timer = GetComponent<TMP_Text>();
        Rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (NowTime > 0)
        {
            NowTime -= Time.deltaTime;
            Timer.text = string.Format("{0:0.00}", NowTime);
            if (NowTime <= 10 && !warning)
            {
                Warning.SetActive(true);
                StartCoroutine(EAE());
                Rect.anchoredPosition = new Vector3(0, 0, 0);
                warning = true;
            }
        }
    }

    IEnumerator EAE()
    {
        WaitForSeconds WF = new WaitForSeconds(1);
        Image s = Warning.GetComponent<Image>();
        Color ss = new Color(0, 0, 0, 0.1f);

        while (NowTime > 0)
        {
            yield return WF;
            Rect.localScale *= 1.1f;
            s.color += ss;
        }
        Timer.text = "Game\nOver";
        Destroy(Player);
    }

    private void OnDisable ()
    {
        Warning.SetActive(false);
    }
}
