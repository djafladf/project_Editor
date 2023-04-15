using System.Collections;
using TMPro;
using UnityEngine;

public class Convey_Object : MonoBehaviour
{
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
