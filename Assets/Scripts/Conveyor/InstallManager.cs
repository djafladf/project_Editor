using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallManager : ScriptableObject
{
    /// <summary>
    /// Infra ��ġ �� �ӽ÷� Layer�� ��� ������ ���
    /// </summary>
    [HideInInspector]
    public GameObject CurLay;
    [HideInInspector]
    public bool cnt = true;    // �̻��� ���� ��ġ���� �� ������ layer�� ��ġ�Ǵ� ���� ������

}
