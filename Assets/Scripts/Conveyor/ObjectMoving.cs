using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMoving : MonoBehaviour
{
    bool Moving = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Belt")
        {
            if (collision.gameObject.name == "Start")
            {
                StopAllCoroutines();
                Moving = true;
                StartCoroutine(OnBelt(collision.transform.parent));
            }
        }
        else if(collision.gameObject.name == "Last Stop")
        {
            Destroy(gameObject);
        }
    }

    IEnumerator OnBelt(Transform Belt)
    {
        Installation s = Belt.GetComponent<Installation>();
        Transform _St = Belt.GetChild(0);
        Transform _Ed = Belt.GetChild(1);
        // 위치 보정(향상성 유지)
        for (; Vector3.Magnitude(transform.position - _St.position) > 0.5f;)    
        {
            transform.Translate((_St.position - transform.position).normalized);
            yield return new WaitForSeconds(0.01f);
        }
        transform.position = _St.position;
        //
        
        Vector3 a = (Belt.GetChild(1).transform.position - Belt.GetChild(0).transform.position).normalized;

        if (a.x != 1 && a.y != 1)    // 꺽임 구현
        {
            double ang = 0;
            for (;Vector3.Magnitude(transform.position - _Ed.position) > 0.5f;)
            {
                if (s.OnWork)
                {
                    double cnt = Math.PI / 180 * ang;
                    transform.Translate(new Vector3((float)Math.Cos(cnt), (float)Math.Sin(cnt), 0));
                    ang += 1.35f;
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
        else     // 일반 움직임
        {
            for (;Vector3.Magnitude(transform.position - _Ed.position) > 0.5f;)
            {
                if (s.OnWork) transform.Translate(a);
                yield return new WaitForSeconds(0.01f);
            }
        }
        yield break;
    }
}
