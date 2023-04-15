using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConveyTest : MonoBehaviour
{
    public Convey_Manager CM;
    public bool working;
    float MakingInterv = 1;
    GameObject cnt;

    int i = 0;
    public void StopWork()
    {
        working = false;
        StopAllCoroutines();
    }
    public void StartWork()
    {
        working = true;
        StartCoroutine(MakeObj());
    }
    /*IEnumerator MakeObj()       //조합기 테스트용(색, 모양 중 하나만 나옴)
    {
        while (true)
        {
            yield return new WaitForSeconds(MakingInterv);
            if (!working) continue;
            int x = Random.Range(0, 2);
            int y = Random.Range(0, 3);
            GameObject cnt;
            if (x == 0)     //색
            {
                cnt = Instantiate(NoneObj,parent);
                cnt.GetComponent<SpriteRenderer>().color = colors[y];
                cnt.GetComponent<Convey_Object>().color = cl[y];
                cnt.transform.position = new Vector3(transform.position.x + 50, transform.position.y, transform.position.z);
            }
            else            //형체
            {
                cnt = Instantiate(Obj[y], parent);
                cnt.GetComponent<Convey_Object>().shape = shape[y];
                cnt.transform.position = new Vector3(transform.position.x + 50, transform.position.y, transform.position.z);
            }
            if (++i == 100) break;
        }
        yield break;
    }*/
    IEnumerator MakeObj()
    {
        var wfs = new WaitForSeconds(MakingInterv);
        string x; string y;
        while (true)
        {
            yield return wfs;
            if (!working) continue;
            x = CM.ColorNames[Random.Range(0, CM.ColorNames.Count)];
            y = CM.ShapeNames[Random.Range(0, CM.ShapeNames.Count)];
            cnt = CM.COM.ReturnObject(y, x);
            cnt.transform.position = new Vector3(transform.position.x + 50, transform.position.y, transform.position.z);
            if (++i == 1000) break;
        }
        yield break;
    }
}
