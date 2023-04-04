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
                Debug.Log("!");
                StopAllCoroutines();
                Moving = true;
                StartCoroutine(MovingObj(collision.transform.parent));
            }
        }
    }

    IEnumerator MovingObj(Transform Belt)
    {
        Transform a1 = Belt.GetChild(0);
        for (; Vector3.Magnitude(transform.position - a1.position) > 0.5f;)    // 위치 보정(항상 가운대로 움직임)
        {
            transform.Translate((a1.position - transform.position).normalized);
            yield return new WaitForSeconds(0.01f);
        }
        transform.position = a1.position;
        Vector3 a = (Belt.GetChild(1).transform.position - Belt.GetChild(0).transform.position).normalized;
        if (a.x != 1 && a.y != 1)    // 꺽임 구현
        {
            
            double ang = 0;
            for (; Moving;)
            {
                double cnt = Math.PI / 180 * ang;
                transform.Translate(new Vector3((float)Math.Cos(cnt),(float)Math.Sin(cnt),0));
                ang += 1.35f;
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            for (; Moving;)
            {
                transform.Translate(a);
                yield return new WaitForSeconds(0.01f);
            }
        }
        yield break;
    }
}
