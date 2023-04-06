using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Convey_Object : MonoBehaviour
{
    public string color;
    public string shape;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Belt")
        {
            StopAllCoroutines();
            StartCoroutine(OnBelt(collision.transform));
        }
        else if(collision.gameObject.tag == "Infra")
        {
            StopAllCoroutines();
            switch (collision.gameObject.GetComponent<Installation>().type)
            {
                case 0: // 목적지
                    Destroy(gameObject); break;
                case 1: // 분류기
                    collision.gameObject.GetComponent<Infra_Classifier>().Classify(gameObject);
                    break;
                case 2: // 분쇄기
                    Destroy(gameObject); break;
                case 3: // 조합기
                    collision.gameObject.GetComponent<Infra_Combine>().Combine(gameObject);
                    break;
                case 4: // 추출기
                    collision.gameObject.GetComponent<Infra_Extractor>().Extract(gameObject);
                    break;
            }
        }
    }

    IEnumerator OnBelt(Transform Belt)
    {
        Installation s = Belt.GetComponent<Installation>();
        Transform _St = Belt;
        Transform _Ed = Belt.GetChild(1);
        // 중앙까지 이동
        for (; Vector3.Magnitude(transform.position - _St.position) > 0.5f;)    
        {
            transform.Translate((_St.position - transform.position).normalized);
            yield return new WaitForSeconds(0.01f);
        }
        transform.position = _St.position;
        
        Vector3 a = (_Ed.position - _St.transform.position).normalized;
        // 끝으로 이동
        for (;Vector3.Magnitude(transform.position - _Ed.position) > 0.5f;)
        {
            if (s.OnWork) transform.Translate(a);
            yield return new WaitForSeconds(0.01f);
        }
        yield break;
    }
}
