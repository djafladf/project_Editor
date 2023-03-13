﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AnimationController : MonoBehaviour
{
    /**
    *   애니메이션 실행
    *   - 각각의 애니메이션들을 리스트 순서대로 실행
    */
    public AnimBase[] anims;

    public void Play()
    {
        StartCoroutine("PlayAnimInOrder");
    }
    
    public void Pause()
    {
        StopCoroutine("PlayAnimInOrder");
    }

    IEnumerator PlayAnimInOrder()
    {
        foreach (AnimBase iter in anims)
        {
            yield return StartCoroutine(iter.Play());
        }
    }
}
