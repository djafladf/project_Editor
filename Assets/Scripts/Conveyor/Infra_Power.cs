using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Infra_Power : MonoBehaviour
{
    public int Fuel = 0;
    public Transform _St;
    Installation Ins;

    private void Awake()
    {
        Ins = GetComponent<Installation>();
    }
    private void Start()
    {
        StartCoroutine(Work());
    }
    public void AddFuel(GameObject A)
    {
        if (Vector3.Magnitude(A.transform.position - _St.position) <= 15)
        {
            if (A.GetComponent<Convey_Object>().shape != "") Fuel += 10;
            if (A.GetComponent<Convey_Object>().color != "") Fuel += 10;
            Destroy(A);
        }
    }

    IEnumerator Work()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            print(Ins.CM.Power);
            if (Fuel != 0) 
            {
                if (Fuel < 10)
                {
                    Ins.CM.Power += Fuel;
                    Fuel = 0;
                }
                else
                {
                    Ins.CM.Power += Fuel;
                    Fuel -= 10;
                }

            }
        }
    }
}
