using System.Collections;
using UnityEngine;

public class Convey_Object : MonoBehaviour
{
    public string color;
    public string shape;
    public bool MoveAble = true;
    public bool ETC;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Belt")
        {
            StopAllCoroutines();
            StartCoroutine(OnBelt(collision.transform));
        }
        else if(collision.gameObject.tag == "Infra")
        {
            if(ETC)StopAllCoroutines();
            switch (collision.gameObject.GetComponent<Installation>().type)
            {
                case 0: // ������
                    collision.gameObject.GetComponent<Infra_Goal>().JudgeObj(gameObject);
                    break;
                case 1: // �з���
                    collision.gameObject.GetComponent<Infra_Classifier>().Classify(gameObject);
                    break;
                case 2: // ������
                    collision.gameObject.GetComponent<Infra_Power>().AddFuel(gameObject);
                    break;
                case 3: // ���ձ�
                    collision.gameObject.GetComponent<Infra_Combine>().Combine(gameObject);
                    break;
                case 4: // �����
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
        ETC = false;
        // �߾ӱ��� �̵�
        for (; Vector3.Magnitude(transform.position - _St.position) >= 0.5f;)    
        {
            if (s.OnWork) transform.Translate((_St.position - transform.position).normalized);
            else yield return new WaitForSeconds(0.5f);
            yield return new WaitForSeconds(0.01f);
        }
        transform.position = _St.position;
        ETC = true;

        Vector3 a = (_Ed.position - _St.transform.position).normalized;
        // ������ �̵�
        for (;Vector3.Magnitude(transform.position - _Ed.position) >= 0.5f;)
        {
            if (s.OnWork) transform.Translate(a);
            else yield return new WaitForSeconds(0.5f);
            yield return new WaitForSeconds(0.01f);
        }
        yield break;
    }

    private void OnDisable()
    {
        MoveAble = true;
        ETC = false;
    }
}
