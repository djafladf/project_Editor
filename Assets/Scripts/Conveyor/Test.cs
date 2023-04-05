using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public GameObject Obj;
    public Transform parent;
    [Range(0, 3)]
    public float MakingInterv;
    public string shape;
    public string[] cl = new string[] {"Red","Blue","Green" };
    public bool working;
    Color[] colors = new Color[] {Color.red,Color.blue,Color.green };

    int i = 0;

    private void Start()
    {
        StartCoroutine(MakeObj());
    }
    IEnumerator MakeObj()
    {
        while(true)
        {
            yield return new WaitForSeconds(MakingInterv);
            if (!working) continue;
            int x = Random.Range(0, 3);
            GameObject cnt = Instantiate(Obj,parent);
            cnt.GetComponent<SpriteRenderer>().color = colors[x];
            cnt.transform.position = new Vector3(transform.position.x + 50, transform.position.y, transform.position.z);
            cnt.GetComponent<Convey_Object>().color = cl[x];
            cnt.GetComponent<Convey_Object>().shape = shape;
            if (++i == 100) break;
        }
        yield break;
    }
}
