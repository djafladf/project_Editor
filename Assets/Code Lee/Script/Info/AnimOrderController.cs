﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnimOrderController : MonoBehaviour
{
    public AnimBase[] anims;
    public GameObject Power;
    // Use this for initialization
    void Start()
    {
    }

    public void realStart()
    {
        StartCoroutine(AnimInOrder());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator AnimInOrder()
    {
        foreach (AnimBase a in anims)
        {
            yield return StartCoroutine(a.PlayAnim());
        }
        Power.GetComponent<PowerOn>().TopInfoOff();

    }
}
