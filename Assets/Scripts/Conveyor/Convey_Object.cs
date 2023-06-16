using System.Collections;
using TMPro;
using UnityEngine;

public class Convey_Object : MonoBehaviour
{
    /**
      * 기본적인 Object의 움직임을 담당한다.
      */
    public string color;
    public string shape;
    public SpriteRenderer spriterenderer;
    public bool MoveAble = true;
    public bool ETC;

    private void Awake()
    {
        if (transform.childCount == 0) spriterenderer = GetComponent<SpriteRenderer>();
        else spriterenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Object가 Infra나 Belt에 충돌한 경우
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Belt"))
        {
            StopAllCoroutines();
            StartCoroutine(OnBelt(collision.transform));
        }
        else if(collision.gameObject.CompareTag("Infra"))
        {
            if(ETC)StopAllCoroutines();
            switch (collision.gameObject.GetComponent<Installation>().type)
            {
                case 0: // 목적지
                    gameObject.SetActive(false);
                    collision.gameObject.GetComponent<Infra_Goal>().JudgeObj(color,shape);
                    break;
                case 1: // 분류기
                    collision.gameObject.GetComponent<Infra_Classifier>().Classify(color, shape, transform);
                    break;
                case 2: // 발전기
                    gameObject.SetActive(false);
                    collision.gameObject.GetComponent<Infra_Power>().AddFuel(color, shape);
                    break;
                case 3: // 조합기
                    collision.gameObject.GetComponent<Infra_Combine>().Combine(color, shape,transform);
                    break;
                case 4: // 추출기
                    collision.gameObject.GetComponent<Infra_Extractor>().Extract(color, shape, transform);
                    break;
            }

        }
    }

    public void ChangeInf(string _Color,string _Shape,Color _ColorT)
    {
        color = _Color; shape = _Shape; spriterenderer.color = _ColorT;
    }


    /// <summary>
    /// Belt상에서 Object의 움직임을 정의
    /// 다른 Object들과의 상호작용을 위해 무조건 Object는 Belt의 중앙 부분으로만 이동한다.
    /// 또한 Belt에 전원이 On 되어있을 때만 움직이며, Off되어 있는 상황이라면 0.5초마다 Belt의 전원이 On 되어 있는지 검사한다.
    /// </summary>
    /// <param name="Belt">현재 Object가 속해있는 Belt</param>
    IEnumerator OnBelt(Transform Belt)
    {
        Installation s = Belt.GetComponent<Installation>();
        Transform _St = Belt;
        Transform _Ed = Belt.GetChild(1);
        ETC = false;

        var wfs1 = new WaitForSeconds(0.5f);
        var wfs2 = new WaitForSeconds(0.01f);
        // 중앙까지 이동
        for (; Vector3.Magnitude(transform.position - _St.position) >= 0.5f;)    
        {
            if (s.OnWork) transform.Translate((_St.position - transform.position).normalized);
            else yield return wfs1;
            yield return wfs2;
        }
        transform.position = _St.position;
        ETC = true;

        Vector3 a = (_Ed.position - _St.transform.position).normalized;
        // 끝으로 이동
        for (;Vector3.Magnitude(transform.position - _Ed.position) >= 0.5f;)
        {
            if (s.OnWork) transform.Translate(a);
            else yield return wfs1;
            yield return wfs2;
        }
        yield break;
    }

    private void OnDisable()
    {
        MoveAble = true;
        ETC = false;
    }
}
