using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Text;

public class Convey_Button : MonoBehaviour
{
    public Sprite Normal;
    public Sprite Fast;
    public Convey_Manager CM;

    bool Is_Click = false;
    public Image image;
    float cnt = 1;

    Convey_Button a1;
    Convey_Button a2;

    private void Awake()
    {
        image = GetComponent<Image>();
        MyUi.ButtonInit(GetComponent<EventTrigger>(),OnPointer, OutPointer,Click);
        if(name == "RollBack")
        {
            a1 = transform.parent.GetChild(1).GetComponent<Convey_Button>();
            a2 = transform.parent.GetChild(2).GetComponent<Convey_Button>();
        }
    }

    void OnPointer(PointerEventData Data)
    {
        image.color = image.color - new Color(0.3f, 0.3f, 0.3f, 0);
    }
    void OutPointer(PointerEventData Data)
    {
        image.color = image.color + new Color(0.3f, 0.3f, 0.3f, 0);
    }
    public void Click(PointerEventData Data)
    {
        if(name == "Speed")
        {
            if (CM.IsPlaying == false) { CM.PlayStart(); Is_Click = true; }
            if (Is_Click)
            {
                image.sprite = Normal;
                Time.timeScale = 1;
            }
            else
            {
                image.sprite = Fast;
                Time.timeScale = 2;
            }
        }
        else if(name == "Stop")
        {
            if (Is_Click)
            {
                image.color = image.color + new Color(0.3f, 0.3f, 0.3f, 0);
                Time.timeScale = cnt;
            }
            else
            {
                image.color = image.color - new Color(0.3f, 0.3f, 0.3f, 0);
                cnt = Time.timeScale;
                Time.timeScale = 0;
            }
        }
        else
        {
            if (Time.timeScale == 2) a1.Click(null);
            if (Time.timeScale == 0) a2.Click(null);
            Time.timeScale = 1;
            CM.RollBack();
        }
        Is_Click = !Is_Click;
    }
}
