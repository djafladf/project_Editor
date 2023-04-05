using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public List<GameObject> Obj;
    public GameObject NoneObj;
    public Transform parent;
    [Range(0, 3)]
    public float MakingInterv;
    public string[] shape;
    public string[] cl;
    public bool working;
    Color[] colors = new Color[] {Color.red,Color.blue,Color.green };

    int i = 0;

    private void Start()
    {
        StartCoroutine(MakeObj());
    }
    IEnumerator MakeObj()       //조합기 테스트용
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
    }
    /*IEnumerator MakeObj()
    {
        while(true)
        {
            yield return new WaitForSeconds(MakingInterv);
            if (!working) continue;
            int x = Random.Range(0, 3);
            int y = Random.Range(0, 3);
            GameObject cnt = Instantiate(Obj[y],parent);
            cnt.GetComponent<SpriteRenderer>().color = colors[x];
            cnt.transform.position = new Vector3(transform.position.x + 50, transform.position.y, transform.position.z);
            cnt.GetComponent<Convey_Object>().color = cl[x];
            cnt.GetComponent<Convey_Object>().shape = shape[y];
            if (++i == 100) break;
        }
        yield break;
    }*/
}
