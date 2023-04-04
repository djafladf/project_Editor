using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject Obj;
    [Range(0, 3)]
    public float MakingInterv;

    private void Start()
    {
        StartCoroutine(MakeObj());
    }
    IEnumerator MakeObj()
    {
        for(int i = 0; i < 100; i++)
        {
            Instantiate(Obj).transform.position = new Vector3(transform.position.x + 50, transform.position.y, transform.position.z);
            yield return new WaitForSeconds(MakingInterv);
        }
        yield break;
    }
}
