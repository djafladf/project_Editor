using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallManager : ScriptableObject
{
    /// <summary>
    /// Infra 설치 시 임시로 Layer를 담는 용으로 사용
    /// </summary>
    [HideInInspector]
    public GameObject CurLay;
    [HideInInspector]
    public bool cnt = true;    // 이상한 곳에 설치했을 때 마지막 layer에 설치되는 버그 방지용

}
